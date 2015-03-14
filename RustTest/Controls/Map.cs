using RustTest.Entities;
using RustTest.Render;
using System;
using System.Drawing;

namespace RustTest.Controls
{
    public class Map : Graphic
    {
        #region Fields
        private Vector2f offset;
        private bool drag = false;
        private Vector2f dragOrigin;
        private Vector2f dragOffset;
        private float mapScale = 1f;
        private int mapWidth = 1098;
        private int mapHeight = 676;
        private Texture playerTexture;

        private World world = null;
        #endregion

        #region Properties                
        /// <summary>
        /// Gets or sets the world.
        /// </summary>
        /// <value>The world.</value>
        public World World {
            get {
                return world;
            }
            set {
                world = value;
            }
        }

        /// <summary>
        /// Gets or sets the player.
        /// </summary>
        /// <value>The player.</value>
        public Texture Player {
            get {
                return playerTexture;
            }
            set {
                playerTexture = value;
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Coords to position.
        /// </summary>
        /// <param name="coord">The coord.</param>
        /// <returns></returns>
        public Vector2f CoordToPos(Vector2f coord) {
            // map bounds
            Vector2f top = new Vector2f(2650, -7100);
            Vector2f bottom = new Vector2f(7835, 1443);

            // calculate
            float y = (((coord.X - top.X) / (bottom.X - top.X)) * (mapHeight * mapScale)) + offset.Y;
            float x = (((coord.Y - top.Y) / (bottom.Y - top.Y)) * (mapWidth * mapScale)) + offset.X;

            return new Vector2f(x, y);
        }

        /// <summary>
        /// Draws the graphic.
        /// </summary>
        /// <param name="context">The context.</param>
        public override void Draw(Context context) {
            // transform
            context.Transform = offset;

            // draw image
            context.FillTexture(0, 0, mapWidth * mapScale, mapHeight * mapScale, Image);

            // reset transform
            context.Transform = new Vector2f();

            // draw entities
            lock (world) {
                // draw entities
                foreach (IEntity entity in world.Entities) {
                    // get configuration
                    ConfigEntity configEntity = Config.ConfigureEntity(entity.Prefab);

                    // get map configuration
                    ConfigMapEntity mapConfig = configEntity.MapConfig;

                    if (!mapConfig.Visible)
                        continue;

                    // get positions
                    Vector2f pos = new Vector2f(entity.Position.X, entity.Position.Z);
                    Vector2f posMap = CoordToPos(pos);

                    // get color
                    Color color = mapConfig.Color;

                    if (entity.IsPlayer) {
                        Player player = (Player)entity;

                        if (player.IsLocalPlayer)
                            color = Config.LocalPlayerColor;
                    }

                    if (mapConfig.Shape == ConfigMapEntityShape.Rectangle) {
                        context.FillRectangle(posMap.X - 5, posMap.Y - 5, 10, 10, color);
                    } else if (mapConfig.Shape == ConfigMapEntityShape.Triangle) {
                        context.FillTexture(posMap.X - 5, posMap.Y - 5, 10, 10, playerTexture, entity.Angle.Y - 90, 5, 5, color);
                    }

                    // tag
                    context.DrawText(posMap.X + 10, posMap.Y, Font, entity.Name, Color.White, TextAlign.MiddleLeft);
                }
            }

            // transform
            context.Transform = offset;

            // grid calculations
            int gridX = (int)((mapWidth * mapScale) / 75);
            int gridY = (int)((mapHeight * mapScale) / 75);

            // draw grid
            for (int x = 0; x < gridX; x++) {
                context.DrawLine((mapWidth * mapScale) / gridX * x, 0, (mapWidth * mapScale) / gridX * x, mapHeight * mapScale, Color.White);

                for (int y = 0; y < gridY; y++) {
                    context.DrawLine(0, (mapHeight * mapScale) / gridY * y, mapWidth * mapScale, (mapHeight * mapScale) / gridY * y, Color.White);
                }   
            }

            // draw map border
            context.DrawRectangle(0, 0, (mapWidth * mapScale) + 1, (mapHeight * mapScale) + 1, Color.LimeGreen);

            // reset transform
            context.Transform = new Vector2f();

            // draw border
            context.DrawRectangle(0, 0, Width, Height, Color.LimeGreen);
        }

        /// <summary>
        /// Called when [mouse down].
        /// </summary>
        /// <param name="pos">The position.</param>
        public override void OnMouseDown(Vector2f pos) {
            drag = true;
            dragOrigin = pos;
            dragOffset = offset;
        }

        /// <summary>
        /// Called when [mouse up].
        /// </summary>
        /// <param name="pos">The position.</param>
        public override void OnMouseUp(Vector2f pos) {
            drag = false;
        }

        /// <summary>
        /// Called when [mouse exit].
        /// </summary>
        /// <param name="pos">The position.</param>
        public override void OnMouseExit(Vector2f pos) {
            drag = false;
        }

        /// <summary>
        /// Called when [mouse hover].
        /// </summary>
        /// <param name="pos">The position.</param>
        public override void OnMouseHover(Vector2f pos) {
            if (drag) {
                float desiredX = dragOffset.X + (pos.X - dragOrigin.X);
                float desiredY = dragOffset.Y + (pos.Y - dragOrigin.Y);

                if (desiredX > 0 || (mapWidth * mapScale) < Width) desiredX = 0;
                if (desiredY > 0 || (mapHeight * mapScale) < Height) desiredY = 0;

                if (-(mapWidth * mapScale) > desiredX - Width && (mapWidth * mapScale) > Width) desiredX = -(mapWidth * mapScale) + Width;
                if (-(mapHeight * mapScale) > desiredY - Height && (mapHeight * mapScale) > Height) desiredY = -(mapHeight * mapScale) + Height;

                offset.X = desiredX;
                offset.Y = desiredY;
            }
        }

        /// <summary>
        /// Called when [mouse wheel].
        /// </summary>
        /// <param name="movement">The movement.</param>
        public override void OnMouseWheel(Vector2f movement) {
            float desiredScale = mapScale +  (movement.Y / 10);

            if (desiredScale <= 0.1)
                desiredScale = 0.1f;

            mapScale = desiredScale;

            if ((mapWidth * mapScale) < Width) offset.X = 0;
            if ((mapHeight * mapScale) < Height) offset.Y = 0;

            if (-(mapWidth * mapScale) > offset.X - Width && (mapWidth * mapScale) > Width) offset.X = -(mapWidth * mapScale) + Width;
            if (-(mapHeight * mapScale) > offset.Y - Height && (mapHeight * mapScale) > Height) offset.Y = -(mapHeight * mapScale) + Height;
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="Map"/> class.
        /// </summary>
        public Map(Texture player)
            : base() {
            this.playerTexture = player;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Map"/> class.
        /// </summary>
        /// <param name="parent">The parent.</param>
        public Map(Texture player, Control parent)
            : base(parent) {
            this.playerTexture = player;
        }
        #endregion
    }
}

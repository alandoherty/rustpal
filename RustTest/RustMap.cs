using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using RustTest.Entities;
using System.Drawing.Drawing2D;

namespace RustTest
{
    public partial class RustMap : UserControl
    {
        #region Constants
        public const float MAP_WIDTH = 1098f;
        public const float MAP_HEIGHT = 676f;
        #endregion

        #region Fields
        private Point offset;
        private bool drag = false;
        private Point dragOrigin;
        private Point dragOffset;
        private float scale = 1.0f;

        private World world;
        #endregion

        #region Properties        
        /// <summary>
        /// Gets or sets the world.
        /// </summary>
        /// <value>The world.</value>
        public World World {
            get {
                return this.world;
            }
            set {
                this.world = value;
            }
        }

        /// <summary>
        /// Gets or sets the scale.
        /// </summary>
        /// <value>The scale.</value>
        public float MapScale {
            get {
                return this.scale;
            }
            set {
                this.scale = value;

                RescaleOffset();
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Coords to point.
        /// </summary>
        /// <param name="coord">The coord.</param>
        /// <returns></returns>
        public Point CoordToPoint(Point coord) {
            // map bounds
            PointF top = new PointF(2900, -7100);
            PointF bottom = new PointF(7835, 1443);

            // calculate
            float y = (((coord.X - top.X) / (bottom.X - top.X)) * (MAP_HEIGHT * scale)) + offset.Y;
            float x = (((coord.Y - top.Y) / (bottom.Y - top.Y)) * (MAP_WIDTH * scale)) + offset.X;

            return new Point((int)x, (int)y);
        }

        /// <summary>
        /// Rescales the offsets.
        /// </summary>
        public void RescaleOffset() {
            int width = (int)(MAP_WIDTH * scale);
            int height = (int)(MAP_HEIGHT * scale);

            if (Width > width) offset.X = 0;
            if (Height > height) offset.Y = 0;

            if (offset.X > 0) offset.X = 0;
            if (offset.Y > 0) offset.Y = 0;
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="RustMap"/> class.
        /// </summary>
        public RustMap() {
            InitializeComponent();
        }
        #endregion

        #region Events
        private void RustMap_Paint(object sender, PaintEventArgs e) {
            e.Graphics.DrawImage(Properties.Resources.RustMap, offset.X, offset.Y, (MAP_WIDTH * scale), (MAP_HEIGHT * scale));

            if (world == null) return;

            if (!drag) {
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
                        Point pos = new Point((int)entity.Position.X, (int)entity.Position.Z);
                        Point posMap = CoordToPoint(pos);

                        // create brush
                        Brush brush = new SolidBrush(mapConfig.Color);

                        if (entity.IsPlayer) {
                            Player player = (Player)entity;

                            if (player.IsLocalPlayer)
                                brush = Brushes.Green;
                        }

                        // angle
                        Matrix myMatrix = new Matrix();
                        //myMatrix.Rotate(entity.Angle.Y, MatrixOrder.Append);
                        myMatrix.Translate(posMap.X, posMap.Y);
                        e.Graphics.Transform = myMatrix;

                        // get shape
                        if (mapConfig.Shape == ConfigMapEntityShape.Rectangle) {
                            e.Graphics.FillRectangle(brush, new Rectangle(0, 0, 10, 10));
                        } else {
                            e.Graphics.FillPolygon(brush, new Point[] {
                                new Point(5, 5),
                                new Point(5, 5),
                                new Point(0, 5)
                            });
                        }

                        // tag
                        if (Wildlife.IsPrefabWildlife(entity.Prefab))
                            e.Graphics.DrawString(configEntity.Name + " " + ((Wildlife)entity).State.ToString(), new Font("Courier New", 9), Brushes.White, new PointF(10, 3));
                        else
                            e.Graphics.DrawString(configEntity.Name, new Font("Courier New", 9), Brushes.White, new PointF(10, 3));
                    }
                }
            }
        }

        private void RustMap_MouseDown(object sender, MouseEventArgs e) {
            drag = true;
            dragOrigin = e.Location;
            dragOffset = offset;
        }

        private void RustMap_MouseUp(object sender, MouseEventArgs e) {
            drag = false;

            this.Refresh();
        }

        private void RustMap_MouseMove(object sender, MouseEventArgs e) {
            if (drag) {
                offset.X = dragOffset.X + (e.Location.X - dragOrigin.X);
                offset.Y = dragOffset.Y + (e.Location.Y - dragOrigin.Y);

                RescaleOffset();

                this.Refresh();
            }
        }
        #endregion
    }
}

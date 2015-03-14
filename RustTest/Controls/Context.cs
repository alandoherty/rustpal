using RustTest.Render;
using SDL2;
using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace RustTest.Controls
{
    public class Context
    {
        #region Fields
        private Window window;
        private Vector2f offset;
        private Vector2f transform;
        #endregion

        #region Properties        
        /// <summary>
        /// Gets or sets the offset.
        /// </summary>
        /// <value>The offset.</value>
        public Vector2f Offset {
            get {
                return offset;
            }
            set {
                offset = value;
            }
        }

        /// <summary>
        /// Gets or sets the transform.
        /// </summary>
        /// <value>The transform.</value>
        public Vector2f Transform {
            get {
                return transform;
            }
            set {
                transform = value;
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Fills the rectangle.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="w">The width.</param>
        /// <param name="h">The height.</param>
        /// <param name="color">The color.</param>
        public void FillRectangle(float x, float y, float w, float h, Color color) {
            // create rectangle
            SDL.SDL_Rect rect = CreateRectangle(x, y, w, h);

            // draw
            ExecuteColor(new Action(delegate() {
                SDL.SDL_RenderFillRect(window.Renderer, ref rect);
            }), color);
        }

        /// <summary>
        /// Fills the surface.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="surface">The surface.</param>
        public void FillSurface(float x, float y, IntPtr surface) {
            // create texture
            Texture tex = Texture.FromSurface(window, surface);

            // draw
            FillTexture(x, y, tex);

            // destroy texture
            SDL.SDL_DestroyTexture(tex.Pointer);
        }

        /// <summary>
        /// Fills the texture.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="tex">The texture.</param>
        public void FillTexture(float x, float y, Texture tex) {
            FillTexture(x, y, tex.Width, tex.Height, tex);
        }

        /// <summary>
        /// Fills the texture.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="w">The w.</param>
        /// <param name="h">The h.</param>
        /// <param name="tex">The tex.</param>
        public void FillTexture(float x, float y, float w, float h, Texture tex) {
            FillTexture(x, y, w, h, tex, 0, 0, 0, SDL.SDL_RendererFlip.SDL_FLIP_NONE, Color.White);
        }

        /// <summary>
        /// Fills the texture.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="w">The w.</param>
        /// <param name="h">The h.</param>
        /// <param name="tex">The tex.</param>
        /// <param name="angle">The angle.</param>
        /// <param name="centerX">The center x.</param>
        /// <param name="centerY">The center y.</param>
        /// <param name="color">The color.</param>
        public void FillTexture(float x, float y, float w, float h, Texture tex, float angle, float centerX, float centerY, Color color) {
            FillTexture(x, y, w, h, tex, angle, centerX, centerY, SDL.SDL_RendererFlip.SDL_FLIP_NONE, color);
        }

        /// <summary>
        /// Fills the texture.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="w">The width.</param>
        /// <param name="h">The height.</param>
        /// <param name="tex">The texture.</param>
        public void FillTexture(float x, float y, float w, float h, Texture tex, float angle, float centerX, float centerY, SDL.SDL_RendererFlip flip, Color color) {
            // create source rect
            SDL.SDL_Rect source = new SDL.SDL_Rect() {
                x = (int)0,
                y = (int)0,
                w = (int)tex.Width,
                h = (int)tex.Height
            };
            
            // create dest rect
            SDL.SDL_Rect dest = CreateRectangle(x, y, w, h);

            // create center
            SDL.SDL_Point center = CreatePoint(centerX, centerY);

            // set color
            SDL.SDL_SetTextureColorMod(tex.Pointer, color.R, color.G, color.B);

            // copy
            SDL.SDL_RenderCopyEx(window.Renderer, tex.Pointer, ref source, ref dest, (double)angle, ref center, flip);
        }

        /// <summary>
        /// Draws the rectangle.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="w">The w.</param>
        /// <param name="h">The h.</param>
        /// <param name="color">The color.</param>
        public void DrawRectangle(float x, float y, float w, float h, Color color) {
            // create rectangle
            SDL.SDL_Rect rect = CreateRectangle(x, y, w, h);

            // draw
            ExecuteColor(new Action(delegate() {
                SDL.SDL_RenderDrawRect(window.Renderer, ref rect);
            }), color);
        }

        public void DrawText(float x, float y, IntPtr font, string text, Color color) {
            // create color
            SDL.SDL_Color colorSdl = CreateColor(color);

            // render text
            IntPtr renderedText = SDL_ttf.TTF_RenderText_Solid(font, text, colorSdl);

            if (renderedText == IntPtr.Zero) {
                Logger.DebugError("renderer failed for text " + text + " with error " + SDL.SDL_GetError());
                return;
            }

            // create texture
            FillSurface(x, y, renderedText);

            // free
            SDL.SDL_FreeSurface(renderedText);
        }

        public void DrawText(float x, float y, IntPtr font, string text, Color color, TextAlign align) {
            // get size
            Vector2f size = GetTextSize(font, text);

            // draw
            if (align == TextAlign.Center) {
                DrawText(x - (size.X / 2), y - (size.Y / 2), font, text, color);
            } else if (align == TextAlign.MiddleLeft) {
                DrawText(x, y - (size.Y / 2), font, text, color);
            } else if (align == TextAlign.BottomLeft) {
                DrawText(x, y + (size.Y / 2), font, text, color);
            }
        }

        public void DrawLine(float x1, float y1, float x2, float y2, Color color) {
            // draw
            ExecuteColor(new Action(delegate() {
                SDL.SDL_RenderDrawLine(window.Renderer,
                    (int)x1 + (int)offset.X + (int)transform.X, (int)y1 + (int)offset.Y + (int)transform.Y,
                    (int)x2 + (int)offset.X + (int)transform.X, (int)y2 + (int)offset.Y + (int)transform.Y
                );
            }), color);
        }

        /// <summary>
        /// Gets the size of the text.
        /// </summary>
        /// <param name="font">The font.</param>
        /// <param name="text">The text.</param>
        /// <returns></returns>
        public Vector2f GetTextSize(IntPtr font, string text) {
            // render text
            IntPtr renderedText = SDL_ttf.TTF_RenderText_Solid(font, text, CreateColor(Color.White));

            // create surface
            SDL.SDL_Surface surface = CreateSurface(renderedText);

            // free surface
            SDL.SDL_FreeSurface(renderedText);

            return new Vector2f(surface.w, surface.h);
        }

        /// <summary>
        /// Executes the targeted draw.
        /// </summary>
        /// <param name="func">The function.</param>
        /// <param name="color">The color.</param>
        private void ExecuteColor(Action func, Color color) {
            // set color
            SDL.SDL_SetRenderDrawColor(window.Renderer, color.R, color.G, color.B, color.A);

            // execute
            func.Invoke();
        }

        private SDL.SDL_Rect CreateRectangle(float x, float y, float w, float h) {
            // create rect
            SDL.SDL_Rect rect = new SDL.SDL_Rect() {
                x = (int)x + (int)offset.X + (int)transform.X,
                y = (int)y + (int)offset.Y + (int)transform.Y,
                w = (int)w,
                h = (int)h
            };

            return rect;
        }

        private SDL.SDL_Color CreateColor(Color color) {
            // create color
            SDL.SDL_Color colorSdl = new SDL.SDL_Color() {
                r = color.R,
                g = color.G,
                b = color.B,
                a = color.A
            };

            return colorSdl;
        }

        private SDL.SDL_Point CreatePoint(float x, float y) {
            // create point
            SDL.SDL_Point pointSdl = new SDL.SDL_Point() {
                x = (int)x,
                y = (int)y
            };

            return pointSdl;
        }

        private SDL.SDL_Surface CreateSurface(IntPtr surface) {
            return (SDL.SDL_Surface)Marshal.PtrToStructure(surface, typeof(SDL.SDL_Surface));
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="Context"/> class.
        /// </summary>
        /// <param name="window">The window.</param>
        public Context(Window window)
            : this(window, new Vector2f()) 
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Context"/> class.
        /// </summary>
        /// <param name="window">The window.</param>
        /// <param name="offset">The offset.</param>
        public Context(Window window, Vector2f offset) {
            this.window = window;
            this.offset = offset;
        }
        #endregion
    }

    public enum TextAlign {
        TopLeft,
        MiddleLeft,
        BottomLeft,
        Center
    }
}

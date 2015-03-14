using RustTest.Controls;
using SDL2;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace RustTest.Render
{
    public class GuiSystem
    {
        #region Fields
        private Control guiRoot;
        private Window window;
        private Vector2f mouse;
        #endregion

        #region Properties
        public ObservableCollection<Control> Controls {
            get {
                return guiRoot.Children;
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Draw the gui system.
        /// </summary>
        public void Draw() {
            foreach (Control c in guiRoot.Children) {
                // create context
                Context context = new Context(window);
                context.Offset = new Vector2f(c.X, c.Y);

                // create clip
                SDL.SDL_Rect clip = new SDL.SDL_Rect() {
                    x = (int)c.X,
                    y = (int)c.Y,
                    w = (int)c.Width,
                    h = (int)c.Height
                };

                // set clip
                SDL.SDL_RenderSetClipRect(window.Renderer, ref clip);

                // draw
                c.Draw(context);

                // clear clip
                SDL.SDL_RenderSetClipRect(window.Renderer, IntPtr.Zero);
            }
        }

        /// <summary>
        /// Handles the SDL event.
        /// </summary>
        /// <param name="e">The e.</param>
        public void HandleEvent(SDL.SDL_Event e) {
            if (e.type == SDL.SDL_EventType.SDL_MOUSEMOTION) {
                // set mouse pos
                mouse = new Vector2f(e.motion.x, e.motion.y);

                // find controls in pos
                foreach (Control c in guiRoot.Children) {
                    c.OnMouseMove(mouse);
                }
            } else if (e.type == SDL.SDL_EventType.SDL_MOUSEBUTTONDOWN) {
                // find controls in pos
                foreach (Control c in guiRoot.Children) {
                    if (c.X <= mouse.X && c.X + c.Width >= mouse.X && c.Y <= mouse.Y && c.Y + c.Height >= mouse.Y)
                        c.OnMouseDown(new Vector2f(mouse.X - c.X, mouse.Y - c.Y));
                }
            } else if (e.type == SDL.SDL_EventType.SDL_MOUSEBUTTONUP) {
                // find controls in pos
                foreach (Control c in guiRoot.Children) {
                    if (c.X <= mouse.X && c.X + c.Width >= mouse.X && c.Y <= mouse.Y && c.Y + c.Height >= mouse.Y)
                        c.OnMouseUp(new Vector2f(mouse.X - c.X, mouse.Y - c.Y));
                }
            } else if (e.type == SDL.SDL_EventType.SDL_MOUSEWHEEL) {
                // find controls in pos
                foreach (Control c in guiRoot.Children) {
                    if (c.X <= mouse.X && c.X + c.Width >= mouse.X && c.Y <= mouse.Y && c.Y + c.Height >= mouse.Y)
                        c.OnMouseWheel(new Vector2f(e.wheel.x, e.wheel.y));
                }
            }
        }
        #endregion

        #region Constructors        
        /// <summary>
        /// Initializes a new instance of the <see cref="GuiSystem"/> class.
        /// </summary>
        /// <param name="window">The window.</param>
        public GuiSystem(Window window) {
            this.window = window;

            // create root
            this.guiRoot = new Control();
        }
        #endregion
    }
}

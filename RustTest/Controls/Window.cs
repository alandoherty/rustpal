using RustTest.Render;
using SDL2;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading;

namespace RustTest.Controls
{
    public class Window
    {
        #region Fields
        private IntPtr pointer = IntPtr.Zero;
        private IntPtr renderer = IntPtr.Zero;

        private GuiSystem guiSystem;
        private Stopwatch stopwatch;
        #endregion

        #region Properties        
        /// <summary>
        /// Gets the renderer.
        /// </summary>
        /// <value>The renderer.</value>
        public IntPtr Renderer {
            get {
                return renderer;
            }
        }

        /// <summary>
        /// Gets the pointer to this window.
        /// </summary>
        /// <value>The pointer.</value>
        public IntPtr Pointer {
            get {
                return pointer;
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Executes the event loop.
        /// </summary>
        public void Run(int fps) {
            // event
            SDL.SDL_Event e;

            // calculate desired fps
            int fpsSleep = (int)(1000 / fps);

            // loop
            while (true) {
                while (SDL.SDL_PollEvent(out e) == 1) {
                    if (e.type == SDL.SDL_EventType.SDL_QUIT)
                        Environment.Exit(0);

                    guiSystem.HandleEvent(e);
                }

               ExecuteTimed(fpsSleep, new Action(Draw));
            }
        }

        private void Draw() {
            // clear
            SDL.SDL_SetRenderDrawColor(renderer, 0, 0, 0, 255);
            SDL.SDL_RenderClear(renderer);

            // draw gui
            guiSystem.Draw();

            // present
            SDL.SDL_RenderPresent(renderer);
        }

        /// <summary>
        /// Executes the action with timing.
        /// </summary>
        /// <param name="time">The maximum miliseconds to wait.</param>
        /// <param name="func">The function.</param>
        private void ExecuteTimed(int time, Action func) {
            // start
            stopwatch.Start();

            // execute
            func.Invoke();

            // sleep
            stopwatch.Stop();

            int sleep = time - (int)stopwatch.ElapsedMilliseconds;

            if (sleep > 0)
                Thread.Sleep(sleep);
            else
                Logger.DebugError("renderer took " + Math.Abs(sleep) + "ms too long");

            // reset
            stopwatch.Reset();
        }
        #endregion

        #region Constructors                
        /// <summary>
        /// Initializes a new instance of the <see cref="Window"/> class.
        /// </summary>
        /// <param name="title">The title.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="world">The world.</param>
        public Window(string title, int width, int height, World world) {
            // initialize sdl
            SDL.SDL_Init(SDL.SDL_INIT_VIDEO);
            SDL_ttf.TTF_Init();
            SDL_image.IMG_Init(SDL_image.IMG_InitFlags.IMG_INIT_JPG | SDL_image.IMG_InitFlags.IMG_INIT_PNG);

            // create renderer stopwatch
            stopwatch = new Stopwatch();

            // log
            Logger.Log("initializing window");

            // create window
            pointer = SDL.SDL_CreateWindow(title, 50, 50, width, height, SDL.SDL_WindowFlags.SDL_WINDOW_SHOWN);

            // log
            Logger.Log("initializing renderer");

            // create renderer
            renderer = SDL.SDL_CreateRenderer(pointer, 0, (uint)(SDL.SDL_RendererFlags.SDL_RENDERER_ACCELERATED | SDL.SDL_RendererFlags.SDL_RENDERER_TARGETTEXTURE));

            // log
            Logger.Log("initializing gui");

            // create gui systen
            guiSystem = new GuiSystem(this);

            // create map
            Map graphic = new Map(new Texture(this, SDL_image.IMG_LoadTexture(renderer, "resources/player.png"))) {
                Image = new Texture(this, SDL_image.IMG_LoadTexture(renderer, "resources/RustMap.jpg")),
                Position = new Vector2f(25, 25),
                Size = new Vector2f(width - 50, height - 50),
                World = world
            };

            guiSystem.Controls.Add(graphic);
        }
        #endregion
    }
}

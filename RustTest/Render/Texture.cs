using RustTest.Controls;
using SDL2;
using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace RustTest.Render
{
    public class Texture
    {
        #region Fields
        private Vector2f size;
        private IntPtr pointer;
        private Window window;
        #endregion

        #region Properties        
        /// <summary>
        /// Gets the width.
        /// </summary>
        /// <value>The width.</value>
        public int Width {
            get {
                return (int)size.X;
            }
        }

        /// <summary>
        /// Gets the height.
        /// </summary>
        /// <value>The height.</value>
        public int Height {
            get {
                return (int)size.Y;
            }
        }

        /// <summary>
        /// Gets the pointer to this texture.
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
        /// Creates a texture from a surface.
        /// </summary>
        /// <param name="window">The window.</param>
        /// <param name="surface">The surface.</param>
        /// <returns></returns>
        public static Texture FromSurface(Window window, IntPtr surface) {
            // create
            Texture tex = new Texture(window, IntPtr.Zero);

            // create
            tex.pointer = SDL.SDL_CreateTextureFromSurface(window.Renderer, surface);

            // get size
            SDL.SDL_Surface surfaceStruct = (SDL.SDL_Surface)Marshal.PtrToStructure(surface, typeof(SDL.SDL_Surface));

            tex.size.X = surfaceStruct.w;
            tex.size.Y = surfaceStruct.h;

            return tex;
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="Texture"/> class.
        /// </summary>
        /// <param name="window">The window.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        public Texture(Window window, int width, int height) {
            this.window = window;

            // create
            pointer = SDL.SDL_CreateTexture(window.Renderer, SDL.SDL_PIXELFORMAT_RGBA8888, (int)SDL.SDL_TextureAccess.SDL_TEXTUREACCESS_TARGET, width, height);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Texture"/> class from a surface.
        /// </summary>
        /// <param name="window">The window.</param>
        /// <param name="texture">The texture.</param>
        public Texture(Window window, IntPtr texture) {
            this.window = window;
            this.pointer = texture;

            if (texture != IntPtr.Zero) {
                // get data
                uint format = 0;
                int access = 0;
                int width = 0;
                int height = 0;
                SDL.SDL_QueryTexture(pointer, out format, out access, out width, out height);

                // set size
                size.X = width;
                size.Y = height;
            }
        }
        #endregion
    }
}

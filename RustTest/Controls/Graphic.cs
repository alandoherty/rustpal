using RustTest.Render;
using System;

namespace RustTest.Controls
{
    public class Graphic : Control
    {
        #region Fields
        private Texture image;
        #endregion

        #region Properties        
        /// <summary>
        /// Gets or sets the image.
        /// </summary>
        /// <value>The image.</value>
        public Texture Image {
            get {
                return image;
            }
            set {
                image = value;
            }
        }
        #endregion

        #region Methods        
        /// <summary>
        /// Draws the graphic.
        /// </summary>
        /// <param name="context">The context.</param>
        public override void Draw(Context context) {
            // background
            base.Draw(context);

            // draw image
            context.FillTexture(0, 0, image);
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="Graphic"/> class.
        /// </summary>
        public Graphic()
            : base() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Graphic"/> class.
        /// </summary>
        /// <param name="parent">The parent.</param>
        public Graphic(Control parent)
            : base(parent) { }
        #endregion
    }
}

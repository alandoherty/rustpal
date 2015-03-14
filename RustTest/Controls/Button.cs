using SDL2;
using System;
using System.Drawing;

namespace RustTest.Controls
{
    public delegate void ButtonClickedHandler(object sender, ButtonClickedEventArgs e);

    public class Button : Control
    {
        #region Fields
        private Color borderColor = Color.White;

        public event ButtonClickedHandler Clicked;
        #endregion

        public override void Draw(Context context) {
            // background
            base.Draw(context);

            // border
            context.DrawRectangle(0, 0, Width, Height, borderColor);

            // text
            context.DrawText(Width / 2, Height / 2, Font, Text, ForegroundColor, TextAlign.Center);
        }

        public override void OnMouseEnter(Vector2f pos) {
            borderColor = Color.LimeGreen;
        }

        public override void OnMouseExit(Vector2f pos) {
            borderColor = Color.White;
        }

        public override void OnMouseDown(Vector2f pos) {
            if (Clicked != null)
                Clicked(this, new ButtonClickedEventArgs(pos));
        }

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="Button"/> class.
        /// </summary>
        public Button()
            : base() {
            BackgroundColor = Color.Black;
            ForegroundColor = Color.White;
        }
        #endregion
    }

    public class ButtonClickedEventArgs : EventArgs
    {
        #region Fields
        private Vector2f mouse;
        #endregion

        #region Properties        
        /// <summary>
        /// Gets the mouse.
        /// </summary>
        /// <value>The mouse.</value>
        public Vector2f Mouse {
            get {
                return mouse;
            }
        }
        #endregion

        #region Constructors        
        /// <summary>
        /// Initializes a new instance of the <see cref="ButtonClickedEventArgs"/> class.
        /// </summary>
        /// <param name="mouse">The mouse.</param>
        public ButtonClickedEventArgs(Vector2f mouse) {
            this.mouse = mouse;
        }
        #endregion
    }
}

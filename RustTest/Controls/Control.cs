using SDL2;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Drawing;

namespace RustTest.Controls
{
    public class Control
    {
        #region Fields
        private Vector2f pos;
        private Vector2f size;
        private bool visible;
        private Control parent;
        private ObservableCollection<Control> children;
        private string text;
        private Color backColor = Color.White;
        private Color foreColor = Color.Black;
        private IntPtr font;

        private bool mouseEntered = false;
        #endregion

        #region Properties        
        /// <summary>
        /// Gets or sets the position.
        /// </summary>
        /// <value>The position.</value>
        public Vector2f Position {
            get {
                return pos;
            }
            set {
                pos = value;
            }
        }

        /// <summary>
        /// Gets or sets the size.
        /// </summary>
        /// <value>The size.</value>
        public Vector2f Size {
            get {
                return size;
            }
            set {
                size = value;
            }
        }

        /// <summary>
        /// Gets or sets the width.
        /// </summary>
        /// <value>The width.</value>
        public float Width {
            get {
                return size.X;
            }
            set {
                size.X = value;
            }
        }

        /// <summary>
        /// Gets or sets the height.
        /// </summary>
        /// <value>The height.</value>
        public float Height {
            get {
                return size.Y;
            }
            set {
                size.Y = value;
            }
        }

        /// <summary>
        /// Gets or sets the x.
        /// </summary>
        /// <value>The x.</value>
        public float X {
            get {
                return pos.X;
            }
            set {
                pos.X = value;
            }
        }

        /// <summary>
        /// Gets or sets the y.
        /// </summary>
        /// <value>The y.</value>
        public float Y {
            get {
                return pos.Y;
            }
            set {
                pos.Y = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Control"/> is visible.
        /// </summary>
        /// <value><c>true</c> if visible; otherwise, <c>false</c>.</value>
        public bool Visible {
            get {
                return visible;
            }
            set {
                visible = value;
            }
        }
        
        /// <summary>
        /// Gets or sets the children.
        /// </summary>
        /// <value>The children.</value>
        public ObservableCollection<Control> Children {
            get {
                return children;
            }
            set {
                children = value;
            }
        }

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        /// <value>The text.</value>
        public string Text {
            get {
                return text;
            }
            set {
                text = value;
            }
        }

        /// <summary>
        /// Gets or sets the color of the foreground.
        /// </summary>
        /// <value>
        /// The color of the foreground.
        /// </value>
        public Color ForegroundColor {
            get {
                return foreColor;
            }
            set {
                foreColor = value;
            }
        }

        /// <summary>
        /// Gets or sets the color of the background.
        /// </summary>
        /// <value>
        /// The color of the background.
        /// </value>
        public Color BackgroundColor {
            get {
                return backColor;
            }
            set {
                backColor = value;
            }
        }

        /// <summary>
        /// Gets or sets the font.
        /// </summary>
        /// <value>The font.</value>
        public IntPtr Font {
            get {
                return font;
            }
            set {
                font = value;
            }
        }
        #endregion

        #region Methods        
        /// <summary>
        /// Draws the control.
        /// </summary>
        /// <param name="context">The context.</param>
        public virtual void Draw(Context context) {
            context.FillRectangle(0, 0, size.X, size.Y, backColor);
        }

        /// <summary>
        /// Called when [mouse move], internal - do not call.
        /// </summary>
        /// <param name="pos">The position.</param>
        public void OnMouseMove(Vector2f pos) {
            if (X <= pos.X && X + Width >= pos.X && Y <= pos.Y && Y + Height >= pos.Y) {
                if (!mouseEntered) {
                    OnMouseEnter(new Vector2f(pos.X - X, pos.Y - Y));
                    mouseEntered = true;
                }

                OnMouseHover(new Vector2f(pos.X - X, pos.Y - Y));
            } else if (mouseEntered) {
                OnMouseExit(new Vector2f(pos.X - X, pos.Y - Y));
                mouseEntered = false;
            }
        }

        public virtual void OnMouseDown(Vector2f pos) {

        }

        public virtual void OnMouseUp(Vector2f pos) {

        }

        public virtual void OnMouseEnter(Vector2f pos) {

        }

        public virtual void OnMouseExit(Vector2f pos) {

        }

        public virtual void OnMouseHover(Vector2f pos) {

        }

        public virtual void OnMouseWheel(Vector2f movement) {

        }
        #endregion

        #region Constructors        
        /// <summary>
        /// Initializes a new instance of the <see cref="Control"/> class.
        /// </summary>
        public Control() {
            this.children = new ObservableCollection<Control>();
            this.font = SDL_ttf.TTF_OpenFont("resources/coders_crux.TTF", 16);

            // handle new controls
            this.children.CollectionChanged += new NotifyCollectionChangedEventHandler(delegate(object s, NotifyCollectionChangedEventArgs e) {
                foreach (Control c in e.NewItems) {
                    c.parent = this;
                }
            });
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Control"/> class.
        /// </summary>
        /// <param name="parent">The parent.</param>
        public Control(Control parent)
            : this() {
            this.parent = parent;
            
            if (parent != null)
                parent.Children.Add(this);
        }
        #endregion
    }
}

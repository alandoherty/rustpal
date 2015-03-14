using System;

namespace RustTest
{
    public struct Vector2f
    {
        #region Fields
        private float x;
        private float y;
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the x.
        /// </summary>
        /// <value>The x.</value>
        public float X {
            get {
                return this.x;
            }
            set {
                this.x = value;
            }
        }

        /// <summary>
        /// Gets or sets the y.
        /// </summary>
        /// <value>The y.</value>
        public float Y {
            get {
                return this.y;
            }
            set {
                this.y = value;
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString() {
            return "{X: " + x + " Y: " + y + "}";
        }
        #endregion

        #region Constructors        
        /// <summary>
        /// Initializes a new instance of the <see cref="Vector2f"/> struct.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        public Vector2f(float x, float y) {
            this.x = x;
            this.y = y;
        }
        #endregion
    }
}

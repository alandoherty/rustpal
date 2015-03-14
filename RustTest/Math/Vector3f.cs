using System;

namespace RustTest
{
    public struct Vector3f
    {
        #region Fields
        private float x;
        private float y;
        private float z;
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

        /// <summary>
        /// Gets or sets the z.
        /// </summary>
        /// <value>The z.</value>
        public float Z {
            get {
                return this.z;
            }
            set {
                this.z = value;
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
            return "{X: " + x + " Y: " + y + " Z: " + z + "}";
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="Vector3f"/> struct.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="z">The z.</param>
        public Vector3f(float x, float y, float z) 
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
        #endregion
    }
}

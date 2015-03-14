using System;

namespace RustTest.Net
{
    public class PacketRPC
    {
        #region Fields
        private string name;
        private ushort targetObject;
        private ushort sourceObject;
        #endregion

        #region Properties        
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name {
            get {
                return this.name;
            }
            set {
                this.name = value;
            }
        }

        /// <summary>
        /// Gets or sets the target object.
        /// </summary>
        /// <value>
        /// The target object.
        /// </value>
        public ushort TargetObject {
            get {
                return this.targetObject;
            }
            set {
                this.targetObject = value;
            }
        }

        /// <summary>
        /// Gets or sets the source object.
        /// </summary>
        /// <value>
        /// The source object.
        /// </value>
        public ushort SourceObject {
            get {
                return this.sourceObject;
            }
            set {
                this.sourceObject = value;
            }
        }
        #endregion

        #region Constructors          
        /// <summary>
        /// Initializes a new instance of the <see cref="PacketRPC"/> class.
        /// </summary>
        public PacketRPC() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="PacketRPC"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="targetObject">The target object.</param>
        /// <param name="sourceObject">The source object.</param>
        public PacketRPC(string name, ushort targetObject, ushort sourceObject) {
            this.name = name;
            this.targetObject = targetObject;
            this.sourceObject = sourceObject;
        }
        #endregion
    }
}

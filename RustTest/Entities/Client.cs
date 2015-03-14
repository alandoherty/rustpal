using RustTest.Net;
using System;

namespace RustTest.Entities
{
    public class Client : Entity
    {
        #region Fields
        private string username = "";
        private byte[] unknown = null;
        #endregion

        #region Properties        
        /// <summary>
        /// Gets the username.
        /// </summary>
        /// <value>The username.</value>
        public string Username {
            get {
                return username;
            }
        }
        #endregion

        #region Methods
        public override void OnDestroy() {
            // log
            Logger.Log(username + " has left the game");
            
            // base destroy
            base.OnDestroy();
        }

        /// <summary>
        /// Unserializes to the specified stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        public override void Unserialize(ULinkStream stream) {
            byte[] data = stream.ReadBytes(8);
            
			// read name
            this.username = stream.ReadString();

            // log
            Logger.Log(username + " has joined the game");
        }

        /// <summary>
        /// Serializes to the specified stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        public override void Serialize(ULinkStream stream) {
            base.Serialize(stream);
        }
        #endregion

        #region Constructors        
        /// <summary>
        /// Initializes a new instance of the <see cref="Client"/> class.
        /// </summary>
        /// <param name="world">The world.</param>
        /// <param name="id">The identifier.</param>
        public Client(World world, ushort id)
            : base(world, id) { }
        #endregion
    }
}

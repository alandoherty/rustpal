using RustTest.Net;
using System;

namespace RustTest.Entities
{
    public class ServerManagement : Entity
    {
        #region Fields
        #endregion

        #region Properties
        #endregion

        #region Methods	
        /// <summary>
        /// Unserializes to the specified stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        public override void Unserialize(ULinkStream stream) {
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
        /// Initializes a new instance of the <see cref="ServerManagement"/> class.
        /// </summary>
        /// <param name="world">The world.</param>
        /// <param name="id">The identifier.</param>
        public ServerManagement(World world, ushort id)
            : base(world, id) {
            this.prefab = "ServerManagement";
        }
        #endregion
    }
}

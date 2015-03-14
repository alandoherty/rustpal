using RustTest.Net;
using System;

namespace RustTest.Entities
{
    public class Backpack : Entity
    {
        #region Fields
        #endregion

        #region Properties
        #endregion

        #region Methods
        public void RecieveNetwork(ULinkStream stream) {

        }
        #endregion

        #region Constructors        
        /// <summary>
        /// Initializes a new instance of the <see cref="Backpack"/> class.
        /// </summary>
        /// <param name="world">The world.</param>
        /// <param name="id">The identifier.</param>
        public Backpack(World world, ushort id)
            : base(world, id) { }
        #endregion
    }
}

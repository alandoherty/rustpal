using RustTest.Net;
using System;

namespace RustTest.Entities
{
    public class SkyManager : Entity
    {
        #region Methods
        public void CL_UpdateSkyState(ULinkStream stream) {
            stream.ReadBytes(43);
        }
        #endregion

        #region Constructors        
        /// <summary>
        /// Initializes a new instance of the <see cref="SkyManager"/> class.
        /// </summary>
        /// <param name="world">The world.</param>
        public SkyManager(World world)
            : base(world, 0x03E4) {
            this.prefab = "SkyManager";
        }
        #endregion
    }
}

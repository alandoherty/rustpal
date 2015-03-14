using RustTest.Net;
using System;

namespace RustTest.Entities
{
    public class ConsoleManager : Entity
    {
        #region Methods
        public void CL_ConsoleCommand(ULinkStream stream) {
            stream.ReadBytes(2);

            // command
            string command = stream.ReadString();
        }

        public void SV_RunConsoleCommand(ULinkStream stream) {
            stream.ReadBytes(2);

            // command
            string command = stream.ReadString();
        }

        public void CL_ConsoleMessage(ULinkStream stream) {
            stream.ReadBytes(2);

            // message
            string message = stream.ReadString();
        }
        #endregion

        #region Constructors        
        /// <summary>
        /// Initializes a new instance of the <see cref="ConsoleManager"/> class.
        /// </summary>
        /// <param name="world">The world.</param>
        public ConsoleManager(World world)
            : base(world, 0x03E5) 
        {
            this.prefab = "ConsoleManager";
        }
        #endregion
    }
}

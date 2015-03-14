using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RustTest.Net
{
    public class PacketPayload
    {
        #region Fields
        protected Packet packet;
        #endregion

        #region Methods        
        /// <summary>
        /// Processes the specified payload with the specified context.
        /// </summary>
        /// <param name="context">The proxy context.</param>
        /// <returns></returns>
        public virtual byte[] Process(ProxyContext context) {
            // do nothing
            return this.packet.Data;
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="PacketPayload"/> class.
        /// </summary>
        /// <param name="packet">The packet.</param>
        public PacketPayload(Packet packet) {
            this.packet = packet;
        }
        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace RustTest.Net.Payloads
{
    public class LinkPayload : PacketPayload
    {
        #region Fields
        #endregion

        #region Methods
        /// <summary>
        /// Processes the specified payload with the specified context.
        /// </summary>
        /// <param name="context">The proxy context.</param>
        /// <returns></returns>
        public override byte[] Process(ProxyContext context) {
            return this.packet.Data;
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="LinkPayload"/> class.
        /// </summary>
        /// <param name="packet">The packet.</param>
        public LinkPayload(Packet packet)
            : base(packet) { }
        #endregion
    }
}

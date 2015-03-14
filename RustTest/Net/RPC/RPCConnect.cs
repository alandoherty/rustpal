using System;

namespace RustTest.Net.RPC
{
    public static class RPCConnect
    {
        #region Methods
        /// <summary>
        /// Handles the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="stream">The stream.</param>
        public static PacketRPC Handle(ProxyContext context, ULinkStream stream) {
            // unknown
            stream.ReadBytes(16);

            // name
            string name = stream.ReadString();

            // log
            Logger.Log("connecting to server as " + name);

            return new PacketRPC("Connect", 0, 0);
        }
        #endregion
    }
}

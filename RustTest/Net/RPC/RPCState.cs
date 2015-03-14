using System;

namespace RustTest.Net.RPC
{
    public static class RPCState
    {
        #region Methods
        /// <summary>
        /// Handles the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="stream">The stream.</param>
        public static PacketRPC Handle(ProxyContext context, ULinkStream stream) {
            // object id
            ushort targetObject = stream.ReadUShort();

            // name
            string name = stream.ReadString();

            return new PacketRPC(name, targetObject, targetObject);
        }
        #endregion
    }
}

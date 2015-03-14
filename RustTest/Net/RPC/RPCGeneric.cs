using RustTest.Net.Payloads;
using System;

namespace RustTest.Net.RPC
{
    public static class RPCGeneric
    {
        #region Methods
        /// <summary>
        /// Handles the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="stream">The stream.</param>
        public static PacketRPC Handle(ProxyContext context, ULinkStream stream) {
            // submode
            RPCGenericMode submode = (RPCGenericMode)stream.ReadUByte();

            if (IsGenericRPC(submode)) {
                if (submode == RPCGenericMode.Buffered) {
                    return HandleBuffered(context, stream);
                }
            } else {
                Logger.DebugError("unhandled generic rpc submode " + submode.ToString("X"));
                return new PacketRPC("Generic", 0, 0);
            }

            return new PacketRPC("Generic", 0, 0);
        }

        /// <summary>
        /// Handles a buffered generic RPC.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="stream">The stream.</param>
        /// <returns></returns>
        public static PacketRPC HandleBuffered(ProxyContext context, ULinkStream stream) {
            // read size
            int size = stream.ReadEncodedInt();

            // read each packet
            for (int i = 0; i < size; i++) {
                // read length
                int length = stream.ReadEncodedInt();

                // read packet
                byte[] packet = stream.ReadBytes(length);

                using (ULinkStream packetStream = new ULinkStream(packet)) {
                    // read mode
                    RPCPayloadMode mode = (RPCPayloadMode)packetStream.ReadUByte();

                    if (!RPCPayload.IsValidMode(mode)) {
                        Logger.DebugError(context, "invalid generic buffered rpc mode 0x" + mode.ToString("X"));
                        return new PacketRPC("Generic", 0, 0);
                    }

                    // process
                    RPCPayload.Process(context, packetStream, mode);
                }
            }

            return new PacketRPC("Generic", 0, 0);
        }

        /// <summary>
        /// Determines whether [is generic RPC] [the specified mode].
        /// </summary>
        /// <param name="mode">The mode.</param>
        /// <returns></returns>
        public static bool IsGenericRPC(RPCGenericMode mode) {
            return (mode == RPCGenericMode.Buffered);
        }

        #endregion
    }

    public enum RPCGenericMode : byte
    {
        Buffered = 0x03,
        KeepAlive = 0x0C
    }
}

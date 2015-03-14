using RustTest.Entities;
using RustTest.Net.RPC;
using System;
using System.Collections.Generic;
using System.IO;

namespace RustTest.Net.Payloads
{
    public class RPCPayload : PacketPayload
    {
        #region Fields
        private PacketRPC rpc;
        #endregion

        #region Methods  
        /// <summary>
        /// Processes the specified payload with the specified context.
        /// </summary>
        /// <param name="context">The proxy context.</param>
        /// <returns>Context.</returns>
        public override byte[] Process(ProxyContext context) {
            using (ULinkStream ms = new ULinkStream(this.packet.Data)) {
                // rpc mode
                RPCPayloadMode mode = (RPCPayloadMode)ms.ReadUByte();

                // check if valid
                if (!IsValidMode(mode)) {
                    //File.WriteAllBytes("packets/RPC_" + mode.ToString("X") + "_" + packet.Tag + ".dat", this.packet.Data);
                    Logger.DebugError(context, "invalid rpc mode 0x" + mode.ToString("X"));
                    return this.packet.Data;
                }

                // process rpc
                this.rpc = Process(context, ms, mode);

                if (this.rpc == null)
                    Logger.DebugError("rpc packet has missing mode handler for " + mode.ToString());
            }

            return this.packet.Data;
        }

        /// <summary>
        /// Determines whether [is valid mode] [the specified mode].
        /// </summary>
        /// <param name="mode">The mode.</param>
        /// <returns></returns>
        public static bool IsValidMode(RPCPayloadMode mode) {
            foreach (RPCPayloadMode payloadMode in Enum.GetValues(typeof(RPCPayloadMode))) {
                if (payloadMode == (RPCPayloadMode)mode)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Determines whether [is user RPC] [the specified mode].
        /// </summary>
        /// <param name="mode">The mode.</param>
        /// <returns></returns>
        public static bool IsUserRPC(RPCPayloadMode mode) {
            return (mode == RPCPayloadMode.Others || mode == RPCPayloadMode.OthersBuffered ||
                mode == RPCPayloadMode.OthersExceptOwner || mode == RPCPayloadMode.Single ||
                mode == RPCPayloadMode.Server);
        }

        /// <summary>
        /// Processes the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="stream">The stream.</param>
        /// <param name="mode">The mode.</param>
        /// <returns></returns>
        public static PacketRPC Process(ProxyContext context, ULinkStream stream, RPCPayloadMode mode) {
            if (IsUserRPC(mode)) {
                return RPCUser.Handle(context, stream, mode);
            } else if (mode == RPCPayloadMode.Instantiate) {
                return RPCInstantiate.Handle(context, stream);
            } else if (mode == RPCPayloadMode.Destroy) {
                return RPCDestroy.Handle(context, stream);
            } else if (mode == RPCPayloadMode.State) {
                return RPCState.Handle(context, stream);
            } else if (mode == RPCPayloadMode.Connect) {
                return RPCConnect.Handle(context, stream);
            } else if (mode == RPCPayloadMode.Generic) {
                return RPCGeneric.Handle(context, stream);
            }

            return null;
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="RPCPayload"/> class.
        /// </summary>
        /// <param name="packet">The packet.</param>
        public RPCPayload(Packet packet)
            : base(packet) { }
        #endregion
    }

    public enum RPCPayloadMode : byte
    {
        Connect = 0x00,
        Generic = 0x04,
        Others = 0x2C,
        Single = 0x2A ,
        OthersExceptOwner = 0x2E,
        OthersBuffered = 0x7C,
        Server = 0x28,
        Instantiate = 0x74,
        Destroy = 0x14,
        State = 0x9C
    }
}

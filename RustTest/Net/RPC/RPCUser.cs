using RustTest.Entities;
using RustTest.Net.Payloads;
using System;

namespace RustTest.Net.RPC
{
    public static class RPCUser
    {
        #region Methods                
        /// <summary>
        /// Handles the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="stream">The stream.</param>
        /// <param name="mode">The mode.</param>
        /// <returns></returns>
        public static PacketRPC Handle(ProxyContext context, ULinkStream stream, RPCPayloadMode mode) {
            // rpc source object
            ushort sourceObject = 0;

            if (mode == RPCPayloadMode.Single || mode == RPCPayloadMode.OthersExceptOwner)
                sourceObject = stream.ReadUShort();

            // rpc target object
            ushort targetObject = stream.ReadUShort();

            if (mode != RPCPayloadMode.Single && mode != RPCPayloadMode.OthersExceptOwner)
                sourceObject = targetObject;

            // rpc name
            string name = stream.ReadString();

            lock (context.World) {
                // invoke rpc
                bool success = context.World.InvokeRPC(targetObject, name, stream);

                // log
                if (!success) {
                    Entity entity = (Entity)context.World.GetEntityByID(targetObject);

                    if (entity == null && name != "GetNetworkUpdate" && name != "Snd" && name != "ReadClientMove" && name != "GetClientMove")
                        Logger.DebugError("entity 0x" + targetObject.ToString("X4") + " is not instantiated for " + name);
                    if (entity != null && entity.Prefab != "!Ng")
                        Logger.DebugError("entity " + entity.Prefab + " is missing rpc method " + name);
                }
            }

            return new PacketRPC(name, targetObject, sourceObject);
        }
        #endregion
    }
}

using System;

namespace RustTest.Net.RPC
{
    public static class RPCDestroy
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

            lock (context.World) {
                // check if in world
                if (!context.World.EntityExists(targetObject)) {
                    Logger.DebugError("entity " + targetObject.ToString("X") + " cannot be destroyed as it does not exist");
                    return new PacketRPC("Destroy", targetObject, targetObject);
                }

                // remove world
                context.World.RemoveEntityByID(targetObject);
            }

            // log
            Logger.DebugError(context, "destroyed entity " + targetObject.ToString("X"));

            return new PacketRPC("Destroy", targetObject, targetObject);
        }
        #endregion
    }
}

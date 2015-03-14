using RustTest.Entities;
using System;

namespace RustTest.Net.RPC
{
    public static class RPCInstantiate
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

            // unknown
            stream.ReadBytes(6);
            byte linkId = stream.ReadUByte();
            stream.ReadBytes(3);

            // position
            Vector3f position = new Vector3f(stream.ReadFloat(), stream.ReadFloat(), stream.ReadFloat());
            Angle2 angle = new Angle2() {
                encoded = stream.ReadInt()
            };

            // unkown
            stream.ReadBytes(12);

            // prefab
            string prefab = stream.ReadString();
            stream.ReadString();
            stream.ReadString();

            // configure
            ConfigEntity configEntity = Config.ConfigureEntity(prefab);

            // create entity
            Entity entity = (Entity)Activator.CreateInstance(configEntity.Instance, context.World, targetObject);

            // set data
            entity.Position = position;
            entity.Prefab = prefab;
            entity.Angle = angle;
            entity.LinkID = linkId;

            // unserialize
            entity.Unserialize(stream);

            lock (context.World) {
                // add entity
                context.World.AddEntity(entity);
            }

            // log
            //Logger.Debug(context, "instantiated entity " + prefab);

            return new PacketRPC("Instantiate", targetObject, targetObject);
        }
        #endregion
    }
}

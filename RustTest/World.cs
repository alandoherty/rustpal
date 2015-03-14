using RustTest.Entities;
using RustTest.Net;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace RustTest
{
    public class World
    {
        #region Fields
        private List<IEntity> entities;
        #endregion

        #region Properties        
        /// <summary>
        /// Gets the entities.
        /// </summary>
        /// <value>The entities.</value>
        public List<IEntity> Entities {
            get {
                return this.entities;
            }
        }

        /// <summary>
        /// Gets the local player.
        /// </summary>
        /// <value>The local player.</value>
        public Player LocalPlayer {
            get {
                foreach (IEntity entity in entities) {
                    if (entity.IsPlayer) {
                        Player player = (Player)entity;
                        if (player.IsLocalPlayer)
                            return player;
                    }
                }

                return null;
            }
        }
        #endregion

        #region Methods        
        /// <summary>
        /// Adds an entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public void AddEntity(IEntity entity) {
            // remove existing entity
            if (EntityExists(entity.ID))
                RemoveEntityByID(entity.ID);
            
            // add
            entities.Add(entity);
        }

        /// <summary>
        /// Gets an entity by it's identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        public IEntity GetEntityByID(ushort id) {
            // find existing
            foreach (IEntity entity in entities) {
                if (entity.ID == id)
                    return entity;
            }

            return null;
        }

        /// <summary>
        /// Gets an entity by it's identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        public bool EntityExists(ushort id) {
            // find existing
            foreach (IEntity entity in entities) {
                if (entity.ID == id)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Removes an entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public void RemoveEntity(IEntity entity) {
            entity.OnDestroy();
            entities.Remove(entity);
        }

        /// <summary>
        /// Removes an entity by it's identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        public void RemoveEntityByID(ushort id) {
            RemoveEntity(GetEntityByID(id));
        }

        /// <summary>
        /// Invokes the RPC.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="stream">The stream.</param>
        /// <returns></returns>
        internal bool InvokeRPC(ushort id, string name, ULinkStream stream) {
            // check if entity exists
            if (!EntityExists(id))
                return false;

            // get entity
            Entity entity = (Entity)GetEntityByID(id);

            // find type
            Type type = entity.GetType();

            // find method
            MethodInfo[] methods = type.GetMethods();
            MethodInfo rpc = null;

            foreach (MethodInfo method in methods) {
                if (method.Name == name) {
                    rpc = method;
                    break;
                }
            }

            if (rpc == null)
                return false;

            // execute
            rpc.Invoke(entity, new object[] { stream });

            return true;
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="World"/> class.
        /// </summary>
        public World() {
            this.entities = new List<IEntity>();

            // add defaults
            this.entities.Add(new SkyManager(this));
            this.entities.Add(new ConsoleManager(this));
        }
        #endregion
    }
}

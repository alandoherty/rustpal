using RustTest.Net;
using System;
using System.Collections.Generic;

namespace RustTest.Entities
{
    public class Player : Entity
    {
        #region Fields
        private float calories;
        private float waterLevel;
        private float radiationLevel;
        private float antiRads;
        private float coreTemperature;
        private float posionLevel;
        private bool local = false;
        #endregion

        #region Properties        
        /// <summary>
        /// Gets or sets the calories.
        /// </summary>
        /// <value>The calories.</value>
        public float Calories {
            get {
                return this.calories;
            }
            set {
                this.calories = value;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is local player.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is local player; otherwise, <c>false</c>.
        /// </value>
        public bool IsLocalPlayer {
            get {
                return this.local;
            }
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>The name.</value>
        public override string Name {
            get {
                // username
                string username = base.Name;

                // find client name
                lock (world) {
                    foreach (IEntity entity in world.Entities) {
                        if (entity.IsClient && entity.LinkID == linkId) {
                            Client client = (Client)entity;
                            username = client.Username;
                        }
                    }
                }

                // find client weapon
                lock (world) {
                    foreach (IEntity entity in world.Entities) {
                        if (entity.IsWeapon && entity.LinkID == linkId) {
                            username += " (" + Config.ConfigureEntity(entity.Prefab).Name + ")";
                        }
                    }
                }

                // if not, do base
                return username;
            }
        }
        #endregion

        #region Methods
        public void ReadClientMove(ULinkStream stream) {
            // read bytes
            stream.ReadBytes(2);

            // read pos
            this.Position = new Vector3f(stream.ReadFloat(), stream.ReadFloat(), stream.ReadFloat());

            // read angle
            this.Angle = new Angle2() {
                encoded = stream.ReadInt()
            };
        }

        public void GetClientMove(ULinkStream stream) {
            // local
            this.local = true;

            // read bytes
            stream.ReadBytes(2);

            // read pos
            this.Position = new Vector3f(stream.ReadFloat(), stream.ReadFloat(), stream.ReadFloat());

            // read angle
            this.Angle = new Angle2() {
                encoded = stream.ReadInt()
            };
        }

        public void RecieveNetwork(ULinkStream stream) {
            // unk
            stream.ReadBytes(2);

            float caloricLevel = stream.ReadFloat();
            float waterLevelLitre = stream.ReadFloat();
            float radiationLevel = stream.ReadFloat();
            float antiRads = stream.ReadFloat();
            float coreTemperature = stream.ReadFloat();
            float poisonLevel = stream.ReadFloat();
        }

        public override void OnDestroy() {
            if (local) {
                List<ushort> removeIds = new List<ushort>();

                lock (world) {
                    foreach (IEntity entity in world.Entities) {
                        if (entity.Prefab == "ServerManagement" ||
                            entity.Prefab == "SkyManager" ||
                            entity.Prefab == "StructureMaster" ||
                            entity.Prefab == "ConsoleManager" ||
                            entity.Prefab == ":client" ||
                            entity.ID == id)
                            continue;

                        removeIds.Add(entity.ID);
                    }

                    foreach (ushort id in removeIds) {
                        world.RemoveEntityByID(id);
                    }
                }
            }
        }
        #endregion

        #region Constructors        
        /// <summary>
        /// Initializes a new instance of the <see cref="Player"/> class.
        /// </summary>
        /// <param name="world">The world.</param>
        /// <param name="id">The identifier.</param>
        public Player(World world, ushort id)
            : base(world, id) { }
        #endregion
    }
}

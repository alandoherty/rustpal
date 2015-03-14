using RustTest.Net;
using System;
using System.IO;

namespace RustTest.Entities
{
    public class Entity : IEntity
    {
        #region Fields
        protected ushort id;
        protected byte linkId;
        protected Vector3f pos;
        protected Angle2 angle;
        protected string prefab;
        protected World world;
        #endregion

        #region Properties        
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public ushort ID {
            get {
                return this.id;
            }
            set {
                this.id = value;
            }
        }

        /// <summary>
        /// Gets the link identifier.
        /// </summary>
        /// <value>The link identifier.</value>
        public byte LinkID {
            get {
                return linkId;
            }
            set {
                linkId = value;
            }
        }

        /// <summary>
        /// Gets or sets the position.
        /// </summary>
        /// <value>The position.</value>
        public Vector3f Position {
            get {
                return this.pos;
            }
            set {
                this.pos = value;
            }
        }

        /// <summary>
        /// Gets or sets the angle.
        /// </summary>
        /// <value>The angle.</value>
        public Angle2 Angle {
            get {
                return this.angle;
            }
            set {
                this.angle = value;
            }
        }

        /// <summary>
        /// Gets the prefab.
        /// </summary>
        /// <value>The prefab.</value>
        public string Prefab {
            get {
                return this.prefab;
            }
            set {
                this.prefab = value;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is player.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is player; otherwise, <c>false</c>.
        /// </value>
        public bool IsPlayer {
            get {
                return this.prefab == Config.Player;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is client.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is client; otherwise, <c>false</c>.
        /// </value>
        public bool IsClient {
            get {
                return this.prefab == ":client";
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is weapon.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is weapon; otherwise, <c>false</c>.
        /// </value>
        public bool IsWeapon {
            get {
                return (this.prefab == "rock.irp"
                    || this.prefab == "m4.irp"
                    || this.prefab == "handcannon.irp"
                    || this.prefab == "hatchet.irp"
                    || this.prefab == "deploy.irp"
                    || this.prefab == "pipeshotgun.irp"
                    || this.prefab == "pistol.irp"
                    || this.prefab == "torch.irp"
                    || this.prefab == "shotgun.irp");
            }
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>The name.</value>
        public virtual string Name {
            get {
                return Config.ConfigureEntity(prefab).Name;
            }
        }
        #endregion

        #region Methods              
        /// <summary>
        /// Called when the entity is about to be destroyed.
        /// </summary>
        public virtual void OnDestroy() {

        }

        public void InterpDestroy(ULinkStream stream) {

        }

        /// <summary>
        /// Unserializes to the specified stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        public virtual void Unserialize(ULinkStream stream) {

        }

        /// <summary>
        /// Serializes to the specified stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public virtual void Serialize(ULinkStream stream) {
            throw new NotImplementedException();
        }
        #endregion

        #region Constructors        
        /// <summary>
        /// Initializes a new instance of the <see cref="Entity"/> class.
        /// </summary>
        /// <param name="world">The world.</param>
        /// <param name="id">The identifier.</param>
        public Entity(World world, ushort id) {
            this.world = world;
            this.id = id;
        }
        #endregion
    }
}

using System;
using System.IO;

namespace RustTest.Net
{
    public class Packet : ICloneable
    {
        #region Fields
        private PacketType type = PacketType.Unknown;
        private byte[] data = null;
        private PacketPayload payload = null;
        private ushort tag = 0;
        #endregion

        #region Properties        
        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>The type.</value>
        public PacketType Type {
            get {
                return this.type;
            }
            set {
                this.type = value;
            }
        }

        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        /// <value>The payload.</value>
        public byte[] Data {
            get {
                return this.data;
            }
            set {
                this.data = value;
            }
        }

        /// <summary>
        /// Gets or sets the payload.
        /// </summary>
        /// <value>The payload.</value>
        public PacketPayload Payload {
            get {
                return this.payload;
            }
            set {
                this.payload = value;
            }
        }

        /// <summary>
        /// Gets or sets the tag.
        /// </summary>
        /// <value>The tag.</value>
        public ushort Tag {
            get {
                return this.tag;
            }
            set {
                this.tag = value;
            }
        }
        #endregion

        #region Methods        
        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>
        /// A new object that is a copy of this instance.
        /// </returns>
        public object Clone() {
            return this.MemberwiseClone();
        }

        /// <summary>
        /// Processes the packet with the specified context.
        /// </summary>
        /// <param name="context">The proxy context.</param>
        public void Process(ProxyContext context) {
            // process
            byte[] data = this.payload.Process(context);

            // assign data
            this.data = data;
        }
        
        /// <summary>
        /// Determines whether [is valid packet type] [the specified type].
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public static bool IsValidType(PacketType type) {
            foreach (PacketType packetType in Enum.GetValues(typeof(PacketType))) {
                if (packetType == (PacketType)type)
                    return true;
            }

            return false;
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="Packet"/> class.
        /// </summary>
        public Packet() {
            this.payload = new PacketPayload(this);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Packet"/> class.
        /// </summary>
        /// <param name="packet">The packet.</param>
        public Packet(Packet packet) {
            this.data = packet.Data;
            this.payload = packet.payload;
            this.tag = packet.Tag;
            this.type = packet.Type;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Packet"/> class.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="data">The data.</param>
        public Packet(PacketType type, byte[] data) {
            this.type = type;
            this.data = data;
            this.payload = new PacketPayload(this);
        }
        #endregion
    }

    public enum PacketType : byte
    {
        Unknown = 0xFF,
        RPC = 0x89,
        BufferedRPC = 0x8A,
        Link = 0x07
    }
}

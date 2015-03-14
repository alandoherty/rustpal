using System;
using System.IO;

namespace RustTest.Net
{
    public class PacketStream : ULinkStream
    {
        #region Methods        
        /// <summary>
        /// Reads the packet.
        /// </summary>
        /// <returns>Packet.</returns>
        public Packet ReadPacket() {
            // read header
            byte type = ReadUByte();
            if (type == 0x00)  return null;
            ushort tag = ReadUShort();
            
            // read payload
            int length = ReadEncodedInt();
            byte[] payload = ReadBytes(length);

            // check valid type
            if (!Packet.IsValidType((PacketType)type) && type != 0x03 && type != 0x01)
                Logger.DebugError("unknown p " + type.ToString("X") + " t 0x" + tag.ToString("X") + " l " + payload.Length);

            return new Packet() {
                Type = (PacketType)type,
                Tag = tag,
                Data = payload
            };
        }

        /// <summary>
        /// Writes the packet.
        /// </summary>
        /// <param name="packet">The packet.</param>
        public void WritePacket(Packet packet) {
            // write header
            WriteUByte((byte)packet.Type);
            WriteUShort(packet.Tag);

            // write data
            WriteUByte((byte)packet.Data.Length);
            WriteBytes(packet.Data);
        }
        #endregion

        #region Constructors        
        /// <summary>
        /// Initializes a new instance of the <see cref="PacketStream"/> class.
        /// </summary>
        /// <param name="buffer">The array of unsigned bytes from which to create the current stream.</param>
        public PacketStream(byte[] buffer)
            : base(buffer) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="PacketStream"/> class.
        /// </summary>
        /// <param name="stream">The stream.</param>
        public PacketStream(Stream stream)
            : base(stream) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="PacketStream"/> class.
        /// </summary>
        public PacketStream()
            : base() { }
        #endregion
    }
}

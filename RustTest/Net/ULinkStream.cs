using System;
using System.IO;

namespace RustTest.Net
{
    public class ULinkStream : Stream
    {
        #region Fields
        private BinaryReader reader = null;
        private BinaryWriter writer = null;
        private Stream baseStream = null;
        #endregion

        #region Properties        
        /// <summary>
        /// When overridden in a derived class, gets a value indicating whether the current stream supports reading.
        /// </summary>
        public override bool CanRead {
            get {
                return this.baseStream.CanRead;
            }
        }

        /// <summary>
        /// When overridden in a derived class, gets a value indicating whether the current stream supports seeking.
        /// </summary>
        public override bool CanSeek {
            get {
                return this.baseStream.CanSeek;
            }
        }

        /// <summary>
        /// When overridden in a derived class, gets a value indicating whether the current stream supports writing.
        /// </summary>
        public override bool CanWrite {
            get {
                return this.baseStream.CanWrite;
            }
        }

        /// <summary>
        /// When overridden in a derived class, gets the length in bytes of the stream.
        /// </summary>
        public override long Length {
            get {
                return this.baseStream.Length;
            }
        }

        /// <summary>
        /// When overridden in a derived class, gets or sets the position within the current stream.
        /// </summary>
        public override long Position {
            get {
                return this.baseStream.Position;
            }
            set {
                this.baseStream.Position = value;
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Reads the string.
        /// </summary>
        /// <returns></returns>
        public string ReadString() {
            return reader.ReadString();
        }

        /// <summary>
        /// Reads the unsigned byte.
        /// </summary>
        /// <returns></returns>
        public byte ReadUByte() {
            return reader.ReadByte();
        }

        /// <summary>
        /// Reads the signed byte.
        /// </summary>
        /// <returns></returns>
        public sbyte ReadSByte() {
            return reader.ReadSByte();
        }

        /// <summary>
        /// Reads the unsigned short.
        /// </summary>
        /// <returns></returns>
        public ushort ReadUShort() {
            return reader.ReadUInt16();
        }

        /// <summary>
        /// Reads the signed short.
        /// </summary>
        /// <returns></returns>
        public short ReadShort() {
            return reader.ReadInt16();
        }

        /// <summary>
        /// Reads the unsigned integer.
        /// </summary>
        /// <returns></returns>
        public uint ReadUInt() {
            return reader.ReadUInt32();
        }

        /// <summary>
        /// Reads the integer.
        /// </summary>
        /// <returns></returns>
        public int ReadInt() {
            return reader.ReadInt32();
        }

        /// <summary>
        /// Reads the encoded int.
        /// </summary>
        /// <returns></returns>
        public int ReadEncodedInt() {
            int returnValue = 0;
            int bitIndex = 0;

            while (bitIndex != 35) {
                byte currentByte = ReadUByte();
                returnValue |= ((int)currentByte & (int)sbyte.MaxValue) << bitIndex;
                bitIndex += 7;

                if (((int)currentByte & 128) == 0)
                    return returnValue;
            }

            throw new FormatException("Invalid 7-bit encoded integer");
        }

        /// <summary>
        /// Reads the bytes.
        /// </summary>
        /// <param name="count">The count.</param>
        /// <returns>Data.</returns>
        public byte[] ReadBytes(int count) {
            byte[] buffer = new byte[count];

            // read
            Read(buffer, 0, count);

            return buffer;
        }

        /// <summary>
        /// Reads the float.
        /// </summary>
        /// <returns></returns>
        public float ReadFloat() {
            return reader.ReadSingle();
        }

        /// <summary>
        /// Writes the string.
        /// </summary>
        /// <param name="str">The string.</param>
        public void WriteString(string str) {
            writer.Write(str);
        }

        /// <summary>
        /// Writes the unsigned byte.
        /// </summary>
        /// <param name="b">The byte.</param>
        public void WriteUByte(byte b) {
            writer.Write(b);
        }

        /// <summary>
        /// Writes the signed byte.
        /// </summary>
        /// <param name="b">The byte.</param>
        public void WriteSByte(sbyte b) {
            writer.Write(b);
        }

        /// <summary>
        /// Writes the unsigned short.
        /// </summary>
        /// <param name="s">The short.</param>
        public void WriteUShort(ushort s) {
            writer.Write(s);
        }

        /// <summary>
        /// Writes the signed short.
        /// </summary>
        /// <param name="s">The signed.</param>
        public void WriteShort(short s) {
            writer.Write(s);
        }

        /// <summary>
        /// Writes the encoded int.
        /// </summary>
        /// <param name="value">The value.</param>
        public void WriteEncodedInt(int value) {
            uint num = (uint)value;

            while (num >= 128U) {
                WriteUByte((byte)(num | 128U));
                num >>= 7;
            }

            WriteUByte((byte)num);
        }

        /// <summary>
        /// Writes the bytes.
        /// </summary>
        /// <param name="data">The data.</param>
        public void WriteBytes(byte[] data) {
            Write(data, 0, data.Length);
        }

        /// <summary>
        /// When overridden in a derived class, reads a sequence of bytes from the current stream and advances the position within the stream by the number of bytes read.
        /// </summary>
        /// <param name="buffer">An array of bytes. When this method returns, the buffer contains the specified byte array with the values between <paramref name="offset" /> and (<paramref name="offset" /> + <paramref name="count" /> - 1) replaced by the bytes read from the current source.</param>
        /// <param name="offset">The zero-based byte offset in <paramref name="buffer" /> at which to begin storing the data read from the current stream.</param>
        /// <param name="count">The maximum number of bytes to be read from the current stream.</param>
        /// <returns>
        /// The total number of bytes read into the buffer. This can be less than the number of bytes requested if that many bytes are not currently available, or zero (0) if the end of the stream has been reached.
        /// </returns>
        public override int Read(byte[] buffer, int offset, int count) {
            return this.baseStream.Read(buffer, offset, count);
        }

        /// <summary>
        /// When overridden in a derived class, sets the position within the current stream.
        /// </summary>
        /// <param name="offset">A byte offset relative to the <paramref name="origin" /> parameter.</param>
        /// <param name="origin">A value of type <see cref="T:System.IO.SeekOrigin" /> indicating the reference point used to obtain the new position.</param>
        /// <returns>
        /// The new position within the current stream.
        /// </returns>
        public override long Seek(long offset, SeekOrigin origin) {
            return this.baseStream.Seek(offset, origin);
        }

        /// <summary>
        /// When overridden in a derived class, sets the length of the current stream.
        /// </summary>
        /// <param name="value">The desired length of the current stream in bytes.</param>
        public override void SetLength(long value) {
            this.baseStream.SetLength(value);
        }

        /// <summary>
        /// When overridden in a derived class, writes a sequence of bytes to the current stream and advances the current position within this stream by the number of bytes written.
        /// </summary>
        /// <param name="buffer">An array of bytes. This method copies <paramref name="count" /> bytes from <paramref name="buffer" /> to the current stream.</param>
        /// <param name="offset">The zero-based byte offset in <paramref name="buffer" /> at which to begin copying bytes to the current stream.</param>
        /// <param name="count">The number of bytes to be written to the current stream.</param>
        public override void Write(byte[] buffer, int offset, int count) {
            this.baseStream.Write(buffer, offset, count);
        }

        /// <summary>
        /// When overridden in a derived class, clears all buffers for this stream and causes any buffered data to be written to the underlying device.
        /// </summary>
        public override void Flush() {
            this.baseStream.Flush();
        }

        /// <summary>
        /// To the array.
        /// </summary>
        /// <returns></returns>
        public byte[] ToArray() {
            // cast
            MemoryStream ms = (MemoryStream)baseStream;

            return ms.ToArray();
        }
        #endregion

        #region Constructors        
        /// <summary>
        /// Initializes a new instance of the <see cref="ULinkStream"/> class.
        /// </summary>
        /// <param name="buffer">The array of unsigned bytes from which to create the current stream.</param>
        public ULinkStream(byte[] buffer)
            : base() {
            this.baseStream = new MemoryStream(buffer);
            this.reader = new BinaryReader(this.baseStream);
            this.writer = new BinaryWriter(this.baseStream);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ULinkStream"/> class.
        /// </summary>
        /// <param name="stream">The stream.</param>
        public ULinkStream(Stream stream)
            : base() {
            this.baseStream = stream;
            this.reader = new BinaryReader(this.baseStream);
            this.writer = new BinaryWriter(this.baseStream);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ULinkStream"/> class.
        /// </summary>
        public ULinkStream()
            : base() {
            this.baseStream = new MemoryStream();
            this.reader = new BinaryReader(this.baseStream);
            this.writer = new BinaryWriter(this.baseStream);
        }
        #endregion
    }
}

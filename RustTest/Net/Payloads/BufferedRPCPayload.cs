using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace RustTest.Net.Payloads
{
    public class BufferedRPCPayload : PacketPayload
    {
        #region Fields
        private static Dictionary<ushort, BufferedSession> sessions;
        #endregion

        #region Methods
        /// <summary>
        /// Processes the specified payload with the specified context.
        /// </summary>
        /// <param name="context">The proxy context.</param>
        /// <returns></returns>
        public override byte[] Process(ProxyContext context) {
            using (ULinkStream stream = new ULinkStream(this.packet.Data)) {
                // read session
                ushort session = stream.ReadUShort();

                // read subtag
                byte subtag = stream.ReadUByte();

                // read size
                int size = stream.ReadEncodedInt();

                // add buffer
                if (!sessions.ContainsKey(session)) {
                    sessions.Add(session, new BufferedSession(size));
                }

                // read data
                List<byte> data = new List<byte>();

                while(true) {
                    if (stream.Position == stream.Length) break;

                    data.Add(stream.ReadUByte());
                }

                // add subtag
                sessions[session].Blocks[subtag] = data;

                // check if complete
                if (sessions[session].Count == size) {
                    // merge blocks
                    List<byte> bufferData = new List<byte>();

                    foreach (List<byte> block in sessions[session].Blocks)
                        bufferData.AddRange(block);

                    //File.WriteAllBytes("packets/b_" + session + ".dat", bufferData.ToArray());

                    using (ULinkStream bufferStream = new ULinkStream(bufferData.ToArray())) {
                        // read header
                        byte code = bufferStream.ReadUByte();

                        if (code != 0x02) {
                            Logger.DebugError("buffer has invalid header code");
                            return this.packet.Data;
                        }

                        bufferStream.ReadBytes(3);
                        bufferStream.ReadEncodedInt();

                        // read packets
                        while (true) {
                            // check if at end
                            if (bufferStream.Position == bufferStream.Length)
                                break;

                            // read length
                            int length = bufferStream.ReadEncodedInt();
                            
                            // read packet
                            byte[] packet = bufferStream.ReadBytes(length);

                            using (ULinkStream packetStream = new ULinkStream(packet)) {
                                // read mode
                                RPCPayloadMode mode = (RPCPayloadMode)packetStream.ReadUByte();

                                if (!RPCPayload.IsValidMode(mode)) {
                                    Logger.DebugError(context, "invalid buffered rpc mode 0x" + mode.ToString("X"));
                                    return this.packet.Data;
                                }

                                // process
                                RPCPayload.Process(context, packetStream, mode);
                            }
                        }
                    }
                }
            }

            return this.packet.Data;
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="BufferedRPCPayload"/> class.
        /// </summary>
        /// <param name="packet">The packet.</param>
        public BufferedRPCPayload(Packet packet)
            : base(packet) { }

        /// <summary>
        /// Initializes the <see cref="BufferedRPCPayload"/> class.
        /// </summary>
        static BufferedRPCPayload() {
            sessions = new Dictionary<ushort, BufferedSession>();
        }
        #endregion
    }

    public enum BufferedRPCType 
    {
        Block = 0x01
    }
}

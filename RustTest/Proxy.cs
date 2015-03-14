using RustTest.Net;
using RustTest.Net.Payloads;
using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;

namespace RustTest
{
    public class Proxy
    {
        #region Fields
        private UdpClient fromClient = null;
        private UdpClient toServer = null;
        private Thread clientThread = null;
        private Thread serverThread = null;
        private IPEndPoint target = null;
        private IPEndPoint source = null;
        private int port = 0;
        private bool connected = false;
        private World world = null;
        #endregion

        #region Properties
        /// <summary>
        /// Gets the world.
        /// </summary>
        /// <value>The world.</value>
        public World World {
            get {
                return world;
            }
        }

        public int Port {
            get {
                return port;
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Redirects client traffic to the server.
        /// </summary>
        private void ClientRedirector() {
            while (true) {
                // receive data
                IPEndPoint src = null;
                byte[] data = null;

                try {
                    src = new IPEndPoint(IPAddress.Any, port);
                    data = fromClient.Receive(ref src);
                } catch (Exception) {
                    Logger.DebugError("read timeout occured on client");
                    continue;
                }

				// source address
                this.source = src;

                // process
                byte[] processedData = Process(ProxySource.Client, data);

                // send
                toServer.Send(data, data.Length, target);

                // connected
                connected = true;
            }
        }

        /// <summary>
        /// Processes the specified source.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="data">The data.</param>
        private byte[] Process(ProxySource source, byte[] data) {
            using (PacketStream newStream = new PacketStream()) {
                using (PacketStream oldStream = new PacketStream(data)) {
                    // read packet
                    Packet p = oldStream.ReadPacket();

                    // handle type
                    if (p.Type == PacketType.RPC)
                        p.Payload = new RPCPayload(p);
                    else if (p.Type == PacketType.BufferedRPC)
                        p.Payload = new BufferedRPCPayload(p);
                    else if (p.Type == PacketType.Link)
                        p.Payload = new LinkPayload(p);

                    // process
                    p.Process(new ProxyContext(source, this));

                    // write packet
                    newStream.WritePacket(p);
                }

                // return
                return newStream.ToArray();
            }
        }

        /*
        private Packet ProcessP(Packet packet) {
            // clone packet
            Packet rewritePacket = (Packet)packet.Clone();

            // setup
            using (MemoryStream newStream = new MemoryStream(rewritePacket.Payload, true)) {
                using (MemoryStream oldStream = new MemoryStream(packet.Payload)) {
                    // reader/writers
                    BinaryReader reader = new BinaryReader(oldStream);
                    BinaryWriter writer = new BinaryWriter(newStream);

                    // packet length type
                    byte lengthType = reader.ReadByte();

                    // network view
                    ushort objectId;

                    // length handlers
                    if (lengthType == 0x2E || lengthType == 0x2A) {
                        reader.ReadBytes(2);

                        // network view
                        objectId = reader.ReadUInt16();
                    } else if (lengthType == 0x7C || lengthType == 0x2C) {
                        objectId = reader.ReadUInt16();
                    } else {
                        Console.WriteLine("[rust] invalid packet length type: " + lengthType.ToString("X"));
                        File.WriteAllBytes("packets/" + lengthType.ToString("X") + "_" + i + ".dat", data);
                        i++;
                        goto handled;
                    }

                    // read string
                    string name = reader.ReadString();

                    if (name == "GetNetworkUpdate") {
                        reader.ReadBytes(5);

                        // get pos/angle
                        Vector3f pos = new Vector3f(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
                        //Vector3f angle = new Vector3f(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());

                        if (OnObjectUpdate != null)
                            OnObjectUpdate.Invoke(objectId, pos, new Vector3f());

                        // handled
                        goto handled;
                    }

                    if (name == "ReadClientMove") {
                        // unk
                        reader.ReadBytes(2);

                        // get pos/angle
                        Vector3f pos = new Vector3f(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
                        //Vector3f angle = new Vector3f(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());

                        if (OnPlayerUpdate != null)
                            OnPlayerUpdate.Invoke(objectId, pos, new Vector3f());

                        goto handled;
                    }

                    if (name == "ClientDeath") {
                        if (OnObjectDeath != null)
                            OnObjectDeath.Invoke(objectId);

                        goto handled;
                    }

                    if (name == "Snd") {
                        goto handled;
                    }

                    if (name == "CL_UpdateSkyState") {
                        // unk
                        reader.ReadBytes(30);

                        // time
                        ushort time = reader.ReadUInt16();
                        goto handled;
                    }

                    if (name == "RecieveNetwork") {
                        // unk
                        reader.ReadBytes(2);

                        float caloricLevel = reader.ReadSingle();
                        float waterLevelLitre = reader.ReadSingle();
                        float radiationLevel = reader.ReadSingle();
                        float antiRads = reader.ReadSingle();
                        float coreTemperature = reader.ReadSingle();
                        float poisonLevel = reader.ReadSingle();

                        if (OnMetabolismUpdate != null)
                            OnMetabolismUpdate.Invoke(caloricLevel);

                        goto handled;
                    }

                    File.WriteAllBytes("packets/" + name + "_" + i + ".dat", data);
                    i++;

                    Console.WriteLine("Received RPC: " + name);
                }

            handled:
                return newStream.ToArray();
            }
        }
        */

        /// <summary>
        /// Redirects server traffic to the client.
        /// </summary>
        private void ServerRedirector() {
            while (true) {
                // check if connected
                if (!connected)
                    continue;

                // receive data
                IPEndPoint remoteIp = null;
                byte[] data = toServer.Receive(ref remoteIp);

                // process
                byte[] processedData = Process(ProxySource.Server, data);

                // send
                fromClient.Send(data, data.Length, source);
            }
        }
        #endregion

        #region Constructors		        
        /// <summary>
        /// Initializes a new instance of the <see cref="Proxy"/> class.
        /// </summary>
        /// <param name="ip">The ip.</param>
        /// <param name="port">The port.</param>
        /// <param name="world">The world.</param>
        public Proxy(string ip, int port, World world) {
            // set world
            this.world = world;

            // create client
            fromClient = new UdpClient(port);

            // create server
            toServer = new UdpClient();

            // log
            Logger.Log("started on port " + port);

            // ip address
            IPAddress address = null;

            try {
                // parse ip first
                address = IPAddress.Parse(ip);
            } catch (Exception) {
                try {
                    // parse host next
                    IPHostEntry hostEntry;
                    hostEntry = Dns.GetHostEntry(ip);

                    address = hostEntry.AddressList[0];
                } catch(Exception) {
                    Logger.LogError("invalid host/ip: " + ip);
                    Environment.Exit(0);
                }
            }

            // log
            Logger.Debug("setup target " + address.ToString());

            // target
            target = new IPEndPoint(address, port);

            // title
            Console.Title = "Proxy " + address.ToString() + " on " + port;

            this.port = port;

			// client thread
            clientThread = new Thread(new ThreadStart(ClientRedirector));
            clientThread.Name = "Proxy_ClientThread";
            clientThread.IsBackground = true;
            clientThread.Start();

			// server thread
            serverThread = new Thread(new ThreadStart(ServerRedirector));
            serverThread.Name = "Proxy_ServerThread";
            serverThread.IsBackground = true;
            serverThread.Start();
        }
        #endregion
    }

    public enum ProxySource {
        Client,
        Server
    }
}

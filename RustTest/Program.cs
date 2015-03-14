using RustTest.Net;
using RustTest.Controls;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace RustTest
{
    class Program
    {
        #region Fields
        private static Proxy proxy;
        private static World world;
        private static Window window;
        #endregion

        #region Properties
        public static Proxy Proxy {
            get {
                return proxy;
            }
        }

        public static World World {
            get {
                return world;
            }
        }
        #endregion

        static void Main(string[] args) {
            try {
                // load configuration
                Config.Load("config");

                // log
                Logger.Log("config " + Config.Entities.Count + " entities loaded");
            } catch (Exception ex) {
                Logger.LogError(ex.Message);
                Console.ReadKey();
                return;
            }

            // create world
            world = new World();

            // create proxy
            proxy = new Proxy(Config.Host, Config.Port, world);

            // run
            window = new Window("Rust Proxy - " + Config.Port, 800, 600, world);
            window.Run(30);
        }
    }
}

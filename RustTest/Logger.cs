using System;

namespace RustTest
{
    public static class Logger
    {
        #region Fields
        private static bool debugging = true;
        #endregion

        #region Properties        
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Logger"/> is debugging.
        /// </summary>
        /// <value>
        ///   <c>true</c> if debugging; otherwise, <c>false</c>.
        /// </value>
        public static bool Debugging {
            get {
                return Logger.debugging;
            }
            set {
                Logger.debugging = value;
            }
        }
        #endregion

        #region Methods                
        /// <summary>
        /// Debugs the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        public static void Debug(string message) {
            ColorLog(ConsoleColor.White, "[rust:Global] " + message, true);
        }

        /// <summary>
        /// Debugs the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="message">The message.</param>
        public static void Debug(ProxyContext context, string message) {
            Debug(context.Source, message);
        }

        /// <summary>
        /// Debugs the specified source.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="message">The message.</param>
        public static void Debug(ProxySource source, string message) {
            ColorLog(ConsoleColor.White, "[rust:" + source.ToString() + "] " + message, true);
        }

        /// <summary>
        /// Debugs the specified data.
        /// </summary>
        /// <param name="data">The data.</param>
        public static void Debug(byte[] data) {
            string log = "";

            for (int i = 0; i < data.Length; i++) {
                log += data[i].ToString("X") + " ";
            }

            // debug
            Debug(log);
        }

        /// <summary>
        /// Debugs the error.
        /// </summary>
        /// <param name="message">The message.</param>
        public static void DebugError(string message) {
            ColorLog(ConsoleColor.Red, "[rust:Global] " + message, true);
        }

        /// <summary>
        /// Debugs the error.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="message">The message.</param>
        public static void DebugError(ProxyContext context, string message) {
            DebugError(context.Source, message);
        }

        /// <summary>
        /// Debugs the error.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="message">The message.</param>
        public static void DebugError(ProxySource source, string message) {
            ColorLog(ConsoleColor.Red, "[rust:" + source.ToString() + "] " + message, true);
        }

        /// <summary>
        /// Logs the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        public static void Log(string message) {
            ColorLog(ConsoleColor.White, "[rust:Global] " + message, false);
        }

        /// <summary>
        /// Logs the error.
        /// </summary>
        /// <param name="message">The message.</param>
        public static void LogError(string message) {
            ColorLog(ConsoleColor.Red, "[rust:Global] " + message, false);
        }

        /// <summary>
        /// Logs the specified color.
        /// </summary>
        /// <param name="color">The color.</param>
        /// <param name="message">The message.</param>
        /// <param name="debug">if set to <c>true</c> [debug].</param>
        private static void ColorLog(ConsoleColor color, string message, bool debug) {
            // debug only messages
            if (debug && !debugging) return;

            // keep old color
            ConsoleColor oldColor = Console.ForegroundColor;

            // set new color
            Console.ForegroundColor = color;

            // log
            Console.WriteLine(message);

            // reset color
            Console.ForegroundColor = oldColor;
        }
        #endregion
    }
}

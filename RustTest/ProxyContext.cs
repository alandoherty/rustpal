using System;

namespace RustTest
{
    public class ProxyContext
    {
        #region Fields
        private ProxySource source;
        private Proxy proxy;
        #endregion

        #region Properties        
        /// <summary>
        /// Gets the proxy source.
        /// </summary>
        /// <value>The proxy source.</value>
        public ProxySource Source {
            get {
                return this.source;
            }
        }

        /// <summary>
        /// Gets the proxy.
        /// </summary>
        /// <value>The proxy.</value>
        public Proxy Proxy {
            get {
                return this.proxy;
            }
        }

        public World World {
            get {
                return this.proxy.World;
            }
        }
        #endregion

        #region Constructors        
        /// <summary>
        /// Initializes a new instance of the <see cref="ProxyContext"/> class.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="proxy">The proxy.</param>
        public ProxyContext(ProxySource source, Proxy proxy) {
            this.source = source;
            this.proxy = proxy;
        }
        #endregion
    }
}

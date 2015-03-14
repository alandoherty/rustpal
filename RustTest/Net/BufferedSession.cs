using System;
using System.Collections.Generic;

namespace RustTest.Net
{
    public class BufferedSession
    {
        #region Fields
        private int size;
        private List<byte>[] blocks;
        #endregion

        #region Properties        
        /// <summary>
        /// Gets the size.
        /// </summary>
        /// <value>The size.</value>
        public int Size {
            get {
                return this.size;
            }
        }

        /// <summary>
        /// Gets the blocks.
        /// </summary>
        /// <value>The blocks.</value>
        public List<byte>[] Blocks {
            get {
                return this.blocks;
            }
        }

        /// <summary>
        /// Gets the count of blocks downloaded.
        /// </summary>
        /// <value>The count.</value>
        public int Count {
            get {
                for (int i = 0; i < this.blocks.Length; i++) {
                    if (this.blocks[i] == null) {
                        return i;
                    }
                }

                return this.Size;
            }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="BufferedSession"/> class.
        /// </summary>
        /// <param name="size">The size.</param>
        public BufferedSession(int size) {
            this.size = size;
            this.blocks = new List<byte>[size];
        }
        #endregion
    }
}

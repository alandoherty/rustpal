using RustTest.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace RustTest
{
    public partial class WindowOld : Form
    {
        #region Methods        
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="WindowOld"/> class.
        /// </summary>
        public WindowOld() {
            InitializeComponent();

            // initialize map
            this.rustMap1.World = Program.World;

            // anti-flicker
            SetStyle(ControlStyles.DoubleBuffer, true);
            SetStyle(ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.UserPaint, true);
        }
        #endregion

        #region Events
        private void btnClearMinimap_Click(object sender, EventArgs e) {
            lock (Program.World) {
                Program.World.Entities.Clear();
            }

            rustMap1.Refresh();
        }

        private void tmrMapRender_Tick(object sender, EventArgs e) {
            rustMap1.Refresh();

            // find all
            lock (Program.World) {
                listView1.Items.Clear();

                int i = 0;
                foreach (IEntity entity in Program.World.Entities) {
                    // clients
                    if (entity.Prefab == ":client") {
                        Client client = (Client)entity;

                        listView1.Items.Add(client.Name);
                        i++;
                    }
                }

                this.Text = "Rust proxy - " + Program.Proxy.Port + " - " + i + " players";
            }
        }

        private void trackBar1_Scroll(object sender, EventArgs e) {
            rustMap1.MapScale = (1f / trackBar1.Value) * 5;
            rustMap1.Refresh();
        }

        private void Window_FormClosed(object sender, FormClosedEventArgs e) {
            Environment.Exit(0);
        }
        #endregion
    }
}

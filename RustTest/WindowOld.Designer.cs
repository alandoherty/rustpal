namespace RustTest
{
    partial class WindowOld
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WindowOld));
            this.listView1 = new System.Windows.Forms.ListView();
            this.clmName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnClearMinimap = new System.Windows.Forms.ToolStripButton();
            this.tmrMapRender = new System.Windows.Forms.Timer(this.components);
            this.trackBar1 = new System.Windows.Forms.TrackBar();
            this.rustMap1 = new RustTest.RustMap();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
            this.SuspendLayout();
            // 
            // listView1
            // 
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.clmName});
            this.listView1.FullRowSelect = true;
            this.listView1.GridLines = true;
            this.listView1.Location = new System.Drawing.Point(8, 29);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(191, 400);
            this.listView1.TabIndex = 0;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            // 
            // clmName
            // 
            this.clmName.Text = "Name";
            this.clmName.Width = 109;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnClearMinimap});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(853, 25);
            this.toolStrip1.TabIndex = 2;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // btnClearMinimap
            // 
            this.btnClearMinimap.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnClearMinimap.Image = ((System.Drawing.Image)(resources.GetObject("btnClearMinimap.Image")));
            this.btnClearMinimap.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnClearMinimap.Name = "btnClearMinimap";
            this.btnClearMinimap.Size = new System.Drawing.Size(23, 22);
            this.btnClearMinimap.Text = "Clear";
            this.btnClearMinimap.Click += new System.EventHandler(this.btnClearMinimap_Click);
            // 
            // tmrMapRender
            // 
            this.tmrMapRender.Enabled = true;
            this.tmrMapRender.Interval = 2000;
            this.tmrMapRender.Tick += new System.EventHandler(this.tmrMapRender_Tick);
            // 
            // trackBar1
            // 
            this.trackBar1.Location = new System.Drawing.Point(711, 28);
            this.trackBar1.Maximum = 20;
            this.trackBar1.Minimum = 1;
            this.trackBar1.Name = "trackBar1";
            this.trackBar1.Size = new System.Drawing.Size(130, 45);
            this.trackBar1.TabIndex = 4;
            this.trackBar1.Value = 5;
            this.trackBar1.Scroll += new System.EventHandler(this.trackBar1_Scroll);
            // 
            // rustMap1
            // 
            this.rustMap1.Location = new System.Drawing.Point(205, 29);
            this.rustMap1.MapScale = 1F;
            this.rustMap1.Name = "rustMap1";
            this.rustMap1.Size = new System.Drawing.Size(500, 400);
            this.rustMap1.TabIndex = 3;
            this.rustMap1.World = null;
            // 
            // Window
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(853, 441);
            this.Controls.Add(this.trackBar1);
            this.Controls.Add(this.rustMap1);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.listView1);
            this.DoubleBuffered = true;
            this.Name = "Window";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Rust Proxy - b42";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Window_FormClosed);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader clmName;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private RustMap rustMap1;
        private System.Windows.Forms.ToolStripButton btnClearMinimap;
        private System.Windows.Forms.Timer tmrMapRender;
        private System.Windows.Forms.TrackBar trackBar1;
    }
}
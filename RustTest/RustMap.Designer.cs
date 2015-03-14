namespace RustTest
{
    partial class RustMap
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.SuspendLayout();
            // 
            // RustMap
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "RustMap";
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.RustMap_Paint);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.RustMap_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.RustMap_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.RustMap_MouseUp);
            this.ResumeLayout(false);

        }

        #endregion
    }
}

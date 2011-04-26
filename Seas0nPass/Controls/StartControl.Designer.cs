namespace Seas0nPass.Controls
{
    partial class StartControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StartControl));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.ipswPictureBox = new System.Windows.Forms.PictureBox();
            this.ipswContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ipswLabel = new System.Windows.Forms.Label();
            this.tetheredPictureBox = new System.Windows.Forms.PictureBox();
            this.tetherLabel = new System.Windows.Forms.Label();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ipswPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tetheredPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 5;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 43F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.ipswPictureBox, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.ipswLabel, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.tetheredPictureBox, 3, 1);
            this.tableLayoutPanel1.Controls.Add(this.tetherLabel, 3, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(433, 303);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // ipswPictureBox
            // 
            this.ipswPictureBox.BackColor = System.Drawing.Color.White;
            this.ipswPictureBox.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("ipswPictureBox.BackgroundImage")));
            this.ipswPictureBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.ipswPictureBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ipswPictureBox.ContextMenuStrip = this.ipswContextMenuStrip;
            this.ipswPictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ipswPictureBox.Location = new System.Drawing.Point(48, 79);
            this.ipswPictureBox.Name = "ipswPictureBox";
            this.ipswPictureBox.Size = new System.Drawing.Size(144, 144);
            this.ipswPictureBox.TabIndex = 0;
            this.ipswPictureBox.TabStop = false;
            this.ipswPictureBox.Click += new System.EventHandler(this.ipswPictureBox_Click);
            this.ipswPictureBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ipswPictureBox_MouseDown);
            this.ipswPictureBox.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ipswPictureBox_MouseUp);
            // 
            // ipswContextMenuStrip
            // 
            this.ipswContextMenuStrip.Name = "ipswContextMenuStrip";
            this.ipswContextMenuStrip.Size = new System.Drawing.Size(153, 26);
            // 
            // ipswLabel
            // 
            this.ipswLabel.AutoSize = true;
            this.ipswLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.ipswLabel.Location = new System.Drawing.Point(48, 226);
            this.ipswLabel.Name = "ipswLabel";
            this.ipswLabel.Size = new System.Drawing.Size(144, 13);
            this.ipswLabel.TabIndex = 1;
            this.ipswLabel.Text = "Create IPSW";
            this.ipswLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tetheredPictureBox
            // 
            this.tetheredPictureBox.BackColor = System.Drawing.Color.White;
            this.tetheredPictureBox.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("tetheredPictureBox.BackgroundImage")));
            this.tetheredPictureBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.tetheredPictureBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tetheredPictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tetheredPictureBox.ErrorImage = ((System.Drawing.Image)(resources.GetObject("tetheredPictureBox.ErrorImage")));
            this.tetheredPictureBox.InitialImage = ((System.Drawing.Image)(resources.GetObject("tetheredPictureBox.InitialImage")));
            this.tetheredPictureBox.Location = new System.Drawing.Point(241, 79);
            this.tetheredPictureBox.Name = "tetheredPictureBox";
            this.tetheredPictureBox.Size = new System.Drawing.Size(144, 144);
            this.tetheredPictureBox.TabIndex = 2;
            this.tetheredPictureBox.TabStop = false;
            this.tetheredPictureBox.Click += new System.EventHandler(this.tetheredPoctureBox_Click);
            this.tetheredPictureBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.tetheredPictureBox_MouseDown);
            this.tetheredPictureBox.MouseUp += new System.Windows.Forms.MouseEventHandler(this.tetheredPictureBox_MouseUp);
            // 
            // tetherLabel
            // 
            this.tetherLabel.AutoSize = true;
            this.tetherLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.tetherLabel.Enabled = false;
            this.tetherLabel.Location = new System.Drawing.Point(241, 226);
            this.tetherLabel.Name = "tetherLabel";
            this.tetherLabel.Size = new System.Drawing.Size(144, 13);
            this.tetherLabel.TabIndex = 3;
            this.tetherLabel.Text = "Boot Tethered";
            this.tetherLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // StartControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "StartControl";
            this.Size = new System.Drawing.Size(433, 303);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ipswPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tetheredPictureBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.PictureBox ipswPictureBox;
        private System.Windows.Forms.Label ipswLabel;
        private System.Windows.Forms.PictureBox tetheredPictureBox;
        private System.Windows.Forms.Label tetherLabel;
        private System.Windows.Forms.ContextMenuStrip ipswContextMenuStrip;
    }
}

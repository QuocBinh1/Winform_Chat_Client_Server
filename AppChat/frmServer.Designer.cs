namespace AppChat
{
    partial class frmServer
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmServer));
            this.btnCreateSocketServer = new System.Windows.Forms.Button();
            this.btnSendServer = new System.Windows.Forms.Button();
            this.txtSend = new System.Windows.Forms.TextBox();
            this.vbserver = new System.Windows.Forms.RichTextBox();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnEmoij = new System.Windows.Forms.ToolStripButton();
            this.btnDeleteMessage = new System.Windows.Forms.ToolStripButton();
            this.btnSendImage = new System.Windows.Forms.ToolStripButton();
            this.btnSendFile = new System.Windows.Forms.ToolStripButton();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCreateSocketServer
            // 
            this.btnCreateSocketServer.BackColor = System.Drawing.Color.Red;
            this.btnCreateSocketServer.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCreateSocketServer.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnCreateSocketServer.Location = new System.Drawing.Point(539, 0);
            this.btnCreateSocketServer.Name = "btnCreateSocketServer";
            this.btnCreateSocketServer.Size = new System.Drawing.Size(158, 44);
            this.btnCreateSocketServer.TabIndex = 1;
            this.btnCreateSocketServer.Text = "Tạo";
            this.btnCreateSocketServer.UseVisualStyleBackColor = false;
            this.btnCreateSocketServer.Click += new System.EventHandler(this.btnCreateSocketServer_Click);
            // 
            // btnSendServer
            // 
            this.btnSendServer.Image = ((System.Drawing.Image)(resources.GetObject("btnSendServer.Image")));
            this.btnSendServer.Location = new System.Drawing.Point(649, 370);
            this.btnSendServer.Name = "btnSendServer";
            this.btnSendServer.Size = new System.Drawing.Size(48, 48);
            this.btnSendServer.TabIndex = 3;
            this.btnSendServer.UseVisualStyleBackColor = true;
            this.btnSendServer.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // txtSend
            // 
            this.txtSend.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSend.Location = new System.Drawing.Point(162, 370);
            this.txtSend.Multiline = true;
            this.txtSend.Name = "txtSend";
            this.txtSend.Size = new System.Drawing.Size(478, 48);
            this.txtSend.TabIndex = 4;
            // 
            // vbserver
            // 
            this.vbserver.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.vbserver.Location = new System.Drawing.Point(162, 44);
            this.vbserver.Name = "vbserver";
            this.vbserver.Size = new System.Drawing.Size(535, 293);
            this.vbserver.TabIndex = 5;
            this.vbserver.Text = "";
            // 
            // toolStrip1
            // 
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip1.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnEmoij,
            this.btnDeleteMessage,
            this.btnSendImage,
            this.btnSendFile});
            this.toolStrip1.Location = new System.Drawing.Point(162, 340);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(129, 27);
            this.toolStrip1.TabIndex = 7;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // btnEmoij
            // 
            this.btnEmoij.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnEmoij.Image = ((System.Drawing.Image)(resources.GetObject("btnEmoij.Image")));
            this.btnEmoij.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnEmoij.Name = "btnEmoij";
            this.btnEmoij.Size = new System.Drawing.Size(29, 24);
            this.btnEmoij.Text = "toolStripButton3";
            this.btnEmoij.Click += new System.EventHandler(this.btnEmoij_Click);
            // 
            // btnDeleteMessage
            // 
            this.btnDeleteMessage.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnDeleteMessage.Image = ((System.Drawing.Image)(resources.GetObject("btnDeleteMessage.Image")));
            this.btnDeleteMessage.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnDeleteMessage.Name = "btnDeleteMessage";
            this.btnDeleteMessage.Size = new System.Drawing.Size(29, 24);
            this.btnDeleteMessage.Text = "toolStripButton1";
            this.btnDeleteMessage.Click += new System.EventHandler(this.btnDeleteMessage_Click_1);
            // 
            // btnSendImage
            // 
            this.btnSendImage.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnSendImage.Image = ((System.Drawing.Image)(resources.GetObject("btnSendImage.Image")));
            this.btnSendImage.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSendImage.Name = "btnSendImage";
            this.btnSendImage.Size = new System.Drawing.Size(29, 24);
            this.btnSendImage.Text = "toolStripButton2";
            this.btnSendImage.Click += new System.EventHandler(this.btnSendImage_Click);
            // 
            // btnSendFile
            // 
            this.btnSendFile.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnSendFile.Image = ((System.Drawing.Image)(resources.GetObject("btnSendFile.Image")));
            this.btnSendFile.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSendFile.Name = "btnSendFile";
            this.btnSendFile.Size = new System.Drawing.Size(29, 24);
            this.btnSendFile.Text = "toolStripButton1";
            this.btnSendFile.Click += new System.EventHandler(this.btnSendFile_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 19.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(162, 3);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(120, 38);
            this.label2.TabIndex = 9;
            this.label2.Text = "Server";
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.groupBox1.Location = new System.Drawing.Point(9, 44);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(147, 371);
            this.groupBox1.TabIndex = 12;
            this.groupBox1.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(141, 16);
            this.label1.TabIndex = 13;
            this.label1.Text = "Chọn người để nhắn tin";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(299, 11);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(192, 29);
            this.label3.TabIndex = 14;
            this.label3.Text = "Địa Chỉ IP server";
            // 
            // frmServer
            // 
            this.AcceptButton = this.btnSendServer;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.ClientSize = new System.Drawing.Size(705, 419);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.vbserver);
            this.Controls.Add(this.txtSend);
            this.Controls.Add(this.btnSendServer);
            this.Controls.Add(this.btnCreateSocketServer);
            this.Name = "frmServer";
            this.Text = "Chat Server";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnCreateSocketServer;
        private System.Windows.Forms.Button btnSendServer;
        private System.Windows.Forms.TextBox txtSend;
        private System.Windows.Forms.RichTextBox vbserver;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton btnSendFile;
        private System.Windows.Forms.ToolStripButton btnSendImage;
        private System.Windows.Forms.ToolStripButton btnEmoij;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ToolStripButton btnDeleteMessage;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
    }
}


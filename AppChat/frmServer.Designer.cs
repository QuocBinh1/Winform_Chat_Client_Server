﻿namespace AppChat
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
            this.btnSendFile = new System.Windows.Forms.ToolStripButton();
            this.btnSendImage = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton3 = new System.Windows.Forms.ToolStripButton();
            this.label2 = new System.Windows.Forms.Label();
            this.lstClients = new System.Windows.Forms.ListBox();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCreateSocketServer
            // 
            this.btnCreateSocketServer.BackColor = System.Drawing.Color.Red;
            this.btnCreateSocketServer.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCreateSocketServer.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnCreateSocketServer.Location = new System.Drawing.Point(424, 10);
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
            this.btnSendServer.Location = new System.Drawing.Point(537, 384);
            this.btnSendServer.Name = "btnSendServer";
            this.btnSendServer.Size = new System.Drawing.Size(48, 48);
            this.btnSendServer.TabIndex = 3;
            this.btnSendServer.UseVisualStyleBackColor = true;
            this.btnSendServer.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // txtSend
            // 
            this.txtSend.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSend.Location = new System.Drawing.Point(12, 384);
            this.txtSend.Multiline = true;
            this.txtSend.Name = "txtSend";
            this.txtSend.Size = new System.Drawing.Size(519, 48);
            this.txtSend.TabIndex = 4;
            // 
            // vbserver
            // 
            this.vbserver.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.vbserver.Location = new System.Drawing.Point(12, 52);
            this.vbserver.Name = "vbserver";
            this.vbserver.Size = new System.Drawing.Size(573, 301);
            this.vbserver.TabIndex = 5;
            this.vbserver.Text = "";
            // 
            // toolStrip1
            // 
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnSendFile,
            this.btnSendImage,
            this.toolStripButton3});
            this.toolStrip1.Location = new System.Drawing.Point(12, 356);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(100, 27);
            this.toolStrip1.TabIndex = 7;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // btnSendFile
            // 
            this.btnSendFile.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnSendFile.Image = ((System.Drawing.Image)(resources.GetObject("btnSendFile.Image")));
            this.btnSendFile.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSendFile.Name = "btnSendFile";
            this.btnSendFile.Size = new System.Drawing.Size(29, 24);
            this.btnSendFile.Text = "toolStripButton1";
            // 
            // btnSendImage
            // 
            this.btnSendImage.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnSendImage.Image = ((System.Drawing.Image)(resources.GetObject("btnSendImage.Image")));
            this.btnSendImage.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSendImage.Name = "btnSendImage";
            this.btnSendImage.Size = new System.Drawing.Size(29, 24);
            this.btnSendImage.Text = "toolStripButton2";
            // 
            // toolStripButton3
            // 
            this.toolStripButton3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton3.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton3.Image")));
            this.toolStripButton3.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton3.Name = "toolStripButton3";
            this.toolStripButton3.Size = new System.Drawing.Size(29, 24);
            this.toolStripButton3.Text = "toolStripButton3";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 19.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(12, 10);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(120, 38);
            this.label2.TabIndex = 9;
            this.label2.Text = "Server";
            // 
            // lstClients
            // 
            this.lstClients.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lstClients.FormattingEnabled = true;
            this.lstClients.ItemHeight = 22;
            this.lstClients.Location = new System.Drawing.Point(160, 6);
            this.lstClients.Name = "lstClients";
            this.lstClients.Size = new System.Drawing.Size(238, 48);
            this.lstClients.TabIndex = 10;
            // 
            // frmServer
            // 
            this.AcceptButton = this.btnSendServer;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.ClientSize = new System.Drawing.Size(594, 444);
            this.Controls.Add(this.lstClients);
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
        private System.Windows.Forms.ToolStripButton toolStripButton3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ListBox lstClients;
    }
}


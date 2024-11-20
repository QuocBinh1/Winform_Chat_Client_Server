namespace ChatClient
{
    partial class frmClient
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmClient));
            this.btnConnectClient = new System.Windows.Forms.Button();
            this.btnSend = new System.Windows.Forms.Button();
            this.txtSend = new System.Windows.Forms.TextBox();
            this.vbclient = new System.Windows.Forms.RichTextBox();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnSendFile = new System.Windows.Forms.ToolStripButton();
            this.btnImg = new System.Windows.Forms.ToolStripButton();
            this.btnEmoij = new System.Windows.Forms.ToolStripButton();
            this.lblClientID = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnDeleteMessage = new System.Windows.Forms.Button();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnConnectClient
            // 
            this.btnConnectClient.BackColor = System.Drawing.Color.Red;
            this.btnConnectClient.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnConnectClient.Location = new System.Drawing.Point(457, 6);
            this.btnConnectClient.Name = "btnConnectClient";
            this.btnConnectClient.Size = new System.Drawing.Size(112, 37);
            this.btnConnectClient.TabIndex = 1;
            this.btnConnectClient.Text = "Kết Nối";
            this.btnConnectClient.UseVisualStyleBackColor = false;
            this.btnConnectClient.Click += new System.EventHandler(this.btnConnectClient_Click);
            // 
            // btnSend
            // 
            this.btnSend.Image = ((System.Drawing.Image)(resources.GetObject("btnSend.Image")));
            this.btnSend.Location = new System.Drawing.Point(511, 391);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(53, 41);
            this.btnSend.TabIndex = 2;
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // txtSend
            // 
            this.txtSend.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSend.Location = new System.Drawing.Point(12, 391);
            this.txtSend.Multiline = true;
            this.txtSend.Name = "txtSend";
            this.txtSend.Size = new System.Drawing.Size(493, 41);
            this.txtSend.TabIndex = 3;
            // 
            // vbclient
            // 
            this.vbclient.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.vbclient.Location = new System.Drawing.Point(12, 49);
            this.vbclient.Name = "vbclient";
            this.vbclient.ReadOnly = true;
            this.vbclient.Size = new System.Drawing.Size(552, 309);
            this.vbclient.TabIndex = 4;
            this.vbclient.Text = "";
            // 
            // toolStrip1
            // 
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnSendFile,
            this.btnImg,
            this.btnEmoij});
            this.toolStrip1.Location = new System.Drawing.Point(12, 361);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(100, 27);
            this.toolStrip1.TabIndex = 8;
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
            // btnImg
            // 
            this.btnImg.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnImg.Image = ((System.Drawing.Image)(resources.GetObject("btnImg.Image")));
            this.btnImg.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnImg.Name = "btnImg";
            this.btnImg.Size = new System.Drawing.Size(29, 24);
            this.btnImg.Text = "toolStripButton2";
            this.btnImg.Click += new System.EventHandler(this.btnImg_Click);
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
            // lblClientID
            // 
            this.lblClientID.AutoSize = true;
            this.lblClientID.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblClientID.Location = new System.Drawing.Point(196, 9);
            this.lblClientID.Name = "lblClientID";
            this.lblClientID.Size = new System.Drawing.Size(119, 29);
            this.lblClientID.TabIndex = 10;
            this.lblClientID.Text = "Địa Chỉ IP";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 19.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(12, 6);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(106, 38);
            this.label2.TabIndex = 11;
            this.label2.Text = "Client";
            // 
            // btnDeleteMessage
            // 
            this.btnDeleteMessage.Location = new System.Drawing.Point(115, 357);
            this.btnDeleteMessage.Name = "btnDeleteMessage";
            this.btnDeleteMessage.Size = new System.Drawing.Size(96, 31);
            this.btnDeleteMessage.TabIndex = 13;
            this.btnDeleteMessage.Text = "Xóa Tin Nhắn";
            this.btnDeleteMessage.UseVisualStyleBackColor = true;
            this.btnDeleteMessage.Click += new System.EventHandler(this.btnDeleteMessage_Click);
            // 
            // frmClient
            // 
            this.AcceptButton = this.btnSend;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.ClientSize = new System.Drawing.Size(581, 441);
            this.Controls.Add(this.btnDeleteMessage);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblClientID);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.vbclient);
            this.Controls.Add(this.txtSend);
            this.Controls.Add(this.btnSend);
            this.Controls.Add(this.btnConnectClient);
            this.Name = "frmClient";
            this.Text = "Chat Client";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnConnectClient;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.TextBox txtSend;
        private System.Windows.Forms.RichTextBox vbclient;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton btnSendFile;
        private System.Windows.Forms.ToolStripButton btnImg;
        private System.Windows.Forms.ToolStripButton btnEmoij;
        private System.Windows.Forms.Label lblClientID;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnDeleteMessage;
    }
}


//frmClient.cs
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Threading;
namespace ChatClient
{
    public partial class frmClient : Form
    {
        private Socket socketclient;
        private IPAddress ia;
        private IPEndPoint iep;
        private int port = 9999;
        private Thread receivingThread;
        public frmClient()
        {
            InitializeComponent();
        }
        private void btnConnectClient_Click(object sender, EventArgs e)
        {
            try
            {
                socketclient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                ia = IPAddress.Parse("127.0.0.1");
                iep = new IPEndPoint(ia, port);
                //socketclient.Connect(iep);
                AppendMessage("Đang kết nối tới server.", true);



                var result = socketclient.BeginConnect(iep, null, null);
                bool success = result.AsyncWaitHandle.WaitOne(100); 
                if (!success){
                    throw new SocketException((int)SocketError.TimedOut);
                }
                socketclient.EndConnect(result); 


                receivingThread = new Thread(ReceiveData);
                receivingThread.IsBackground = true;
                receivingThread.Start();
                ClearMessages();
                AppendMessage("Kết Nối Thành Công", true);
                lblClientID.Text = socketclient.LocalEndPoint.ToString();
            }
            catch (SocketException ex)
            {
                MessageBox.Show("Lỗi : Server chưa tạo socket \n  " + ex.Message, "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void ReceiveData()
        {
            while (true)
            {
                try
                {
                    byte[] buffer = new byte[1024];
                    int receivedBytes = socketclient.Receive(buffer);
                    string text = Encoding.UTF8.GetString(buffer, 0, receivedBytes);
                    if (text.StartsWith("[delete]"))
                    {
                        DeleteMessage(text.Substring(8));
                    }
                    else
                    {
                        AppendMessage(text, false);
                    }
                }
                catch (SocketException ex)
                {
                    AppendMessage("Connection lost: " + ex.Message, false);
                    break;
                }
            }
        }
        private void btnSend_Click(object sender, EventArgs e)
        {
            try
            {
                // Kiểm tra nếu socketclient chưa được khởi tạo hoặc không kết nối
                if (socketclient == null || !socketclient.Connected)
                {
                    MessageBox.Show("Vui lòng kết nối tới máy chủ trước khi gửi tin nhắn.",
                                    "Chưa kết nối",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Warning);
                    return;
                }
                string currentTime = DateTime.Now.ToString("HH:mm:ss");
                string message = txtSend.Text;
                string fullMessage = $"[{currentTime}-{socketclient.LocalEndPoint}]: {message}";
                byte[] data = Encoding.UTF8.GetBytes(fullMessage);
                socketclient.Send(data);

                AppendMessage($"[{currentTime}-Bạn]: {message}", true);
                txtSend.Clear();
            }
            catch (SocketException ex)
            {
                AppendMessage("Failed to send message: " + ex.Message, true);
            }
        }
        private void AppendMessage(string message, bool isClient)
        {
            vbclient.Invoke((MethodInvoker)delegate
            {
                vbclient.SelectionAlignment = isClient ? HorizontalAlignment.Right : HorizontalAlignment.Left;
                vbclient.SelectionColor = isClient ? Color.Blue : Color.Green;
                vbclient.AppendText(message + "\n");
                vbclient.SelectionColor = vbclient.ForeColor; // Reset color
            });
        }
        private void ClearMessages()
        {
            vbclient.Invoke((MethodInvoker)delegate
            {
                vbclient.Clear();
            });
        }
        private void frmClient_FormClosing(object sender, FormClosingEventArgs e)
        {
            receivingThread?.Abort();
            socketclient?.Close();
        }
        private void DeleteMessage(string message)
        {
            vbclient.Invoke((MethodInvoker)delegate
            {
                int index = vbclient.Text.IndexOf(message);
                if (index >= 0)
                {
                    vbclient.Select(index, message.Length);
                    vbclient.SelectedText = string.Empty;
                }
            });
        }
        private void btnDeleteMessage_Click(object sender, EventArgs e)
        {
            if (vbclient.SelectedText.Length > 0)
            {
                vbclient.ReadOnly = false;
                string selectedText = vbclient.SelectedText;
                vbclient.SelectedText = string.Empty;

                // Gửi lệnh xóa tới server
                byte[] data = Encoding.UTF8.GetBytes("[delete]" + selectedText);
                socketclient.Send(data);
                vbclient.ReadOnly = true;
            }
            else
            {
                MessageBox.Show("Vui lòng bôi đen tin nhắn cần xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnEmoij_Click(object sender, EventArgs e)
        {
           
            FlowLayoutPanel emojiPanel = new FlowLayoutPanel();
            emojiPanel.FlowDirection = FlowDirection.LeftToRight;
            emojiPanel.WrapContents = true;
            emojiPanel.AutoScroll = true;
            emojiPanel.Size = new Size(300, 200);
            emojiPanel.Location = new Point(70, 150);
            // Danh sách các emoji
            string[] emojis = new string[]{
                "\U0001F600", "\U0001F609", "\U0001F44D", "\U0001F622", "\U0001F60E",
                "\U0001F618", "\U0001F60D", "\U0001F61E", "\U0001F636", "\U0001F910",
                "\U0001F496", "\U0001F319", "\U0001F525", "\U0001F4A9", "\U0001F4A3",
                "\U0001F4A8", "\U0001F4A6", "\U0001F4A5", "\U0001F4AB", "\U0001F4AC",
                "\U0001F4AD", "\U0001F6C0", "\U0001F6C1", "\U0001F6C2", "\U0001F6C3",
                "\U0001F6C4", "\U0001F6C5", "\U0001F6CB", "\U0001F6CC", "\U0001F6CD",
            };
            // Tạo nút cho từng emoji
            foreach (var emoji in emojis)
            {
                Button emojiButton = new Button();
                emojiButton.Text = emoji;
                emojiButton.Font = new Font("Segoe UI Emoji", 20); // Sử dụng font hỗ trợ emoji
                emojiButton.Click += (s, args) =>
                {
                    txtSend.SelectedText = emoji;   // Chèn emoji vào RichTextBox
                    emojiPanel.Hide();    // Ẩn bảng emoji sau khi chọn emoji
                };
                emojiButton.Size = new Size(50, 50); // Kích thước button chứa emoji
                emojiPanel.Controls.Add(emojiButton);
            }
            // Hiển thị panel chứa emoji
            this.Controls.Add(emojiPanel);
            // Đảm bảo bảng emoji hiển thị trên cùng các điều khiển khác
            emojiPanel.BringToFront();
        }

        private void btnImg_Click(object sender, EventArgs e)
        {

        }
    }
}

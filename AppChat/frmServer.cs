//frmServer
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

namespace AppChat
{
    public partial class frmServer : Form
    {
        private Socket socketserver;
        private IPEndPoint iep;
        private IPAddress ia;
        private int port = 9999;
        private Thread listeningThread;
        private Dictionary<Socket, string> clientSockets = new Dictionary<Socket, string>();



        public frmServer()
        {
            InitializeComponent();
        }

        private void AppendMessage(string message, bool isServer)
        {
            vbserver.Invoke((MethodInvoker)delegate
            {
                vbserver.SelectionAlignment = isServer ? HorizontalAlignment.Right : HorizontalAlignment.Left;
                vbserver.SelectionColor = isServer ? Color.Blue : Color.Green;
                vbserver.AppendText(message + "\n");
                vbserver.SelectionColor = vbserver.ForeColor;
            });
        }

        private void ReceiveData(object obj)
        {
            Socket clientSocket = (Socket)obj;
            byte[] buffer = new byte[1024];
            int receivedBytes;
            try
            {
                while ((receivedBytes = clientSocket.Receive(buffer)) > 0)
                {
                    string text = Encoding.UTF8.GetString(buffer, 0, receivedBytes);
                    if (text.StartsWith("[delete]"))
                    {
                        DeleteMessage(text.Substring(8));
                    }
                    else
                    {
                        AppendMessage(text, false);
                        BroadcastMessage(text, clientSocket);
                    }
                }
            }
            catch (SocketException ex)
            {
                AppendMessage("Client đã ngắt kết nối: " + ex.Message, false);
            }
            finally
            {
                clientSockets.Remove(clientSocket);
                clientSocket.Close();
                UpdateClientList();
            }
        }

        private void BroadcastMessage(string message, Socket excludeSocket)
        {
            byte[] data = Encoding.UTF8.GetBytes(message);
            foreach (var client in clientSockets.Keys)
            {
                if (client != excludeSocket)
                {
                    client.Send(data);
                }
            }
        }

        private void SendMessageToClient(string message, string clientIP)
        {
            byte[] data = Encoding.UTF8.GetBytes(message);
            foreach (var client in clientSockets)
            {
                if (client.Value == clientIP)
                {
                    client.Key.Send(data);
                    break;
                }
            }
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            string message = txtSend.Text;
            string currentTime = DateTime.Now.ToString("HH:mm:ss");

            string fullMessage = $"[{currentTime}-Server]: {message}";

            byte[] data = Encoding.UTF8.GetBytes(fullMessage);

            if (lstClients.SelectedItem != null)
            {
                string selectedClientIP = lstClients.SelectedItem.ToString();
                SendMessageToClient(fullMessage, selectedClientIP);
            }
            else
            {
                foreach (var client in clientSockets.Keys)
                {
                    client.Send(data);
                }
            }

            AppendMessage(fullMessage, true);
            txtSend.Clear();
        }

        private void btnCreateSocketServer_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn có muốn tạo socket không?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                try
                {
                    socketserver = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    ia = IPAddress.Parse("127.0.0.1");
                    iep = new IPEndPoint(ia, port);
                    socketserver.Bind(iep);
                    socketserver.Listen(50);
                    MessageBox.Show("Đã tạo socket thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    listeningThread = new Thread(ListenForClients);
                    listeningThread.IsBackground = true;
                    listeningThread.Start();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi tạo socket: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Hủy tạo socket", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void ListenForClients()
        {
            while (true)
            {
                try
                {
                    Socket tempClient = socketserver.Accept();
                    string clientInfo = tempClient.RemoteEndPoint.ToString();

                    this.Invoke((MethodInvoker)delegate
                    {
                        clientSockets.Add(tempClient, clientInfo);
                        MessageBox.Show("Đã chấp nhận kết nối từ " + clientInfo, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        Thread receiveThread = new Thread(ReceiveData);
                        receiveThread.IsBackground = true;
                        receiveThread.Start(tempClient);

                        UpdateClientList();
                    });
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi lắng nghe kết nối: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                }
            }
        }


        private void frmServer_FormClosing(object sender, FormClosingEventArgs e)
        {
            listeningThread?.Abort();
            foreach (var client in clientSockets.Keys)
            {
                client.Close();
            }
            socketserver?.Close();
        }

        private void UpdateClientList()
        {
            lstClients.Invoke((MethodInvoker)delegate
            {
                lstClients.Items.Clear();
                foreach (var client in clientSockets.Values)
                {
                    lstClients.Items.Add(client);
                }
            });
        }

        private void DeleteMessage(string message)
        {
            vbserver.Invoke((MethodInvoker)delegate
            {
                int index = vbserver.Text.IndexOf(message);
                if (index >= 0)
                {
                    vbserver.Select(index, message.Length);
                    vbserver.SelectedText = string.Empty;
                }
            });

            // Gửi lệnh xóa tin nhắn tới tất cả các client
            BroadcastMessage("[delete]" + message, null);
        }


        private void btnDeleteMessage_Click(object sender, EventArgs e)
        {
            if (vbserver.SelectedText.Length > 0)
            {
                vbserver.ReadOnly = false;
                string selectedText = vbserver.SelectedText;
                vbserver.SelectedText = string.Empty;

                // Gửi tin nhắn xóa tới các client khác
                BroadcastMessage("[delete]" + selectedText, null);
                vbserver.ReadOnly = true;
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
            emojiPanel.Location = new Point(70,150);
            // Danh sách các emoji
            string[] emojis = new string[]
            {
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


    }
}

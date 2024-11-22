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
using System.IO;
using System.Runtime.Remoting.Messaging;
using System.Runtime.InteropServices;

namespace ChatClient
{
    public partial class frmClient : Form
    {
        private Socket client , server;
        private IPAddress ia;
        private IPEndPoint iep;
        private int port = 9999;
        private Thread receivingThread;
        private string currentTime = DateTime.Now.ToString("HH:mm");
        private List<string> messages = new List<string>();

        public frmClient()
        {
            InitializeComponent();
        }
        private void btnConnectClient_Click(object sender, EventArgs e)
        {
            try
            {
                client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                ia = IPAddress.Parse("127.0.0.1");
                iep = new IPEndPoint(ia, port);
                //socketclient.Connect(iep);
                AppendMessage("Đang kết nối tới server.", true);
                var result = client.BeginConnect(iep, null, null);
                bool success = result.AsyncWaitHandle.WaitOne(2000); 
                if (!success){
                    throw new SocketException((int)SocketError.TimedOut);
                }
                client.EndConnect(result); 
                receivingThread = new Thread(ReceiveData);
                receivingThread.IsBackground = true;
                receivingThread.Start();
                ClearMessages();
                AppendMessage("Kết Nối Thành Công", true);
                lblClientID.Text = client.LocalEndPoint.ToString();
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
                    if (!client.Connected)
                    {
                        AppendMessage("Server đã ngắt kết nối.", false);
                        break;
                    }

                    byte[] data = new byte[1024 * 1024];
                    int receivedBytes = client.Receive(data);

                    // Nếu không có dữ liệu, ngắt kết nối
                    if (receivedBytes == 0)
                    {
                        AppendMessage("Server đã ngắt kết nối.", false);
                        break;
                    }

                    string message = Encoding.UTF8.GetString(data, 0, receivedBytes);
                    ProcessMessage(message);
                }
                catch (SocketException ex)
                {
                    AppendMessage("Lỗi nhận dữ liệu: " + ex.Message, false);
                    break;
                }
                catch (Exception ex)
                {
                    AppendMessage("Lỗi không xác định: " + ex.Message, false);
                    break;
                }
            }
        }
        private void ProcessMessage(string message)
        {
            if (message.StartsWith("IMAGE:"))
            {
                string[] parts = message.Split(':');
                string fileName = parts[1];
                int fileSize = int.Parse(parts[2]);
                // Nhận dữ liệu ảnh
                byte[] imageData = new byte[fileSize];
                int totalReceived = 0;
                while (totalReceived < fileSize)
                {
                    int bytesReceived = client.Receive(imageData, totalReceived, fileSize - totalReceived, SocketFlags.None);
                    totalReceived += bytesReceived;
                }
                // Hiển thị ảnh trên client
                if (totalReceived == fileSize)
                {
                    AppendMessage($"[{currentTime}-Server]{fileName}", false);
                    ShowImageInRichTextBox(imageData, HorizontalAlignment.Left);
                }
                else
                {
                    AppendMessage($"Failed to receive full image: {fileName}", false);
                }
            }
            else if (message.StartsWith("FILE:"))
            {
                string[] parts = message.Split(':');
                string fileName = parts[1];
                int fileSize = int.Parse(parts[2]);

                byte[] fileData = new byte[fileSize];
                int totalReceived = 0;
                while (totalReceived < fileSize)
                {
                    int bytesReceived = client.Receive(fileData, totalReceived, fileSize - totalReceived, SocketFlags.None);
                    totalReceived += bytesReceived;
                }

                // Lưu tệp vào thư mục Downloads
                string downloadPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads", fileName);
                File.WriteAllBytes(downloadPath, fileData);
                AppendMessage($"Tệp {fileName} đã được tải về: {downloadPath}", false);
            }

            else
            {
                // Xử lý tin nhắn văn bản
                AppendMessage(message, false);
            }
        }
        private void btnSend_Click(object sender, EventArgs e)
        {
            try
            {
                if (client == null || !client.Connected)
                {
                    MessageBox.Show("Vui lòng kết nối tới máy chủ trước khi gửi tin nhắn.",
                                    "Chưa kết nối",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Warning);
                    return;
                }

                string message = txtSend.Text;
                if (string.IsNullOrWhiteSpace(message))
                {
                    MessageBox.Show("Vui lòng nhập tin nhắn trước khi gửi.",
                                    "Thông báo",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Warning);
                    return;
                }

                string fullMessage = $"[{currentTime}-{client.LocalEndPoint}]: {message}";
                byte[] data = Encoding.UTF8.GetBytes(fullMessage);

                client.Send(data);
                AppendMessage($"[{currentTime}-Bạn]: {message}", true);
                txtSend.Clear();
            }
            catch (SocketException ex)
            {
                AppendMessage("Không thể gửi tin nhắn: " + ex.Message, true);
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
            vbclient.ReadOnly = false;
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";
                openFileDialog.Title = "Select an Image";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = openFileDialog.FileName;
                    byte[] fileData = File.ReadAllBytes(filePath);
                    string fileName = Path.GetFileName(filePath);

                    // Tạo header chứa thông tin ảnh
                    string messageHeader = $"IMAGE:{fileName}:{fileData.Length}";
                    byte[] headerData = Encoding.UTF8.GetBytes(messageHeader);
                    Thread.Sleep(500); // Đảm bảo header được gửi trước

                    client.Send(headerData);
                    client.Send(fileData);

                    AppendMessage($"CLIENT: {fileName}", true);
                    ShowImageInRichTextBox(fileData, HorizontalAlignment.Right);
                }
            }
            //vbclient.ReadOnly = true;
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
                client.Send(data);
                vbclient.ReadOnly = true;
            }
            else
            {
                MessageBox.Show("Vui lòng bôi đen tin nhắn cần xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void ShowImageInRichTextBox(byte[] imageData, HorizontalAlignment alignment)
        {
            this.Invoke((MethodInvoker)delegate
            {
                using (MemoryStream ms = new MemoryStream(imageData))
                {
                    Image img = null;

                    try
                    {
                        img = Image.FromStream(ms);
                        // Đảm bảo Clipboard thao tác an toàn
                        bool clipboardSuccess = false;
                        int attempts = 3; // Thử tối đa 3 lần

                        while (!clipboardSuccess && attempts > 0)
                        {
                            try
                            {
                                Clipboard.SetImage(img);
                                clipboardSuccess = true;
                            }
                            catch (ExternalException)
                            {
                                attempts--;
                                Thread.Sleep(100); // Chờ trước khi thử lại
                            }
                            finally
                            {
                                img?.Dispose(); // Giải phóng bộ nhớ
                            }
                        }
                        if (!clipboardSuccess)
                        {
                            MessageBox.Show("Không thể thao tác với Clipboard. Vui lòng thử lại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        // Dán ảnh vào RichTextBox
                        vbclient.Select(vbclient.TextLength, 0);
                        vbclient.SelectionAlignment = alignment;
                        vbclient.Paste(); // Dán ảnh vào RichTextBox
                        vbclient.AppendText("\n");
                        vbclient.SelectionAlignment = HorizontalAlignment.Left;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Lỗi khi hiển thị hình ảnh: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    }
                    finally
                    {
                        img?.Dispose(); // Giải phóng bộ nhớ
                    }
                }
            });
        }


        private void btnSendFile_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Title = "Select a File to Send";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = openFileDialog.FileName;
                    byte[] fileData = File.ReadAllBytes(filePath);
                    string fileName = Path.GetFileName(filePath);

                    // Gửi header chứa thông tin tệp
                    string header = $"FILE:{fileName}:{fileData.Length}";
                    byte[] headerData = Encoding.UTF8.GetBytes(header);
                    client.Send(headerData);
                    Thread.Sleep(100); // Đảm bảo header được gửi trước

                    // Gửi dữ liệu tệp
                    client.Send(fileData);

                    AppendMessage($"{currentTime}-[Bạn đã gửi tệp]: {fileName}", true);
                }
            }
        }


        private void frmClient_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (receivingThread != null && receivingThread.IsAlive)
            {
                try
                {
                    receivingThread.Abort();
                }
                catch (ThreadAbortException) { }
            }
            client?.Close();
        }

    }
}

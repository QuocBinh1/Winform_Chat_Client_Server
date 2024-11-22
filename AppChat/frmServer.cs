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
using System.Reflection.Emit;
using System.IO;
using System.Runtime.InteropServices;

namespace AppChat
{
    public partial class frmServer : Form
    {
        private Socket server;
        private IPEndPoint iep;
        private IPAddress ia;
        private int port = 9999;
        private Thread listeningThread;
        private Dictionary<Socket, string> clientSockets = new Dictionary<Socket, string>();
        private List<Socket> clients = new List<Socket>();
        private string currentTime = DateTime.Now.ToString("HH:mm");
        

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
            byte[] data = new byte[1024 * 1024 * 2];
            try
            {
                while (true)
                {
                    int receivedBytes = clientSocket.Receive(data);
                    if (receivedBytes == 0) throw new SocketException(); // Ngắt kết nối
                    string text = Encoding.UTF8.GetString(data, 0, receivedBytes);
                    if (text.StartsWith("IMAGE:"))
                    {
                        string[] parts = text.Split(':');
                        string fileName = parts[1];
                        int fileSize = int.Parse(parts[2]);
                        byte[] imageData = new byte[fileSize];
                        int totalReceived = 0;
                        while (totalReceived < fileSize)
                        {
                            int bytesReceived = clientSocket.Receive(imageData, totalReceived, fileSize - totalReceived, SocketFlags.None);
                            totalReceived += bytesReceived;
                        }
                        // Save or display the image
                        AppendMessage($"Received image {currentTime}: {fileName}", false);
                        ShowImageInRichTextBox(imageData, HorizontalAlignment.Left);
                        BroadcastImage(fileName, imageData, clientSocket);
                    }
                    else if (text.StartsWith("FILE:"))
                    {
                        string[] parts = text.Split(':');
                        string fileName = parts[1];
                        int fileSize = int.Parse(parts[2]);

                        byte[] fileData = new byte[fileSize];
                        int totalReceived = 0;
                        while (totalReceived < fileSize)
                        {
                            int bytesReceived = clientSocket.Receive(fileData, totalReceived, fileSize - totalReceived, SocketFlags.None);
                            totalReceived += bytesReceived;
                        }

                        // Lưu tệp vào thư mục ServerFiles
                        string savePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ServerFiles", fileName);
                        Directory.CreateDirectory(Path.GetDirectoryName(savePath));
                        File.WriteAllBytes(savePath, fileData);

                        AppendMessage($"Đã nhận được tệp {fileName} từ client. Lưu tại: {savePath}", false);

                        // Gửi tệp đến các client khác
                        BroadcastFile(fileName, fileData, clientSocket);
                    }
                    else if (text.StartsWith("[delete]"))
                    {
                        string messageToDelete = text.Substring(8);
                        // Xóa trên server
                        DeleteMessage(messageToDelete);

                        // Phát lệnh xóa tới tất cả các client
                        BroadcastMessage("[delete]" + messageToDelete, null);
                    }
                    else
                    {
                        AppendMessage($"{text}", false);
                        // Phát sóng tin nhắn
                        BroadcastMessage(text, clientSocket);
                    }
                }
            }
            catch (SocketException)
            {
                AppendMessage($"Client {clientSockets[clientSocket]} đã ngắt kết nối.", false);
            }
            finally
            {
                clientSockets.Remove(clientSocket);
                clientSocket.Close();
                UpdateClientList();
            }
        }
        private void BroadcastFile(string fileName, byte[] fileData, Socket excludeSocket)
        {
            string header = $"FILE:{fileName}:{fileData.Length}";
            byte[] headerData = Encoding.UTF8.GetBytes(header);

            foreach (var clientSocket in clientSockets.Keys.ToList())
            {
                if (clientSocket != excludeSocket && clientSocket.Connected)
                {
                    try
                    {
                        clientSocket.Send(headerData);
                        Thread.Sleep(100);
                        clientSocket.Send(fileData);
                    }
                    catch (SocketException ex)
                    {
                        AppendMessage($"Lỗi gửi tệp tới client: {ex.Message}", true);
                    }
                }
            }
        }

        private void BroadcastImage(string fileName, byte[] imageData, Socket excludeSocket)
        {
            // Tạo header cho dữ liệu ảnh
            string header = $"IMAGE:{fileName}:{imageData.Length}";
            byte[] headerData = Encoding.UTF8.GetBytes(header);

            foreach (var clientSocket in clientSockets.Keys.ToList())
            {
                if (clientSocket != excludeSocket && clientSocket.Connected)
                {
                    try
                    {
                        // Gửi header trước
                        clientSocket.Send(headerData);
                        Thread.Sleep(100); // Thêm thời gian chờ ngắn để đảm bảo header được nhận trước
                                           // Gửi dữ liệu ảnh
                        clientSocket.Send(imageData);
                    }
                    catch (SocketException ex)
                    {
                        AppendMessage($"Lỗi gửi ảnh tới client: {ex.Message}", true);
                        clientSockets.Remove(clientSocket);
                        clientSocket.Close();
                    }
                }
            }
        }


        private void BroadcastMessage(string message, Socket excludeSocket)
        {
            byte[] data = Encoding.UTF8.GetBytes(message);
            foreach (var clientSocket in clientSockets.Keys.ToList())
            {
                if (clientSocket != excludeSocket && clientSocket.Connected)
                {
                    try
                    {
                        clientSocket.Send(data);
                    }
                    catch (SocketException ex)
                    {
                        AppendMessage($"Lỗi gửi tới client: {ex.Message}", true);
                        clientSockets.Remove(clientSocket);
                        clientSocket.Close();
                    }
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
            if (string.IsNullOrWhiteSpace(message)) return;

            string fullMessage = $"[{currentTime}-Server]: {message}";
            byte[] data = Encoding.UTF8.GetBytes(fullMessage);

            // Lấy danh sách các client được chọn
            List<Socket> selectedClients = new List<Socket>();
            foreach (var control in groupBox1.Controls)
            {
                if (control is CheckBox checkBox && checkBox.Checked)
                {
                    if (checkBox.Tag is Socket clientSocket && clientSocket.Connected)
                    {
                        selectedClients.Add(clientSocket);
                    }
                }
            }

            // Gửi tin nhắn đến các client đã chọn
            foreach (var clientSocket in selectedClients)
            {
                try
                {
                    clientSocket.Send(data);
                }
                catch (SocketException ex)
                {
                    AppendMessage($"Lỗi gửi tới client: {ex.Message}", true);
                    clientSockets.Remove(clientSocket);
                }
            }

            // Gửi tin nhắn đến tất cả nếu không có client nào được chọn
            if (selectedClients.Count == 0)
            {
                foreach (var clientSocket in clientSockets.Keys.ToList())
                {
                    if (clientSocket.Connected)
                    {
                        try
                        {
                            clientSocket.Send(data);
                        }
                        catch (SocketException ex)
                        {
                            AppendMessage($"Lỗi gửi tới client: {ex.Message}", true);
                            clientSockets.Remove(clientSocket);
                        }
                    }
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
                    server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    ia = IPAddress.Parse("127.0.0.1");
                    iep = new IPEndPoint(ia, port);
                    server.Bind(iep);
                    server.Listen(50);
                    MessageBox.Show("Đã tạo socket thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    listeningThread = new Thread(ListenForClients);
                    listeningThread.IsBackground = true;
                    listeningThread.Start();
                    label3.Text = server.LocalEndPoint.ToString();

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
                    Socket tempClient = server.Accept();
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
            server?.Close();
        }

        private void UpdateClientList()
        {
            groupBox1.Invoke((MethodInvoker)delegate
            {
                groupBox1.Controls.Clear();
                int y = 10; // Vị trí ban đầu
                foreach (var client in clientSockets.ToList())
                {
                    if (client.Key.Connected)
                    {
                        CheckBox checkBox = new CheckBox
                        {
                            Text = client.Value,
                            Tag = client.Key,
                            Location = new Point(10, y),
                            AutoSize = true
                        };
                        groupBox1.Controls.Add(checkBox);
                        y += 30;
                    }
                    else
                    {
                        clientSockets.Remove(client.Key);
                        client.Key.Close();
                    }
                }
            });
        }



        private void DeleteMessage(string message)
        {
            // Xóa tin nhắn trên server
            vbserver.Invoke((MethodInvoker)delegate
            {
                int index = vbserver.Text.IndexOf(message);
                if (index >= 0)
                {
                    vbserver.Select(index, message.Length);
                    vbserver.SelectedText = string.Empty;
                }
            });

            // Gửi lệnh xóa tới tất cả các client
            BroadcastMessage("[delete]" + message, null);
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

        private void btnDeleteMessage_Click_1(object sender, EventArgs e)
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

                    // Tạo header chứa thông tin tệp
                    string header = $"FILE:{fileName}:{fileData.Length}";
                    byte[] headerData = Encoding.UTF8.GetBytes(header);

                    // Gửi tệp đến tất cả các client
                    foreach (var clientSocket in clientSockets.Keys.ToList())
                    {
                        if (clientSocket.Connected)
                        {
                            try
                            {
                                clientSocket.Send(headerData);
                                Thread.Sleep(100); // Đảm bảo header được gửi trước
                                clientSocket.Send(fileData);
                            }
                            catch (SocketException ex)
                            {
                                AppendMessage($"Lỗi gửi tệp: {ex.Message}", true);
                            }
                        }
                    }

                    AppendMessage($"Bạn đã gửi tệp: {fileName}", true);
                }
            }
        }

        private void btnSendImage_Click(object sender, EventArgs e)
        {
            vbserver.ReadOnly = false;
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";
                openFileDialog.Title = "Select an Image";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = openFileDialog.FileName;
                    byte[] fileData = File.ReadAllBytes(filePath);
                    string fileName = Path.GetFileName(filePath);

                    // Tạo header chứa thông tin về ảnh
                    string messageHeader = $"IMAGE:{fileName}:{fileData.Length}";
                    byte[] headerData = Encoding.UTF8.GetBytes(messageHeader);

                    // Lấy danh sách các client được chọn
                    List<Socket> selectedClients = new List<Socket>();
                    foreach (var control in groupBox1.Controls)
                    {
                        if (control is CheckBox checkBox && checkBox.Checked)
                        {
                            if (checkBox.Tag is Socket clientSocket && clientSocket.Connected)
                            {
                                selectedClients.Add(clientSocket);
                            }
                        }
                    }

                    // Nếu không có client nào được chọn, gửi đến tất cả client
                    if (selectedClients.Count == 0)
                    {
                        selectedClients.AddRange(clientSockets.Keys.Where(client => client.Connected));
                    }

                    // Gửi ảnh đến các client được chọn
                    foreach (var clientSocket in selectedClients)
                    {
                        try
                        {
                            // Gửi header
                            clientSocket.Send(headerData);
                            Thread.Sleep(100); // Đảm bảo header được gửi trước

                            // Gửi dữ liệu ảnh
                            clientSocket.Send(fileData);
                        }
                        catch (SocketException ex)
                        {
                            AppendMessage($"Lỗi khi gửi ảnh tới client: {ex.Message}", true);
                            clientSockets.Remove(clientSocket);
                            clientSocket.Close();
                        }
                    }

                    // Hiển thị ảnh trên giao diện server
                    AppendMessage($"[{currentTime}-Bạn]: {fileName}", true);
                    ShowImageInRichTextBox(fileData, HorizontalAlignment.Right);
                }
            }
        }


        private void ShowImageInRichTextBox(byte[] imageData, HorizontalAlignment alignment)
        {
            this.Invoke((MethodInvoker)delegate
            {
                using (MemoryStream ms = new MemoryStream(imageData))
                {
                    Image img = Image.FromStream(ms);

                    // Đảm bảo thao tác với Clipboard trong STA thread
                    if (Clipboard.ContainsImage())
                    {
                        
                        try
                        {
                            Clipboard.Clear();  // Xóa clipboard nếu có hình ảnh cũ
                            // Thao tác với clipboard trong STA thread
                            Clipboard.SetImage(img);
                        }
                        catch (ExternalException ex)
                        {
                            // Log lỗi nếu không thành công
                            MessageBox.Show($"Lỗi khi thao tác với Clipboard: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                   
                    // Tiến hành dán ảnh vào RichTextBox
                    vbserver.Select(vbserver.TextLength, 0);
                    vbserver.SelectionAlignment = alignment;
                    vbserver.Paste();  // Dán ảnh vào RichTextBox
                    vbserver.AppendText("\n");
                    vbserver.SelectionAlignment = HorizontalAlignment.Left;
                }
            });
        }




    }
}

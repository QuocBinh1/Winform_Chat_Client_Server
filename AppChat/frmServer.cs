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
namespace AppChat{
    public partial class frmServer : Form
    {
        //khai báo
        Socket socketserver, socketclient;
        IPEndPoint iep;
        IPAddress ia;
        int port = 9999;
        Thread listeningThread;

        public frmServer()
        {
            InitializeComponent();
        }

        //định dạng phần tin nhắn chia bên trái phải
        private void AppendMessage(string message, bool isServer)
        {
            vbserver.Invoke((MethodInvoker)delegate
            {
                vbserver.SelectionAlignment = isServer ? HorizontalAlignment.Right : HorizontalAlignment.Left;
                vbserver.SelectionColor = isServer ? Color.Blue : Color.Green;
                vbserver.AppendText(message + "\n");
                vbserver.SelectionColor = vbserver.ForeColor; // Reset color
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
                    AppendMessage("[Client]: " + text, false);
                }
            }
            catch (SocketException ex)
            {
                AppendMessage("Client đã ngắt kết nối: " + ex.Message, false);
            }
            finally
            {
                clientSocket.Close();
            }
        }


        private void btnSend_Click(object sender, EventArgs e)
        {
            if (socketclient != null && socketclient.Connected)
            {
                string message = txtSend.Text;
                byte[] data = Encoding.UTF8.GetBytes(message);
                socketclient.Send(data);

                AppendMessage("[Server]: " + message, true);
                txtSend.Clear();
            }
            else
            {
                MessageBox.Show("Không có kết nối nào để gửi dữ liệu.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void btnCreateSocketServer_Click(object sender, EventArgs e){
           
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

                    // Khởi động luồng lắng nghe kết nối từ client
                    listeningThread = new Thread(ListenForConnections);
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

        //luôn luôn chấp nhận kết nối từ client
        private void ListenForClients()
        {
            while (true)
            {
                socketclient = socketserver.Accept();
                Thread receiveThread = new Thread(ReceiveData);
                receiveThread.IsBackground = true;
                receiveThread.Start(socketclient);
            }
        }

        //hiển thị hộp thoại xác nhận kết nối từ client , nếu kh chập nhận sẽ không gửi tin nhắn đc 
        private void ListenForConnections()
        {
            while (true)
            {
                try
                {
                    // Lắng nghe kết nối đến
                    Socket tempClient = socketserver.Accept();

                    // Lấy thông tin thiết bị kết nối
                    string clientInfo = tempClient.RemoteEndPoint.ToString();

                    // Hiển thị hộp thoại xác nhận trên UI thread
                    this.Invoke((MethodInvoker)delegate
                    {
                        DialogResult dialogResult = MessageBox.Show($"Thiết bị {clientInfo} đang cố kết nối. Bạn có muốn chấp nhận không?",
                            "Yêu cầu kết nối", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                        if (dialogResult == DialogResult.Yes)
                        {
                            socketclient = tempClient;
                            MessageBox.Show("Đã chấp nhận kết nối từ " + clientInfo, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            // Khởi động luồng nhận dữ liệu từ client
                            Thread receiveThread = new Thread(ReceiveData);
                            receiveThread.IsBackground = true;
                            receiveThread.Start(socketclient);
                        }
                        else
                        {
                            tempClient.Close();
                            MessageBox.Show("Đã từ chối kết nối từ " + clientInfo, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    });
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi lắng nghe kết nối: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                }
            }
        }

        private void btnSendImage_Click(object sender, EventArgs e)
        {

        }

        //đóng các kết nối, luồng
        private void frmServer_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Đảm bảo đóng các kết nối và luồng khi form đóng
            listeningThread?.Abort();
            socketclient?.Close();
            socketserver?.Close();
        }

        

    }
}

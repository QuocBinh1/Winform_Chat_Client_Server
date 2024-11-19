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
                socketclient.Connect(iep);

                AppendMessage("Connecting to server...", true);

                receivingThread = new Thread(ReceiveData);
                receivingThread.IsBackground = true;
                receivingThread.Start();
                ClearMessages();
                AppendMessage("Connected to server...", true);
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
                    AppendMessage(text, false);
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
                string message = txtSend.Text;
                string fullMessage = $"[{socketclient.LocalEndPoint}]: {message}";
                byte[] data = Encoding.UTF8.GetBytes(fullMessage);
                socketclient.Send(data);

                AppendMessage($"[You]: {message}", true);
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
    }
}

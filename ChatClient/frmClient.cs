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
using AppChat;
namespace ChatClient
{
    public partial class frmClient : Form
    {
        Socket socketclient , server;
        IPAddress ia;
        IPEndPoint iep;
        int port = 9999;
        Thread receivingThread;
        public frmClient()
        {
            InitializeComponent();
        }

        private void frmClient_Load(object sender, EventArgs e)
        {

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

                // Clear the "Connecting to server..." message after successful connection
                ClearMessages();
                AppendMessage("Connected to server...", true);
            }
            catch (SocketException ex)
            {
                MessageBox.Show("Unable to connect to server: " + ex.Message, "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

                    AppendMessage("[Server]: " + text, false);
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
                byte[] data = Encoding.UTF8.GetBytes(message);
                socketclient.Send(data);

                AppendMessage("[Client]: " + message, true);
                txtSend.Clear();
            }
            catch (SocketException ex)
            {
                AppendMessage("Failed to send message: " + ex.Message, true);
            }
        }
        private void frmClient_FormClosing(object sender, FormClosingEventArgs e)
        {
            receivingThread?.Abort();
            socketclient?.Close();
        }

        private void btnSendImage_Click(object sender, EventArgs e)
        {

        }

        private void btnSendFile_Click(object sender, EventArgs e)
        {

        }


    }
}

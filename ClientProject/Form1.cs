using SuperSimpleTcp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClientProject
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        SimpleTcpClient client; //TCP Client'ı çağırdığımız kütüphane

        private void btnSend_Click(object sender, EventArgs e)
        {
         if(client.IsConnected)
            {
                if(!string.IsNullOrEmpty(txtMessage.Text)) //Mesaj olup olmama kontrolünü yapıyor. Null değilse;
                {
                    Thread.Sleep(1000);
                    client.Send(txtMessage.Text); //Mesajı gönderiyoruz.
                    txtInfo.Text += $"Me: {txtMessage.Text}{Environment.NewLine}";
                    txtMessage.Text = string.Empty;
                }
            }
        }

        private void btnConnect_Click(object sender, EventArgs e) //bağlantıyı gönderiyoruz.
        {
            try //hata yönetimi
            {
                Thread.Sleep(1000); //1 saniye bekleyip öyle açıyor
                client.Connect(); //client bağlanıyor
                btnSend.Enabled = true;
                btnConnect.Enabled = false;
            }
            catch (Exception ex) //bağlanmadığı durumda
            {

                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error); //bağlanamadım mesajı verir.
            }
        }

        private void Form1_Load(object sender, EventArgs e) //form'daki ekran bağlantılarını sağladık.
        {
            client = new(txtIP.Text);
            client.Events.Connected += Events_Connected;
            client.Events.Disconnected += Events_Disconnected;
            client.Events.DataReceived += Events_DataReceived;
            btnSend.Enabled = false;

        }

        private void Events_DataReceived(object sender, DataReceivedEventArgs e)
        {
            this.Invoke((MethodInvoker)delegate
            {
                txtInfo.Text += $"Server: {Encoding.UTF8.GetString(e.Data)}{Environment.NewLine}"; //encoding.UTF--> gelen datayı stringe dönüştürüyoor.
            }); //birden fazla client bağlamamıza yarıyor.
            }

        private void Events_Disconnected(object sender, ConnectionEventArgs e)
        {
            this.Invoke((MethodInvoker)delegate
            {
                txtInfo.Text += $"Server Disconnected.{Environment.NewLine}";
            });
        }

        private void Events_Connected(object sender, ConnectionEventArgs e)
        {
            this.Invoke((MethodInvoker)delegate
            {
                txtInfo.Text += $"Server Connected.{Environment.NewLine}";
            });

        }
    }
}

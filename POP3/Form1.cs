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
using System.Net.Security;
using System.Net.NetworkInformation;
using System.IO;
using System.Web;

namespace POP3
{
    public partial class fmMain : Form
    {
        TcpClient tcpClient;
        SslStream sslStream;
        StreamWriter sw;
        StreamReader reader;
        string message;

        public fmMain()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                tcpClient = new TcpClient();
                tcpClient.Connect("pop.mail.ru", 995);
                if (tcpClient.Connected)
                {
                    sslStream = new SslStream(tcpClient.GetStream());
                    sslStream.AuthenticateAsClient("pop.mail.ru");
                    sw = new StreamWriter(sslStream);
                    reader = new StreamReader(sslStream);
                    if (tbLogin.Text == "" || tbPassword.Text == "")
                    {
                        lblError.Text = "Заполните поля <Login> и <Password>";
                    }
                    else
                    {
                        lblError.Text = "";
                        sw.WriteLine("USER " + tbLogin.Text);
                        sw.Flush();
                        message = reader.ReadLine();
                        listBox1.Items.Add(message);
                        sw.WriteLine("PASS " + tbPassword.Text);
                        sw.Flush();
                        message = reader.ReadLine();
                        listBox1.Items.Add(message);
                        message = reader.ReadLine();
                        listBox1.Items.Add(message);

                        btnEnter.Enabled = true;
                        cbCommand.Enabled = true;
                        tbArg.Enabled = true;
                        btnExit.Enabled = true;
                    }
                }
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            switch (cbCommand.Text)
            {
                case "LIST":
                    sw.WriteLine(cbCommand.Text + " " + tbArg.Text);
                    sw.Flush();
                    message = reader.ReadLine();
                    listBox1.Items.Add(message);
                    break;
                case "DELE":
                    sw.WriteLine(cbCommand.Text + " " + tbArg.Text);
                    sw.Flush();
                    message = reader.ReadLine();
                    listBox1.Items.Add(message);
                    break;
                default:
                    sw.WriteLine(cbCommand.Text);
                    sw.Flush();
                    message = reader.ReadLine();
                    listBox1.Items.Add(message);
                    break;
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            sw.WriteLine("QUIT");
            sw.Flush();
            message = reader.ReadLine();
            listBox1.Items.Add(message);

            btnEnter.Enabled = false;
            cbCommand.Enabled = false;
            tbArg.Enabled = false;
            btnExit.Enabled = false;
        }
    }
}

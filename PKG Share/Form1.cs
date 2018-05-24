using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Net;
using System.Threading;
using System.Diagnostics;


namespace PKG_Share
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        string myversion = "1.0.0.0";
        private DataTable dt = new DataTable();
        private string filebasename;
        private string filename;
        private string filename2;
        private string Count;
        WebClient webClient;               // Our WebClient that will be doing the downloading for us
        Stopwatch sw = new Stopwatch();    // The stopwatch which we will be using to calculate the download spe
        public void DownloadFile(string urlAddress, string location)
        {
            using (webClient = new WebClient())
            {
                webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(Completed);
                webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(ProgressChanged);

                // The variable that will be holding the url address (making sure it starts with http://)
                Uri URL = urlAddress.StartsWith("http://", StringComparison.OrdinalIgnoreCase) ? new Uri(urlAddress) : new Uri("http://" + urlAddress);

                // Start the stopwatch which we will be using to calculate the download speed
                sw.Start();

                try
                {
                    // Start downloading the file
                    webClient.DownloadFileAsync(URL, location);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
        // The event that will fire whenever the progress of the WebClient is changed
        private void ProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            // Calculate download speed and output it to labelSpeed.
            this.label3.Text ="Speed: "+ string.Format("{0} kb/s", (e.BytesReceived / 1024d / sw.Elapsed.TotalSeconds).ToString("0.00"));

            // Update the progressbar percentage only when the value is not the same.
            progressBar1.Value = e.ProgressPercentage;

            // Show the percentage on our label.
            this.label4.Text = "Perentage: "+e.ProgressPercentage.ToString() + "%";

            // Update the label with how much data have been downloaded so far and the total size of the file we are currently downloading
            this.label5.Text ="Total Download: "+ string.Format("{0} MB's / {1} MB's",
                (e.BytesReceived / 1024d / 1024d).ToString("0.00"),
                (e.TotalBytesToReceive / 1024d / 1024d).ToString("0.00"));
        }

        // The event that will trigger when the WebClient is completed
        private void Completed(object sender, AsyncCompletedEventArgs e)
        {
            // Reset the stopwatch.
            sw.Reset();

            if (e.Cancelled == true)
            {
                MessageBox.Show("Download has been canceled.", "Cancled", MessageBoxButtons.OK, MessageBoxIcon.Error);
                progressBar1.Value = 0;
                progressBar1.Text = "";
                label3.Text = "Speed:";
                label4.Text = "Perentage:";
                label5.Text = "Total Download:";
                this.linkLabel1.Text = "Download";
            }
            else
            {
                progressBar1.Value = 0;
                progressBar1.Text = "";
                label3.Text = "Speed:";
                label4.Text = "Perentage:";
                label5.Text = "Total Download:";
                this.linkLabel1.Text = "Download";
                MessageBox.Show("Download Completed And Added To PKG Folder");
                this.Text = "PKG Share";
            }
        }
        

        private void Form1_Load(object sender, EventArgs e)
        {
            MessageBox.Show("Hello And Welcome To PKG Share \n\r---Rule Number One Have Fun And Enjoy \n\r---The PKG's supplied have been supplied by users from the PS3 Scene more thanks for them for adding there pkg's", "Hello and Welcome", MessageBoxButtons.OK, MessageBoxIcon.Information);

            try
            {
                if (System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable() == false & this.Text == "PKG Share v0.1")
                {
                    MessageBox.Show("You Need An Active Internet Connection To Run This Application");
                    Application.Exit();
                }
                else
                {
                    string appPath = Application.StartupPath;
                    if (!Directory.Exists(appPath + "\\Workables"))
                        Directory.CreateDirectory(appPath + "\\Workables");

                    //  DownloadFile(@"http://localhost:1275/xDPx/pkglist.txt", appPath + @"\\db");
                    //  Thread.Sleep(1000);

                    if (File.Exists(appPath + @"\\db") == true)
                    {
                        {
                            //var PKGid = line.Split(';');
                            //  for (int i = 0; i <= 6; i++)
                            // {
                            //  }
                            try
                            {
                                this.dt.Columns.Add("Game ID");
                                this.dt.Columns.Add("Name");
                                this.dt.Columns.Add("Type");
                                this.dt.Columns.Add("Region");
                                this.dt.Columns.Add("psnlink");
                                this.dt.Columns.Add("pnglink");
                                this.dt.Columns.Add("rapname");
                                this.dt.Columns.Add("rapdata");
                                this.dt.Columns.Add("info");
                                this.dt.Columns.Add("postedby");
                                string str2;
                                FileInfo info3 = new FileInfo("db")
                                {
                                    Attributes = FileAttributes.Normal
                                };
                                byte[] bytes = System.IO.File.ReadAllBytes("db");
                                StringReader reader = new StringReader(Encoding.GetEncoding(0x4e4).GetString(bytes));
                                while ((str2 = reader.ReadLine()) != null)
                                {
                                    string[] values = str2.Split(new char[] { ';' });
                                    this.dt.Rows.Add(values);
                                    this.Count = this.dt.Rows.Count.ToString();
                                }
                                this.dataGridView1.DataSource = this.dt;
                                this.dataGridView1.Columns[0].Width = 100;
                                this.dataGridView1.Columns[1].Width = 0x160;
                                this.dataGridView1.Columns[2].Width = 0x48;
                                this.dataGridView1.Columns[3].Width = 60;
                                this.dataGridView1.Columns[4].Visible = false;
                                this.dataGridView1.Columns[5].Visible = false;
                                this.dataGridView1.Columns[6].Visible = false;
                                this.dataGridView1.Columns[7].Visible = false;
                                this.dataGridView1.Columns[8].Visible = false;
                                this.dataGridView1.Columns[9].Visible = false;
                                info3.Attributes = FileAttributes.Hidden;
                                reader.Close();
                            }
                            catch
                            {
                                MessageBox.Show("Could not load Database", "Database", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                                FileInfo info4 = new FileInfo("db")
                                {
                                    Attributes = FileAttributes.Hidden
                                };
                            }
                            //this.comboBox1.SelectedIndex = this.comboBox1.FindStringExact("All");
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An Internal Error Acoured " + ex.Message);
                System.IO.File.WriteAllText(Application.StartupPath + "\\errorlog.txt", ex.ToString());
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (this.dataGridView1.RowCount != 0)
            {
                this.textBox1.Text = this.dataGridView1.CurrentRow.Cells["info"].Value.ToString();
                //this.size.Text = "Size";
                this.pictureBox1.ImageLocation = this.dataGridView1.CurrentRow.Cells["pnglink"].Value.ToString();
                this.pictureBox1.Image = this.pictureBox1.InitialImage;
               // this.richTextBox2.Text = this.dataGridView1.CurrentRow.Cells["postedby"].Value.ToString();
                this.label2.Text = this.dataGridView1.CurrentRow.Cells["Game ID"].Value.ToString();
                //if (this.richTextBox2.Text == "")
                //{
                //    this.richTextBox2.Text = "anonymous";
                //}
                switch (this.dataGridView1.CurrentRow.Cells["rapdata"].Value.ToString())
                {
                    case " ":
                    case "":
                        return;
                }
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (this.dt.Rows.Count != 0)
            {
                string extension = Path.GetExtension(this.dataGridView1.CurrentRow.Cells["psnlink"].Value.ToString());
                int index = extension.IndexOf('?');
                if (-1 != index)
                {
                    extension = extension.Substring(0, index);
                }
                string str2 = this.dataGridView1.CurrentRow.Cells["name"].Value.ToString().Replace('\\', ' ').Replace('/', ' ').Replace(':', ' ').Replace('*', ' ').Replace('?', ' ').Replace('"', ' ').Replace('<', ' ').Replace('>', ' ').Replace('|', ' ');
                this.filebasename = str2 + extension;
                this.filename = "pkg/" + this.dataGridView1.CurrentRow.Cells["Game ID"].Value.ToString() + " " + this.filebasename;
                if (this.linkLabel1.Text == "Download")
                {
                    if (!System.IO.File.Exists(this.filename) || (MessageBox.Show("Download again?", "Already downloaded!", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.OK))
                    {
                        string str4;
                        if (!Directory.Exists("pkg"))
                        {
                            Directory.CreateDirectory("pkg");
                        }
                        this.linkLabel1.Text = "Cancel";
                        this.sw.Start();
                        string str3 = this.dataGridView1.CurrentRow.Cells["psnlink"].Value.ToString();
                        if (!str3.StartsWith("http://", StringComparison.OrdinalIgnoreCase) && !str3.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
                        {
                            str4 = "http://" + str3;
                        }
                        else
                        {
                            str4 = str3;
                        }
                        DownloadFile(str4, this.filename);
                       // this.downl.Text = this.dataGridView1.CurrentRow.Cells["name"].Value.ToString();
                       // this.label16.Text = "Downloading:";
                    }
                }
                else
                {
                    this.webClient.CancelAsync();
                    this.linkLabel1.Text = "Download Package";
                }
            }
        }
    }
}

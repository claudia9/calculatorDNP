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
using System.Xml;
using System.Xml.Linq;

namespace Exercise12
{
    public partial class Form1 : Form
    {
        private VIA.DNP1MathLib math;
        bool threadRunning = true;
 
        public Form1()
        {
            InitializeComponent();
            math = new VIA.DNP1MathLib();
        }


        bool bGetInput(ref int n1, ref int n2)
        {
            n1 = n2 = 0; 
            try
            {
 
                n1 = int.Parse(this.textBox1.Text);
                n2 = int.Parse(this.textBox2.Text);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("You entered something wrong!");
            }
            return false;
        }
        private void btnAddClick(object sender, EventArgs e)
        {
            int n1 = 0;
            int n2 = 0;
            if ( bGetInput( ref n1, ref n2 ) == true )
            {
                textBox3.Text = math.Add(n1, n2).ToString();
                
                string s = n1.ToString() + " + " + n2.ToString() + " = " + textBox3.Text;
                listBox1.Items.Add(s);
            }
        }

        private void btnSubClick(object sender, EventArgs e)
        {
            int n1 = 0;
            int n2 = 0;
            if (bGetInput(ref n1, ref n2) == true)
            {
                textBox3.Text = math.Sub(n1, n2).ToString();
                string s = n1.ToString() + " - " + n2.ToString() + " = " + textBox3.Text;
                listBox1.Items.Add(s);
            }
        }

        private void btnMulClick(object sender, EventArgs e)
        {
            int n1 = 0;
            int n2 = 0;
            if (bGetInput(ref n1, ref n2) == true)
            {
                textBox3.Text = math.Mul(n1, n2).ToString();
                string s = n1.ToString() + " * " + n2.ToString() + " = " + textBox3.Text;
                listBox1.Items.Add(s);
            }
        }

        private void btnDivClick(object sender, EventArgs e)
        {
            int n1 = 0;
            int n2 = 0;
            if (bGetInput(ref n1, ref n2) == true)
            {

                try
                {
                    textBox3.Text = math.Div(n1, n2).ToString();
                    string s = n1.ToString() + " / " + n2.ToString() + " = " + textBox3.Text;
                    listBox1.Items.Add(s);
                }
                catch (Exception)
                {

                    MessageBox.Show( "UPS!");
                }
            }
        }
        private void btnClrClick(object sender, EventArgs e)
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            listBox1.Items.Clear();
        }

        private void OnFileOpen(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "XML Files|*.xml|All files (*.*)|*.*";
            DialogResult dr = ofd.ShowDialog();

            if (dr == DialogResult.OK)
            {
                string filename = ofd.FileName;

                using (XmlReader reader = XmlReader.Create(ofd.FileName))
                {
                    reader.ReadStartElement("data");

                    while (reader.Name == "calculation")
                    {
                        XElement el = (XElement)XNode.ReadFrom(reader);
                        listBox1.Items.Add(el.Value);
                    }

                    reader.ReadEndElement();
                }
            }
        }

        private void OnFileSave(object sender, EventArgs e)
        {
            SaveFileDialog ofd = new SaveFileDialog();
            ofd.Filter = "XML Files|*.xml|All files (*.*)|*.*";
            DialogResult dr = ofd.ShowDialog();

            if ( dr == DialogResult.OK )
            {
               //string filename = ofd.FileName;
               XmlWriter xmlWriter = XmlWriter.Create(ofd.FileName);
     

               xmlWriter.WriteStartDocument();
               xmlWriter.WriteStartElement("data");

               foreach (var item in listBox1.Items )
               {
                   xmlWriter.WriteStartElement("calculation");
                   xmlWriter.WriteString( item.ToString() );
                   xmlWriter.WriteEndElement();
               }

               xmlWriter.WriteEndDocument();
               xmlWriter.Close();
            }
        }

        private void OnStressTest(object sender, EventArgs e)
        {
            // ThreadEngine();
            new Thread(ThreadEngine).Start();
        }

        void ThreadEngine()
        {
            while( threadRunning )
            {
                string total = "";
                Random r = new Random();
                int t1 = r.Next(100);
                int t2 = r.Next(100);
                int result = 0;
                int whichMethod = r.Next(3);
                switch (whichMethod)
                {
                    case 0:
                        {
                            result = math.Add(t1, t2);
                            string s = t1.ToString() + " + " + t2.ToString() + " = " + result.ToString();
                            total = s;
                            break;
                        }
                    case 1:
                        {
                            result = math.Sub(t1, t2);
                            string s = t1.ToString() + " - " + t2.ToString() + " = " + result.ToString();
                            total = s;
                            break;
                        }
                    case 2:
                        {
                            result = math.Mul(t1, t2);
                            string s = t1.ToString() + " * " + t2.ToString() + " = " + result.ToString();
                            total = s;
                            break;
                        }
                    case 3:
                        {
                            if (t2 > 0)
                            {
                                result = math.Div(t1, t2);
                                string s = t1.ToString() + " / " + t2.ToString() + " = " + result.ToString();
                                total = s;
                            }
                            break;
                        }
                }


                if (this.InvokeRequired)
                {
                    MethodInvoker del = delegate { this.listBox1.Items.Add(total); };
                    this.Invoke(del);
                }
                else
                {
                    this.listBox1.Items.Add(total);
                }

                Thread.Sleep(500);
            }
        }


        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            threadRunning = false;
        }


        private async void OnStressTestAsync(object sender, EventArgs e)
        {
            int count = 0;
            while ( count++ < 10 )
            {
                string total = "";
                Random r = new Random();
                int t1 = r.Next(100);
                int t2 = r.Next(100);
                int result = 0;
                int whichMethod = r.Next(3);
                switch (whichMethod)
                {
                    case 0:
                        {
                            result = math.Add(t1, t2);
                            string s = t1.ToString() + " + " + t2.ToString() + " = " + result.ToString();
                            total = s;
                            break;
                        }
                    case 1:
                        {
                            result = math.Sub(t1, t2);
                            string s = t1.ToString() + " - " + t2.ToString() + " = " + result.ToString();
                            total = s;
                            break;
                        }
                    case 2:
                        {
                            result = math.Mul(t1, t2);
                            string s = t1.ToString() + " * " + t2.ToString() + " = " + result.ToString();
                            total = s;
                            break;
                        }
                    case 3:
                        {
                            if (t2 > 0)
                            {
                                result = math.Div(t1, t2);
                                string s = t1.ToString() + " / " + t2.ToString() + " = " + result.ToString();
                                total = s;
                            }
                            break;
                        }
                }

                MethodInvoker del = delegate { this.listBox1.Items.Add(total); };
                await Task.Run(() => this.Invoke(del));

                
                Task delayTask = Task.Delay(1000);
                await delayTask;
            }
        }
    }
}

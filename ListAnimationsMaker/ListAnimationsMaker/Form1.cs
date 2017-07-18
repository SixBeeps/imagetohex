using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace ListAnimationsMaker
{
    public partial class Form1 : Form
    {
        public Bitmap bmp;
        public bool loaded = false;
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e) //Load Button
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Portable Network Graphics|*.PNG;";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                StreamReader sr = new StreamReader(ofd.FileName);
                Image i = Image.FromStream(sr.BaseStream);
                bmp = new Bitmap(i);
                loaded = true;
                if (bmp.Width%16 != 0 || bmp.Height%9 != 0)
                {
                    MessageBox.Show("Image is not 16x9. Image is unable to play in List Animations",
                        "Image Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    sr.Close();
                    loaded = false;
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void generate_Click(object sender, EventArgs e)
        {
            if (loaded)
            {
                string outStr = "";
                string hex = "";
                for (int y = 0; y < bmp.Height; y++)
                {
                    for (int x = 0; x < bmp.Width; x++)
                    {
                        if (bmp.GetPixel(x, y) == Color.FromArgb(255, 0, 0, 0))
                        {
                            outStr += "1";
                        }
                        else if (bmp.GetPixel(x, y) == Color.FromArgb(0, 0, 0, 0))
                        {
                            outStr += "0";
                        }
                        else
                        {
                            MessageBox.Show("An invalid color (" + bmp.GetPixel(x, y).ToString() + ") was detected. Please use black or transparent.",
                                "Image Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            break;
                        }
                    }
                }
                for (int i = 0; i < outStr.Length / 4; i++)
                {
                    switch (outStr.Substring(i * 4, 4))
                    {
                        case "0000":
                            hex += "0";
                            break;
                        case "0001":
                            hex += "1";
                            break;
                        case "0010":
                            hex += "2";
                            break;
                        case "0011":
                            hex += "3";
                            break;
                        case "0100":
                            hex += "4";
                            break;
                        case "0101":
                            hex += "5";
                            break;
                        case "0110":
                            hex += "6";
                            break;
                        case "0111":
                            hex += "7";
                            break;
                        case "1000":
                            hex += "8";
                            break;
                        case "1001":
                            hex += "9";
                            break;
                        case "1010":
                            hex += "A";
                            break;
                        case "1011":
                            hex += "B";
                            break;
                        case "1100":
                            hex += "C";
                            break;
                        case "1101":
                            hex += "D";
                            break;
                        case "1110":
                            hex += "E";
                            break;
                        case "1111":
                            hex += "F";
                            break;
                        default:
                            MessageBox.Show("Bin to Hex conversion failed.", "Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                            break;
                    }
                }
                textBox1.Text = hex;
                System.GC.Collect();
            } else
            {
                MessageBox.Show("Please load an image first.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://scratch.mit.edu/projects/168707761/#player");
        }
    }
}
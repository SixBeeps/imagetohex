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
        public int blackValue;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e) //Load Button
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Portable Network Graphics|*.PNG;|JPEG Images|*.JPG;*.JPEG";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                StreamReader sr = new StreamReader(ofd.FileName);
                Image i = Image.FromStream(sr.BaseStream);
                bmp = new Bitmap(i);
                loaded = true;
                loadBtn.BackColor = Color.LimeGreen;
                if (bmp.Width%16 != 0 || bmp.Height%9 != 0)
                {
                    MessageBox.Show("Image is not 16x9. Image is unable to play in List Animations",
                        "Image Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    sr.Close();
                    loaded = false;
                    //loadBtn.BackColor = Control.DefaultBackColor;
                    loadBtn.BackColor = Color.Transparent;
                }
                if (bmp.Height > 500)
                {
                    if (MessageBox.Show("This image is rather big, and turning it into hex could majorly slow down your computer.\nAre you sure you want to use this image?", "Image Warning",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                    {
                        sr.Close();
                        loaded = false;
                        loadBtn.BackColor = Control.DefaultBackColor;
                    }
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
                        if (bmp.GetPixel(x, y) == Color.FromArgb(255, blackValue, blackValue, blackValue))
                        { //BLACK
                            if (!Invert.Checked)
                                outStr += "1";
                            else
                                outStr += "0";
                        }
                        else if (bmp.GetPixel(x, y) == Color.FromArgb(0, 0, 0, 0) || bmp.GetPixel(x, y) == Color.White)
                        {//TRANSPARENT
                            if (!Invert.Checked)
                                outStr += "0";
                            else
                                outStr += "1";
                        }
                        else
                        {
                            if (MessageBox.Show("An invalid color (" + bmp.GetPixel(x, y).ToString() + ") was detected. How would you like to treat this color (Y=Transparent, N=Black)?",
                                "Image Error", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                            {
                                outStr += "0";
                            } else
                            {
                                outStr += "1";
                            }
                        }
                    }
                    progressBar.Value = (int)Math.Floor((decimal)(y/bmp.Height))*100;
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
                            MessageBox.Show("An invalid binary string was detected. Please contact me if you see this.", "Error",
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

        private void bvBar_Scroll(object sender, EventArgs e)
        {
            blackValue = bvBar.Value;
            bvBox.Text = blackValue.ToString();
        }

        private void bvBox_TextChanged(object sender, EventArgs e)
        {
            if (Int32.TryParse(bvBox.Text, out blackValue))
            {
                if (blackValue >= 0 && blackValue <= 50)
                    bvBar.Value = blackValue;
                else
                    MessageBox.Show("Please enter a number from 0 to 50.", "Hold on a second...",
                        MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }
    }
}
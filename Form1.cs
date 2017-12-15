using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            //宣告openFileDialog1 

            openFileDialog1.Filter = "All Files|*.*";
            //設定可以開啟的檔案格式
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.pictureBox1.Image = Image.FromFile(openFileDialog1.FileName);
                this.pictureBox2.Image = Image.FromFile(openFileDialog1.FileName);
                //顯示apple在pictureBox1的框架內
            }
            else
                System.Console.Write("data error");
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Bitmap apple = new Bitmap(pictureBox2.Image);
            int m = apple.Width, n = apple.Height;
            int cdfMinR = 256, cdfMinG = 256, cdfMinB = 256;
            int[] cdfR = new int[256];
            int[] cdfG = new int[256];
            int[] cdfB = new int[256];
            for (int r = 0; r < m; r++)
            {
                for (int c = 0; c < n; c++)
                {
                    Color co = apple.GetPixel(r, c);
                    cdfR[co.R]++;
                    cdfG[co.G]++;
                    cdfB[co.B]++;
                    cdfMinR = Math.Min(cdfMinR, co.R);
                    cdfMinG = Math.Min(cdfMinG, co.G);
                    cdfMinB = Math.Min(cdfMinB, co.B);
                    /*
                    int epsilon=0;
                    double C=50,gamma = 0.2;
                    int R = (int)(C * Math.Pow(co.R + epsilon, gamma));
                    int G = (int)(C * Math.Pow(co.G + epsilon, gamma));
                    int B = (int)(C * Math.Pow(co.B + epsilon, gamma));
                    apple.SetPixel(r, c, Color.FromArgb(R, G, B));
                    */
                }
            }
            for(int i=0;i<256-1;i++)
            {
                cdfR[i + 1] += cdfR[i];
                cdfG[i + 1] += cdfG[i];
                cdfB[i + 1] += cdfB[i];
            }
            for(int r=0;r<m;r++)
            {
                for(int c=0;c<n;c++)
                {
                    Color co = apple.GetPixel(r, c);
                    double tempR = 255*(cdfR[co.R] - cdfR[cdfMinR]) / (m * n - cdfR[cdfMinR]);
                    double tempG = 255*(cdfG[co.G] - cdfG[cdfMinG]) / (m * n - cdfG[cdfMinG]);
                    double tempB = 255*(cdfB[co.B] - cdfB[cdfMinB]) / (m * n - cdfB[cdfMinB]);
                    int R = (int)Math.Round(tempR);
                    int G = (int)Math.Round(tempG);
                    int B = (int)Math.Round(tempB);
                    apple.SetPixel(r, c, Color.FromArgb(R, G, B));
                }
            }
            this.pictureBox2.Image = apple;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //pictureBox2.Image.Save(@"D:\巫佶翰\study\影像辨識技術\作業\all\1.jpg");
            pictureBox2.Image.Save(@"C:\Users\saop0\Pictures\Saved Pictures\result.jpg");
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            Bitmap apple = new Bitmap(pictureBox2.Image);
            for (int r = 0; r < apple.Width; r++)
            {
                for (int c = 0; c < apple.Height; c++)
                {
                    Color co = apple.GetPixel(r, c);
                    int R = co.R & 224;
                    int G = co.G & 224;
                    int B = co.B & 224;
                    apple.SetPixel(r, c, Color.FromArgb(R, G, B));
                }
            }
            this.pictureBox2.Image = apple;
        }

        private void button2_Click_2(object sender, EventArgs e)
        {
            Bitmap picture = new Bitmap(pictureBox2.Image);
            Bitmap newPicture = new Bitmap(pictureBox1.Image);

            int edge = (5 - 1) / 2;
            for (int r = edge; r < picture.Width - edge; r++)
            {
                for (int c = edge; c < picture.Height - edge; c++)
                {
                    int sum = 0;
                    for (int i = -edge; i <= edge; i++)
                    {
                        for (int j = -edge; j <= edge; j++)
                        {
                            Color co = picture.GetPixel(r + i, c + j);
                            sum += co.R;
                        }
                    }
                    int average = sum / 25;
                    picture.SetPixel(r, c, Color.FromArgb(average, average, average));
                }
            }

            int[,] vertical = new int[3, 3] { { -1, -2, -1 }, { 0, 0, 0 }, { 1, 2, 1 } };
            int[,] horizental = new int[3, 3] { { -1, 0, 1 }, { -2, 0, 2 }, { -1, 0, 1 } };
            for (int r = 1; r < picture.Width - 1; r++)
            {
                for (int c = 1; c < picture.Height - 1; c++)
                {
                    int vSum = 0, hSum = 0;
                    Color center = picture.GetPixel(r, c);
                    for (int i = -1; i <= 1; i++)
                    {
                        for (int j = -1; j <= 1; j++)
                        {
                            Color co = picture.GetPixel(r + i, c + j);
                            int grayscale = (co.R + co.G + co.B) / 3;
                            vSum += grayscale * vertical[i + 1, j + 1];
                            hSum += grayscale * horizental[i + 1, j + 1];
                        }
                    }
                    int g = (int)Math.Sqrt(vSum * vSum + hSum * hSum);
                    //int g = vSum;
                    if (g < 0)
                        g = 0;
                    if (g > 255)
                        g = 255;
                    //g = 255 - g;
                    newPicture.SetPixel(r, c, Color.FromArgb(g, g, g));
                }
            }

            this.pictureBox2.Image = newPicture;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Bitmap picture = new Bitmap(pictureBox2.Image);
            Bitmap newPicture = new Bitmap(pictureBox1.Image);
            int[,] vertical = new int[3, 3] { { -1, -2, -1 }, { 0, 0, 0 }, { 1, 2, 1 } };
            int[,] horizental = new int[3, 3] { { -1, 0, 1 }, { -2, 0, 2 }, { -1, 0, 1 } };
            for (int r = 1; r < picture.Width - 1; r++)
            {
                for (int c = 1; c < picture.Height - 1; c++)
                {
                    int vSum = 0, hSum = 0;
                    Color center = picture.GetPixel(r, c);
                    for (int i = -1; i <= 1; i++)
                    {
                        for (int j = -1; j <= 1; j++)
                        {
                            Color co = picture.GetPixel(r + i, c + j);
                            int grayscale = (co.R + co.G + co.B) / 3;
                            vSum += grayscale * vertical[i + 1, j + 1];
                            hSum += grayscale * horizental[i + 1, j + 1];
                        }
                    }
                    int g = (int)Math.Sqrt(vSum * vSum + hSum * hSum);
                    //g = 255 - g;
                    if (g < 0)
                        g = 0;
                    if (g > 255)
                        g = 255;
                    newPicture.SetPixel(r, c, Color.FromArgb(g, g, g));
                }
            }
            this.pictureBox2.Image = newPicture;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Bitmap apple = new Bitmap(pictureBox2.Image);
            for (int r = 0; r < apple.Width; r++)
            {
                for (int c = 0; c < apple.Height; c++)
                {
                    Color co = apple.GetPixel(r, c);
                    int gray = (int)(co.R + co.G + co.B) / 3;
                    apple.SetPixel(r, c, Color.FromArgb(gray, gray, gray));
                }
            }
            this.pictureBox2.Image = apple;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Bitmap picture = new Bitmap(pictureBox2.Image);
            Bitmap newPicture = new Bitmap(pictureBox1.Image);
            for (int r = 1; r < picture.Width - 1; r++)
            {
                for (int c = 1; c < picture.Height - 1; c++)
                {
                    Color center = picture.GetPixel(r, c);
                    Color north = picture.GetPixel(r - 1, c);
                    Color south = picture.GetPixel(r + 1, c);
                    Color west = picture.GetPixel(r, c - 1);
                    Color east = picture.GetPixel(r, c + 1);
                    int R = 4 * center.R - (north.R + south.R + west.R + east.R);
                    int G = 4 * center.G - (north.G + south.G + west.G + east.G);
                    int B = 4 * center.B - (north.B + south.B + west.B + east.B);
                    int g = (R + G + B) / 3;
                    if (g < 0)
                        g = 0;
                    if (g > 255)
                        g = 255;
                    /*
                    if (g > 50)
                        g = 255;
                    */
                    newPicture.SetPixel(r, c, Color.FromArgb(g, g, g));
                }
            }
            this.pictureBox2.Image = newPicture;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            int[,] LoG = new int[5, 5] { { 0, 0, -1, 0, 0 }, { 0, -1, -2, -1, 0 }, { -1, -2, 16, -2, -1 }, { 0, -1, -2, -1, 0 }, { 0, 0, -1, 0, 0 } };
            int edge = (5 - 1) / 2;
            Bitmap picture = new Bitmap(pictureBox2.Image);
            for (int r = edge; r < picture.Width - edge; r++)
            {
                for (int c = edge; c < picture.Height - edge; c++)
                {
                    int R = 0, G = 0, B = 0;
                    for (int i = -edge; i <= edge; i++)
                    {
                        for (int j = -edge; j <= edge; j++)
                        {
                            Color co = picture.GetPixel(r + i, c + j);
                            R += co.R * (LoG[i + edge, j + edge]);
                            G += co.G * (LoG[i + edge, j + edge]);
                            B += co.B * (LoG[i + edge, j + edge]);
                        }
                    }
                    if (R < 0)
                        R = 0;
                    if (R > 255)
                        R = 255;
                    if (G < 0)
                        G = 0;
                    if (G > 255)
                        G = 255;
                    if (B < 0)
                        B = 0;
                    if (B > 255)
                        B = 255;
                    picture.SetPixel(r, c, Color.FromArgb(R, G, B));
                }
            }
            this.pictureBox2.Image = picture;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            Bitmap apple = new Bitmap(pictureBox2.Image);
            for (int r = 0; r < apple.Width; r++)
            {
                for (int c = 0; c < apple.Height; c++)
                {
                    Color co = apple.GetPixel(r, c);
                    int C = 100, b = 10;
                    int R = (int)(C * Math.Log(co.R + 1, b));
                    int G = (int)(C * Math.Log(co.G + 1, b));
                    int B = (int)(C * Math.Log(co.B + 1, b));
                    apple.SetPixel(r, c, Color.FromArgb(R, G, B));
                }
            }
            this.pictureBox2.Image = apple;
        }

        private void button9_Click(object sender, EventArgs e)
        {
            Bitmap apple = new Bitmap(pictureBox2.Image);
            for (int r = 0; r < apple.Width; r++)
            {
                for (int c = 0; c < apple.Height; c++)
                {
                    Color co = apple.GetPixel(r, c);
                    apple.SetPixel(r, c, Color.FromArgb(255 - co.R, 255 - co.G, 255 - co.B));
                }
            }
            this.pictureBox2.Image = apple;
        }

        private void button10_Click(object sender, EventArgs e)
        {
            Bitmap apple = new Bitmap(pictureBox2.Image);
            for (int r = 0; r < apple.Width; r++)
            {
                for (int c = 0; c < apple.Height; c++)
                {
                    Color co = apple.GetPixel(r, c);
                    int epsilon = 0;
                    double C = 50, gamma = 0.2;
                    int R = (int)(C * Math.Pow(co.R + epsilon, gamma));
                    int G = (int)(C * Math.Pow(co.G + epsilon, gamma));
                    int B = (int)(C * Math.Pow(co.B + epsilon, gamma));
                    apple.SetPixel(r, c, Color.FromArgb(R, G, B));
                }
            }
            this.pictureBox2.Image = apple;
        }

        private void button11_Click(object sender, EventArgs e)
        {
            int edge = (5 - 1) / 2;
            Bitmap picture = new Bitmap(pictureBox2.Image);
            for (int r = edge; r < picture.Width - edge; r++)
            {
                for (int c = edge; c < picture.Height - edge; c++)
                {
                    int sumR = 0, sumG = 0, sumB = 0;
                    for (int i = -edge; i <= edge; i++)
                    {
                        for (int j = -edge; j <= edge; j++)
                        {
                            Color co = picture.GetPixel(r + i, c + j);
                            sumR += co.R;
                            sumG += co.G;
                            sumB += co.B;
                        }
                    }
                    picture.SetPixel(r, c, Color.FromArgb(sumR/25, sumG/25, sumB/25));
                }
            }
            this.pictureBox2.Image = picture;
        }

        private void button12_Click(object sender, EventArgs e)
        {
            pictureBox2.Image = pictureBox1.Image;
        }

        private void button13_Click(object sender, EventArgs e)
        {
            Bitmap picture1 = new Bitmap(pictureBox1.Image);
            Bitmap picture2 = new Bitmap(pictureBox2.Image);
            for (int r = 0; r < picture1.Width; r++)
            {
                for (int c = 0; c < picture1.Height; c++)
                {
                    Color c1 = picture1.GetPixel(r, c);
                    Color c2 = picture2.GetPixel(r, c);

                    picture2.SetPixel(r, c, Color.FromArgb(c1.R+c2.R>255? 255: c1.R + c2.R, c1.G + c2.G > 255 ? 255 : c1.G + c2.G, c1.B + c2.B > 255 ? 255 : c1.B + c2.B));
                }
            }
            this.pictureBox2.Image = picture2;
        }

        private void button14_Click(object sender, EventArgs e)
        {
            Bitmap picture = new Bitmap(pictureBox1.Image);
            for (int r = 0; r < picture.Width; r++)
            {
                for (int c = 0; c < picture.Height; c++)
                {
                    Color co = picture.GetPixel(r, c);
                    picture.SetPixel(r, c, Color.FromArgb(co.R & 128 * 255, co.G & 128 * 255, co.B & 128 * 255));
                }
            }
            this.pictureBox2.Image = picture;
        }

        private void button15_Click(object sender, EventArgs e)
        {
            Bitmap picture = new Bitmap(pictureBox1.Image);
            for (int r = 0; r < picture.Width; r++)
            {
                for (int c = 0; c < picture.Height; c++)
                {
                    Color co = picture.GetPixel(r, c);
                    int p = ((co.R + co.G + co.B) / 3)>200? 255:0;
                    picture.SetPixel(r, c, Color.FromArgb(p, p, p));
                }
            }
            this.pictureBox2.Image = picture;
        }

        private void button16_Click(object sender, EventArgs e)
        {
            Bitmap picture = new Bitmap(pictureBox1.Image);
            List<int> g1 = new List<int>();
            List<int> g2 = new List<int>();
            double T = 100;
            double Tzero = 0;
            while (T-Tzero>0)
            {
                for (int r = 0; r < picture.Width; r++)
                {
                    for (int c = 0; c < picture.Height; c++)
                    {
                        Color co = picture.GetPixel(r, c);
                        if (co.R > T)
                        {
                            g1.Add(co.R);
                        }
                        else
                        {
                            g2.Add(co.R);
                        }
                    }
                }
                double mean1 = g1.Average();
                double mean2 = g2.Average();
                T = (mean1 + mean2) / 2;
                Tzero = T;
            }


            for (int r = 0; r < picture.Width; r++)
            {
                for (int c = 0; c < picture.Height; c++)
                {
                    Color co = picture.GetPixel(r, c);
                    int g;
                    if (co.R>T)
                    {
                        g = 255;
                    }else
                    {
                        g = 0;
                    }
                    picture.SetPixel(r, c, Color.FromArgb(g, g, g));
                }
            }
            this.pictureBox2.Image = picture;
        }
    }
}

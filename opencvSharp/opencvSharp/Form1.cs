using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//using opencvsharp
using OpenCvSharp;
using OpenCvSharp.Extensions;

namespace opencvSharp
{
    public partial class Form1 : Form
    {
        System.Drawing.Point RectstartPoint;//获取鼠标按下时的坐标
        System.Drawing.Rectangle Rect;      //重绘区域
        bool blnDraw = false;               //重新绘制感兴趣区域标志
        Bitmap mapHist;                     //存储数据的位图
        int[] countpixel;                   //存储每个直方图的数据
        int maxPixel;
        Mat ImageROI;
        Mat img;
        System.Drawing.Rectangle RealImageRect;
        public Form1()
        {
            InitializeComponent();
        }

        private void img_show_Click(object sender, EventArgs e)
        {
            img = new Mat("D:/postgraduation/detect_Chyleblood/program/img/img_lemon.png", ImreadModes.Color);
            pictureBox1.Image = img.ToBitmap();
        }
        //获取鼠标开始按下时的坐标
        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            RectstartPoint = e.Location;
            Invalidate();
            blnDraw = true;
            
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if(blnDraw)
            {
                if(e.Button != MouseButtons.Left)//如果不是鼠标左键按下
                {
                    return;
                }
                System.Drawing.Point tempendPoint = e.Location;//记录框的位置和大小
                //pictureBox1上开始点的坐标

                Rect.Location = new System.Drawing.Point(
                    Math.Min(RectstartPoint.X, tempendPoint.X), 
                    Math.Min(RectstartPoint.Y, tempendPoint.Y));
                //picture上矩形的大小
                Rect.Size = new System.Drawing.Size(
                    Math.Abs(RectstartPoint.X - tempendPoint.X),
                    Math.Abs(RectstartPoint.Y - tempendPoint.Y));
                pictureBox1.Invalidate();
                //最后点的位置
                int X0, Y0;
                Utilities.ConvertCoordinates(pictureBox1, out X0, out Y0, e.X,e.Y);
                //在控件中
                textBox1.Text = Convert.ToString("picturebox最后的点坐标为" + e.X + "," + e.Y);
                textBox2.Text = Convert.ToString("picturebox开始点的坐标为" + RectstartPoint.X + "," + RectstartPoint.Y);
                textBox3.Text = Convert.ToString("picturebox的大小为" + Rect.X + "," + Rect.Y);
                //Create ROI感兴趣区域
                Utilities.ConvertCoordinates(pictureBox1, out X0, out Y0, RectstartPoint.X, RectstartPoint.Y);
                int X1, Y1;
                Utilities.ConvertCoordinates(pictureBox1, out X1, out Y1, tempendPoint.X, tempendPoint.Y);
                //感兴趣区域 左上角坐标-宽-高
                RealImageRect.Location = new System.Drawing.Point(Math.Min(X0, X1), Math.Min(Y0, Y1));
                RealImageRect.Size = new System.Drawing.Size(Math.Abs(X0 - X1), Math.Abs(Y0 - Y1));

                Rect tmp_rect = new Rect(RealImageRect.X, RealImageRect.Y, RealImageRect.Width, RealImageRect.Height);
                ImageROI = new Mat(img, tmp_rect);            
            }

        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            pictureBox2.Image = ImageROI.ToBitmap();
            blnDraw = false;
        }
        //感兴趣框的绘制
        private void pictureBox1_Paint(object sender,PaintEventArgs e)
        {
            if(blnDraw)
            {
                if(pictureBox1.Image != null)
                {
                    if(Rect != null && Rect.Width>0 && Rect.Height>0)
                    {
                        e.Graphics.DrawRectangle(new Pen(Color.Red, 1), Rect);//重新绘制的颜色为红色
                    }
                }
            }
        }
        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
        
        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }


        private void Color_Histogram_Click(object sender, EventArgs e)
        {
            if(pictureBox2.Image == null)
            {
                //如果pixtureBox2中无图像
            }
            else
            {
                mapHist = (Bitmap)pictureBox2.Image;
                countpixel = new int[256];//对256个灰度级进行分层
                //将图像数据复制到byte中
                Rectangle rect = new Rectangle(0, 0, mapHist.Width, mapHist.Height);
                BitmapData mapdata = mapHist.LockBits(rect, ImageLockMode.ReadWrite, mapHist.PixelFormat);
                IntPtr intPtr = mapdata.Scan0;
                int bytes = mapHist.Width * mapHist.Height * 3;
                Byte[] grayvalues = new byte[bytes];
                Marshal.Copy(intPtr, grayvalues, 0, bytes);
                //统计直方图信息
                byte temp = 0;
                maxPixel = 0;
                Array.Clear(countpixel, 0, 256);
                for(int i=0;i<bytes;i++)
                {
                    temp = grayvalues[i];
                    countpixel[temp]++;
                    if(countpixel[temp]>maxPixel)
                    {
                        maxPixel = countpixel[temp];
                    }
                }
                Marshal.Copy(grayvalues, 0, intPtr, bytes);
                mapHist.UnlockBits(mapdata);
                Graphics g = pictureBox3.CreateGraphics();
                Pen curPen = new Pen(Brushes.Black, 1);
                g.DrawLine(curPen, 50, 240, 320, 240);
                g.DrawLine(curPen, 50, 240, 50, 30);
                g.DrawLine(curPen, 100, 240, 100, 242);
                g.DrawLine(curPen, 150, 240, 150, 242);
                g.DrawLine(curPen, 200, 240, 200, 242);
                g.DrawLine(curPen, 250, 240, 250, 242);
                g.DrawLine(curPen, 300, 240, 300, 242);
                g.DrawString("0", new Font("New Timer", 8), Brushes.Black, new PointF(46, 242));
                g.DrawString("50", new Font("New Timer", 8), Brushes.Black, new PointF(92, 242));
                g.DrawString("100", new Font("New Timer", 8), Brushes.Black, new PointF(139, 242));
                g.DrawString("150", new Font("New Timer", 8), Brushes.Black, new PointF(189, 242));
                g.DrawString("200", new Font("New Timer", 8), Brushes.Black, new PointF(239, 242));
                g.DrawString("250", new Font("New Timer", 8), Brushes.Black, new PointF(289, 242));
                g.DrawLine(curPen, 48, 40, 50, 40);
                g.DrawString("0", new Font("New Timer", 8), Brushes.Black, new PointF(34, 234));
                g.DrawString(maxPixel.ToString(), new Font("New Timer", 8), Brushes.Black, new PointF(18, 34));

                double Temp = 0;
                for (int i = 0; i < 256; i++)
                {
                    Temp = 200.0 * countpixel[i] / maxPixel;
                    g.DrawLine(curPen, 50 + i, 240, 50 + i, 240 - (int)Temp);
                }
                curPen.Dispose();

            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            
            Bitmap RGB_map;
            BitmapData RGBmap_Data;
            Mat RGB_mat;
            Byte[] R_byte, G_byte, B_byte;
            Byte[] values;
            int R_count=0, G_count=0, B_count=0;
            int R_maxpixel = 0, G_maxpixel = 0, B_maxpixel = 0;
            int Temp = 0;
            IntPtr ptr;
            if(pictureBox2.Image == null)
            {
                //如果pictureBox2为空
            }
            else
            {
                //当pictureBox2不为空的情况
                R_byte = new Byte[256];
                G_byte = new Byte[256];
                B_byte = new Byte[256];
                RGB_map = (Bitmap)pictureBox2.Image;
                Rectangle rect = new Rectangle(0, 0, RGB_map.Width, RGB_map.Height);
                RGBmap_Data = RGB_map.LockBits(rect, ImageLockMode.ReadWrite, RGB_map.PixelFormat);
                ptr = RGBmap_Data.Scan0;
                int bytenumber = RGB_map.Width * RGB_map.Height * 3;
                values = new Byte[bytenumber];
                Marshal.Copy(ptr, values, 0, bytenumber);
                Array.Clear(R_byte, 0, 256);
                Array.Clear(G_byte, 0, 256);
                Array.Clear(B_byte, 0, 256);
                for (int i = 0; i < (int)bytenumber/3; i = i + 3)
                {
                    int temp = values[i];
                    R_byte[temp]++;
                    R_count++;
                    temp = values[i + 1];
                    G_byte[temp]++;
                    G_count++;
                    temp = values[i + 2];
                    B_byte[temp]++;
                    B_count++;
                }
                R_maxpixel = Utilities.max(R_byte);
                G_maxpixel = Utilities.max(G_byte);
                B_maxpixel = Utilities.max(B_byte);
                Marshal.Copy(values, 0, ptr, bytenumber);
                RGB_map.UnlockBits(RGBmap_Data);
                //绘制直方图
                Graphics R = pictureBox3.CreateGraphics();
                Pen R_pen = new Pen(Brushes.Red, 1);
                R.DrawLine(R_pen, 50, 110, 320,110 );
                R.DrawLine(R_pen, 50, 30, 50, 110);
                Graphics G = pictureBox3.CreateGraphics();
                Pen G_pen = new Pen(Brushes.Green, 1);
                G.DrawLine(G_pen, 50, 210, 320, 210);
                G.DrawLine(G_pen, 50, 130, 50, 210);
                Graphics B = pictureBox3.CreateGraphics();
                Pen B_pen = new Pen(Brushes.Blue, 1);
                B.DrawLine(B_pen, 50, 310, 320, 310);
                B.DrawLine(B_pen, 50, 230, 50, 310);
                for (int i = 0; i < 256; i++)
                {
                    Temp = (int)80.0 * R_byte[i] / R_maxpixel;
                    R.DrawLine(R_pen, 50 + i, 110, 50 + i, 110 - (int)Temp);
                    Temp = (int)80.0 * G_byte[i] / G_maxpixel;
                    G.DrawLine(G_pen, 50 + i, 210, 50 + i, 210 - (int)Temp);
                    Temp = (int)80.0 * B_byte[i] / B_maxpixel;
                    B.DrawLine(B_pen, 50 + i, 310, 50 + i, 310 - (int)Temp);
                }
                R_pen.Dispose();
                G_pen.Dispose();
                B_pen.Dispose();
            }
        }
    }
}

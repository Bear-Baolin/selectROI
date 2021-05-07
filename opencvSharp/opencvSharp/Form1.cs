using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
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
    }
}

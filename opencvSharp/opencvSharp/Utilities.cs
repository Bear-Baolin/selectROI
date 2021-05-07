using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace opencvSharp
{
    class Utilities
    {
        //坐标转换
        /************************
         * 图片左边转换
         * Input输入：pictureBox 坐标X，Y
         * Output输出：Image图像上对应的坐标
         * ***********************/
         public static void ConvertCoordinates(PictureBox pic,
             out int X0,out int Y0,int x,int y)
        {
            int pic_hgt = pic.ClientSize.Height;
            int pic_wid = pic.ClientSize.Width;
            int img_hgt = pic.Image.Height;
            int img_wid = pic.Image.Width;
            X0 = x;
            Y0 = y;
            switch(pic.SizeMode)
            {
                case PictureBoxSizeMode.AutoSize:
                case PictureBoxSizeMode.StretchImage:
                    X0 = (int)(img_wid * x / (float)pic_wid);
                    Y0 = (int)(img_hgt * y / (float)pic_hgt);
                    break;
            }
        }
    }
}

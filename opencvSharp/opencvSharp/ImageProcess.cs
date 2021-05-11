using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenCvSharp;
using OpenCvSharp.Extensions;
namespace opencvSharp
{
    /**********************************
     * 图像处理类
     * 类中应包括对彩色图像的颜色空间变换、
     * 绘制直方图、以及基于像素和基于直方图的图像处理等
     * *******************************/
    class ImageProcess
    {
        /********************************
         * 颜色空间的转换
         * 输入为RGB通道的图像
         * 输出为HSV空间的图像
         * ****************************/
        public void RbtoHsv(float R,float G,float B,out float H,out float S,out float V)
        {
            float min, max, temp;
            temp = Math.Min(R, G);
            min = Math.Min(temp, B);
            temp = Math.Max(R, G);
            max = Math.Max(temp, B);
            H = 0;
            if(max == min)
            {
                H = 0;
            }
            else if(max == R && G > B)
            {
                H = 60 * (G - B) * 1.0f / (max - min) + 0;
            }
            else if(max == R && G < B)
            {
                H = 60 * (B - G) * 1.0f / (max - min) + 360;
            }
            else if(max == G)
            {
                H = 60 * (B - R) * 1.0f / (max - min) + 120;
            }
            else if(max == B)
            {
                H = 60 * (R - G) * 1.0f / (max - min) + 240;
            }
            if(max == 0)
            {
                S = 0;
            }
            else
            {
                S = (max - min) * 1.0f / max;
            }
            V = max;
        }
    }
}

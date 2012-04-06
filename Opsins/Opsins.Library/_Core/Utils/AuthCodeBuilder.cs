using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Opsins
{
    /// <summary>
    /// 验证码生成器
    /// </summary>
    public class AuthCodeBuilder
    {
        public AuthCodeBuilder()
        {
            Length = this._length;
            FontSize = this._fontSize;
            Chaos = this._chaos;
            BackgroundColor = this._backgroundColor;
            ChaosColor = this._chaosColor;
            CodeSerial = this._codeSerial;
            Colors = this._colors;
            Fonts = this._fonts;
            Padding = this._padding;
        }

        #region 验证码长度(默认4个验证码的长度)
        int _length = 4;
        public int Length
        {
            get { return _length; }
            set { _length = value; }
        }
        #endregion

        #region 验证码字体大小(为了显示扭曲效果，默认40像素，可以自行修改)
        int _fontSize = 40;
        public int FontSize
        {
            get { return _fontSize; }
            set { _fontSize = value; }
        }
        #endregion

        #region 边框补(默认1像素)
        int _padding = 2;
        public int Padding
        {
            get { return _padding; }
            set { _padding = value; }
        }
        #endregion

        #region 是否输出燥点(默认不输出)
        bool _chaos = true;
        public bool Chaos
        {
            get { return _chaos; }
            set { _chaos = value; }
        }
        #endregion

        #region 输出燥点的颜色(默认灰色)
        Color _chaosColor = Color.LightGray;
        public Color ChaosColor
        {
            get { return _chaosColor; }
            set { _chaosColor = value; }
        }
        #endregion

        #region 自定义背景色(默认白色)
        Color[] _backgroundColor = { Color.YellowGreen, Color.Yellow, Color.WhiteSmoke, Color.Wheat, Color.Gainsboro, Color.Turquoise, Color.Thistle, Color.Pink };
        public Color[] BackgroundColor
        {
            get { return _backgroundColor; }
            set { _backgroundColor = value; }
        }
        #endregion

        #region 自定义随机颜色数组
        Color[] _colors = { Color.Black, Color.Red, Color.DarkBlue, Color.Green, Color.Orange, Color.Brown, Color.DarkCyan, Color.Purple, Color.DeepPink, Color.Aqua };
        public Color[] Colors
        {
            get { return _colors; }
            set { _colors = value; }
        }
        #endregion

        #region 自定义字体数组
        string[] _fonts = { "Constantia" };
        public string[] Fonts
        {
            get { return _fonts; }
            set { _fonts = value; }
        }
        #endregion

        #region 自定义随机码字符串序列(使用逗号分隔)
        //string codeSerial = "0,1,2,3,4,5,6,7,8,9,a,b,c,d,e,f,g,h,j,k,l,m,n,p,q,r,s,t,u,v,w,x,y,z,A,B,C,D,E,F,G,H,J,K,L,M,N,P,Q,R,S,T,U,V,W,X,Y,Z";
        string _codeSerial = "A,B,C,D,E,F,G,H,J,K,L,M,N,P,Q,R,S,T,U,V,W,X,Y,Z";
        public string CodeSerial
        {
            get { return _codeSerial; }
            set { _codeSerial = value; }
        }
        #endregion

        #region 产生波形滤镜效果

        private const double PI = 3.1415926535897932384626433832795;
        private const double PI2 = 6.283185307179586476925286766559;

        /// <summary>
        /// 正弦曲线Wave扭曲图片（Edit By 51aspx.com）
        /// </summary>
        /// <param name="srcBmp">图片路径</param>
        /// <param name="bXDir">如果扭曲则选择为True</param>
        /// <param name="nMultValue">波形的幅度倍数，越大扭曲的程度越高，一般为3</param>
        /// <param name="dPhase">波形的起始相位，取值区间[0-2*PI)</param>
        /// <returns></returns>
        public System.Drawing.Bitmap TwistImage(Bitmap srcBmp, bool bXDir, double dMultValue, double dPhase)
        {
            System.Drawing.Bitmap destBmp = new Bitmap(srcBmp.Width, srcBmp.Height);

            // 将位图背景填充为白色
            System.Drawing.Graphics graph = System.Drawing.Graphics.FromImage(destBmp);
            graph.FillRectangle(new SolidBrush(System.Drawing.Color.White), 0, 0, destBmp.Width, destBmp.Height);
            graph.Dispose();

            double dBaseAxisLen = bXDir ? (double)destBmp.Height : (double)destBmp.Width;

            for (int i = 0; i < destBmp.Width; i++)
            {
                for (int j = 0; j < destBmp.Height; j++)
                {
                    double dx = 0;
                    dx = bXDir ? (PI2 * (double)j) / dBaseAxisLen : (PI2 * (double)i) / dBaseAxisLen;
                    dx += dPhase;
                    double dy = Math.Sin(dx);

                    // 取得当前点的颜色
                    int nOldX = 0, nOldY = 0;
                    nOldX = bXDir ? i + (int)(dy * dMultValue) : i;
                    nOldY = bXDir ? j : j + (int)(dy * dMultValue);

                    System.Drawing.Color color = srcBmp.GetPixel(i, j);
                    if (nOldX >= 0 && nOldX < destBmp.Width
                     && nOldY >= 0 && nOldY < destBmp.Height)
                    {
                        destBmp.SetPixel(nOldX, nOldY, color);
                    }
                }
            }

            return destBmp;
        }

        #endregion

        #region 生成校验码图片
        public Bitmap CreateImageCode(string code)
        {
            int fSize = FontSize;
            int fWidth = fSize + Padding;

            int imageWidth = (int)(code.Length * fWidth) + 4 + Padding * 2;
            int imageHeight = fSize * 2 + Padding;

            System.Drawing.Bitmap image = new System.Drawing.Bitmap(imageWidth, imageHeight);

            Graphics g = Graphics.FromImage(image);

            Random rand = new Random();

            int bkindex = rand.Next(_backgroundColor.Length - 1);
            g.Clear(BackgroundColor[bkindex]);

            //给背景添加随机生成的燥点
            if (this.Chaos)
            {

                Pen pen = new Pen(ChaosColor, 0);
                int c = Length * 10;

                for (int i = 0; i < c; i++)
                {
                    int x = rand.Next(image.Width);
                    int y = rand.Next(image.Height);

                    g.DrawRectangle(pen, x, y, 1, 1);
                }
            }

            int left = 0, top = 0, top1 = 1, top2 = 1;

            int n1 = (imageHeight - FontSize - Padding * 2);
            int n2 = n1 / 4;
            top1 = n2;
            top2 = n2 * 2;

            Font f;
            Brush b;

            int cindex, findex;

            //随机字体和颜色的验证码字符
            for (int i = 0; i < code.Length; i++)
            {
                cindex = rand.Next(Colors.Length - 1);
                findex = rand.Next(Fonts.Length - 1);

                f = new System.Drawing.Font(Fonts[findex], fSize, System.Drawing.FontStyle.Bold);
                b = new System.Drawing.SolidBrush(Colors[cindex]);

                if (i % 2 == 1)
                {
                    top = top2;
                }
                else
                {
                    top = top1;
                }

                left = i * fWidth;

                g.DrawString(code.Substring(i, 1), f, b, left, top);
            }

            //画一个边框 边框颜色为Color.Gainsboro
            g.DrawRectangle(new Pen(Color.Gainsboro, 0), 0, 0, image.Width - 1, image.Height - 1);
            g.Dispose();

            //产生波形（Add By 51aspx.com）
            //image = TwistImage(image, true, 8, 4);

            return image;
        }
        #endregion

        #region 将创建好的图片输出到页面
        //public void CreateImageOnPage(string code, HttpContext context)
        //{
        //    System.IO.MemoryStream ms = new System.IO.MemoryStream();
        //    Bitmap image = CreateBitmap(code);

        //    image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);

        //    context.Response.ClearContent();
        //    context.Response.ContentType = "image/Jpeg";
        //    context.Response.BinaryWrite(ms.GetBuffer());

        //    ms.Close();
        //    ms = null;
        //    image.Dispose();
        //    image = null;
        //}
        #endregion

        #region 生成随机字符码
        public string CreateRandomCode(int codeLen)
        {
            if (codeLen == 0)
            {
                codeLen = Length;
            }

            string[] arr = CodeSerial.Split(',');

            string code = "";

            int randValue = -1;

            Random rand = new Random(unchecked((int)DateTime.Now.Ticks));

            for (int i = 0; i < codeLen; i++)
            {
                randValue = rand.Next(0, arr.Length - 1);

                code += arr[randValue];
            }

            return code;
        }
        public string CreateRandomCode()
        {
            return CreateRandomCode(0);
        }
        #endregion



        /// <summary>
        /// 根据指定参数返回BitMap对象
        /// 引用如下：
        /// using System.Drawing;
        /// 调用例子如下：
        ///     eg1、保存为图象文件为
        ///     Bitmap srBmp = srBitmap(srs);
        ///     srBmp.Save(Directory.GetCurrentDirectory() + "\\srs.gif", System.Drawing.Imaging.ImageFormat.Gif);
        ///     srBmp.Dispose();
        ///     eg2。网页中调用方式如下
        ///     Bitmap srBmp = srBitmap(srs);
        ///     System.IO.MemoryStream srMS = new System.IO.MemoryStream();
        ///     srBmp.Save(srMS,System.Drawing.Imaging.ImageFormat.Gif);
        ///     Response.ClearContent();
        ///     Response.ContentType = "image/gif";
        ///     Response.BinaryWrite(srMS.ToArray());
        ///     srBmp.Dispose();
        /// </summary>
        /// <param name="srs"></param>
        /// <returns></returns>
        public static Bitmap CreateBitmap(string srs)
        {
            //定义图片背景颜色
            Color[] BackgroundColors = { Color.AliceBlue, Color.Yellow, Color.LightBlue, Color.Wheat, Color.Gainsboro, Color.Pink };
            //定义图片弯曲的角度
            int srseedangle = 50;
            //定义图象
            Bitmap srBmp = new Bitmap(srs.Length * 30, 40);
            //画图
            Graphics srGraph = Graphics.FromImage(srBmp);

            //定义随即数
            Random srRandom = new Random();

            //图像背景
            int bkindex = srRandom.Next(BackgroundColors.Length - 1);
            srGraph.Clear(Color.AliceBlue);

            //给图象画边框
            srGraph.DrawRectangle(new Pen(Color.Gainsboro, 0), 0, 0, srBmp.Width - 1, srBmp.Height - 1);
            
            //定义画笔
            Pen srPen = new Pen(Color.LightGray, 0);
            //画噪点
            for (int i = 0; i < 50; i++)
            {
                srGraph.DrawRectangle(srPen, srRandom.Next(1, srBmp.Width - 2), srRandom.Next(1, srBmp.Height - 2), 1, 1);
            }

            //画线
            Point[] pointArr = new Point[] 
            {
                new Point(0, 0),
                new Point(0, 10),
                new Point(10, 20),
                new Point(20, 15),
                new Point(30, 0),
                new Point(40, 12),
                new Point(20, 30),
                new Point(10, 30),
                new Point(0, 50),
                new Point(0, 30),
                
                new Point(100, 0),
                new Point(100, 10),
                new Point(100, 20),
                new Point(100, 15),
                new Point(100, 0),
                new Point(100, 12),
                new Point(100, 30),
                new Point(100, 30),
                new Point(100, 50),
                new Point(100, 30)
            };

            Point[] points = new Point[4];
            Point[] points2 = new Point[4];
            for (int i = 0; i < 4; i++)
            {
                points[i] = pointArr[srRandom.Next(20)];
                points2[i] = pointArr[srRandom.Next(20)];
            }
            Brush brush = new SolidBrush(Color.LightSkyBlue);
            Pen pen = new Pen(brush, 3);
            pen.DashPattern = new float[] { 2, 10 };
            pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
            srGraph.DrawBeziers(pen, points);
            srGraph.DrawBeziers(pen, points2);

            //将字符串转化为字符数组
            char[] srchars = srs.ToCharArray();
            //封状文本
            StringFormat srFormat = new StringFormat(StringFormatFlags.NoClip);
            //设置文本垂直居中
            srFormat.Alignment = StringAlignment.Center;
            //设置文本水平居中
            srFormat.LineAlignment = StringAlignment.Center;
            //定义字体颜色
            Color[] srColors = { Color.Black, Color.Red, Color.DarkSalmon, Color.DeepPink, Color.DarkOliveGreen, Color.DarkCyan };
            //定义字体
            string[] srFonts = { "Microsoft Sans Serif", "Comic Sans MS", "Arial", "宋体" };
            //填充图形
            Brush srBrush = new SolidBrush(srColors[srRandom.Next(5)]);
            //循环画出每个字符
            for (int i = 0, j = srchars.Length; i < j; i++)
            {
                //定义字体 参数分别为字体样式 字体大小 字体字形
                Font srFont = new Font(srFonts[srRandom.Next(4)], srRandom.Next(18, 28), FontStyle.Regular);

                //定义坐标
                Point srPoint = new Point(22, 22);
                //定义倾斜角度
                float srangle = srRandom.Next(-srseedangle, srseedangle);
                //倾斜
                srGraph.TranslateTransform(srPoint.X, srPoint.Y);
                srGraph.RotateTransform(srangle);
                //填充字符
                srGraph.DrawString(srchars[i].ToString(), srFont, srBrush, 1, 1, srFormat);
                //回归正常
                srGraph.RotateTransform(-srangle);
                srGraph.TranslateTransform(2, -srPoint.Y);
            }
            srGraph.Dispose();
            return srBmp;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;

namespace Opsins
{
    /// <summary>
    /// 缩略模式
    /// </summary>
    public enum ThumbnailMode
    {
        /// <summary>
        /// 指定高宽缩放（可能变形）
        /// </summary>
        HW,
        /// <summary>
        /// 指定宽，高按比例       
        /// </summary>
        W,
        /// <summary>
        /// 指定高，宽按比例
        /// </summary>
        H,
        /// <summary>
        /// 指定高宽裁减（不变形） 
        /// </summary>
        CUT
    }

    /// <summary>
    /// 图片处理
    /// </summary>
    public static class ImageHelper
    {
        #region 生成缩略图

        /// <summary>
        /// 生成缩略图，默认缩略模式为高宽缩放（可能变形）
        /// </summary>
        /// <param name="originalImagePath">源图路径（物理路径）</param>
        /// <param name="zoomValue">缩放宽高值：new int[2,2] { {680,448},{279,184} }</param>
        public static void MakeThumbnail(string originalImagePath, int[,] zoomValue)
        {
            if (zoomValue.GetLength(1) > 2) throw new ArgumentException("缩放值错误");
            for (int i = 0; i < zoomValue.GetLength(0); i++)
            {
                MakeThumbnail(originalImagePath, zoomValue[i, 0], zoomValue[i, 1], ThumbnailMode.HW);
            }
        }

        /// <summary>
        /// 生成缩略图，默认缩略模式为高宽缩放（可能变形）
        /// </summary>
        /// <param name="originalImagePath">源图路径（物理路径）</param>
        /// <param name="width">缩略图宽度</param>
        /// <param name="height">缩略图高度</param>
        public static void MakeThumbnail(string originalImagePath, int width, int height)
        {
            MakeThumbnail(originalImagePath, width, height, ThumbnailMode.HW);
        }

        /// <summary>
        /// 生成缩略图
        /// </summary>
        /// <param name="originalImagePath">源图路径（物理路径）</param>
        /// <param name="width">缩略图宽度</param>
        /// <param name="height">缩略图高度</param>
        /// <param name="mode">生成缩略图的方式</param>    
        public static void MakeThumbnail(string originalImagePath, int width, int height, ThumbnailMode mode)
        {
            System.Drawing.Image originalImage = System.Drawing.Image.FromFile(originalImagePath);

            int towidth = width;
            int toheight = height;

            int x = 0;
            int y = 0;
            int ow = originalImage.Width;
            int oh = originalImage.Height;

            string extension = originalImagePath.GetSubstring('.', 1);
            //0:第一段文件路径，1：扩展名，2：宽，3：高，4：扩展名
            string thumbnailPath = string.Format("{0}.{1}.{2}x{3}.{4}", originalImagePath.Substring(0, originalImagePath.IndexOf(extension) - 1), extension, width, height, extension);

            switch (mode)
            {
                case ThumbnailMode.HW://指定高宽缩放（可能变形）                
                    break;
                case ThumbnailMode.W://指定宽，高按比例                    
                    toheight = originalImage.Height * width / originalImage.Width;
                    break;
                case ThumbnailMode.H://指定高，宽按比例
                    towidth = originalImage.Width * height / originalImage.Height;
                    break;
                case ThumbnailMode.CUT://指定高宽裁减（不变形）                
                    if ((double)originalImage.Width / (double)originalImage.Height > (double)towidth / (double)toheight)
                    {
                        oh = originalImage.Height;
                        ow = originalImage.Height * towidth / toheight;
                        y = 0;
                        x = (originalImage.Width - ow) / 2;
                    }
                    else
                    {
                        ow = originalImage.Width;
                        oh = originalImage.Width * height / towidth;
                        x = 0;
                        y = (originalImage.Height - oh) / 2;
                    }
                    break;
                default:
                    break;
            }

            //新建一个bmp图片
            System.Drawing.Image bitmap = new System.Drawing.Bitmap(towidth, toheight);

            //新建一个画板
            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(bitmap);

            //设置高质量插值法
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

            //设置高质量,低速度呈现平滑程度
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            //清空画布并以透明背景色填充
            g.Clear(System.Drawing.Color.Transparent);

            //在指定位置并且按指定大小绘制原图片的指定部分
            g.DrawImage(originalImage, new System.Drawing.Rectangle(0, 0, towidth, toheight),
                new System.Drawing.Rectangle(x, y, ow, oh),
                System.Drawing.GraphicsUnit.Pixel);
            try
            {
                bitmap.Save(thumbnailPath);
            }
            catch (System.Exception e)
            {
                throw e;
            }
            finally
            {
                originalImage.Dispose();
                bitmap.Dispose();
                g.Dispose();
            }
        }

        /// <summary>
        /// 生成缩略图，默认缩略模式为高宽缩放（可能变形）
        /// </summary>
        /// <param name="originalImagePath">源图路径（物理路径）</param>
        /// <param name="thumbnailPath">缩略图路径（物理路径）</param>
        /// <param name="width">缩略图宽度</param>
        /// <param name="height">缩略图高度</param>
        public static void MakeThumbnail(string originalImagePath, string thumbnailPath, int width, int height)
        {
            MakeThumbnail(originalImagePath, thumbnailPath, width, height, ThumbnailMode.HW);
        }

        /// <summary>
        /// 生成缩略图
        /// </summary>
        /// <param name="originalImagePath">源图路径（物理路径）</param>
        /// <param name="thumbnailPath">缩略图路径（物理路径）</param>
        /// <param name="width">缩略图宽度</param>
        /// <param name="height">缩略图高度</param>
        /// <param name="mode">生成缩略图的方式</param>    
        public static void MakeThumbnail(string originalImagePath, string thumbnailPath, int width, int height, ThumbnailMode mode)
        {
            System.Drawing.Image originalImage = System.Drawing.Image.FromFile(originalImagePath);

            int towidth = width;
            int toheight = height;

            int x = 0;
            int y = 0;
            int ow = originalImage.Width;
            int oh = originalImage.Height;

            switch (mode)
            {
                case ThumbnailMode.HW://指定高宽缩放（可能变形）                
                    break;
                case ThumbnailMode.W://指定宽，高按比例                    
                    toheight = originalImage.Height * width / originalImage.Width;
                    break;
                case ThumbnailMode.H://指定高，宽按比例
                    towidth = originalImage.Width * height / originalImage.Height;
                    break;
                case ThumbnailMode.CUT://指定高宽裁减（不变形）                
                    if ((double)originalImage.Width / (double)originalImage.Height > (double)towidth / (double)toheight)
                    {
                        oh = originalImage.Height;
                        ow = originalImage.Height * towidth / toheight;
                        y = 0;
                        x = (originalImage.Width - ow) / 2;
                    }
                    else
                    {
                        ow = originalImage.Width;
                        oh = originalImage.Width * height / towidth;
                        x = 0;
                        y = (originalImage.Height - oh) / 2;
                    }
                    break;
                default:
                    break;
            }

            //新建一个bmp图片
            System.Drawing.Image bitmap = new System.Drawing.Bitmap(towidth, toheight);

            //新建一个画板
            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(bitmap);

            //设置高质量插值法
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

            //设置高质量,低速度呈现平滑程度
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            //清空画布并以透明背景色填充
            g.Clear(System.Drawing.Color.Transparent);

            //在指定位置并且按指定大小绘制原图片的指定部分
            g.DrawImage(originalImage, new System.Drawing.Rectangle(0, 0, towidth, toheight),
                new System.Drawing.Rectangle(x, y, ow, oh),
                System.Drawing.GraphicsUnit.Pixel);
            try
            {
                //以jpg格式保存缩略图
                bitmap.Save(thumbnailPath);
                //bitmap.Save(thumbnailPath, System.Drawing.Imaging.ImageFormat.Jpeg);
            }
            catch (System.Exception e)
            {
                throw e;
            }
            finally
            {
                originalImage.Dispose();
                bitmap.Dispose();
                g.Dispose();
            }
        }


        /// <summary>
        /// 切割后生成缩略图
        /// </summary>
        /// <param name="originalImagePath">源图路径（物理路径）</param>
        /// <param name="thumbnailPath">缩略图路径（物理路径）</param>
        /// <param name="toW">缩略图最终宽度</param>
        /// <param name="toH">缩略图最终高度</param>
        /// <param name="X">X坐标（zoom为1时）</param>
        /// <param name="Y">Y坐标（zoom为1时）</param>
        /// <param name="W">选择区域宽（zoom为1时）</param>
        /// <param name="H">选择区域高（zoom为1时）</param>
        public static void MakeThumbnail(string originalImagePath, string thumbnailPath, int toW, int toH, int X, int Y, int W, int H)
        {
            System.Drawing.Image originalImage = System.Drawing.Image.FromFile(originalImagePath);
            int towidth = toW;
            int toheight = toH;
            int x = X;
            int y = Y;
            int ow = W;
            int oh = H;            //新建一个bmp图片
            System.Drawing.Image bitmap = new System.Drawing.Bitmap(towidth, toheight);

            //新建一个画板
            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(bitmap);

            //设置高质量插值法
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

            //设置高质量,低速度呈现平滑程度
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            //清空画布并以透明背景色填充
            g.Clear(System.Drawing.Color.Transparent);

            //在指定位置并且按指定大小绘制原图片的指定部分
            g.DrawImage(originalImage, new System.Drawing.Rectangle(0, 0, towidth, toheight),
            new System.Drawing.Rectangle(x, y, ow, oh),
            System.Drawing.GraphicsUnit.Pixel);
            try
            {
                //以jpg格式保存缩略图
                bitmap.Save(thumbnailPath, System.Drawing.Imaging.ImageFormat.Jpeg);

            }
            catch (System.Exception e)
            {
                throw e;
            }
            finally
            {
                originalImage.Dispose();
                bitmap.Dispose();
                g.Dispose();
            }
        }

        #endregion

        #region 在图片上增加文字水印
        /// <summary>
        /// 在图片上增加文字水印
        /// </summary>
        /// <param name="Path">原服务器图片路径</param>
        /// <param name="Path_sy">生成的带文字水印的图片路径</param>
        /// <param name="addText">水印文字</param>
        public static void AddWater(string Path, string Path_sy, string addText)
        {
            System.Drawing.Image image = System.Drawing.Image.FromFile(Path);
            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(image);
            g.DrawImage(image, 0, 0, image.Width, image.Height);
            System.Drawing.Font f = new System.Drawing.Font("Verdana", 60);
            System.Drawing.Brush b = new System.Drawing.SolidBrush(System.Drawing.Color.Green);

            g.DrawString(addText, f, b, 35, 35);
            g.Dispose();

            image.Save(Path_sy);
            image.Dispose();
        }
        #endregion

        #region 在图片上生成图片水印
        /// <summary>
        /// 加图片水印
        /// </summary>
        /// <param name="filename">文件名</param>
        /// <param name="watermarkFilename">水印文件名</param>
        /// <param name="watermarkStatus">图片水印位置:0=不使用 1=左上 2=中上 3=右上 4=左中 ... 9=右下</param>
        /// <param name="quality">是否是高质量图片 取值范围0--100</param> 
        /// <param name="watermarkTransparency">图片水印透明度 取值范围1--10 (10为不透明)</param>

        public static void AddImageSignPic(string Path, string filename, string watermarkFilename, int watermarkStatus, int quality, int watermarkTransparency)
        {
            System.Drawing.Image img = System.Drawing.Image.FromFile(Path);
            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(img);

            //设置高质量插值法
            //g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
            //设置高质量,低速度呈现平滑程度
            //g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            System.Drawing.Image watermark = new System.Drawing.Bitmap(watermarkFilename);

            if (watermark.Height >= img.Height || watermark.Width >= img.Width)
            {
                return;
            }

            System.Drawing.Imaging.ImageAttributes imageAttributes = new System.Drawing.Imaging.ImageAttributes();
            System.Drawing.Imaging.ColorMap colorMap = new System.Drawing.Imaging.ColorMap();

            colorMap.OldColor = System.Drawing.Color.FromArgb(255, 0, 255, 0);
            colorMap.NewColor = System.Drawing.Color.FromArgb(0, 0, 0, 0);
            System.Drawing.Imaging.ColorMap[] remapTable = { colorMap };

            imageAttributes.SetRemapTable(remapTable, System.Drawing.Imaging.ColorAdjustType.Bitmap);

            float transparency = 0.5F;
            if (watermarkTransparency >= 1 && watermarkTransparency <= 10)
            {
                transparency = (watermarkTransparency / 10.0F);
            }

            float[][] colorMatrixElements = {
                                                new float[] {1.0f,  0.0f,  0.0f,  0.0f, 0.0f},
                                                new float[] {0.0f,  1.0f,  0.0f,  0.0f, 0.0f},
                                                new float[] {0.0f,  0.0f,  1.0f,  0.0f, 0.0f},
                                                new float[] {0.0f,  0.0f,  0.0f,  transparency, 0.0f},
                                                new float[] {0.0f,  0.0f,  0.0f,  0.0f, 1.0f}
                                            };

            System.Drawing.Imaging.ColorMatrix colorMatrix = new System.Drawing.Imaging.ColorMatrix(colorMatrixElements);

            imageAttributes.SetColorMatrix(colorMatrix, System.Drawing.Imaging.ColorMatrixFlag.Default, System.Drawing.Imaging.ColorAdjustType.Bitmap);

            int xpos = 0;
            int ypos = 0;

            switch (watermarkStatus)
            {
                case 1:
                    xpos = (int)(img.Width * (float).01);
                    ypos = (int)(img.Height * (float).01);
                    break;
                case 2:
                    xpos = (int)((img.Width * (float).50) - (watermark.Width / 2));
                    ypos = (int)(img.Height * (float).01);
                    break;
                case 3:
                    xpos = (int)((img.Width * (float).99) - (watermark.Width));
                    ypos = (int)(img.Height * (float).01);
                    break;
                case 4:
                    xpos = (int)(img.Width * (float).01);
                    ypos = (int)((img.Height * (float).50) - (watermark.Height / 2));
                    break;
                case 5:
                    xpos = (int)((img.Width * (float).50) - (watermark.Width / 2));
                    ypos = (int)((img.Height * (float).50) - (watermark.Height / 2));
                    break;
                case 6:
                    xpos = (int)((img.Width * (float).99) - (watermark.Width));
                    ypos = (int)((img.Height * (float).50) - (watermark.Height / 2));
                    break;
                case 7:
                    xpos = (int)(img.Width * (float).01);
                    ypos = (int)((img.Height * (float).99) - watermark.Height);
                    break;
                case 8:
                    xpos = (int)((img.Width * (float).50) - (watermark.Width / 2));
                    ypos = (int)((img.Height * (float).99) - watermark.Height);
                    break;
                case 9:
                    xpos = (int)((img.Width * (float).99) - (watermark.Width));
                    ypos = (int)((img.Height * (float).99) - watermark.Height);
                    break;
            }

            g.DrawImage(watermark, new System.Drawing.Rectangle(xpos, ypos, watermark.Width, watermark.Height), 0, 0, watermark.Width, watermark.Height, System.Drawing.GraphicsUnit.Pixel, imageAttributes);

            System.Drawing.Imaging.ImageCodecInfo[] codecs = System.Drawing.Imaging.ImageCodecInfo.GetImageEncoders();
            System.Drawing.Imaging.ImageCodecInfo ici = null;
            foreach (System.Drawing.Imaging.ImageCodecInfo codec in codecs)
            {
                //if (codec.MimeType.IndexOf("jpeg") > -1)
                if (codec.MimeType.Contains("jpeg"))
                {
                    ici = codec;
                }
            }
            System.Drawing.Imaging.EncoderParameters encoderParams = new System.Drawing.Imaging.EncoderParameters();
            long[] qualityParam = new long[1];
            if (quality < 0 || quality > 100)
            {
                quality = 80;
            }
            qualityParam[0] = quality;

            System.Drawing.Imaging.EncoderParameter encoderParam = new System.Drawing.Imaging.EncoderParameter(System.Drawing.Imaging.Encoder.Quality, qualityParam);
            encoderParams.Param[0] = encoderParam;

            if (ici != null)
            {
                img.Save(filename, ici, encoderParams);
            }
            else
            {
                img.Save(filename);
            }

            g.Dispose();
            img.Dispose();
            watermark.Dispose();
            imageAttributes.Dispose();
        }

        /// <summary>
        /// 在图片上生成图片水印
        /// </summary>
        /// <param name="Path">原服务器图片路径</param>
        /// <param name="Path_syp">生成的带图片水印的图片路径</param>
        /// <param name="Path_sypf">水印图片路径</param>
        public static void AddWaterPic(string Path, string Path_syp, string Path_sypf)
        {
            System.Drawing.Image image = System.Drawing.Image.FromFile(Path);
            System.Drawing.Image copyImage = System.Drawing.Image.FromFile(Path_sypf);
            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(image);
            g.DrawImage(copyImage, new System.Drawing.Rectangle(image.Width - copyImage.Width, image.Height - copyImage.Height, copyImage.Width, copyImage.Height), 0, 0, copyImage.Width, copyImage.Height, System.Drawing.GraphicsUnit.Pixel);
            g.Dispose();

            image.Save(Path_syp);
            image.Dispose();
        }
        #endregion

        #region 图片剪切处理

        /// <summary>
        /// 图片剪切[不追加文件名]
        /// </summary>
        /// <param name="origPath">原始图片地址</param>
        /// <param name="partStartPointX">X轴开始坐标</param>
        /// <param name="partStartPointY">Y轴开始坐标</param>
        /// <param name="partWidthHeight">部件宽高数组：如：new int[,] new{{680,448}}; 宽：680；高：448</param>
        /// <param name="origStartPointX">原始X轴开始坐标</param>
        /// <param name="origStartPointY">原始Y轴开始坐标</param>
        /// <param name="imageWidth">图片宽度</param>
        /// <param name="imageHeight">图片高度</param>
        /// 默认在原始路径下追加文件名[追加的方式是在原始路径后加上剪切的宽高]
        /// 如：原始文件名为：egXXXX.jpg，则追加后的文件名为：exXXXX.jpg.680x448.jpg
        /// <returns>返回剪切图片的路径</returns>
        public static string CutImage(string origPath, int partStartPointX, int partStartPointY, int[,] partWidthHeight, int origStartPointX, int origStartPointY, int imageWidth, int imageHeight)
        {
            string agnomen;
            return CutImage(origPath, partStartPointX, partStartPointY, partWidthHeight[0, 0], partWidthHeight[0, 1],
                            origStartPointX, origStartPointY, imageWidth, imageHeight, false, out agnomen);
        }

        /// <summary>
        /// 图片剪切[默认追加文件名]
        /// </summary>
        /// <param name="origPath">原始图片地址</param>
        /// <param name="partStartPointX">X轴开始坐标</param>
        /// <param name="partStartPointY">Y轴开始坐标</param>
        /// <param name="partWidthHeight">部件宽高数组：如：new int[,] new{{680,448}}; 宽：680；高：448</param>
        /// <param name="origStartPointX">原始X轴开始坐标</param>
        /// <param name="origStartPointY">原始Y轴开始坐标</param>
        /// <param name="imageWidth">图片宽度</param>
        /// <param name="imageHeight">图片高度</param>
        /// <param name="agnomen">返回剪切图片的路径</param>
        /// 默认在原始路径下追加文件名[追加的方式是在原始路径后加上剪切的宽高]
        /// 如：原始文件名为：egXXXX.jpg，则追加后的文件名为：exXXXX.jpg.680x448.jpg
        /// <returns>返回剪切图片的路径</returns>
        public static string CutImage(string origPath, int partStartPointX, int partStartPointY, int[,] partWidthHeight, int origStartPointX, int origStartPointY, int imageWidth, int imageHeight, out string agnomen)
        {
            return CutImage(origPath, partStartPointX, partStartPointY, partWidthHeight[0, 0], partWidthHeight[0, 1],
                            origStartPointX, origStartPointY, imageWidth, imageHeight, true, out agnomen);
        }

        /// <summary>
        /// 图片剪切，不追加则会将剪切的图片替换掉原始上传的图片
        /// </summary>
        /// <param name="origPath">原始图片地址</param>
        /// <param name="partStartPointX">X轴开始坐标</param>
        /// <param name="partStartPointY">Y轴开始坐标</param>
        /// <param name="partWidthHeight">部件宽高数组：如：new int[,] new{{680,448}}; 宽：680；高：448</param>
        /// <param name="origStartPointX">原始X轴开始坐标</param>
        /// <param name="origStartPointY">原始Y轴开始坐标</param>
        /// <param name="imageWidth">图片宽度</param>
        /// <param name="imageHeight">图片高度</param>
        /// <param name="appendFileName">
        /// 是否在原始路径下追加文件名[追加的方式是在原始路径后加上剪切的宽高]，不追加则会将剪切的图片替换掉原始上传的图片
        /// 如：原始文件名为：egXXXX.jpg，则追加后的文件名为：exXXXX.jpg.680x448.jpg
        /// </param>
        /// <param name="agnomen">返回剪切图片的路径</param>
        /// <returns>返回剪切图片的路径</returns>
        public static string CutImage(string origPath, int partStartPointX, int partStartPointY, int[,] partWidthHeight, int origStartPointX, int origStartPointY, int imageWidth, int imageHeight, bool appendFileName, out string agnomen)
        {
            return CutImage(origPath, partStartPointX, partStartPointY, partWidthHeight[0, 0], partWidthHeight[0, 1],
                            origStartPointX, origStartPointY, imageWidth, imageHeight, appendFileName, out agnomen);
        }

        /// <summary>
        /// 图片剪切，不追加则会将剪切的图片替换掉原始上传的图片
        /// </summary>
        /// <param name="origPath">原始图片地址</param>
        /// <param name="partStartPointX">X轴开始坐标</param>
        /// <param name="partStartPointY">Y轴开始坐标</param>
        /// <param name="partWidth">部件宽度</param>
        /// <param name="partHeight">部件高度</param>
        /// <param name="origStartPointX">原始X轴开始坐标</param>
        /// <param name="origStartPointY">原始Y轴开始坐标</param>
        /// <param name="imageWidth">图片宽度</param>
        /// <param name="imageHeight">图片高度</param>
        /// <param name="appendFileName">
        /// 是否在原始路径下追加文件名[追加的方式是在原始路径后加上剪切的宽高]，不追加则会将剪切的图片替换掉原始上传的图片
        /// 如：原始文件名为：egXXXX.jpg，则追加后的文件名为：exXXXX.jpg.680x448.jpg
        /// </param>
        /// <param name="agnomen">剪切附加名</param>
        /// <returns>返回剪切图片的路径</returns>
        public static string CutImage(string origPath, int partStartPointX, int partStartPointY, int partWidth, int partHeight, int origStartPointX, int origStartPointY, int imageWidth, int imageHeight, bool appendFileName, out string agnomen)
        {
            string filePath;
            if (appendFileName)
            {
                //附加在原图片文件名后的附加名
                agnomen = "." + partWidth + "x" + partHeight + origPath.Substring(origPath.LastIndexOf('.'));
                //设置剪切文件保存路径
                filePath = origPath + agnomen;
            }
            else
            {
                agnomen = "no";
                filePath = origPath.Replace("XXXX", "XXXC");
            }

            using (Image origImage = Image.FromFile(origPath))
            {
                if (origImage.Width == imageWidth && origImage.Height == imageHeight)
                {
                    return CutImage(origPath, partStartPointX, partStartPointY, partWidth, partHeight, origStartPointX,
                             origStartPointY, appendFileName, out agnomen);
                }

                Bitmap thumbImage = MakeThumbnail(origImage, imageWidth, imageHeight);

                Bitmap partImage = new Bitmap(partWidth, partHeight);

                Graphics graphics = Graphics.FromImage(partImage);
                Rectangle destRect = new Rectangle(new Point(partStartPointX, partStartPointY), new Size(partWidth, partHeight)); //目标位置

                Rectangle origRect = new Rectangle(new Point(origStartPointX, origStartPointY), new Size(partWidth, partHeight)); //原图位置（默认从原图中截取的图片大小等于目标图片的大小）

                Graphics grap = Graphics.FromImage(partImage);
                grap.Clear(Color.White);
                //指定高质量的双三次插值法，执行预筛选以确保高质量的收缩，此模式可产生质量最高的转换图像
                grap.InterpolationMode = InterpolationMode.HighQualityBicubic;
                //指定高质量、低速度呈现
                grap.SmoothingMode = SmoothingMode.HighQuality;

                graphics.DrawImage(thumbImage, destRect, origRect, GraphicsUnit.Pixel);
                //文字水印
                //Font f = new Font("Lucida Grande", 6);
                //Brush b = new SolidBrush(Color.Gray);
                //grap.DrawString("www.iwas.cn",f,b,0,0);

                grap.Dispose();
                origImage.Dispose();

                if (File.Exists(filePath))
                {
                    File.SetAttributes(filePath, FileAttributes.Normal);
                    File.Delete(filePath);
                }
                //保存图片
                partImage.Save(filePath, ImageFormat.Jpeg);

                partImage.Dispose();
                thumbImage.Dispose();
            }
            if (!appendFileName)
            {
                File.Delete(origPath);
            }
            return filePath;
        }

        /// <summary>
        /// 保存剪切图片，不追加则会将剪切的图片替换掉原始上传的图片
        /// </summary>
        /// <param name="origPath">原始图片地址</param>
        /// <param name="partStartPointX">X轴开始坐标</param>
        /// <param name="partStartPointY">Y轴开始坐标</param>
        /// <param name="partWidth">部件宽度</param>
        /// <param name="partHeight">部件高度</param>
        /// <param name="origStartPointX">原始X轴开始坐标</param>
        /// <param name="origStartPointY">原始Y轴开始坐标</param>
        /// <param name="appendFileName">
        /// 是否在原始路径下追加文件名[追加的方式是在原始路径后加上剪切的宽高]
        /// 如：原始文件名为：egXXXX.jpg，则追加后的文件名为：exXXXX.jpg.680x448.jpg
        /// </param>
        /// <param name="agnomen">剪切附加名</param>
        /// <returns>返回剪切图片的路径</returns>
        public static string CutImage(string origPath, int partStartPointX, int partStartPointY, int partWidth, int partHeight, int origStartPointX, int origStartPointY, bool appendFileName, out string agnomen)
        {
            string filePath;
            if (appendFileName)
            {
                //附加在原图片文件名后的附加名
                agnomen = "." + partWidth + "x" + partHeight + origPath.Substring(origPath.LastIndexOf('.'));
                //设置剪切文件保存路径
                filePath = origPath + agnomen;
            }
            else
            {
                agnomen = "no";
                filePath = origPath.Replace("XXXX", "XXXC");
            }

            using (Image originalImg = Image.FromFile(origPath))
            {
                Bitmap partImg = new Bitmap(partWidth, partHeight);
                Graphics graphics = Graphics.FromImage(partImg);
                Rectangle destRect = new Rectangle(new Point(partStartPointX, partStartPointY), new Size(partWidth, partHeight));//目标位置
                Rectangle origRect = new Rectangle(new Point(origStartPointX, origStartPointY), new Size(partWidth, partHeight));//原图位置（默认从原图中截取的图片大小等于目标图片的大小）


                Graphics grap = Graphics.FromImage(partImg);

                grap.Clear(Color.White);
                // 指定高质量的双三次插值法。执行预筛选以确保高质量的收缩。此模式可产生质量最高的转换图像。 
                grap.InterpolationMode = InterpolationMode.HighQualityBicubic;
                // 指定高质量、低速度呈现。 
                grap.SmoothingMode = SmoothingMode.HighQuality;

                graphics.DrawImage(originalImg, destRect, origRect, GraphicsUnit.Pixel);
                // 文字水印 
                //Font f = new Font("Lucida Grande", 6);
                //Brush b = new SolidBrush(Color.Gray);
                //G.DrawString("iWas", f, b, 0, 0);
                grap.Dispose();

                originalImg.Dispose();
                if (File.Exists(filePath))
                {
                    File.SetAttributes(filePath, FileAttributes.Normal);
                    File.Delete(filePath);
                }
                partImg.Save(filePath, ImageFormat.Jpeg);
                partImg.Dispose();
            }
            if (!appendFileName)
            {
                File.Delete(origPath);
            }
            return filePath;
        }


        /// <summary>
        /// 保存剪切图片
        /// </summary>
        /// <param name="origPath">原始图片地址</param>
        /// <param name="savedPath">保存图片地址</param>
        /// <param name="partStartPointX">X轴开始坐标</param>
        /// <param name="partStartPointY">Y轴开始坐标</param>
        /// <param name="partWidth">部件宽度</param>
        /// <param name="partHeight">部件高度</param>
        /// <param name="origStartPointX">原始X轴开始坐标</param>
        /// <param name="origStartPointY">原始Y轴开始坐标</param>
        /// <param name="imageWidth">图片宽度</param>
        /// <param name="imageHeight">图片高度</param>
        /// <returns></returns>
        public static string CutImage(string origPath, string savedPath, int partStartPointX, int partStartPointY, int partWidth, int partHeight, int origStartPointX, int origStartPointY, int imageWidth, int imageHeight)
        {
            using (Image originalImg = Image.FromFile(origPath))
            {
                if (originalImg.Width == imageWidth && originalImg.Height == imageHeight)
                {
                    return CutImage(origPath, savedPath, partStartPointX, partStartPointY, partWidth, partHeight,
                            origStartPointX, origStartPointY);
                }
                string filename = ID.SetLsh() + ".jpg";
                string filePath = savedPath + "\\" + filename;

                Bitmap thumimg = MakeThumbnail(originalImg, imageWidth, imageHeight);

                Bitmap partImg = new Bitmap(partWidth, partHeight);

                Graphics graphics = Graphics.FromImage(partImg);
                Rectangle destRect = new Rectangle(new Point(partStartPointX, partStartPointY), new Size(partWidth, partHeight));//目标位置
                Rectangle origRect = new Rectangle(new Point(origStartPointX, origStartPointY), new Size(partWidth, partHeight));//原图位置（默认从原图中截取的图片大小等于目标图片的大小）

                Graphics G = Graphics.FromImage(partImg);

                G.Clear(Color.White);
                // 指定高质量的双三次插值法。执行预筛选以确保高质量的收缩。此模式可产生质量最高的转换图像。 
                G.InterpolationMode = InterpolationMode.HighQualityBicubic;
                // 指定高质量、低速度呈现。 
                G.SmoothingMode = SmoothingMode.HighQuality;

                graphics.DrawImage(thumimg, destRect, origRect, GraphicsUnit.Pixel);
                // 文字水印 
                //Font f = new Font("Lucida Grande", 6);
                //Brush b = new SolidBrush(Color.Gray);
                //G.DrawString("iWas", f, b, 0, 0);
                G.Dispose();

                originalImg.Dispose();
                if (File.Exists(filePath))
                {
                    File.SetAttributes(filePath, FileAttributes.Normal);
                    File.Delete(filePath);
                }
                partImg.Save(filePath, ImageFormat.Jpeg);

                partImg.Dispose();
                thumimg.Dispose();
                return filePath;
            }
        }


        /// <summary>
        /// 保存剪切图片
        /// </summary>
        /// <param name="origPath">原始图片地址</param>
        /// <param name="savedPath">保存图片地址</param>
        /// <param name="partStartPointX">X轴开始坐标</param>
        /// <param name="partStartPointY">Y轴开始坐标</param>
        /// <param name="partWidth">部件宽度</param>
        /// <param name="partHeight">部件高度</param>
        /// <param name="origStartPointX">原始X轴开始坐标</param>
        /// <param name="origStartPointY">原始Y轴开始坐标</param>
        /// <returns></returns>
        public static string CutImage(string origPath, string savedPath, int partStartPointX, int partStartPointY, int partWidth, int partHeight, int origStartPointX, int origStartPointY)
        {
            string filename = ID.SetLsh() + ".jpg";
            string filePath = savedPath + "\\" + filename;

            using (Image originalImg = Image.FromFile(origPath))
            {
                Bitmap partImg = new Bitmap(partWidth, partHeight);
                Graphics graphics = Graphics.FromImage(partImg);
                Rectangle destRect = new Rectangle(new Point(partStartPointX, partStartPointY), new Size(partWidth, partHeight));//目标位置
                Rectangle origRect = new Rectangle(new Point(origStartPointX, origStartPointY), new Size(partWidth, partHeight));//原图位置（默认从原图中截取的图片大小等于目标图片的大小）


                Graphics G = Graphics.FromImage(partImg);

                G.Clear(Color.White);
                // 指定高质量的双三次插值法。执行预筛选以确保高质量的收缩。此模式可产生质量最高的转换图像。 
                G.InterpolationMode = InterpolationMode.HighQualityBicubic;
                // 指定高质量、低速度呈现。 
                G.SmoothingMode = SmoothingMode.HighQuality;

                graphics.DrawImage(originalImg, destRect, origRect, GraphicsUnit.Pixel);
                // 注释 文字水印  
                //Font f = new Font("Lucida Grande", 6);
                //Brush b = new SolidBrush(Color.Gray);
                //G.DrawString("iWas", f, b, 0, 0);
                G.Dispose();

                originalImg.Dispose();
                if (File.Exists(filePath))
                {
                    File.SetAttributes(filePath, FileAttributes.Normal);
                    File.Delete(filePath);
                }
                partImg.Save(filePath, ImageFormat.Jpeg);
                partImg.Dispose();
            }
            return filename;
        }

        /// <summary>
        /// 生成缩略图
        /// </summary>
        /// <param name="fromImg">原图</param>
        /// <param name="width">宽</param>
        /// <param name="height">高</param>
        /// <returns>缩略后的图片</returns>
        public static Bitmap MakeThumbnail(Image fromImg, int width, int height)
        {
            Bitmap bmp = new Bitmap(width, height);
            int ow = fromImg.Width;
            int oh = fromImg.Height;

            //新建一个画板
            Graphics g = Graphics.FromImage(bmp);

            //设置高质量插值法
            g.InterpolationMode = InterpolationMode.High;
            //设置高质量,低速度呈现平滑程度
            g.SmoothingMode = SmoothingMode.HighQuality;
            //清空画布并以透明背景色填充
            g.Clear(Color.Transparent);

            g.DrawImage(fromImg, new Rectangle(0, 0, width, height),
                new Rectangle(0, 0, ow, oh),
                GraphicsUnit.Pixel);

            return bmp;
        }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Windows.Forms;

namespace Lyra2.UtilShared
{
    public class GraphicUtils
    {
        #region    Overlay Images

        /// <summary>
        /// Creates a new image, using the baseImage as base and drawing the overlayImage over the baseImage
        /// </summary>
        /// <param name="baseImage">base image</param>
        /// <param name="overlayImage">overlay image</param>
        /// <returns>combined image</returns>
        public static Image CombineImages(Image baseImage, Image overlayImage)
        {
            Bitmap result = new Bitmap(baseImage);
            Graphics g = Graphics.FromImage(result);
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.DrawImage(overlayImage, 0, 0);
            return result;
        }


        #endregion Overlay Images

        #region     Measure Strings

        private static readonly Graphics MGraphics = Graphics.FromImage(new Bitmap(256, 256));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <param name="font"></param>
        /// <returns></returns>
        public static Size MeasureString(string text, Font font)
        {
            SizeF size = MGraphics.MeasureString(text, font);
            return new Size((int)size.Width, (int)size.Height);
        }

        #endregion  Measure Strings

        #region     Drag&Drop Image

        private const int AlphaSteps = 8;
        private const float OpaquePart = 0.4f;
        private const int MaxTextLength = 64;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <param name="font"></param>
        /// <returns></returns>
        public static Image GetDragAndDropImage(string text, Font font)
        {
            return GetDragAndDropImage(null, text, font);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="icon"></param>
        /// <param name="text"></param>
        /// <param name="font"></param>
        /// <returns></returns>
        public static Image GetDragAndDropImage(Image icon, string text, Font font)
        {
            #region    Precondition

            if (string.IsNullOrEmpty(text) || font == null)
            {
                return null;
            }

            #endregion Precondition

            text = TrimText(text, MaxTextLength);
            Size textSize = MeasureString(text, font);
            Bitmap textImage = new Bitmap(textSize.Width + 2, textSize.Height + 2);
            Graphics g = Graphics.FromImage(textImage);
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
            g.DrawString(text, font, new SolidBrush(Color.FromArgb(171, 171, 171)), 1, 1);
            g.DrawString(text, font, new SolidBrush(Color.FromArgb(64, 64, 64)), 0, 0);
            g.Dispose();
            int left = 0;
            Bitmap dragAndDropImage;

            if (icon != null)
            {
                dragAndDropImage = new Bitmap(icon.Width + textImage.Width + 10, Math.Max(icon.Height, textImage.Height) + 4);
                g = Graphics.FromImage(dragAndDropImage);
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.DrawImage(icon, 2, (dragAndDropImage.Height - icon.Height) / 2);
                left = icon.Width + 4;
            }
            else
            {
                dragAndDropImage = new Bitmap(textImage.Width, textImage.Height);
                g = Graphics.FromImage(dragAndDropImage);
                g.SmoothingMode = SmoothingMode.AntiAlias;
            }
            int top = (dragAndDropImage.Height - textImage.Height) / 2 + 2;
            int textLeft = 0;
            int textWidth = (int)(textImage.Width * OpaquePart);
            g.DrawImage(textImage, new Rectangle(left, top, textWidth, textImage.Height), textLeft, 0, textWidth,
                        textImage.Height, GraphicsUnit.Pixel);
            left += textWidth;
            textLeft += textWidth;
            textWidth = (textImage.Width - textLeft) / AlphaSteps;
            for (int i = 0; i < AlphaSteps; i++)
            {
                g.DrawImage(textImage, new Rectangle(left, top, textWidth, textImage.Height), textLeft, 0,
                            textWidth, textImage.Height, GraphicsUnit.Pixel, GetAttributes(i));
                left += textWidth;
                textLeft += textWidth;
            }
            g.Dispose();
            return dragAndDropImage;
        }

        private static List<ImageAttributes> alphaMatrixList;

        private static ImageAttributes GetAttributes(int alphaStep)
        {
            #region    Precondition

            if (alphaStep < 0) return new ImageAttributes();

            #endregion Precondition

            if (alphaMatrixList == null)
            {
                alphaMatrixList = new List<ImageAttributes>(AlphaSteps);
                for (int i = 0; i < AlphaSteps; i++)
                {
                    float[][] matrixContent = new float[][]
                        {
                            new float[] {1, 0, 0, 0, 0},
                            new float[] {0, 1, 0, 0, 0},
                            new float[] {0, 0, 1, 0, 0},
                            new float[] {0, 0, 0, 1-i*(1.0f/AlphaSteps), 0},
                            new float[] {0, 0, 0, 0, 1}
                        };
                    ColorMatrix matrix = new ColorMatrix(matrixContent);
                    ImageAttributes attrs = new ImageAttributes();
                    attrs.SetColorMatrix(matrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
                    alphaMatrixList.Add(attrs);
                }
            }
            return alphaMatrixList[Math.Min(alphaStep, AlphaSteps - 1)];
        }

        private static string TrimText(string text, int maxLength)
        {
            if (text.Length > maxLength)
            {
                text = text.Substring(0, maxLength - 1) + "…";
            }
            return text;
        }

        #endregion  Drag&Drop Image

        #region     Sizing

        /// <summary>
        /// Fits the given item into the container
        /// </summary>
        /// <param name="item"></param>
        /// <param name="container"></param>
        /// <param name="padding"></param>
        /// <param name="keepRatio"></param>
        /// <returns></returns>
        public static Size FitSize(Size item, Size container, int padding, bool keepRatio)
        {
            int width = item.Width;
            int height = item.Height;
            float hDiff = container.Width - 2 * padding - width;
            float vDiff = container.Height - 2 * padding - height;
            if (hDiff < 0)
            {
                if (keepRatio)
                {
                    height = (int)(height * (width + hDiff) / width);
                }
                width += (int)hDiff;
            }
            if (vDiff < 0)
            {
                if (keepRatio)
                {
                    width = (int)(width * (height + vDiff) / height);
                }
                height += (int)vDiff;
            }
            return new Size(width, height);
        }

        /// <summary>
        /// Fits and aligns the given item in the container
        /// </summary>
        /// <param name="item"></param>
        /// <param name="container"></param>
        /// <param name="padding"></param>
        /// <param name="keepRatio"></param>
        /// <param name="align"></param>
        /// <returns></returns>
        public static Rectangle FitSizeAndAlign(Size item, Size container, int padding, bool keepRatio, Alignment align)
        {
            Size resultSize = FitSize(item, container, padding, keepRatio);
            return Align(resultSize, container, align);
        }

        /// <summary>
        /// Aligns the given item in the container
        /// </summary>
        /// <param name="item"></param>
        /// <param name="container"></param>
        /// <param name="align"></param>
        /// <returns></returns>
        public static Rectangle Align(Size item, Size container, Alignment align)
        {
            int x = 0;
            int y = 0;
            if ((align & Alignment.Center) == Alignment.Center)
            {
                x = (container.Width - item.Width) / 2;
            }
            else if ((align & Alignment.Right) == Alignment.Right)
            {
                x = container.Width - item.Width;
            }

            if ((align & Alignment.Middle) == Alignment.Middle)
            {
                y = (container.Height - item.Height) / 2;
            }
            else if ((align & Alignment.Bottom) == Alignment.Bottom)
            {
                y = container.Height - item.Height;
            }
            return new Rectangle(new Point(x, y), item);
        }

        /// <summary>
        /// Alignment Flags
        /// </summary>
        [Flags]
        public enum Alignment
        {
            Left = 0x1, Center = 0x10, Right = 0x100, Top = 0x1000, Middle = 0x10000, Bottom = 0x100000
        }

        #endregion  Sizing

        #region     Drawing

        /// <summary>
        /// 
        /// </summary>
        /// <param name="img"></param>
        /// <param name="bg"></param>
        /// <param name="margin"></param>
        /// <returns></returns>
        public static Image DrawImageBackground(Image img, Image bg, Padding margin)
        {
            #region    Precondition

            margin.Left = Math.Max(0, Math.Min(bg.Width, margin.Left));
            margin.Bottom = Math.Max(0, Math.Min(bg.Height, margin.Bottom));
            margin.Right = Math.Max(0, Math.Min(bg.Width, margin.Right));
            margin.Top = Math.Max(0, Math.Min(bg.Height, margin.Top));

            #endregion Precondition

            Graphics g = Graphics.FromImage(img);

            // draw corners (fix)
            g.DrawImage(bg, new Rectangle(0, 0, margin.Left, margin.Top), new Rectangle(0, 0, margin.Left, margin.Top), GraphicsUnit.Pixel);
            g.DrawImage(bg, new Rectangle(0, img.Height - margin.Bottom, margin.Left, margin.Bottom), new Rectangle(0, bg.Height - margin.Bottom, margin.Left, margin.Bottom), GraphicsUnit.Pixel);
            g.DrawImage(bg, new Rectangle(img.Width - margin.Right, img.Height - margin.Bottom, margin.Right, margin.Bottom),
                new Rectangle(bg.Width - margin.Right, bg.Height - margin.Bottom, margin.Right, margin.Bottom), GraphicsUnit.Pixel);
            g.DrawImage(bg, new Rectangle(img.Width - margin.Right, 0, margin.Right, margin.Top), new Rectangle(bg.Width - margin.Right, 0, margin.Right, margin.Top), GraphicsUnit.Pixel);

            // draw margin.s (scaled)
            g.DrawImage(bg, new Rectangle(0, margin.Top, margin.Left, img.Height - margin.Vertical),
                new Rectangle(0, margin.Top, margin.Left, bg.Height - margin.Vertical), GraphicsUnit.Pixel);
            g.DrawImage(bg, new Rectangle(margin.Left, img.Height - margin.Bottom, img.Width - margin.Horizontal, margin.Bottom),
                new Rectangle(margin.Left, bg.Height - margin.Bottom, bg.Width - margin.Horizontal, margin.Bottom), GraphicsUnit.Pixel);
            g.DrawImage(bg, new Rectangle(img.Width - margin.Right, margin.Top, margin.Right, img.Height - margin.Vertical),
                new Rectangle(bg.Width - margin.Right, margin.Top, margin.Right, bg.Height - margin.Vertical), GraphicsUnit.Pixel);
            g.DrawImage(bg, new Rectangle(margin.Left, 0, img.Width - margin.Horizontal, margin.Top),
                new Rectangle(margin.Left, 0, bg.Width - margin.Horizontal, margin.Top), GraphicsUnit.Pixel);

            // draw middle (scaled)
            g.DrawImage(bg, new Rectangle(margin.Left, margin.Top, img.Width - margin.Horizontal, img.Height - margin.Vertical),
                new Rectangle(margin.Left, margin.Top, bg.Width - margin.Horizontal, bg.Height - margin.Vertical), GraphicsUnit.Pixel);
            return img;
        }

        #endregion  Drawing
    }
}

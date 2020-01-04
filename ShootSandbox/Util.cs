using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ShootSandbox
{
    class Util
    {

        public static Bitmap RotateImage(Image img, float rotationAngle)
        {
            return RotateImage(img, rotationAngle, false);
        }

        
        /*
         * Ik weet dat deze functie veel te groot is, wat vooral komt door de fancySizing. Dat
         * berekent precies de dimensies van de resulterende Image, waardoor je niet onnodig memory
         * opneemt. Denk niet dat ik het ga gebruiken omdat ik niet verwacht veel te hoeven optimizen
         * voor zo'n simpel spelletje, maar wou het gewoon proberen werkend te krijgen.
         */
        public static Bitmap RotateImage(Image img, float rotationAngle, bool fancySizing)
        {

            if (rotationAngle < 0) rotationAngle = 360 + rotationAngle;

            int rotWidth = 0;
            int rotHeight = 0;

            if (fancySizing)
            {
                if (rotationAngle % 180 == 0)
                {
                    rotWidth = img.Width;
                    rotHeight = img.Height;
                }
                else if ((rotationAngle - 90) % 180 == 0)
                {
                    rotWidth = img.Height;
                    rotHeight = img.Width;
                }
                else
                {
                    bool widthFacingUp = rotationAngle < 90 || (rotationAngle > 180 && rotationAngle - 180 < 90);
                    int facingUp = widthFacingUp ? img.Width : img.Height;
                    int facingSide = widthFacingUp ? img.Height : img.Width;

                    rotWidth  = (int)((facingUp * Math.Cos(DegreeToRadian(rotationAngle % 90))) + (facingSide * Math.Cos(DegreeToRadian(90 - (rotationAngle % 90)))));
                    rotHeight = (int)((facingSide * Math.Cos(DegreeToRadian(rotationAngle % 90))) + (facingUp * Math.Cos(DegreeToRadian(90 - (rotationAngle % 90)))));
                }

                if (rotWidth % 2 != img.Width % 2)
                    rotWidth++;

                if (rotHeight % 2 != img.Height % 2)
                    rotHeight++;
            }
            else
            {
                int maxDimension = (int)Math.Sqrt((img.Width * img.Width) + (img.Height * img.Height));
                rotWidth = maxDimension;
                rotHeight = maxDimension;
            }

            Bitmap bmp = new Bitmap(rotWidth, rotHeight);
            Graphics gfx = Graphics.FromImage(bmp);

      //      gfx.FillRectangle(Brushes.Black, 0, 0, rotWidth, rotHeight);

            gfx.TranslateTransform((float)bmp.Width / 2, (float)bmp.Height / 2);
            gfx.RotateTransform(rotationAngle);
            gfx.TranslateTransform(-(float)bmp.Width / 2, -(float)bmp.Height / 2);

            gfx.InterpolationMode = InterpolationMode.HighQualityBicubic;
            gfx.SmoothingMode = SmoothingMode.None;

            gfx.DrawImage(img, new Point((rotWidth / 2) - (img.Width/2), (rotHeight / 2) - (img.Height / 2)));

            gfx.Dispose();

            return bmp;
        }

        /* Credit: https://softwarebydefault.com/2013/04/12/bitmap-color-tint/
         * Heb deze niet zelf geschreven, alleen wat aangepast. Zie niet echt
         * een reden om hem zelf opnieuw te typen, want snap hoe die werkt.
         */
        public static Image TintImage(Bitmap sourceBitmap, float redTint,
                                 float greenTint, float blueTint)
        {
            BitmapData sourceData = sourceBitmap.LockBits(new Rectangle(0, 0,
                            sourceBitmap.Width, sourceBitmap.Height),
                            ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);


            byte[] pixelBuffer = new byte[sourceData.Stride * sourceData.Height];


            Marshal.Copy(sourceData.Scan0, pixelBuffer, 0, pixelBuffer.Length);


            sourceBitmap.UnlockBits(sourceData);


            float blue = 0;
            float green = 0;
            float red = 0;


            for (int k = 0; k + 4 < pixelBuffer.Length; k += 4)
            {
                /*
                blue = pixelBuffer[k] + (255 - pixelBuffer[k]) * blueTint;
                green = pixelBuffer[k + 1] + (255 - pixelBuffer[k + 1]) * greenTint;
                red = pixelBuffer[k + 2] + (255 - pixelBuffer[k + 2]) * redTint;
                */

                blue = pixelBuffer[k]  * blueTint;
                green = pixelBuffer[k + 1]  * greenTint;
                red = pixelBuffer[k + 2]  * redTint;


                if (blue > 255)
                { blue = 255; }


                if (green > 255)
                { green = 255; }


                if (red > 255)
                { red = 255; }


                pixelBuffer[k] = (byte)blue;
                pixelBuffer[k + 1] = (byte)green;
                pixelBuffer[k + 2] = (byte)red;


            }


            Bitmap resultBitmap = new Bitmap(sourceBitmap.Width, sourceBitmap.Height);


            BitmapData resultData = resultBitmap.LockBits(new Rectangle(0, 0,
                                    resultBitmap.Width, resultBitmap.Height),
                                    ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);


            Marshal.Copy(pixelBuffer, 0, resultData.Scan0, pixelBuffer.Length);
            resultBitmap.UnlockBits(resultData);


            return resultBitmap;
        }

        public static Image ResizeImage(Image img, int width, int height)
        {
            Bitmap newImg = new Bitmap(width, height);
            Graphics gfx = Graphics.FromImage(newImg);

            gfx.InterpolationMode = InterpolationMode.NearestNeighbor;

            gfx.DrawImage(img, 0, 0, width, height);

            gfx.Dispose();

            return newImg;
        }

        public static float RandomFloat(float range)
        {
            if (range == 0) return 0;
            return (float)((Program.Random.NextDouble()* range) - range / 2);
        }

        public static Vector2 InvertVector(Vector2 vector)
        {
            return new Vector2(-vector.X, -vector.Y);
        }

        public static Vector2 MultiplyVector(Vector2 vector, float mul)
        {
            return Vector2.Multiply(vector, new Vector2(mul, mul));
        }

        public static Vector2 DirectedVector(float length, float angle)
        {
            return RotateVector2(new Vector2(length, 0), angle);
        }

        public static Vector2 RotateVector2(Vector2 vector, float rotation)
        {
            rotation = DegreeToRadian(rotation);

            return new Vector2((float)((vector.X * Math.Cos(rotation)) - (vector.Y * Math.Sin(rotation))), (float)((vector.Y * Math.Cos(rotation)) + (vector.X * Math.Sin(rotation))));
        }

        public static float AngleBetween(Point a, Point b)
        {
            return RadianToDegree((float) Math.Atan2(a.Y - b.Y, a.X - b.X));
        }

        public static Point Vector2ToPoint(Vector2 vec)
        {
            return new Point((int)vec.X, (int)vec.Y);
        }

        public static Vector2 PointToVector2(Point point)
        {
            return new Vector2(point.X, point.Y);
        }

        public static float DegreeToRadian(float angle)
        {
            return (float) (Math.PI * angle / 180.0);
        }

        public static float RadianToDegree(float angle)
        {
            return (float) (angle * (180.0 / Math.PI));
        }

        public static float MoveTowards(float currentValue, float center, float change)
        {
            if(currentValue == center) goto end;

            float dif = center - currentValue;

            if (Math.Abs(dif) > change)
                dif = change * (dif < 0 ? -1 : 1);

            currentValue += dif;

            end:  return currentValue;
        }
    }
}

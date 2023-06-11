
using ModelLibrary;
using System.Drawing;
using System.Text;

namespace WaterCompanyServicesAPI
{
    public static class Helper
    {
        public static byte[] GenerateNewSubscriptionDocument(Subscription sub, Consumer cons)
        {
            //first, create a dummy bitmap just to get a graphics object
            Image img = new Bitmap(1, 1);
            Graphics drawing = Graphics.FromImage(img);
            StringBuilder sb = new StringBuilder();
            sb.Append($"Dear {cons.ConsumerName}:");
            sb.Append(Environment.NewLine);
            sb.Append("New subscription added to your subscriptions successfully");
            sb.Append(Environment.NewLine);
            sb.Append($"New Subscription Barcode: {sub.ConsumerBarCode}");
            sb.Append(Environment.NewLine);
            sb.Append($"New Subscription Number: {sub.ConsumerSubscriptionNo}");

            Font font = new Font("Times New Roman", 16);
            //measure the string to see how big the image needs to be
            SizeF textSize = drawing.MeasureString(sb.ToString(), font);

            //free up the dummy image and old graphics object
            img.Dispose();
            drawing.Dispose();

            //create a new image of the right size
            img = new Bitmap((int)textSize.Width, (int)textSize.Height);

            drawing = Graphics.FromImage(img);

            //paint the background
            drawing.Clear(Color.White);

            //create a brush for the text
            Brush textBrush = new SolidBrush(Color.Black);

            drawing.DrawString(sb.ToString(), font, textBrush, 0, 0);

            drawing.Save();

            textBrush.Dispose();
            drawing.Dispose();

            return imageToByteArray(img);
        }

        public static byte[] imageToByteArray(this System.Drawing.Image image)
        {
            try
            {
                using (var ms = new MemoryStream())
                {
                    image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                    return ms.ToArray();
                }
            }
            catch(Exception ex)
            {
                System.Diagnostics.Debug.Write(ex.ToString());
                return null;
            }

        }
    }
}


using Microsoft.AspNetCore.SignalR;
using ModelLibrary;
using System.Drawing;
using System.Text;

namespace WaterCompanyServicesAPI
{
    public static class Helper
    {
        public static byte[] GenerateNewSubscriptionDocument(Subscription sub, Consumer cons)
        {
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
            sb.Append($"Collect Date : {DateTime.Now.AddDays(1).ToString("dd/MM/yyyy")}");
            sb.Append(Environment.NewLine);

            Font font = new Font("Times New Roman", 16);
            SizeF textSize = drawing.MeasureString(sb.ToString(), font);

            img.Dispose();
            drawing.Dispose();

            img = new Bitmap((int)textSize.Width, (int)textSize.Height);

            drawing = Graphics.FromImage(img);

            drawing.Clear(Color.White);

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

        public static byte[] GenerateRepairResult(string notes, Consumer? cons)
        {
            Image img = new Bitmap(1, 1);
            Graphics drawing = Graphics.FromImage(img);
            StringBuilder sb = new StringBuilder();
            sb.Append($"Dear {cons.ConsumerName}:");
            sb.Append(Environment.NewLine);
            sb.Append("The Meter was repaird successfully");
            sb.Append(Environment.NewLine);            
            sb.Append($"Maintainance Done : {notes}");
            sb.Append(Environment.NewLine);            
            sb.Append($"Collect Date : {DateTime.Now.AddDays(1).ToString("dd/MM/yyyy")}");
            sb.Append(Environment.NewLine);

            Font font = new Font("Times New Roman", 16);
            SizeF textSize = drawing.MeasureString(sb.ToString(), font);

            img.Dispose();
            drawing.Dispose();

            img = new Bitmap((int)textSize.Width, (int)textSize.Height);

            drawing = Graphics.FromImage(img);

            drawing.Clear(Color.White);

            Brush textBrush = new SolidBrush(Color.Black);

            drawing.DrawString(sb.ToString(), font, textBrush, 0, 0);

            drawing.Save();

            textBrush.Dispose();
            drawing.Dispose();

            return imageToByteArray(img);
        }
    }
}

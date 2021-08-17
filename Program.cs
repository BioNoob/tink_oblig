using System;
using System.Drawing;
using System.Net;
using System.Windows.Forms;
using tink_oblig.classes;
using tink_oblig.Properties;
using Tinkoff.Trading.OpenApi.Network;

namespace tink_oblig
{
    static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new LoginForm());
        }
        private static Context _CurrentContext;
        public static Context CurrentContext
        {
            get
            {
                return _CurrentContext;
            }
            set
            {
                InnerAccount = new Accounts();
                _CurrentContext = value;
            }
        }
        public static Accounts InnerAccount { get; set; }
        public static Image DrawText(String text, Font font, Color textColor, Color backColor)
        {
            //first, create a dummy bitmap just to get a graphics object
            Image img = new Bitmap(1, 1);
            Graphics drawing = Graphics.FromImage(img);

            //measure the string to see how big the image needs to be
            SizeF textSize = drawing.MeasureString(text, font);

            //free up the dummy image and old graphics object
            img.Dispose();
            drawing.Dispose();

            //create a new image of the right size
            img = new Bitmap((int)80, 80);

            drawing = Graphics.FromImage(img);

            //paint the background
            drawing.Clear(backColor);

            //create a brush for the text
            Brush textBrush = new SolidBrush(textColor);

            drawing.DrawString(text, font, textBrush, 40 - textSize.Width / 2, 40 + 1 - textSize.Height / 2);

            drawing.Save();

            textBrush.Dispose();
            drawing.Dispose();

            return img;

        }
    }
    public class xWebClient : WebClient
    {
        protected override WebRequest GetWebRequest(Uri uri)
        {
            WebRequest w = base.GetWebRequest(uri);
            w.Timeout = 100000;
            return w;
        }
    }
}

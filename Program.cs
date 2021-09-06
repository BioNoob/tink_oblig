using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net;
using System.Windows.Forms;
using tink_oblig.classes;
using Tinkoff.Trading.OpenApi.Network;
using static tink_oblig.classes.Accounts;

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
            ShowedForms = new List<TagWatcher>();
            Application.Run(LoginForm);
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
        public static LoginForm LoginForm = new LoginForm();
        public static ManagerForm mf;
        public static List<TagWatcher> ShowedForms { get; set; }
        public static void RemoveFromShowed(TagWatcher rm)
        {
            ShowedForms.Remove(rm);
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
    public class TagWatcher
    {
        public string Name { get; private set; }

        public SeeHistory Mode { get; private set; }
        public TagWatcher(string name, SeeHistory md)
        {
            Name = name;
            Mode = md;
        }
        public override string ToString()
        {
            string buf = "";
            switch (Mode)
            {
                case SeeHistory.NoHistrory:
                    buf = "Открытые позиции";
                    break;
                case SeeHistory.History:
                    buf = "Закрытые позиции";
                    break;
                case SeeHistory.WithHistory:
                    buf = "Совместные позиции";
                    break;
            }
            return $"{Name}  {buf}";
        }
    }
}

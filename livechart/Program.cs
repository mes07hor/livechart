using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace livechart
{
    static class Program
    {
        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            FirstForm FirstForm = new FirstForm();
            Application.Run(FirstForm);

            //Form1 mainForm = new Form1();
            //SplashForm.ShowSplash(new Form1());
            //Application.Run(new Form1());
            //SplashForm.ShowSplash(mainForm);
            //Application.Run(mainForm);
        }
    }
}

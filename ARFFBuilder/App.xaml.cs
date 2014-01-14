using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Runtime.InteropServices;
using CommandLine;

namespace ARFFBuilder
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        [DllImport("Kernel32.dll")]
        public static extern bool AttachConsole(int processId);

        [DllImport("Kernel32.dll")]
        public static extern bool FreeConsole();
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            if (e.Args.Count() > 0)           
                ConsoleWork(e.Args);            
            else
                new GUI.MainWindow().ShowDialog();            

            this.Shutdown();
        }

        private void ConsoleWork(string[] args)
        {
            AttachConsole(-1);

            ConsoleOptions options = new ConsoleOptions();
            if (CommandLine.Parser.Default.ParseArguments(args, options))
            {
                Console.WriteLine("I am working ...");
                IBitmap bitmap = new Bitmap(options.GetSettings());
                bitmap.BuildBitmap();
                bitmap.WriteToARFFFile();

                Console.WriteLine("I am done. Yeah!");
            }
            
            FreeConsole();
        }

        
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LightPackViz
{
    static class Program
    {
       

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);


            using (var show = new LightShow(new LightPack()))
            using (var tray = new VizTray()) {
                var music = new MusicAnalyser(1024);
                var analysis = new SpectrumAnalyser(music).LogScale().Trim(256).FluxThreshold(1.2);
                analysis.FluxAvailable += show.OnFluxAvailable;

                var debug = new SpectrumDebugView();
                analysis.BandsAvailable += debug.Update;
                debug.Show();

                new LightTest(show.Lights).Show();
                Application.Run();

            }
        }
    }
}

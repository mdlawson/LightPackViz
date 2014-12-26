using System;

namespace LightPackViz {
    public class LightShow : IDisposable {

        public LightPack Lights { private set; get; }

        public LightShow() {
            Lights = new LightPack();
            Lights.Connect();
            Lights.Smoothing = 30;
        }

        public void OnBandsAvailable(object sender, double[] bands) {
            //Lights.Brightness = (int)(bands[0] + bands[1])/2;
            Lights.Brightness = (int) bands[bands.Length - 1];
        }

        public void Dispose() {
            Lights.Dispose();
        }

        public void OnFluxAvailable(object sender, double flux) {
            //if (flux < 0) Lights.Brightness += (int) (flux*60000);
            if (flux < 0) Lights.Brightness = 0;
            else Lights.Brightness = (int) (flux*35000);
        }
    }
}
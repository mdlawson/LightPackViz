using System;

namespace LightPackViz {
    public class LightShow : IDisposable {

        public ILights Lights { private set; get; }

        public LightShow(ILights lights) {
            Lights = lights;
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
//            int brightness = 0;
//            if (flux > 0) { 
//                brightness = (int) (flux*35000);
//                if (brightness > 100) brightness = 100;
//            }
            int brightness = Lights.Brightness + (int) (flux/200);
            if (brightness > 100) brightness = 100;
            if (brightness < 0) brightness = 0;
            Lights.Brightness = brightness;
        }
    }
}
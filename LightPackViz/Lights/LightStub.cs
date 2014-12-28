using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightPackViz.Lights {
    class LightStub : ILights {

        private Color[] leds;

        public Color this[int i] {
            get { return leds[i]; }
            set { leds[i] = value; }
        }

        public LightStub() {
            LedCount = 10;
            LedMap = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            leds = new Color[LedCount];
            for (var i = 0; i < leds.Length; i++) {
                leds[i] = Color.Red;
            }
        }

        public int Brightness { get; set; }

        public float Gamma { get; set; }

        public string Host { get; private set; }

        public int LedCount { get; private set; }

        public int[] LedMap { get; set; }

        public bool Locked { get; private set; }

        public int Port { get; private set; }

        public string Profile { get; set; }

        public string[] Profiles { get; private set; }

        public int Smoothing { get; set; }

        public string Status { get; set; }

        public string StatusApi { get; private set; }

        public string Version { get; private set; }

        public void Connect() {
        }

        public void Connect(string host, int port, string apiKey) {
            Host = host;
            Port = port;
        }

        public void Disconnect() {

        }

        public void Dispose() {

        }

        public bool Lock() {
            Locked = true;
            return Locked;
        }

        public bool UnLock() {
            Locked = false;
            return !Locked;
        }
    }
}

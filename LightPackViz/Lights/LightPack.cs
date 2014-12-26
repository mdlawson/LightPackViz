using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace LightPackViz {
    public class LightPack : IDisposable {
        private string apiKey;

        private TcpClient client;
        
        private Color[] Leds;
        private float gamma;
        private int brightness;
        private int smoothing;

        public string Host { get; private set; }
        public int Port { get; private set; }

        public string Version { get; private set; }

        public bool Locked { get; private set; } = false;

        public int[] LedMap { get; set; }

        public LightPack() {
            Host = "127.0.0.1";
            Port = 3636;
            apiKey = "";
            LedMap = new int[] {1, 2, 3 , 4, 5, 6, 7, 8, 9, 10};
        }

        private string readData() {
            var bytesReceived = new Byte[1024];
            var data = "";

            var bytes = client.Client.Receive(bytesReceived, bytesReceived.Length, 0);
            data = data + Encoding.UTF8.GetString(bytesReceived, 0, bytes);
            data = data.Replace("\r\n", "");

            return data;
        }

        private void sendData(string data) {
            var bytesSent = Encoding.UTF8.GetBytes(data + "\r\n");
            client.Client.Send(bytesSent);
        }

        private string getProp(string prop) {
            sendData("get" + prop);
            var s = readData();
            return s.Substring(s.IndexOf(':') + 1);
        }

        private string setProp(string prop, string value) {
            sendData("set" + prop + ":" + value);
            return readData();
        }

        public void Connect(string host, int port, string apiKey) {
            this.Host = host;
            this.Port = port;
            this.apiKey = apiKey;
            Connect();
        }

        public void Connect() {
                client = new TcpClient();
                client.Connect(Host, Port);
                Version = readData();
                Version = Version.Substring(Version.IndexOf('v') + 1);
                Version = Version.Substring(0, Version.IndexOf(" "));
                sendData("apikey:" + apiKey);
                if (readData() == "ok") {
                    Leds = new Color[LedCount];
                    Lock();
                    Brightness = 100;
                } else {
                    throw new Exception("Invalid key");
                }
        }

        public void Disconnect() {
            if (client.Connected) {
                client.Client.Disconnect(false);
            }
        }


        public string Status {
            get {
                return getProp("status");
            }
            set {
                setProp("status", value);
            }
        }

        public string StatusApi => getProp("statusapi");

        public string Profile {
            get {
                return getProp("profile");
            }
            set {
                setProp("profile", value);
            }
        }

        public string[] Profiles => getProp("profiles").Split(';');

        public int LedCount => int.Parse(getProp("countleds"));

        public Color this[int i] {
            get {
                return Leds[i];
            }
            set {
                var s = setProp("color", LedMap[i] + "-" + value.R + "," + value.G + "," + value.B + ";");
                if (s == "ok") {
                    Leds[i] = value;
                }
            }
        }

        public int Smoothing {
            get {
                return smoothing;
            }
            set {
                var s = setProp("smooth", value.ToString());
                if (s == "ok") {
                    smoothing = value;
                }
            }
        }

        public float Gamma {
            get {
                return gamma;
            }
            set {
                var s = setProp("gamma", value.ToString());
                if (s == "ok") {
                    gamma = value;
                }
            }
        }

        public int Brightness {
            get {
                return brightness;
            }
            set {
                var s = setProp("brightness", value.ToString());
                if (s == "ok") {
                    brightness = value;
                }
            }
        }

        public bool Lock() {
            sendData("lock");
            return Locked = (readData() == "lock:success");
        }

        public bool UnLock() {
            sendData("unlock");
            return !(Locked = (readData() != "unlock:success"));
        }

        public void Dispose() {
            if (Locked) {
                UnLock();
            }
        }
    }
}

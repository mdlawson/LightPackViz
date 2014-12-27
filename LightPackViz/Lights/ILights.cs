using System.Drawing;

namespace LightPackViz {
    public interface ILights {
        Color this[int i] { get; set; }

        int Brightness { get; set; }
        float Gamma { get; set; }
        string Host { get; }
        int LedCount { get; }
        int[] LedMap { get; set; }
        bool Locked { get; }
        int Port { get; }
        string Profile { get; set; }
        string[] Profiles { get; }
        int Smoothing { get; set; }
        string Status { get; set; }
        string StatusApi { get; }
        string Version { get; }

        void Connect();
        void Connect(string host, int port, string apiKey);
        void Disconnect();
        void Dispose();
        bool Lock();
        bool UnLock();
    }
}
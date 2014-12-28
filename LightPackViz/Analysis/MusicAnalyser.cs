using System;
using System.Deployment.Application;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using NAudio.Dsp;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;

namespace LightPackViz {
    public class MusicAnalyser {

        private IWaveIn waveIn;
        private int fftLength;
//        private static int numBands = 256;
//        private static int bandWidth = (fftLength / 2) / (numBands * 2);
//        private static double fftScale = 1/fftLength;
        private Complex[] fftBuffer;
        private int fftPos;
        private int m;
        private double[] spectrum;
//        private double[] bands;
//        private int skip = 2;
//        private int count;

//        public event EventHandler<double[]> BandsAvailable;
        public event EventHandler<double[]> SpectrumAvailable;

        public MusicAnalyser(int fftLength) {
            this.fftLength = fftLength;
            fftBuffer = new Complex[fftLength];
            spectrum = new double[fftLength / 2];
//            bands = new double[numBands];
            m = (int) Math.Log(fftLength, 2.0);

            waveIn = new WasapiLoopbackCapture();
            //waveIn = new WasapiCapture(WasapiCapture.GetWhatYouHearDevice());
            waveIn.DataAvailable += onDataAvailable;
            waveIn.StartRecording();
        }

        public WaveFormat Format => waveIn.WaveFormat;
        public int SpectrumSize => spectrum.Length;

        private void onDataAvailable(object sender, WaveInEventArgs e) {
            var buffer = e.Buffer;
            var bytesRecorded = e.BytesRecorded;
            var bufferIncrement = waveIn.WaveFormat.BlockAlign;

            for (var index = 0; index < bytesRecorded; index += bufferIncrement) {
                var sample32 = BitConverter.ToSingle(buffer, index);// + BitConverter.ToSingle(buffer, index + 4);
                fftBuffer[fftPos].X = (float)(sample32 * FastFourierTransform.BlackmannHarrisWindow(fftPos, fftLength));
                fftBuffer[fftPos].Y = 0; // This is always zero with audio.
                fftPos++;
                if (fftPos >= fftLength) {
                    fftPos = 0;
                    FastFourierTransform.FFT(true, m, fftBuffer);
                    for (var i = 0; i < spectrum.Length; i++) {
                        var complex = fftBuffer[i];
                        spectrum[i] = complex.X*complex.X + complex.Y*complex.Y;
                    }
                    if (SpectrumAvailable != null) SpectrumAvailable(this, spectrum);
                }
            }
        }

        //
        //        private int beatIndex = 0;
        //        private int beatMissCount = 0;
        //        private static int beatMissMax = 5;
        //
        //        private void onFFTCalculated() {
        //            double biggestDelta = 0;
        //            int biggestDeltaIndex = 0;
        //            for (var n = 0; n < numBands - 1; n++) {
        //                double total = 0;
        //                var start = n * bandWidth;
        //                for (var p = 0; p < bandWidth; p++) {
        //                    total += spectrum[start + p];
        //                }
        //                var val = 15 * Math.Log10(total) + 135;
        //                val = val < 0 ? 0 : val;
        //                var delta = Math.Abs(val - bands[n]) /* * Math.Log10(numBands - n)*/;
        //                if (delta > biggestDelta) {
        //                    biggestDelta = delta;
        //                    biggestDeltaIndex = n;
        //                }
        //                bands[n] = val;
        //            }
        //            if (beatIndex != biggestDeltaIndex) beatMissCount++;
        //            if (beatMissCount == beatMissMax) {
        //                beatIndex = biggestDeltaIndex;
        //            }
        //            bands[bands.Length - 1] = bands[beatIndex];
        //            if (BandsAvailable != null) BandsAvailable(this, bands);
        //        }

    }
}
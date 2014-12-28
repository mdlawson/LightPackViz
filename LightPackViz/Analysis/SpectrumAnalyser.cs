using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms.VisualStyles;
using System.Xml.Schema;
using NAudio.MediaFoundation;

namespace LightPackViz {
    public class SpectrumAnalyser {

        private enum Average {
            LINEAR, LOG, NONE
        }

        private enum Scale {
            LOG, LINEAR
        }

        private int spectrumSize;
        private float sampleRate;
        private double[] bands;
        private Average average;
        private Scale scale;

        private double[] lastTotals;
        private int lastTotal;
        private double threshold;

        public event EventHandler<double> FluxAvailable;
        public event EventHandler<double[]> BandsAvailable;

        public SpectrumAnalyser(MusicAnalyser music) {
            lastTotals = new double[10];
            music.SpectrumAvailable += OnSpectrumAvailable;
            this.sampleRate = music.Format.SampleRate;
            this.spectrumSize = music.SpectrumSize;
            NoAverage();
        }

        public SpectrumAnalyser NoAverage() {
            bands = new double[spectrumSize];
            average = Average.NONE;
            return this;
        }

        public SpectrumAnalyser LinearAverage(int count) {
            bands = new double[count];
            average = Average.LINEAR;
            return this;
        }

        public SpectrumAnalyser LogAverage() {
            return this;
        }

        public SpectrumAnalyser LogScale() {
            scale = Scale.LOG;
            return this;
        }

        public SpectrumAnalyser LinearScale() {
            scale = Scale.LINEAR;
            return this;
        }

        public SpectrumAnalyser FluxThreshold(double threshold) {
            this.threshold = threshold;
            return this;
        }

        public SpectrumAnalyser Trim(int num) {
            bands = new double[bands.Length - num];
            return this;
        }

        public void OnSpectrumAvailable(object sender, double[] spectrum) {
            switch (average) {
                case Average.LINEAR: 
                    var width = (int) spectrumSize/bands.Length;
                    for (var i = 0; i < bands.Length; i++) {
                        double avg = 0;
                        var start = i*width;
                        var end = start + width;
                        for (var j = start; j < end; j++) {
                            avg += spectrum[j];
                        }
                        bands[i] = avg/width;
                    }
                    break;
                default:
                    for (var i = 0; i < bands.Length; i++) {
                        bands[i] = spectrum[i];
                    }
                    break;
            }

            switch (scale) {
                case Scale.LOG:
                    for (var i = 0; i < bands.Length; i++) {
                        bands[i] = 10 * Math.Log10(bands[i]) + 100;
                    }
                    break;
            }

            if (FluxAvailable != null) {
                double total = 0;
                foreach (double band in bands) total += band;
                var flux = total - addTotal(total);
                //var target = lastTotals.Average() * threshold;
                //if (Math.Abs(total) > target) FluxAvailable(this, flux); // TODO fix threshold?
                FluxAvailable(this, flux);
            }

            

            if (BandsAvailable != null) BandsAvailable(this, bands);

        }

        private double addTotal(double total) {
            double prev = lastTotals[lastTotal];
            lastTotal = (lastTotal + 1)%lastTotals.Length;
            lastTotals[lastTotal] = total;
            return prev;
        }
    }
}
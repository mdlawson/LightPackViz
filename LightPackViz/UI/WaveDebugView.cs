using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NAudio.Wave;
using ZedGraph;

namespace LightPackViz {
    public partial class WaveDebugView : Form {

        private GraphPane graph;
        private RollingPointPairList points;
        private IWaveIn waveIn;

        private int count;

        public WaveDebugView() {
            InitializeComponent();

            graph = zedGraphControl1.GraphPane;
            points = new RollingPointPairList(1000);
            graph.AddCurve("Wave", points, Color.Blue, SymbolType.None);

            graph.XAxis.Scale.Min = 0;
            graph.XAxis.Scale.Max = 1000;
            graph.XAxis.Scale.MinorStep = 50;
            graph.XAxis.Scale.MajorStep = 100;

            zedGraphControl1.AxisChange();

            
            
            waveIn = new WasapiCapture(WasapiCapture.GetWhatYouHearDevice());

            waveIn.DataAvailable += update;
            waveIn.StartRecording();
        }

        private void update(object sender, WaveInEventArgs e) {
            var buffer = e.Buffer;
            var bytesRecorded = e.BytesRecorded;
            var bufferIncrement = waveIn.WaveFormat.BlockAlign;


            for (var index = 0; index < bytesRecorded; index += bufferIncrement) {
                var sample32 = BitConverter.ToSingle(buffer, index);
                points.Add(count++, sample32);
            }
            

            Scale xScale = zedGraphControl1.GraphPane.XAxis.Scale;
            if (count > xScale.Max - xScale.MajorStep) {
                xScale.Max = count + xScale.MajorStep;
                xScale.Min = xScale.Max - 1000;
            }

            zedGraphControl1.AxisChange();
            zedGraphControl1.Invalidate();
        }
    }
}

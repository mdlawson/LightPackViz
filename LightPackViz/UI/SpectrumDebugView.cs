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
    public partial class SpectrumDebugView : Form {

        private GraphPane graph;
        private PointPairList points;
        private IWaveIn waveIn;

        private int count;

        public SpectrumDebugView() {
            InitializeComponent();

            graph = zedGraphControl1.GraphPane;
            points = new PointPairList();
            graph.AddBar("Wave", points, Color.Blue);

            graph.YAxis.Scale.Min = 0;
            graph.YAxis.Scale.Max = 100;
            graph.XAxis.Scale.Min = 0;
            graph.XAxis.Scale.Max = 18;

            zedGraphControl1.AxisChange();
        }

        public void Update(object sender, double[] bands) {

            points.Clear();

            for (var index = 0; index < bands.Length; index++) {
                points.Add(index, bands[index]);
            }
            
            zedGraphControl1.Invalidate();
        }
    }
}

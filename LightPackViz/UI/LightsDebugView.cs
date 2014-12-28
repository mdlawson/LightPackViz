using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LightPackViz.UI {
    public partial class LightsDebugView : Form {

        private ILights lights;
        private int lightsPerRow;
        private Timer timer;
        private double lastBrightness;

        public LightsDebugView(LightShow show) {
            timer = new Timer();
            lights = show.Lights;
            InitializeComponent();

            grid.DefaultCellStyle.BackColor = Color.Black;
            grid.ColumnCount = lightsPerRow = (lights.LedCount/2);
            grid.RowCount = 2;

            timer.Interval = 20;
            timer.Tick += update;
            timer.Start();
        }

        private void update(object sender, EventArgs args) {
            double brightness = 0.5*(lights.Brightness/100.0) + 0.5*lastBrightness;
            for (var i = 0; i < lights.LedCount; i++) {
                DataGridViewCell cell = cellForLight(i);
                cell.Style.BackColor = Color.FromArgb((int)(255*brightness),0,0);
            }
            lastBrightness = brightness;
        }

        private DataGridViewCell cellForLight(int num) {
            return num >= lightsPerRow ? grid[num - lightsPerRow, 1] : grid[num, 0];
        }
    }
}

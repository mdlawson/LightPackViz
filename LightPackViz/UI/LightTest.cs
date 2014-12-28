using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LightPackViz
{
    public partial class LightTest : Form {
        private ILights lights;
        public LightTest(ILights lights) {
            this.lights = lights;
            InitializeComponent();
            lights.Status = "on";
        }

        private void lightButton_Click(object sender, EventArgs e) {
            var num = int.Parse(((Button)sender).Tag.ToString()) - 1;
            lights[num] = lights[num] == Color.Red ? Color.Black : Color.Red;
        }

    }
}

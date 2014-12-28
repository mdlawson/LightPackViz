using System;
using System.Drawing;
using System.Windows.Forms;

namespace LightPackViz {
    public class VizTray : IDisposable {

        private NotifyIcon ni;
        private ContextMenuStrip menu;
        public VizTray() {
            menu = buildMenu();
            ni = new NotifyIcon { Text = "LightMusic", Icon = SystemIcons.Application, Visible = true, ContextMenuStrip = menu};
            
        }

        private ContextMenuStrip buildMenu() {
            var menu = new ContextMenuStrip();

            ToolStripMenuItem item;
            var sep = new ToolStripSeparator();

            item = new ToolStripMenuItem {Text = "Exit"};
            item.Click += onExit;
            menu.Items.Add(item);

            return menu;
        }

        private void onExit(object sender, EventArgs e) {
            Application.Exit();
        }

        public void Dispose() {
            ni.Dispose();
        }
    }
}
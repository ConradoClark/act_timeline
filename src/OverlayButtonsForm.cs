using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace ACTTimeline
{
    public partial class OverlayButtonsForm : Form
    {
        TimelineController controller;

        public OverlayButtonsForm(TimelineController controller_)
        {
            controller = controller_;

            InitializeComponent();

            this.ShowInTaskbar = false;

            // Force set small window size below OS minimum.
            Win32APIUtils.SetWindowSize(Handle, 55, 20);

            Win32APIUtils.SetWS_EX_NOACTIVATE(Handle, true);

            controller.PausedUpdate += controller_PausedUpdate;
            controller_PausedUpdate(null, EventArgs.Empty);

            Opacity = 0.8;

            Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width - 2, Height, 5, 5));
        }


        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]

        private static extern IntPtr CreateRoundRectRgn(
            int nLeftRect, // x-coordinate of upper-left corner
            int nTopRect, // y-coordinate of upper-left corner
            int nRightRect, // x-coordinate of lower-right corner
            int nBottomRect, // y-coordinate of lower-right corner
            int nWidthEllipse, // height of ellipse
            int nHeightEllipse // width of ellipse
            );

        void controller_PausedUpdate(object sender, EventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => { controller_PausedUpdate(sender, e); }));
                return;
            }

            pictureBox2.Image = controller.Paused ? global::ACTTimeline.Properties.Resources.play_icon : global::ACTTimeline.Properties.Resources.stop_icon;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            controller.CurrentTime = 0;
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            controller.Paused = !controller.Paused;
        }
    }
}

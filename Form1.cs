using System;
using System.Drawing;
using System.Windows.Forms;

namespace WindowsFormsApp4
{
    public partial class Form1 : Form
    {
        private TaskManager manager = new TaskManager();

        public Form1()
        {
            InitializeComponent();
            this.MouseDown += Form1_MouseDown;
            this.DoubleBuffered = true;
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                manager.AddTask(e.Location, Redraw);
            }
            else if (e.Button == MouseButtons.Right)
            {
                manager.RemoveNearest(e.Location);
                Invalidate();
            }
        }

        private void Redraw()
        {
            if (IsDisposed || !IsHandleCreated)
                return;

            if (InvokeRequired)
            {
                try
                {
                    BeginInvoke(new Action(Invalidate));
                }
                catch { }
            }
            else
            {
                Invalidate();
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            manager.StopAll();
            base.OnFormClosing(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            var tasks = manager.GetTasks();

            foreach (var t in tasks)
            {
                e.Graphics.DrawString(
                    t.GetNumber().ToString(),
                    this.Font,
                    t.Brush, 
                    t.Location);
            }
        }
    }
}
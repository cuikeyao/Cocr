using Cocr.Util;

namespace Cocr
{
    public partial class ScreenForm : Form
    {
        Panel panel;
        Point startPoint = new(0, 0);
        Point endPoint = new(0, 0);

        IntPtr arrowPtr;

        private MainForm mainForm;

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern IntPtr LoadCursorFromFile(string fileName);

        public ScreenForm(MainForm mainForm)
        {
            InitializeComponent();


            this.mainForm = mainForm;
            this.SetStyle(ControlStyles.DoubleBuffer | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);
            this.UpdateStyles();

            arrowPtr = LoadCursorFromFile(ResourcesUtils.getResource("arrow.cur"));
            this.Cursor = new Cursor(arrowPtr);
            this.StartPosition = FormStartPosition.WindowsDefaultLocation;
            this.WindowState = FormWindowState.Maximized;
            this.FormBorderStyle = FormBorderStyle.None;
            // 置顶
            this.TopMost = false;
            this.AutoSize = true;

            panel = new Panel();
            this.Controls.Add(panel);

            panel.MouseDoubleClick += Panel_MouseDoubleClick;
            panel.MouseDown += panel_MouseDown;
            panel.MouseUp += panel_MouseUp;
        }

        public void CaptureScreen()
        {
            Bitmap bitmap = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            using (Graphics graphics = Graphics.FromImage(bitmap))
            {
                graphics.CopyFromScreen(Screen.PrimaryScreen.Bounds.X, Screen.PrimaryScreen.Bounds.Y, 0, 0, Screen.PrimaryScreen.Bounds.Size);
            }

            Image image = bitmap;

            panel.Size = image.Size;
            panel.BackgroundImage = image;
        }

        private void Panel_MouseDoubleClick(object? sender, MouseEventArgs e)
        {
            mainForm.Visible = true;
            mainForm.Opacity = 1;
            CloseForm(sender);
        }

        private void panel_MouseDown(object? sender, MouseEventArgs e)
        {
            startPoint = e.Location;
            if (e.Button == MouseButtons.Right)
            {
                mainForm.Visible = true;
                mainForm.Opacity = 1;
                CloseForm(sender);
            }
        }

        private async void panel_MouseUp(object? sender, MouseEventArgs e)
        {

            endPoint = e.Location;

            int x = Math.Abs(startPoint.X - endPoint.X);
            int y = Math.Abs(endPoint.Y - startPoint.Y);

            if (x == 0 || y == 0)
            {
                return;
            }

            mainForm.resultTextBox.Text = "";

            Bitmap bitmap = new Bitmap(x, y);

            using (Graphics graphics = Graphics.FromImage(bitmap))
            {
                graphics.CopyFromScreen(new Point(Math.Min(startPoint.X, endPoint.X), Math.Min(startPoint.Y, endPoint.Y)), new Point(0, 0), new Size(x, y));
            }

            mainForm.pictureBox.Location = new Point(0, 0);
            mainForm.pictureBox.Image = (Bitmap)(bitmap.Clone());

            CloseForm(sender);

            mainForm.backgroundWorker1.RunWorkerAsync(bitmap.Clone());
            //ocrResult = engine.DetectText(bitmap);
            mainForm.Visible = true;
            mainForm.Opacity = 1;
        }
        private void CloseForm(object? sender)
        {
            if (sender == null)
            {
                return;
            }
            Control control = ((Panel)sender);
            Form form = control.FindForm();
            form.Close();
        }
    }
}

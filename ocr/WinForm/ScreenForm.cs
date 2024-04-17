using Cocr.Util;
using System.Diagnostics;

namespace Cocr.WinForm
{
    // 截图界面
    public partial class ScreenForm : Form
    {
        Point startPoint = new(0, 0);
        Point endPoint = new(0, 0);
        private Pen pen = new Pen(Color.Black, 1);

        IntPtr arrowPtr;

        private MainForm mainForm;

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern IntPtr LoadCursorFromFile(string fileName);

        public ScreenForm(MainForm mainForm)
        {
            InitializeComponent();

            this.mainForm = mainForm;
            this.DoubleBuffered = true; // 开启双缓冲减少闪烁
            arrowPtr = LoadCursorFromFile(ResourcesUtils.getResource("arrow.cur"));
            this.Cursor = new Cursor(arrowPtr);
            this.StartPosition = FormStartPosition.WindowsDefaultLocation;
            this.WindowState = FormWindowState.Maximized;
            this.FormBorderStyle = FormBorderStyle.None;
            // 置顶
            this.TopMost = false;
            this.AutoSize = true;

            this.MouseDoubleClick += Form_MouseDoubleClick;
            this.MouseClick += Form_MouseClick;
            this.MouseDown += Form_MouseDown;
            this.MouseUp += Form_MouseUp;
            this.MouseMove += Form_MouseMove;
        }

        public void CaptureScreen()
        {
            Bitmap bitmap = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            using (Graphics graphics = Graphics.FromImage(bitmap))
            {
                graphics.CopyFromScreen(Screen.PrimaryScreen.Bounds.X, Screen.PrimaryScreen.Bounds.Y, 0, 0, Screen.PrimaryScreen.Bounds.Size);
            }

            Image image = bitmap;

            this.Size = image.Size;
            this.BackgroundImage = image;
        }

        private void Form_MouseDoubleClick(object? sender, MouseEventArgs e)
        {
            mainForm.Visible = true;
            mainForm.Opacity = 1;
            CloseForm(sender);
        }

        private void Form_MouseClick(object? sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                mainForm.Visible = true;
                mainForm.Opacity = 1;
                CloseForm(sender);
            }
        }

        private void Form_MouseDown(object? sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                startPoint = e.Location;
                endPoint = e.Location;
                this.Invalidate();
            }
        }

        private void Form_MouseUp(object? sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                return;
            }
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
                graphics.CopyFromScreen(new Point(Math.Min(startPoint.X + 1, endPoint.X), Math.Min(startPoint.Y + 1, endPoint.Y)), new Point(0, 0), new Size(x - 1, y - 1));
            }

            mainForm.pictureBox.Location = new Point(0, 0);
            mainForm.pictureBox.Image = (Bitmap)(bitmap.Clone());

            CloseForm(sender);

            mainForm.backgroundWorker1.RunWorkerAsync(bitmap.Clone());
            mainForm.Visible = true;
            mainForm.Opacity = 1;
        }

        private void Form_MouseMove(object? sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                endPoint = e.Location; // 更新拖拽结束点
                this.Invalidate(); // 请求重绘窗体
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Rectangle rect = GetRectangleFromPoints(startPoint, endPoint);
            Debug.WriteLine(startPoint.X + "," + startPoint.Y + " " + endPoint.X + "," + endPoint.Y);
            e.Graphics.DrawRectangle(pen, rect);
        }

        private Rectangle GetRectangleFromPoints(Point p1, Point p2)
        {
            return new Rectangle(
                Math.Min(p1.X, p2.X), // 矩形左上角X坐标
                Math.Min(p1.Y, p2.Y), // 矩形左上角Y坐标
                Math.Abs(p1.X - p2.X), // 矩形宽度
                Math.Abs(p1.Y - p2.Y)); // 矩形高度
        }

        private void CloseForm(object? sender)
        {
            this.Close();
        }
    }
}

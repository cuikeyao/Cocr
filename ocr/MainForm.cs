using Cocr.WinControl;
using Cocr.Util;
using Cocr.WinForm;
using System.ComponentModel;
using System.Diagnostics;
using System.Net;
using System.Text;

namespace Cocr
{
    public partial class MainForm : Form
    {
        private Point lastLocation;
        private bool isMoving = false;
        public PictureBox? pictureBox;

        private string ocrResult;

        private IntPtr viewPtr;

        private static HttpListener httpListener;
        private static bool isServiceRunning = false;
        CancellationTokenSource serverCancellationTokenSource;

        public MainForm()
        {
            InitializeComponent();
            InitializeControl();
            InitializeBackgroundWorker();
        }

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern IntPtr LoadCursorFromFile(string fileName);

        private void InitializeControl()
        {
            viewPtr = LoadCursorFromFile(ResourcesUtils.getResource("view.cur"));

            pictureBox = new PictureBox();
            pictureBox.SizeMode = PictureBoxSizeMode.AutoSize;
            pictureBox.Size = new Size(0, 0);
            pictureBox.Dock = DockStyle.None;
            pictureBox.MouseDown += pictureBox_MouseDown;
            pictureBox.MouseUp += pictureBox_MouseUp;
            pictureBox.MouseMove += pictureBox_MouseMove;
            pictureBox.MouseDoubleClick += PictureBox_MouseDoubleClick;
            pictureBox.MouseEnter += PictureBox_MouseEnter;
            pictureBox.MouseLeave += PictureBox_MouseLeave;
            imagePanel.Controls.Add(pictureBox);
        }

        private void PictureBox_MouseLeave(object? sender, EventArgs e)
        {
            PictureBox pictureBox = (PictureBox)sender;
            pictureBox.Cursor = Cursors.Default;
        }

        private void PictureBox_MouseEnter(object? sender, EventArgs e)
        {
            PictureBox pictureBox = (PictureBox)sender;
            pictureBox.Cursor = new Cursor(viewPtr);
        }

        private void PictureBox_MouseDoubleClick(object? sender, MouseEventArgs e)
        {
            if (sender == null) return;
            PictureBox pictureBox = (PictureBox)sender;

            FullForm fullForm = new FullForm(pictureBox.Image);

            fullForm.Show();
        }

        private void InitializeBackgroundWorker()
        {
            // 允许报告进度
            backgroundWorker1.WorkerReportsProgress = true;
            backgroundWorker1.DoWork += backgroundWorker1_DoWork;
            backgroundWorker1.ProgressChanged += BackgroundWorker1_ProgressChanged;
            backgroundWorker1.RunWorkerCompleted += backgroundWorker1_RunWorkerCompleted;
        }

        private void backgroundWorker1_DoWork(object? sender, DoWorkEventArgs e)
        {
            if (sender == null) return;
            BackgroundWorker worker = sender as BackgroundWorker;
            CancellationTokenSource cts = new CancellationTokenSource();
            CancellationToken token = cts.Token;

            Task task = Task.Run(() =>
            {
                int i = 0;
                while (!token.IsCancellationRequested)
                {
                    worker.ReportProgress(Interlocked.Increment(ref i));
                    Thread.Sleep(1000);
                }
            });
            Bitmap bitmap = (Bitmap)e.Argument;

            ocrResult = OCRUtils.getInstance().getResult(bitmap);

            cts.Cancel();
        }

        private void BackgroundWorker1_ProgressChanged(object? sender, ProgressChangedEventArgs e)
        {
            string i = e.ProgressPercentage.ToString();
            int repeat = int.Parse(i) % 4;
            this.captureButton.Enabled = false;
            String text = "识别中" + String.Concat(Enumerable.Repeat("·", repeat));
            this.captureButton.Text = text.PadLeft(text.Length + repeat, '\u0020');
        }

        private void backgroundWorker1_RunWorkerCompleted(object? sender, RunWorkerCompletedEventArgs e)
        {
            resultTextBox.Text = ocrResult;
            captureButton.Text = "截图";
            captureButton.Enabled = true;
        }

        private async void capture_Click(object sender, EventArgs e)
        {
            this.Opacity = 0;
            this.Visible = false;

            //创建一个form用来显示截图
            ScreenForm screenForm = new ScreenForm(this);

            screenForm.CaptureScreen();

            screenForm.Show();
        }

        // 图片平移到任意位置
        private void pictureBox_MouseDown(object? sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                lastLocation = e.Location;
                isMoving = true;
            }
        }

        private void pictureBox_MouseUp(object? sender, MouseEventArgs e)
        {
            isMoving = false;
        }

        // 鼠标拖动图片
        private void pictureBox_MouseMove(object? sender, MouseEventArgs e)
        {
            if (isMoving)
            {
                int newX = pictureBox.Location.X + e.X - lastLocation.X;
                int newY = pictureBox.Location.Y + e.Y - lastLocation.Y;

                if (newX < imagePanel.Width - pictureBox.Width)
                {
                    newX = imagePanel.Width - pictureBox.Width;
                }
                if (newY < imagePanel.Height - pictureBox.Height)
                {
                    newY = imagePanel.Height - pictureBox.Height;
                }
                if (newX > 0)
                {
                    newX = 0;
                }
                if (newY > 0)
                {
                    newY = 0;
                }

                pictureBox.Location = new Point(newX, newY);
            }
        }

        private void serverButton_Click(object sender, EventArgs e)
        {
            ButtonCheck buttonCheck = (ButtonCheck)sender;
            if (buttonCheck.Checked) {
                StartHttpServer();
                MessageBox.Show("服务已启动，监听端口：7262");
            } else {
                StopHttpServer();
            }
            isServiceRunning = !isServiceRunning;
        }

        private void StartHttpServer()
        {
            serverCancellationTokenSource = new CancellationTokenSource();
            try
            {
                httpListener = new HttpListener();
                httpListener.Start();
                // 在后台线程开启服务
                Task.Run(() => StartService(), serverCancellationTokenSource.Token);
            }
            catch (Exception ex)
            {
                MessageBox.Show("无法启动服务器: " + ex.Message);
            }
        }

        private void StopHttpServer()
        {
            serverCancellationTokenSource.Cancel();
        }

        private void StartService()
        {
            httpListener.Prefixes.Add("http://*:7262/");
            while (!serverCancellationTokenSource.Token.IsCancellationRequested)
            {
                try
                {
                    // 等待请求并获得上下文对象
                    HttpListenerContext context = httpListener.GetContext();

                    // 处理请求
                    HandleRequest(context);
                } catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    // 监听器已停止
                    break;
                }
            }
            try
            {
                httpListener.Stop();
                httpListener.Close();
            } catch (HttpListenerException ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private void HandleRequest(HttpListenerContext context)
        {
            // 只接受POST方法
            if (context.Request.HttpMethod != "POST")
            {
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                context.Response.Close();
                return;
            }

            try
            {
                // 读取请求体中的内容
                using (StreamReader reader = new StreamReader(context.Request.InputStream, context.Request.ContentEncoding))
                {
                    string requestBody = reader.ReadToEnd();

                    // 在此处处理请求，产生响应字符串
                    string responseString = ProcessRequest(requestBody);

                    // 响应客户端
                    byte[] buffer = Encoding.UTF8.GetBytes(responseString);
                    context.Response.ContentLength64 = buffer.Length;
                    context.Response.OutputStream.Write(buffer, 0, buffer.Length);
                }
            }
            catch (Exception ex)
            {
                // 记录或处理异常
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            }
            finally
            {
                // 关闭响应
                context.Response.Close();
            }
        }

        private string ProcessRequest(string requestBody)
        {
            // 处理请求和生成响应的业务逻辑
            // 例如: 将输入字符串返回
            return OCRUtils.getInstance().getResult(requestBody);
        }
    }
}
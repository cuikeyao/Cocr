using Cocr.Util;
using PaddleOCRSharp;
using System.ComponentModel;
using System.Diagnostics;

namespace Cocr
{
    public partial class MainForm : Form
    {
        private Point lastLocation;
        private bool isMoving = false;
        public PictureBox? pictureBox;

        PaddleOCREngine? engine;
        OCRResult? ocrResult;

        IntPtr viewPtr;

        public MainForm()
        {
            InitializeComponent();
            InitializeControl();
            InitializeBackgroundWorker();
            this.SetStyle(ControlStyles.DoubleBuffer | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);
            this.UpdateStyles();
        }

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern IntPtr LoadCursorFromFile(string fileName);

        private void InitializeControl()
        {
            viewPtr = LoadCursorFromFile(ResourcesUtils.getResource("view.cur"));

            pictureBox = new PictureBox();
            imagePanel.Controls.Add(pictureBox);
            pictureBox.SizeMode = PictureBoxSizeMode.AutoSize;
            pictureBox.Size = new Size(0, 0);
            pictureBox.Dock = DockStyle.None;
            pictureBox.MouseDown += pictureBox_MouseDown;
            pictureBox.MouseUp += pictureBox_MouseUp;
            pictureBox.MouseMove += pictureBox_MouseMove;
            pictureBox.MouseDoubleClick += PictureBox_MouseDoubleClick;
            pictureBox.MouseEnter += PictureBox_MouseEnter;
            pictureBox.MouseLeave += PictureBox_MouseLeave;
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
            PictureBox fullPictureBox = new PictureBox();

            Form fullForm = new Form();
            fullForm.BackColor = Color.White;
            fullForm.MinimumSize = new Size(960, 720);
            fullForm.AutoSize = true;
            fullForm.Resize += FullForm_SizeChanged;
            fullForm.Controls.Add(fullPictureBox);
            fullForm.StartPosition = FormStartPosition.CenterScreen;

            fullPictureBox.SizeMode = PictureBoxSizeMode.AutoSize;
            fullPictureBox.Image = pictureBox.Image;
            fullPictureBox.Size = pictureBox.Image.Size;

            fullPictureBox.Location = new Point((fullForm.Width - fullPictureBox.Width) / 2, (fullForm.Height - fullPictureBox.Height) / 2);

            fullForm.Show();
        }

        private void FullForm_SizeChanged(object? sender, EventArgs e)
        {
            if (sender == null) return;
            Form fullForm = (Form)sender;
            Control control = fullForm.Controls.OfType<PictureBox>().FirstOrDefault();
            PictureBox pictureBox = (PictureBox)control;

            // 计算 PictureBox 的新位置，使其位于 Form 的中间
            int x = (fullForm.Width - pictureBox.Width) / 2;
            int y = (fullForm.Height - pictureBox.Height) / 2;
            pictureBox.Location = new Point(x, y);

            Debug.WriteLine($"form: width:{fullForm.Width}, height:{fullForm.Height}");
            Debug.WriteLine($"pictureBox: width:{control.Width}, height:{control.Height}");
            Debug.WriteLine($"image: width:{pictureBox.Image.Width}, height:{pictureBox.Image.Height}");
            Debug.WriteLine("");
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

            ocrResult = engine.DetectText(bitmap);

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
            resultTextBox.Text = ocrResult.Text;
            captureButton.Text = "截图";
            captureButton.Enabled = true;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            //自带轻量版中英文模型V4模型
            OCRModelConfig? config = null;

            //服务器中英文模型
            // OCRModelConfig config = new OCRModelConfig();
            //string root = System.IO.Path.GetDirectoryName(typeof(OCRModelConfig).Assembly.Location);
            //string modelPathroot = root + @"\inferenceserver";
            //config.det_infer = modelPathroot + @"\ch_ppocr_server_v2.0_det_infer";
            //config.cls_infer = modelPathroot + @"\ch_ppocr_mobile_v2.0_cls_infer";
            //config.rec_infer = modelPathroot + @"\ch_ppocr_server_v2.0_rec_infer";
            //config.keys = modelPathroot + @"\ppocr_keys.txt";

            //英文和数字模型V3
            //OCRModelConfig config = new OCRModelConfig();
            //string root = PaddleOCRSharp.EngineBase.GetRootDirectory();
            //string modelPathroot = root + @"\en_v3";
            //config.det_infer = modelPathroot + @"\en_PP-OCRv3_det_infer";
            //config.cls_infer = modelPathroot + @"\ch_ppocr_mobile_v2.0_cls_infer";
            //config.rec_infer = modelPathroot + @"\en_PP-OCRv3_rec_infer";
            //config.keys = modelPathroot + @"\en_dict.txt";

            //OCR参数
            OCRParameter oCRParameter = new OCRParameter();
            oCRParameter.cpu_math_library_num_threads = 10;//预测并发线程数
            oCRParameter.enable_mkldnn = true;
            oCRParameter.cls = false; //是否执行文字方向分类；默认false
            oCRParameter.det = true;//是否开启文本框检测，用于检测文本块
            oCRParameter.use_angle_cls = false;//是否开启方向检测，用于检测识别180旋转
            oCRParameter.det_db_score_mode = true;//是否使用多段线，即文字区域是用多段线还是用矩形，
            oCRParameter.max_side_len = 1920;
            oCRParameter.rec_img_h = 48;
            oCRParameter.rec_img_w = 320;
            oCRParameter.det_db_thresh = 0.3f;
            oCRParameter.det_db_box_thresh = 0.618f;

            //识别结果对象
            ocrResult = new OCRResult();
            //初始化OCR引擎
            engine = new PaddleOCREngine(config, oCRParameter);

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
    }
}
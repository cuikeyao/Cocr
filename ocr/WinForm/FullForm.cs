namespace Cocr.WinForm
{
    public partial class FullForm : Form
    {
        private PictureBox pictureBox;

        public FullForm(Image image)
        {
            InitializeComponent();

            pictureBox = new PictureBox();
            pictureBox.SizeMode = PictureBoxSizeMode.AutoSize;
            pictureBox.Image = (Image)(image.Clone());
            pictureBox.Size = image.Size;

            this.Icon = Resource.favicon;
            this.BackColor = Color.White;
            this.MinimumSize = new Size(960, 720);
            this.Controls.Add(pictureBox);
            this.StartPosition = FormStartPosition.CenterScreen;

            pictureBox.Location = new Point((this.Width - pictureBox.Width) / 2, (this.Height - pictureBox.Height) / 2);

            this.Resize += FullForm_SizeChanged;
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
        }
    }
}

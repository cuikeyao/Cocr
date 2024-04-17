using Cocr.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cocr.WinForm
{
    public partial class FullForm : Form
    {
        private PictureBox pictureBox;

        public FullForm(PictureBox pictureBox)
        {
            InitializeComponent();

            this.pictureBox = pictureBox;
            this.Icon = new Icon(ResourcesUtils.getResource("favicon.ico"));
            this.BackColor = Color.White;
            this.MinimumSize = new Size(960, 720);
            this.AutoSize = true;
            this.Controls.Add(pictureBox);
            this.StartPosition = FormStartPosition.CenterScreen;

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

            Debug.WriteLine($"form: width:{fullForm.Width}, height:{fullForm.Height}");
            Debug.WriteLine($"pictureBox: width:{control.Width}, height:{control.Height}");
            Debug.WriteLine($"image: width:{pictureBox.Image.Width}, height:{pictureBox.Image.Height}");
            Debug.WriteLine("");
        }

    }
}

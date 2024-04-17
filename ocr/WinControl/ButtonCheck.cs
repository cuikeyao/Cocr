using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cocr.WinControl
{
    public partial class ButtonCheck : UserControl
    {
        /// <summary>
        /// 是否选中
        /// </summary>
        bool isCheck = true;

        /// <summary>
        /// 枚举
        /// </summary>
        public enum CheckStyle
        {
            style1 = 0
        };
        public ButtonCheck()
        {
            InitializeComponent();
            //设置Style支持透明背景色并且双缓冲
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            this.SetStyle(ControlStyles.Selectable, true);
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.BackColor = Color.Transparent;

            this.Cursor = Cursors.Hand;
            this.Size = new Size(87, 27);
        }
        /// <summary>
        /// 是否选中
        /// </summary>
        public bool Checked
        {
            set { isCheck = value; this.Invalidate(); }
            get { return isCheck; }
        }

        CheckStyle checkStyle = CheckStyle.style1;

        /// <summary>
        /// 样式
        /// </summary>
        public CheckStyle CheckStyleX
        {
            set { checkStyle = value; this.Invalidate(); }
            get { return checkStyle; }
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            Bitmap bitMapOn = null;
            Bitmap bitMapOff = null;

            if (checkStyle == CheckStyle.style1)
            {
                bitMapOn = Resource.open;
                bitMapOff = Resource.close;
            }

            Graphics g = e.Graphics;
            Rectangle rec = new Rectangle(0, 0, this.Size.Width, this.Size.Height);

            if (isCheck)
            {
                g.DrawImage(bitMapOn, rec);
            }
            else
            {
                g.DrawImage(bitMapOff, rec);
            }
        }
        //特别说明：ButtonCheck_Click这个事件要添加控制方法产生，而不是直接在这里写
        private void ButtonCheck_Click(object sender, EventArgs e)
        {
            isCheck = !isCheck;
            this.Invalidate();
        }

        private void ButtonCheck_click(object sender, EventArgs e)
        {
            isCheck = !isCheck;
            this.Invalidate();
        }
    }
}

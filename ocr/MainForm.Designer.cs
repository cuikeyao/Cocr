namespace Cocr
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            captureButton = new Button();
            resultTextBox = new RichTextBox();
            imagePanel = new Panel();
            backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            SuspendLayout();
            // 
            // captureButton
            // 
            captureButton.BackColor = SystemColors.ButtonFace;
            captureButton.Font = new Font("Consolas", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            captureButton.Location = new Point(347, 379);
            captureButton.Name = "captureButton";
            captureButton.Size = new Size(110, 43);
            captureButton.TabIndex = 0;
            captureButton.Text = "截图";
            captureButton.UseVisualStyleBackColor = false;
            captureButton.Click += capture_Click;
            // 
            // resultTextBox
            // 
            resultTextBox.Location = new Point(411, 44);
            resultTextBox.Name = "resultTextBox";
            resultTextBox.ReadOnly = true;
            resultTextBox.Size = new Size(377, 310);
            resultTextBox.TabIndex = 1;
            resultTextBox.Text = "";
            // 
            // imagePanel
            // 
            imagePanel.BackColor = SystemColors.ButtonHighlight;
            imagePanel.Location = new Point(12, 44);
            imagePanel.Name = "imagePanel";
            imagePanel.Size = new Size(377, 310);
            imagePanel.TabIndex = 2;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.Control;
            ClientSize = new Size(800, 450);
            Controls.Add(imagePanel);
            Controls.Add(resultTextBox);
            Controls.Add(captureButton);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Cocr";
            Load += MainForm_Load;
            ResumeLayout(false);
        }

        #endregion
        private Panel imagePanel;
        public System.ComponentModel.BackgroundWorker backgroundWorker1;
        public Button captureButton;
        public RichTextBox resultTextBox;
    }
}

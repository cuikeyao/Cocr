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
            serverButton = new WinControl.ButtonCheck();
            serverStatusLabel = new Label();
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
            // serverButton
            // 
            serverButton.BackColor = Color.Transparent;
            serverButton.Checked = false;
            serverButton.CheckStyleX = WinControl.ButtonCheck.CheckStyle.style1;
            serverButton.Location = new Point(92, 12);
            serverButton.Name = "serverButton";
            serverButton.Size = new Size(54, 20);
            serverButton.TabIndex = 3;
            serverButton.Click += serverButton_Click;
            // 
            // serverStatusLabel
            // 
            serverStatusLabel.AutoSize = true;
            serverStatusLabel.Font = new Font("Microsoft YaHei UI", 10.5F, FontStyle.Regular, GraphicsUnit.Point, 134);
            serverStatusLabel.Location = new Point(18, 11);
            serverStatusLabel.Name = "serverStatusLabel";
            serverStatusLabel.Size = new Size(68, 20);
            serverStatusLabel.TabIndex = 4;
            serverStatusLabel.Text = "服务状态:";
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.Control;
            ClientSize = new Size(800, 450);
            Controls.Add(serverStatusLabel);
            Controls.Add(serverButton);
            Controls.Add(imagePanel);
            Controls.Add(resultTextBox);
            Controls.Add(captureButton);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Cocr";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Panel imagePanel;
        public System.ComponentModel.BackgroundWorker backgroundWorker1;
        public Button captureButton;
        public RichTextBox resultTextBox;
        private WinControl.ButtonCheck serverButton;
        private Label serverStatusLabel;
    }
}

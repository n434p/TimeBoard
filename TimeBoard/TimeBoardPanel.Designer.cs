using System;

namespace TimeBoard
{
    partial class TimeBoardPanel
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (dispatcherTimer != null)
            {
                dispatcherTimer.Stop();
                dispatcherTimer = null;
            }

            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.mainPanel = new System.Windows.Forms.Panel();
            this.addClockBtn = new System.Windows.Forms.Button();
            this.lightThemeBtn = new System.Windows.Forms.Button();
            this.analogTypeBtn = new System.Windows.Forms.Button();
            this.digitalTypeBtn = new System.Windows.Forms.Button();
            this.darkThemeBtn = new System.Windows.Forms.Button();
            this.mSizeBtn = new System.Windows.Forms.Button();
            this.lSizeBtn = new System.Windows.Forms.Button();
            this.sSizeBtn = new System.Windows.Forms.Button();
            this.separator2 = new System.Windows.Forms.PictureBox();
            this.separator1 = new System.Windows.Forms.PictureBox();
            this.clocksPanel = new BufferedFlowLayoutPanel();
            this.separator3 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.separator2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.separator1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.separator3)).BeginInit();
            this.SuspendLayout();
            //
            // mainPanel
            //
            this.mainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainPanel.AutoScroll = false;
            this.mainPanel.AutoSize = true;
            this.mainPanel.Margin = new System.Windows.Forms.Padding(0);
            // 
            // addClockBtn
            // 
            this.addClockBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.addClockBtn.FlatAppearance.BorderSize = 0;
            this.addClockBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.addClockBtn.Location = new System.Drawing.Point(458, 5);
            this.addClockBtn.Margin = new System.Windows.Forms.Padding(0);
            this.addClockBtn.Name = "addClockBtn";
            this.addClockBtn.Size = new System.Drawing.Size(44, 20);
            this.addClockBtn.TabIndex = 0;
            this.addClockBtn.Click += new System.EventHandler(this.OnAddClockBtn_Click);
            // 
            // lightThemeBtn
            // 
            this.lightThemeBtn.FlatAppearance.BorderSize = 0;
            this.lightThemeBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lightThemeBtn.Location = new System.Drawing.Point(99, 5);
            this.lightThemeBtn.Margin = new System.Windows.Forms.Padding(0);
            this.lightThemeBtn.Name = "lightThemeBtn";
            this.lightThemeBtn.Size = new System.Drawing.Size(24, 20);
            this.lightThemeBtn.TabIndex = 6;
            this.lightThemeBtn.Click += new System.EventHandler(this.OnThemeBtn_Click);
            // 
            // analogTypeBtn
            // 
            this.analogTypeBtn.FlatAppearance.BorderSize = 0;
            this.analogTypeBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.analogTypeBtn.Location = new System.Drawing.Point(5, 5);
            this.analogTypeBtn.Margin = new System.Windows.Forms.Padding(0);
            this.analogTypeBtn.Name = "analogTypeBtn";
            this.analogTypeBtn.Size = new System.Drawing.Size(24, 20);
            this.analogTypeBtn.TabIndex = 1;
            this.analogTypeBtn.Click += new System.EventHandler(this.OnClockTypeChanged);
            // 
            // digitalTypeBtn
            // 
            this.digitalTypeBtn.FlatAppearance.BorderSize = 0;
            this.digitalTypeBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.digitalTypeBtn.Location = new System.Drawing.Point(34, 5);
            this.digitalTypeBtn.Margin = new System.Windows.Forms.Padding(0);
            this.digitalTypeBtn.Name = "digitalTypeBtn";
            this.digitalTypeBtn.Size = new System.Drawing.Size(24, 20);
            this.digitalTypeBtn.TabIndex = 2;
            this.digitalTypeBtn.Click += new System.EventHandler(this.OnClockTypeChanged);
            // 
            // darkThemeBtn
            // 
            this.darkThemeBtn.FlatAppearance.BorderSize = 0;
            this.darkThemeBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.darkThemeBtn.Location = new System.Drawing.Point(70, 5);
            this.darkThemeBtn.Margin = new System.Windows.Forms.Padding(0);
            this.darkThemeBtn.Name = "darkThemeBtn";
            this.darkThemeBtn.Size = new System.Drawing.Size(24, 20);
            this.darkThemeBtn.TabIndex = 7;
            this.darkThemeBtn.Click += new System.EventHandler(this.OnThemeBtn_Click);
            // 
            // mSizeBtn
            // 
            this.mSizeBtn.FlatAppearance.BorderSize = 0;
            this.mSizeBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.mSizeBtn.Location = new System.Drawing.Point(162, 5);
            this.mSizeBtn.Margin = new System.Windows.Forms.Padding(5);
            this.mSizeBtn.Name = "mSizeBtn";
            this.mSizeBtn.Size = new System.Drawing.Size(24, 20);
            this.mSizeBtn.TabIndex = 3;
            this.mSizeBtn.Click += new System.EventHandler(this.OnClockScaleChanged);
            // 
            // lSizeBtn
            // 
            this.lSizeBtn.FlatAppearance.BorderSize = 0;
            this.lSizeBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lSizeBtn.Location = new System.Drawing.Point(191, 5);
            this.lSizeBtn.Margin = new System.Windows.Forms.Padding(5);
            this.lSizeBtn.Name = "lSizeBtn";
            this.lSizeBtn.Size = new System.Drawing.Size(24, 20);
            this.lSizeBtn.TabIndex = 5;
            this.lSizeBtn.Click += new System.EventHandler(this.OnClockScaleChanged);
            // 
            // sSizeBtn
            // 
            this.sSizeBtn.FlatAppearance.BorderSize = 0;
            this.sSizeBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.sSizeBtn.Location = new System.Drawing.Point(133, 5);
            this.sSizeBtn.Margin = new System.Windows.Forms.Padding(5);
            this.sSizeBtn.Name = "sSizeBtn";
            this.sSizeBtn.Size = new System.Drawing.Size(24, 20);
            this.sSizeBtn.TabIndex = 4;
            this.sSizeBtn.Click += new System.EventHandler(this.OnClockScaleChanged);
            // 
            // separator2
            // 
            this.separator2.BackgroundImage = TimeBoard.Properties.Resources.separators_light;
            this.separator2.Location = new System.Drawing.Point(127, 5);
            this.separator2.Name = "separator2";
            this.separator2.Size = new System.Drawing.Size(2, 20);
            this.separator2.TabIndex = 8;
            this.separator2.TabStop = false;
            // 
            // separator1
            // 
            this.separator1.BackgroundImage = global::TimeBoard.Properties.Resources.separators_light;
            this.separator1.Location = new System.Drawing.Point(63, 5);
            this.separator1.Name = "separator1";
            this.separator1.Size = new System.Drawing.Size(2, 20);
            this.separator1.TabIndex = 8;
            this.separator1.TabStop = false;
            // 
            // clocksPanel
            // 
            this.clocksPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.clocksPanel.Location = new System.Drawing.Point(5, 30);
            this.clocksPanel.Name = "clocksPanel";
            this.clocksPanel.Size = new System.Drawing.Size(497, 301);
            this.clocksPanel.TabIndex = 10;
            // 
            // separator3
            // 
            this.separator3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.separator3.Location = new System.Drawing.Point(5, 30);
            this.separator3.Name = "separator3";
            this.separator3.Size = new System.Drawing.Size(497, 2);
            this.separator3.TabIndex = 9;
            this.separator3.TabStop = false;
            // 
            // TimeBoardPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(507, 337);

            this.Controls.Add(this.mainPanel);
            this.Controls.Add(this.separator3);
            this.Controls.Add(this.clocksPanel);
            this.Controls.Add(this.addClockBtn);
            this.Controls.Add(this.separator2);
            this.Controls.Add(this.separator1);
            this.Controls.Add(this.lightThemeBtn);
            this.Controls.Add(this.analogTypeBtn);
            this.Controls.Add(this.digitalTypeBtn);
            this.Controls.Add(this.darkThemeBtn);
            this.Controls.Add(this.mSizeBtn);
            this.Controls.Add(this.lSizeBtn);
            this.Controls.Add(this.sSizeBtn);

            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.ForeColor = System.Drawing.SystemColors.AppWorkspace;
            this.Name = "TimeBoardPanel";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.separator2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.separator1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.separator3)).EndInit();
            this.ResumeLayout(false);

        }



        #endregion

        private System.Windows.Forms.Panel mainPanel;
        private System.Windows.Forms.Button addClockBtn;
        private System.Windows.Forms.Button mSizeBtn;
        private System.Windows.Forms.Button sSizeBtn;
        private System.Windows.Forms.Button lSizeBtn;
        private System.Windows.Forms.Button darkThemeBtn;
        private System.Windows.Forms.Button digitalTypeBtn;
        private System.Windows.Forms.Button analogTypeBtn;
        private System.Windows.Forms.Button lightThemeBtn;
        private System.Windows.Forms.PictureBox separator1;
        private System.Windows.Forms.PictureBox separator2;
        private BufferedFlowLayoutPanel clocksPanel;
        private System.Windows.Forms.PictureBox separator3;
    }

    class BufferedFlowLayoutPanel : System.Windows.Forms.FlowLayoutPanel 
    {
        public BufferedFlowLayoutPanel()
        {
            DoubleBuffered = true;
            FlowDirection = System.Windows.Forms.FlowDirection.LeftToRight;
        }
    }

}


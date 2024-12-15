using System.Windows.Forms;

namespace MultiMediaProject3
{
    partial class Form1
    {
        /// <summary>
        ///Gerekli tasarımcı değişkeni.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///Kullanılan tüm kaynakları temizleyin.
        /// </summary>
        ///<param name="disposing">yönetilen kaynaklar dispose edilmeliyse doğru; aksi halde yanlış.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer üretilen kod

        /// <summary>
        /// Tasarımcı desteği için gerekli metot - bu metodun 
        ///içeriğini kod düzenleyici ile değiştirmeyin.
        /// </summary>
        private void InitializeComponent()
        {
            this.timeLabel = new System.Windows.Forms.Label();
            this.Start = new System.Windows.Forms.Button();
            this.PreviusBtn = new System.Windows.Forms.Button();
            this.NextBtn = new System.Windows.Forms.Button();
            this.RandomBtn = new System.Windows.Forms.Button();
            this.LoopBtn = new System.Windows.Forms.Button();
            this.AddMusic = new System.Windows.Forms.Button();
            this.PlayListBox = new System.Windows.Forms.ListBox();
            this.playbackTrackBar = new System.Windows.Forms.TrackBar();
            this.trackBar2 = new System.Windows.Forms.TrackBar();
            this.RewindBtn = new System.Windows.Forms.Button();
            this.waveformBox = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.playbackTrackBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.waveformBox)).BeginInit();
            this.SuspendLayout();
            // 
            // timeLabel
            // 
            this.timeLabel.AutoSize = true;
            this.timeLabel.Location = new System.Drawing.Point(57, 312);
            this.timeLabel.Name = "timeLabel";
            this.timeLabel.Size = new System.Drawing.Size(78, 20);
            this.timeLabel.TabIndex = 0;
            this.timeLabel.Text = "timeLabel";
            // 
            // Start
            // 
            this.Start.Location = new System.Drawing.Point(533, 385);
            this.Start.Name = "Start";
            this.Start.Size = new System.Drawing.Size(107, 41);
            this.Start.TabIndex = 1;
            this.Start.Text = "Start/Stop";
            this.Start.UseVisualStyleBackColor = true;
            this.Start.Click += new System.EventHandler(this.Start_Click);
            // 
            // PreviusBtn
            // 
            this.PreviusBtn.Location = new System.Drawing.Point(418, 385);
            this.PreviusBtn.Name = "PreviusBtn";
            this.PreviusBtn.Size = new System.Drawing.Size(77, 41);
            this.PreviusBtn.TabIndex = 2;
            this.PreviusBtn.Text = "PreviusBtn";
            this.PreviusBtn.UseVisualStyleBackColor = true;
            this.PreviusBtn.Click += new System.EventHandler(this.PreviusBtn_Click);
            // 
            // NextBtn
            // 
            this.NextBtn.Location = new System.Drawing.Point(659, 385);
            this.NextBtn.Name = "NextBtn";
            this.NextBtn.Size = new System.Drawing.Size(84, 41);
            this.NextBtn.TabIndex = 3;
            this.NextBtn.Text = "NEXT";
            this.NextBtn.UseVisualStyleBackColor = true;
            this.NextBtn.Click += new System.EventHandler(this.NextBtn_Click);
            // 
            // RandomBtn
            // 
            this.RandomBtn.Location = new System.Drawing.Point(266, 385);
            this.RandomBtn.Name = "RandomBtn";
            this.RandomBtn.Size = new System.Drawing.Size(80, 41);
            this.RandomBtn.TabIndex = 4;
            this.RandomBtn.Text = "Random";
            this.RandomBtn.UseVisualStyleBackColor = true;
            this.RandomBtn.Click += new System.EventHandler(this.RandomBtn_Click);
            // 
            // LoopBtn
            // 
            this.LoopBtn.Location = new System.Drawing.Point(814, 385);
            this.LoopBtn.Name = "LoopBtn";
            this.LoopBtn.Size = new System.Drawing.Size(83, 41);
            this.LoopBtn.TabIndex = 5;
            this.LoopBtn.Text = "Loop";
            this.LoopBtn.UseVisualStyleBackColor = true;
            this.LoopBtn.Click += new System.EventHandler(this.LoopBtn_Click);
            // 
            // AddMusic
            // 
            this.AddMusic.Location = new System.Drawing.Point(1053, 37);
            this.AddMusic.Name = "AddMusic";
            this.AddMusic.Size = new System.Drawing.Size(93, 38);
            this.AddMusic.TabIndex = 6;
            this.AddMusic.Text = "AddMusic";
            this.AddMusic.UseVisualStyleBackColor = true;
            this.AddMusic.Click += new System.EventHandler(this.AddMusic_Click);
            // 
            // PlayListBox
            // 
            this.PlayListBox.FormattingEnabled = true;
            this.PlayListBox.ItemHeight = 20;
            this.PlayListBox.Location = new System.Drawing.Point(780, 88);
            this.PlayListBox.Name = "PlayListBox";
            this.PlayListBox.Size = new System.Drawing.Size(350, 244);
            this.PlayListBox.TabIndex = 7;
            // 
            // playbackTrackBar
            // 
            this.playbackTrackBar.Location = new System.Drawing.Point(167, 312);
            this.playbackTrackBar.Name = "playbackTrackBar";
            this.playbackTrackBar.Size = new System.Drawing.Size(516, 69);
            this.playbackTrackBar.TabIndex = 8;
            this.playbackTrackBar.Scroll += new System.EventHandler(this.trackBar1_Scroll);
            // 
            // trackBar2
            // 
            this.trackBar2.Location = new System.Drawing.Point(470, 289);
            this.trackBar2.Name = "trackBar2";
            this.trackBar2.Size = new System.Drawing.Size(8, 69);
            this.trackBar2.TabIndex = 9;
            // 
            // RewindBtn
            // 
            this.RewindBtn.Location = new System.Drawing.Point(140, 385);
            this.RewindBtn.Name = "RewindBtn";
            this.RewindBtn.Size = new System.Drawing.Size(78, 41);
            this.RewindBtn.TabIndex = 10;
            this.RewindBtn.Text = "Rewind";
            this.RewindBtn.UseVisualStyleBackColor = true;
            this.RewindBtn.Click += new System.EventHandler(this.RewindBtn_Click);
            // 
            // waveformBox
            // 
            this.waveformBox.Location = new System.Drawing.Point(192, 37);
            this.waveformBox.Name = "waveformBox";
            this.waveformBox.Size = new System.Drawing.Size(503, 246);
            this.waveformBox.TabIndex = 11;
            this.waveformBox.TabStop = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1244, 480);
            this.Controls.Add(this.waveformBox);
            this.Controls.Add(this.RewindBtn);
            this.Controls.Add(this.trackBar2);
            this.Controls.Add(this.playbackTrackBar);
            this.Controls.Add(this.PlayListBox);
            this.Controls.Add(this.AddMusic);
            this.Controls.Add(this.LoopBtn);
            this.Controls.Add(this.RandomBtn);
            this.Controls.Add(this.NextBtn);
            this.Controls.Add(this.PreviusBtn);
            this.Controls.Add(this.Start);
            this.Controls.Add(this.timeLabel);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.playbackTrackBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.waveformBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label timeLabel;
        private System.Windows.Forms.Button Start;
        private System.Windows.Forms.Button PreviusBtn;
        private System.Windows.Forms.Button NextBtn;
        private System.Windows.Forms.Button RandomBtn;
        private System.Windows.Forms.Button AddMusic;
        private System.Windows.Forms.Button LoopBtn;
        private System.Windows.Forms.ListBox PlayListBox;
        private TrackBar playbackTrackBar;
        private TrackBar trackBar2;
        private Button RewindBtn;
        private PictureBox waveformBox;
    }
}


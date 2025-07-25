namespace App1_Client
{
    partial class Form1
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
            components = new System.ComponentModel.Container();
            txtIP = new TextBox();
            txtPort = new TextBox();
            txtEnlem = new TextBox();
            txtBoylam = new TextBox();
            txtAciklama = new TextBox();
            btnConnect = new Button();
            btnVeriGonder = new Button();
            heartbeatTimer = new System.Windows.Forms.Timer(components);
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            label5 = new Label();
            label6 = new Label();
            label7 = new Label();
            label8 = new Label();
            label9 = new Label();
            URL = new TextBox();
            btnDosyaSec = new Button();
            btnVideoStreamBaslat = new Button();
            SuspendLayout();
            // 
            // txtIP
            // 
            txtIP.Location = new Point(112, 50);
            txtIP.Name = "txtIP";
            txtIP.Size = new Size(119, 23);
            txtIP.TabIndex = 0;
            // 
            // txtPort
            // 
            txtPort.Location = new Point(112, 79);
            txtPort.Name = "txtPort";
            txtPort.Size = new Size(119, 23);
            txtPort.TabIndex = 1;
            // 
            // txtEnlem
            // 
            txtEnlem.Location = new Point(112, 186);
            txtEnlem.Name = "txtEnlem";
            txtEnlem.Size = new Size(119, 23);
            txtEnlem.TabIndex = 2;
            // 
            // txtBoylam
            // 
            txtBoylam.Location = new Point(112, 216);
            txtBoylam.Name = "txtBoylam";
            txtBoylam.Size = new Size(119, 23);
            txtBoylam.TabIndex = 3;
            // 
            // txtAciklama
            // 
            txtAciklama.Location = new Point(112, 248);
            txtAciklama.Name = "txtAciklama";
            txtAciklama.Size = new Size(119, 23);
            txtAciklama.TabIndex = 4;
            // 
            // btnConnect
            // 
            btnConnect.BackColor = Color.Yellow;
            btnConnect.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 162);
            btnConnect.Location = new Point(112, 108);
            btnConnect.Name = "btnConnect";
            btnConnect.Size = new Size(119, 42);
            btnConnect.TabIndex = 5;
            btnConnect.Text = "BAĞLAN";
            btnConnect.UseVisualStyleBackColor = false;
            // 
            // btnVeriGonder
            // 
            btnVeriGonder.BackColor = Color.Yellow;
            btnVeriGonder.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 162);
            btnVeriGonder.Location = new Point(112, 277);
            btnVeriGonder.Name = "btnVeriGonder";
            btnVeriGonder.Size = new Size(119, 41);
            btnVeriGonder.TabIndex = 6;
            btnVeriGonder.Text = "GÖNDER";
            btnVeriGonder.UseVisualStyleBackColor = false;
            // 
            // heartbeatTimer
            // 
            heartbeatTimer.Tick += heartbeatTimer_Tick;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI Semibold", 14F, FontStyle.Bold);
            label1.Location = new Point(43, 9);
            label1.Name = "label1";
            label1.Size = new Size(194, 25);
            label1.TabIndex = 7;
            label1.Text = "BAĞLANTI AYARLARI";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 10F);
            label2.Location = new Point(12, 51);
            label2.Name = "label2";
            label2.Size = new Size(24, 19);
            label2.TabIndex = 8;
            label2.Text = "IP:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 10F);
            label3.Location = new Point(12, 80);
            label3.Name = "label3";
            label3.Size = new Size(45, 19);
            label3.TabIndex = 9;
            label3.Text = "PORT:";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 10F);
            label4.Location = new Point(12, 187);
            label4.Name = "label4";
            label4.Size = new Size(56, 19);
            label4.TabIndex = 10;
            label4.Text = "ENLEM:";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI", 10F);
            label5.Location = new Point(12, 249);
            label5.Name = "label5";
            label5.Size = new Size(80, 19);
            label5.TabIndex = 11;
            label5.Text = "AÇIKLAMA:";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Segoe UI", 10F);
            label6.Location = new Point(12, 217);
            label6.Name = "label6";
            label6.Size = new Size(68, 19);
            label6.TabIndex = 12;
            label6.Text = "BOYLAM:";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Font = new Font("Segoe UI Semibold", 14F, FontStyle.Bold);
            label7.Location = new Point(43, 153);
            label7.Name = "label7";
            label7.Size = new Size(207, 25);
            label7.TabIndex = 13;
            label7.Text = "YENİ KOORDİNAT İLET";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Font = new Font("Segoe UI Semibold", 14F, FontStyle.Bold);
            label8.Location = new Point(43, 321);
            label8.Name = "label8";
            label8.Size = new Size(216, 25);
            label8.TabIndex = 14;
            label8.Text = "VİDEO STREAM BAŞLAT";
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Font = new Font("Segoe UI", 10F);
            label9.Location = new Point(12, 365);
            label9.Name = "label9";
            label9.Size = new Size(37, 19);
            label9.TabIndex = 15;
            label9.Text = "URL:";
            // 
            // URL
            // 
            URL.Location = new Point(112, 364);
            URL.Name = "URL";
            URL.Size = new Size(119, 23);
            URL.TabIndex = 16;
            // 
            // btnDosyaSec
            // 
            btnDosyaSec.BackColor = Color.Cornsilk;
            btnDosyaSec.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 162);
            btnDosyaSec.Location = new Point(112, 393);
            btnDosyaSec.Name = "btnDosyaSec";
            btnDosyaSec.Size = new Size(119, 23);
            btnDosyaSec.TabIndex = 17;
            btnDosyaSec.Text = "DOSYA SEÇ";
            btnDosyaSec.UseVisualStyleBackColor = false;
            btnDosyaSec.Click += btnDosyaSec_Click;
            // 
            // btnVideoStreamBaslat
            // 
            btnVideoStreamBaslat.BackColor = Color.Yellow;
            btnVideoStreamBaslat.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 162);
            btnVideoStreamBaslat.Location = new Point(112, 422);
            btnVideoStreamBaslat.Name = "btnVideoStreamBaslat";
            btnVideoStreamBaslat.Size = new Size(119, 41);
            btnVideoStreamBaslat.TabIndex = 18;
            btnVideoStreamBaslat.Text = "BAŞLAT";
            btnVideoStreamBaslat.UseVisualStyleBackColor = false;
            btnVideoStreamBaslat.Click += btnVideoStreamBaslat_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(303, 473);
            Controls.Add(btnVideoStreamBaslat);
            Controls.Add(btnDosyaSec);
            Controls.Add(URL);
            Controls.Add(label9);
            Controls.Add(label8);
            Controls.Add(label7);
            Controls.Add(label6);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(btnVeriGonder);
            Controls.Add(btnConnect);
            Controls.Add(txtAciklama);
            Controls.Add(txtBoylam);
            Controls.Add(txtEnlem);
            Controls.Add(txtPort);
            Controls.Add(txtIP);
            Name = "Form1";
            Text = "Form1";
            Load += Form1_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox txtIP;
        private TextBox txtPort;
        private TextBox txtEnlem;
        private TextBox txtBoylam;
        private TextBox txtAciklama;
        private Button btnConnect;
        private Button btnVeriGonder;
        private System.Windows.Forms.Timer heartbeatTimer;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
        private Label label7;
        private Label label8;
        private Label label9;
        private TextBox URL;
        private Button btnDosyaSec;
        private Button btnVideoStreamBaslat;
    }
}

namespace App2_Server
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
            txtPort = new TextBox();
            heartbeatCheckTimer = new System.Windows.Forms.Timer(components);
            txtIP = new TextBox();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            dataGridCoords = new DataGridView();
            gecisbuton = new Button();
            label4 = new Label();
            istemcilist = new DataGridView();
            panelMapContainer = new Panel();
            atesEt = new Button();
            btnReload = new Button();
            txtMermi = new TextBox();
            pictureBox1 = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)dataGridCoords).BeginInit();
            ((System.ComponentModel.ISupportInitialize)istemcilist).BeginInit();
            panelMapContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // txtPort
            // 
            txtPort.Location = new Point(107, 79);
            txtPort.Name = "txtPort";
            txtPort.Size = new Size(136, 23);
            txtPort.TabIndex = 0;
            // 
            // txtIP
            // 
            txtIP.Location = new Point(107, 50);
            txtIP.Name = "txtIP";
            txtIP.Size = new Size(136, 23);
            txtIP.TabIndex = 4;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 12F);
            label1.Location = new Point(46, 9);
            label1.Name = "label1";
            label1.Size = new Size(155, 21);
            label1.TabIndex = 5;
            label1.Text = "BAĞLANTI AYARLARI";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 10F);
            label2.Location = new Point(34, 53);
            label2.Name = "label2";
            label2.Size = new Size(24, 19);
            label2.TabIndex = 6;
            label2.Text = "IP:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 10F);
            label3.Location = new Point(34, 83);
            label3.Name = "label3";
            label3.Size = new Size(45, 19);
            label3.TabIndex = 7;
            label3.Text = "PORT:";
            // 
            // dataGridCoords
            // 
            dataGridCoords.BackgroundColor = SystemColors.Control;
            dataGridCoords.BorderStyle = BorderStyle.None;
            dataGridCoords.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridCoords.Location = new Point(3, 3);
            dataGridCoords.Name = "dataGridCoords";
            dataGridCoords.Size = new Size(643, 94);
            dataGridCoords.TabIndex = 3;
            // 
            // gecisbuton
            // 
            gecisbuton.BackColor = Color.Yellow;
            gecisbuton.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 162);
            gecisbuton.Location = new Point(771, 438);
            gecisbuton.Name = "gecisbuton";
            gecisbuton.Size = new Size(163, 36);
            gecisbuton.TabIndex = 9;
            gecisbuton.Text = "VİDEO";
            gecisbuton.UseVisualStyleBackColor = false;
            gecisbuton.Click += gecisbuton_Click;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 12F);
            label4.Location = new Point(46, 249);
            label4.Name = "label4";
            label4.Size = new Size(167, 21);
            label4.TabIndex = 12;
            label4.Text = "BAĞLI İSTEMCİ LİSTESİ";
            // 
            // istemcilist
            // 
            istemcilist.BackgroundColor = SystemColors.Control;
            istemcilist.BorderStyle = BorderStyle.None;
            istemcilist.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            istemcilist.Location = new Point(12, 273);
            istemcilist.Name = "istemcilist";
            istemcilist.Size = new Size(270, 122);
            istemcilist.TabIndex = 11;
            // 
            // panelMapContainer
            // 
            panelMapContainer.Controls.Add(dataGridCoords);
            panelMapContainer.Location = new Point(288, 2);
            panelMapContainer.Name = "panelMapContainer";
            panelMapContainer.Size = new Size(649, 429);
            panelMapContainer.TabIndex = 13;
            // 
            // atesEt
            // 
            atesEt.BackColor = Color.Yellow;
            atesEt.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 162);
            atesEt.Location = new Point(288, 437);
            atesEt.Name = "atesEt";
            atesEt.Size = new Size(162, 36);
            atesEt.TabIndex = 15;
            atesEt.Text = "ATEŞ ET";
            atesEt.UseVisualStyleBackColor = false;
            atesEt.Visible = false;
            atesEt.Click += atesEt_Click;
            // 
            // btnReload
            // 
            btnReload.BackColor = Color.Yellow;
            btnReload.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 162);
            btnReload.Location = new Point(456, 438);
            btnReload.Name = "btnReload";
            btnReload.Size = new Size(163, 36);
            btnReload.TabIndex = 16;
            btnReload.Text = "MERMİ +100";
            btnReload.UseVisualStyleBackColor = false;
            btnReload.Visible = false;
            btnReload.Click += btnReload_Click;
            // 
            // txtMermi
            // 
            txtMermi.Location = new Point(625, 446);
            txtMermi.Name = "txtMermi";
            txtMermi.Size = new Size(140, 23);
            txtMermi.TabIndex = 18;
            txtMermi.Visible = false;
            // 
            // pictureBox1
            // 
            pictureBox1.BackColor = Color.Black;
            pictureBox1.Location = new Point(288, 2);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(649, 430);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 4;
            pictureBox1.TabStop = false;
            pictureBox1.Visible = false;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(949, 507);
            Controls.Add(pictureBox1);
            Controls.Add(txtMermi);
            Controls.Add(btnReload);
            Controls.Add(atesEt);
            Controls.Add(panelMapContainer);
            Controls.Add(label4);
            Controls.Add(istemcilist);
            Controls.Add(gecisbuton);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(txtIP);
            Controls.Add(txtPort);
            Name = "Form1";
            Text = "Form1";
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)dataGridCoords).EndInit();
            ((System.ComponentModel.ISupportInitialize)istemcilist).EndInit();
            panelMapContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox txtPort;
        private System.Windows.Forms.Timer heartbeatCheckTimer;
        private TextBox txtIP;
        private Label label1;
        private Label label2;
        private Label label3;
        private DataGridView dataGridCoords;
        private Button gecisbuton;
        private Label label4;
        private DataGridView istemcilist;
        private Button atesEt;
        private Button btnReload;
        private TextBox txtMermi;
        private PictureBox pictureBox1;
    }
}

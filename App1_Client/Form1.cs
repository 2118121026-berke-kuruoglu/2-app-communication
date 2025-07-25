using System.Net.Sockets;
using System.Text;
using MessagePack;
using System.Diagnostics;

namespace App1_Client
{
    public partial class Form1 : Form
    {
        TcpClient client;
        NetworkStream stream;
        private Label notificationLabel;
        private System.Windows.Forms.Timer notificationTimer;
        private Process _ffmpegProcess;

        public Form1()
        {
            InitializeComponent();
            this.FormClosing += new FormClosingEventHandler(Form1_FormClosing);

            heartbeatTimer.Interval = 1000; // 1 saniye
            heartbeatTimer.Tick += heartbeatTimer_Tick;

            btnConnect.Click += btnConnect_Click;
            btnVeriGonder.Click += btnVeriGonder_Click;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            InitializeNotificationPanel();
            txtIP.Text = "127.0.0.1";
            txtPort.Text = "9000";


        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            // Butonun mevcut metnine göre işlem yap
            if (btnConnect.Text == "BAĞLAN")
            {
                // Bağlanma modunda:
                try
                {
                    string ip = txtIP.Text;
                    int port = int.Parse(txtPort.Text);

                    // Yeni bir TcpClient örneği oluştur ve bağlan
                    client = new TcpClient();
                    client.Connect(ip, port); // Bağlantıyı kur

                    // önceki bağlantıyı kapat
                    stream?.Close();
                    stream = null;

                    stream = client.GetStream(); // Ağ akışını al

                    heartbeatTimer.Start(); // Heartbeat timer'ı başlat
                    ShowNotification("Bağlantı başarılı!");
                    btnConnect.Text = "BAĞLANTIYI KES"; // Buton metnini değiştir
                }
                catch (FormatException)
                {
                    ShowNotification("Lütfen geçerli bir port numarası girin.");
                }
                catch (Exception ex)
                {
                    ShowNotification("Bağlantı hatası: " + ex.Message);

                    if (stream != null)
                    {
                        stream.Dispose();
                        stream = null;
                    }
                    if (client != null)
                    {
                        client.Dispose();
                        client = null;
                    }
                    heartbeatTimer.Stop();
                }
            }
            else 
            {

                try
                {

                    if (heartbeatTimer != null)
                    {
                        heartbeatTimer.Stop();
                    }


                    if (stream != null)
                    {
                        stream.Close();
                        stream.Dispose();
                        stream = null;
                    }

                    if (client != null)
                    {
                        client.Close();
                        client.Dispose(); 
                        client = null;
                    }

                    ShowNotification("Bağlantı kesildi.");
                    btnConnect.Text = "BAĞLAN";
                }
                catch (Exception ex)
                {
                    ShowNotification("Bağlantı kesilirken hata oluştu: " + ex.Message);
                }
            }
        }

        public enum MessageType : byte
        {
            CoordinateData = 0,
            Heartbeat = 1,
            StartRTSP = 2
        }

        private void heartbeatTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                if (stream != null && stream.CanWrite)
                {
                    byte[] lengthBytes = BitConverter.GetBytes(1); // sadece 1 byte'lık message type
                    byte[] message = new byte[5]; // 4 byte uzunluk + 1 byte message type
                    Array.Copy(lengthBytes, 0, message, 0, 4);
                    message[4] = (byte)MessageType.Heartbeat;

                    stream.Write(message, 0, message.Length);
                }
            }
            catch (Exception ex)
            {
                ShowNotification("Heartbeat hatası: " + ex.Message);
            }
        }

        private void btnVeriGonder_Click(object sender, EventArgs e)
        {
            try
            {
                if (stream != null && stream.CanWrite)
                {
                    KoordinatData data = new KoordinatData
                    {
                        Enlem = double.Parse(txtEnlem.Text),
                        Boylam = double.Parse(txtBoylam.Text),
                        Aciklama = txtAciklama.Text
                    };

                    byte[] payload = MessagePackSerializer.Serialize(data);

                    byte[] lengthBytes = BitConverter.GetBytes(payload.Length + 1);
                    byte[] message = new byte[4 + 1 + payload.Length];

                    Array.Copy(lengthBytes, 0, message, 0, 4);
                    message[4] = (byte)MessageType.CoordinateData;
                    Array.Copy(payload, 0, message, 5, payload.Length);

                    stream.Write(message, 0, message.Length);

                    ShowNotification("Koordinat verisi gönderildi!");
                }
            }
            catch (Exception ex)
            {
                ShowNotification("Veri gönderim hatası: " + ex.Message);
            }
        }

        private void InitializeNotificationPanel()
        {
            notificationLabel = new Label
            {
                Text = "",
                AutoSize = false,
                Height = 30,
                Dock = DockStyle.Bottom,
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.LightYellow,
                ForeColor = Color.Black,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Visible = false
            };

            notificationTimer = new System.Windows.Forms.Timer();
            notificationTimer.Interval = 3000; // 3 saniye
            notificationTimer.Tick += (s, e) =>
            {
                notificationLabel.Visible = false;
                notificationTimer.Stop();
            };

            this.Controls.Add(notificationLabel);
            this.Controls.SetChildIndex(notificationLabel, 0);
            notificationLabel.BringToFront();
        }

        public void ShowNotification(string message)
        {
            notificationLabel.Text = message;
            notificationLabel.Visible = true;
            notificationTimer.Start();
        }

        private void btnDosyaSec_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Video Dosyaları|*.mp4;*.avi;*.mkv|Tüm Dosyalar|*.*";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                URL.Text = ofd.FileName;
            }
        }

        private void btnVideoStreamBaslat_Click(object sender, EventArgs e)
        {
            string rtspUrl = "rtsp://localhost:8554/live";
            if (btnVideoStreamBaslat.Text == "BAŞLAT")
            {
                string newFilePath = URL.Text;
                PushNewVideo(newFilePath, rtspUrl);
                NotifyStartRTSPToServer();

                btnVideoStreamBaslat.Text = "DURDUR";
            }
            else
            {
                StopRTSPStream();
                btnVideoStreamBaslat.Text = "BAŞLAT";
            }
        }

        public void PushNewVideo(string newFilePath, string rtspUrl)
        {
            StopRTSPStream();

            Task.Delay(500).Wait();

            StartRTSPStream(newFilePath, rtspUrl);
        }

        public void StartRTSPStream(string sourcePath, string rtspUrl)
        {
            string ffmpegArgs = $"-re -stream_loop -1 -i \"{sourcePath}\" -s 640x480 -c:v libx264 -preset ultrafast -tune zerolatency -an -f rtsp -rtsp_transport tcp {rtspUrl}";

            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = "C:/Users/berko/source/repos/TcpVideoCommunication/App1_Client/bin/Debug/net8.0-windows/ffmpeg.exe",
                Arguments = ffmpegArgs,
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };

            _ffmpegProcess = new Process { StartInfo = startInfo };

            _ffmpegProcess.OutputDataReceived += (sender, e) => { Console.WriteLine($"FFmpeg Output: {e.Data}"); };
            _ffmpegProcess.ErrorDataReceived += (sender, e) => { Console.WriteLine($"FFmpeg Error: {e.Data}"); };

            _ffmpegProcess.Start();
            _ffmpegProcess.BeginOutputReadLine();
            _ffmpegProcess.BeginErrorReadLine();

            ShowNotification($"FFmpeg RTSP yayını başlatıldı: {rtspUrl}");
        }

        public void StopRTSPStream()
        {
            if (_ffmpegProcess != null && !_ffmpegProcess.HasExited)
            {
                try
                {
                    _ffmpegProcess.CloseMainWindow();

                    _ffmpegProcess.Dispose();
                    _ffmpegProcess = null;
                    ShowNotification("FFmpeg RTSP yayını durduruldu.");
                }
                catch (Exception ex)
                {
                    ShowNotification($"FFmpeg sürecini sonlandırırken hata: {ex.Message}");
                }
            }
        }

        private void NotifyStartRTSPToServer()
        {
            if (stream != null && stream.CanWrite)
            {
                byte[] lengthBytes = BitConverter.GetBytes(1); // sadece 1 byte'lık message type
                byte[] message = new byte[5]; // 4 byte uzunluk + 1 byte message type
                Array.Copy(lengthBytes, 0, message, 0, 4);
                message[4] = (byte)MessageType.StartRTSP;

                stream.Write(message, 0, message.Length);
                ShowNotification("App2'ye RTSP başlat mesajı gönderildi.");
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {

            if (client != null)
            {
                try
                {

                    if (stream != null)
                    {
                        stream.Close();
                        stream.Dispose();
                        stream = null;
                    }
                    client.Close();
                    client = null;
                    ShowNotification("Sunucu bağlantısı kesildi.");
                }
                catch (Exception ex)
                {
                    ShowNotification("Bağlantı kapatılırken hata oluştu: " + ex.Message);
                }
            }

            if (heartbeatTimer != null && heartbeatTimer.Enabled)
            {
                heartbeatTimer.Stop();
            }

            if (_ffmpegProcess != null && !_ffmpegProcess.HasExited)
            {
                try
                {
                    _ffmpegProcess.Kill();
                    _ffmpegProcess.Dispose();
                    _ffmpegProcess = null;
                    ShowNotification("FFmpeg süreci sonlandırıldı.");
                }
                catch (Exception ex)
                {
                    ShowNotification("FFmpeg süreci sonlandırılırken hata: " + ex.Message);
                }
            }
        }

        [MessagePackObject]
        public class KoordinatData
        {
            [Key(0)]
            public double Enlem { get; set; }

            [Key(1)]
            public double Boylam { get; set; }

            [Key(2)]
            public string Aciklama { get; set; }
        }

    }
}


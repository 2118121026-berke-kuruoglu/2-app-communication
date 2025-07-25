using GMap.NET;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;
using LibVLCSharp.Shared;
using MessagePack;
using System.Diagnostics;
using System.Globalization;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;


namespace App2_Server
{
    public partial class Form1 : Form
    {
        private Panel panelMapContainer;
        TcpListener listener;
        NetworkStream stream;
        Thread listenThread;
        private DateTime? lastHeartbeatTime = null;

        private Label notificationLabel;
        private System.Windows.Forms.Timer notificationTimer;
        private GMapOverlay markersOverlay;

        private Label lblConnectionStatus;
        private GMap.NET.WindowsForms.GMapControl gMapControl1;

        LibVLC _libVLC;
        MediaPlayer _mediaPlayer;
        WriteableBitmapBuffer _videoBuffer;
        int ammoCount = 100;

        private string clientIP = "";

        public Form1()
        {
            InitializeComponent();
            InitializeNotificationPanel();

            Core.Initialize();

            _libVLC = new LibVLC();

            lblConnectionStatus = new Label
            {
                Text = "BAĞLANTI : PASİF",
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Width = txtPort.Width,
                Height = 30,
                BackColor = Color.Red,
                ForeColor = Color.White,
                Location = new Point(txtPort.Left, txtPort.Bottom + 5),
                BorderStyle = BorderStyle.FixedSingle
            };
            Controls.Add(lblConnectionStatus);

            heartbeatCheckTimer.Interval = 2000;
            heartbeatCheckTimer.Tick += HeartbeatCheckTimer_Tick;

            dataGridCoords.Columns.Clear();
            dataGridCoords.Columns.Add("TarihSaat", "Tarih/Saat");
            dataGridCoords.Columns.Add("Enlem", "Enlem");
            dataGridCoords.Columns.Add("Boylam", "Boylam");
            dataGridCoords.Columns.Add("Aciklama", "Açıklama");

            dataGridCoords.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridCoords.ReadOnly = true;
            dataGridCoords.AllowUserToAddRows = false;
            dataGridCoords.AllowUserToDeleteRows = false;

            dataGridCoords.SelectionChanged += DataGridCoords_SelectionChanged;
            Load += Form1_Load;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            StartServer();

            txtIP.Text = "127.0.0.1";
            txtPort.Text = "9000";
            panelMapContainer.Controls.Clear();


            // DataGridView ve GMapControl ayarları - bu kısım aynen kalacak
            dataGridCoords.Dock = DockStyle.Top;
            dataGridCoords.Height = 100;
            panelMapContainer.Controls.Add(dataGridCoords);

            gMapControl1 = new GMapControl
            {
                Dock = DockStyle.Fill,
                MapProvider = GMap.NET.MapProviders.GoogleMapProvider.Instance,
                MinZoom = 0,
                MaxZoom = 24,
                Zoom = 10,
                ShowCenter = false,
                DragButton = MouseButtons.Left,
            };

            markersOverlay = new GMapOverlay("markers");
            gMapControl1.Overlays.Add(markersOverlay);
            markersOverlay.Markers.Clear();
            panelMapContainer.Controls.Add(gMapControl1);
            panelMapContainer.Controls.SetChildIndex(dataGridCoords, 1);
            panelMapContainer.Controls.SetChildIndex(gMapControl1, 0);
            panelMapContainer.Visible = true;

            // İstemci listesi yapılandırması - bu kısım aynen kalacak
            istemcilist.Columns.Clear();
            istemcilist.Columns.Add("ClientName", "İstemci Adı");
            istemcilist.Columns.Add("Status", "Durum");
            istemcilist.ReadOnly = true;
            istemcilist.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }

        private void StopVideo()
        {
            if (_mediaPlayer != null)
            {
                if (_mediaPlayer.IsPlaying)
                    _mediaPlayer.Stop();

                _mediaPlayer.Dispose();
                _mediaPlayer = null;
            }

            if (pictureBox1.Image != null)
            {
                pictureBox1.Image.Dispose();
                pictureBox1.Image = null;
            }
        }

        public void UpdateClientList(string clientName, bool isConnected)
        {
            // İstemci adını listede ara
            foreach (DataGridViewRow row in istemcilist.Rows)
            {
                if (row.Cells["ClientName"].Value?.ToString() == clientName)
                {
                    // Güncelle
                    row.Cells["Status"].Value = isConnected ? "Aktif" : "Pasif";
                    row.DefaultCellStyle.BackColor = isConnected ? Color.LightGreen : Color.LightCoral;
                    return;
                }
            }

            // Yeni istemci ekle
            istemcilist.Rows.Add(clientName, isConnected ? "Aktif" : "Pasif");
            int newIndex = istemcilist.Rows.Count - 1;
            istemcilist.Rows[newIndex].DefaultCellStyle.BackColor = isConnected ? Color.LightGreen : Color.LightCoral;
        }

        private void DataGridCoords_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridCoords.SelectedRows.Count == 0 || gMapControl1 == null || markersOverlay == null)
                return;

            var row = dataGridCoords.SelectedRows[0];
            if (row.Cells["Enlem"].Value == null || row.Cells["Boylam"].Value == null)
                return;

            if (double.TryParse(row.Cells["Enlem"].Value.ToString(), NumberStyles.Float, CultureInfo.InvariantCulture, out double enlem) &&
                double.TryParse(row.Cells["Boylam"].Value.ToString(), NumberStyles.Float, CultureInfo.InvariantCulture, out double boylam))
            {
                var point = new PointLatLng(enlem, boylam);
                markersOverlay.Markers.Clear();

                var marker = new GMarkerGoogle(point, GMarkerGoogleType.red_dot)
                {
                    ToolTipText = row.Cells["Aciklama"].Value?.ToString(),
                    ToolTipMode = MarkerTooltipMode.OnMouseOver
                };

                markersOverlay.Markers.Add(marker);

                gMapControl1.Position = point;
                gMapControl1.Zoom = 15;

            }
            else
            {
                ShowNotification("Geçersiz enlem veya boylam formatı!");
            }

        }

        private void StartServer()
        {
            try
            {
                int port = string.IsNullOrWhiteSpace(txtPort.Text) ? 5000 : int.Parse(txtPort.Text.Trim());
                listener = new TcpListener(IPAddress.Any, port);
                listener.Start();

                listenThread = new Thread(ListenForClient)
                {
                    IsBackground = true
                };
                listenThread.Start();

                heartbeatCheckTimer.Start();
                ShowNotification("Sunucu başlatıldı!");
            }
            catch (Exception ex)
            {
                ShowNotification("Sunucu başlatma hatası: " + ex.Message);
            }
        }

        private void UpdateConnectionStatus(bool isConnected)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => UpdateConnectionStatus(isConnected)));
                return;
            }

            lblConnectionStatus.Text = isConnected ? "BAĞLANTI : AKTİF" : "BAĞLANTI : PASİF";
            lblConnectionStatus.BackColor = isConnected ? Color.Green : Color.Red;
            lblConnectionStatus.ForeColor = Color.White;

            // Bağlı istemcinin durumu da güncellensin
            if (!string.IsNullOrEmpty(clientIP))
            {
                UpdateClientList(clientIP, isConnected);
            }
        }

        private void ListenForClient()
        {
            while (true) // Sürekli dinlemeye devam et
            {
                try
                {
                    TcpClient client = listener.AcceptTcpClient();
                    stream?.Close(); // varsa eski bağlantıyı kapat
                    stream = client.GetStream();
                    lastHeartbeatTime = DateTime.Now;

                    clientIP = ((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString();
                    Invoke(() => UpdateClientList(clientIP, true));
                    UpdateConnectionStatus(true);

                    while (client.Connected)
                    {
                        byte[] lengthBytes = new byte[4];
                        int readLen = 0;
                        while (readLen < 4)
                        {
                            int r = stream.Read(lengthBytes, readLen, 4 - readLen);
                            if (r == 0) throw new Exception("Bağlantı kapandı.");
                            readLen += r;
                        }

                        int messageLength = BitConverter.ToInt32(lengthBytes, 0);
                        if (messageLength < 1) continue;

                        byte[] messageBytes = new byte[messageLength];
                        int totalRead = 0;
                        while (totalRead < messageLength)
                        {
                            int bytesRead = stream.Read(messageBytes, totalRead, messageLength - totalRead);
                            if (bytesRead == 0) throw new Exception("Bağlantı kapandı.");
                            totalRead += bytesRead;
                        }

                        byte messageType = messageBytes[0];
                        byte[] payload = new byte[messageLength - 1];
                        Array.Copy(messageBytes, 1, payload, 0, payload.Length);

                        switch ((MessageType)messageType)
                        {
                            case MessageType.Heartbeat:
                                lastHeartbeatTime = DateTime.Now;
                                break;

                            case MessageType.CoordinateData:
                                try
                                {
                                    KoordinatData data = MessagePackSerializer.Deserialize<KoordinatData>(payload);
                                    Invoke(() =>
                                    {
                                        dataGridCoords.Rows.Add(
                                            DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                                            data.Enlem,
                                            data.Boylam,
                                            data.Aciklama
                                        );
                                        AddMarker(data);
                                    });
                                }
                                catch (Exception ex)
                                {
                                    Invoke(() => ShowNotification("Koordinat mesajı çözümlenemedi: " + ex.Message));
                                }
                                break;

                            case MessageType.StartRTSP:
                                this.Invoke(() =>
                                {
                                    StartVideo("rtsp://localhost:8554/live");
                                });
                                break;

                            default:
                                Invoke(() => ShowNotification("Bilinmeyen mesaj tipi: " + messageType));
                                break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    stream?.Close();
                    stream = null;
                    lastHeartbeatTime = null;
                    UpdateConnectionStatus(false);
                    Invoke(() => ShowNotification("Dinleme hatası: " + ex.Message));

                    Thread.Sleep(1000);
                }
            }
        }

        public enum MessageType : byte
        {
            CoordinateData = 0,
            Heartbeat = 1,
            StartRTSP = 2
        }


        private void StartVideo(string rtspUrl)
        {
            StopVideo();

            _mediaPlayer = new MediaPlayer(_libVLC);


            _mediaPlayer.SetVideoFormat("RV32", 640, 480, 640 * 4);
            _mediaPlayer.SetVideoCallbacks(LockCallback, UnlockCallback, DisplayCallback);

            var media = new Media(_libVLC, rtspUrl, FromType.FromLocation);
            _mediaPlayer.Media = media;

            bool started = _mediaPlayer.Play();
            MessageBox.Show(started ? "BAŞLADI" : "BAŞLAMADI");
        }

        private void HeartbeatCheckTimer_Tick(object sender, EventArgs e)
        {
            if (lastHeartbeatTime.HasValue)
            {
                var secondsSinceLastHeartbeat = (DateTime.Now - lastHeartbeatTime.Value).TotalSeconds;

                if (secondsSinceLastHeartbeat > 3)
                {
                    UpdateConnectionStatus(false); // uzun süredir heartbeat alınmadı
                }
                else
                {
                    UpdateConnectionStatus(true); // hala canlı bağlantı var
                }
            }
            else
            {
                UpdateConnectionStatus(false); // hiç heartbeat alınmamış
            }
        }

        private void AddMarker(KoordinatData data)
        {
            if (gMapControl1 == null || markersOverlay == null) return;

            markersOverlay.Markers.Clear();

            var point = new PointLatLng(data.Enlem, data.Boylam);
            var marker = new GMarkerGoogle(point, GMarkerGoogleType.red_dot)
            {
                ToolTipText = data.Aciklama,
                ToolTipMode = MarkerTooltipMode.OnMouseOver
            };

            markersOverlay.Markers.Add(marker);

            gMapControl1.Position = point;


            if (gMapControl1.Zoom < 14)
                gMapControl1.Zoom = 15;
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
                BackColor = Color.DarkGray,
                ForeColor = Color.White,
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
            notificationLabel.BringToFront();
        }

        private IntPtr LockCallback(IntPtr opaque, IntPtr planes)
        {
            if (_videoBuffer == null)
                _videoBuffer = new WriteableBitmapBuffer(640, 480);

            Marshal.WriteIntPtr(planes, _videoBuffer.Data);
            return _videoBuffer.Data;
        }

        private void UnlockCallback(IntPtr opaque, IntPtr picture, IntPtr planes)
        {
            // İşlem yapılmasa da callback’in çağrıldığını garanti etmeliyiz
        }

        private void DisplayCallback(IntPtr opaque, IntPtr picture)
        {
            Debug.WriteLine("DisplayCallback çalıştı");
            Bitmap frame = _videoBuffer.ToBitmap();

            using (Graphics g = Graphics.FromImage(frame))
            {
                int width = frame.Width;
                int height = frame.Height;

                int centerX = width / 2;
                int centerY = height / 2;
                int crosshairSize = 20;

                using (Pen pen = new Pen(Color.Red, 2))
                {
                    g.DrawLine(pen, centerX - crosshairSize, centerY, centerX + crosshairSize, centerY);
                    g.DrawLine(pen, centerX, centerY - crosshairSize, centerX, centerY + crosshairSize);
                }

                // Yazılar
                using (Font font = new Font("Segoe UI", 14, FontStyle.Bold))
                using (SolidBrush brush = new SolidBrush(Color.Yellow))
                {
                    string ammoText = $"Mermi: {ammoCount}";
                    SizeF ammoSize = g.MeasureString(ammoText, font);
                    PointF ammoPos = new PointF(10, height - ammoSize.Height - 10);
                    g.DrawString(ammoText, font, brush, ammoPos);

                    string dateTimeNow = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    SizeF dateSize = g.MeasureString(dateTimeNow, font);
                    PointF datePos = new PointF(width - dateSize.Width - 10, height - dateSize.Height - 10);
                    g.DrawString(dateTimeNow, font, brush, datePos);
                }
            }
                if (pictureBox1.IsHandleCreated)
                {
                    pictureBox1.BeginInvoke(new Action(() =>
                    {
                        pictureBox1.Image?.Dispose();
                        pictureBox1.Image = new Bitmap(frame);
                        frame.Dispose();
                    }));
                }
                else
                {
                    frame.Dispose();
                }
        }

        public void ShowNotification(string message)
        {
            if (notificationLabel.InvokeRequired)
            {
                notificationLabel.Invoke(new Action(() => ShowNotification(message)));
                return;
            }

            notificationLabel.Text = message;
            notificationLabel.Visible = true;
            notificationTimer.Start();
        }

        private void gecisbuton_Click(object sender, EventArgs e)
        {
            if (panelMapContainer.Visible)
            {
                panelMapContainer.Visible = false;
                if (pictureBox1 != null)
                {
                    pictureBox1.Visible = true;
                    txtMermi.Visible = true;
                    mermiguncelle();
                    atesEt.Visible = true;
                    btnReload.Visible = true;
                }
                gecisbuton.Text = "HARİTA";

            }

            else
            {
                panelMapContainer.Visible = true;
                if (pictureBox1 != null)
                {
                    pictureBox1.Visible = false;
                    txtMermi.Visible = false;
                    mermiguncelle();
                    atesEt.Visible = false;
                    btnReload.Visible = false;
                }
                gecisbuton.Text = "VİDEO";
            }
        }

        private void atesEt_Click(object sender, EventArgs e)
        {
            if (ammoCount == 0)
            {
                ShowNotification("Merminiz bitti");
            }
            else
            {
                ammoCount--;
                mermiguncelle();
            }
        }

        private void btnReload_Click(object sender, EventArgs e)
        {
            ammoCount += 100;
            mermiguncelle();
        }

        private void mermiguncelle()
        {
            txtMermi.Text = "Mermi: " + ammoCount;
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

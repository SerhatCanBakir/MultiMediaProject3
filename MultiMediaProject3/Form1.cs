using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TagLib;





namespace MultiMediaProject3
{
    using System;
    using System.IO;
    using System.Windows.Forms.VisualStyles;
    using NAudio.Wave;

    public partial class Form1 : Form
    {
        private PlayList playList;
        private Timer playbackTimer;
        private ContextMenuStrip listMenu;

        public Form1()
        {
            InitializeComponent();

            // Form Load işlemi sırasında varsayılan ayarlar
            this.Load += Form1_Load;

            PlayListBox.MouseDoubleClick += new MouseEventHandler(PlayListBox_MouseDoubleClick);
            PlayListBox.MouseUp += PlayListBox_MouseUp;
            playList = new PlayList();
            playbackTimer = new Timer { Interval = 1000 };
            playbackTimer.Tick += (sender, e) => {
                UpdateTimeLabel(sender, e);
                UpdateTrackBar();
                var currentSong = playList.GetCurrentSong();
                if (currentSong != null)
                {
                   DrawWaveform(currentSong.FilePath, currentSong.CurrentTime);
                }
            };
            listMenu = new ContextMenuStrip();
            listMenu.Items.Add("Remove", null, PlayListBox_RemoveSong);
            listMenu.Items.Add("Clear The List", null, PlayListBox_ClearList);
            listMenu.Items.Add("Get Upper ", null, PlayListBox_Upper);
            listMenu.Items.Add("Get Down", null, PlayListBox_Down);
            listMenu.Items.Add("Import a Playlist", null, (sender, e) => { PlayListBox_Load(); });
            listMenu.Items.Add("Save the Playlist",null, (sender, e) => { PlayListBox_Save(); });
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Dalga formu için varsayılan mesaj veya çizim
            DrawPlaceholderWaveform("No song loaded.");
        }

        private void DrawPlaceholderWaveform(string message)
        {
            var bitmap = new Bitmap(waveformBox.Width, waveformBox.Height);
            var graphics = Graphics.FromImage(bitmap);
            graphics.Clear(Color.Black);

            using (Font font = new Font("Arial", 12, FontStyle.Bold))
            using (Brush brush = new SolidBrush(Color.White))
            {
                var textSize = graphics.MeasureString(message, font);
                graphics.DrawString(
                    message,
                    font,
                    brush,
                    (waveformBox.Width - textSize.Width) / 2,
                    (waveformBox.Height - textSize.Height) / 2
                );
            }

            waveformBox.Image = bitmap;
        }

        private void UpdateTrackBar()
        {
            var currentSong = playList.GetCurrentSong();
            if (currentSong != null && currentSong.Duration.TotalSeconds > 0)
            {
                TrackBar trackBar = Controls["playbackTrackBar"] as TrackBar;
                if (trackBar != null)
                {
                    trackBar.Maximum = (int)currentSong.Duration.TotalSeconds; // Toplam süreyi TrackBar'ın maksimum değeri yap
                    trackBar.Value = (int)currentSong.CurrentTime.TotalSeconds; // Mevcut süreyi ayarla
                }
            }
        }

        private void UpdateTimeLabel(object sender, EventArgs e)
        {
            var currentSong = playList.GetCurrentSong();
            if (currentSong != null)
            {
                // Zaman formatını güncelle
                string currentTime = currentSong.CurrentTime.ToString(@"mm\:ss");
                string totalTime = currentSong.Duration.ToString(@"mm\:ss");

                // Label güncelle
                Controls["timeLabel"].Text = $"{currentTime} / {totalTime}";
            }
        }

        private void AddMusic_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog
            {
                Filter = "Audio Files (*.mp3;*.wav)|*.mp3;*.wav",
                Multiselect = true
            };

            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                foreach (var filePath in fileDialog.FileNames)
                {
                    try
                    {
                        var song = new AudioManager(filePath);
                        playList.AddToList(song);
                        Console.WriteLine("Album : " + song.AlbumArt);
                        if (PlayListBox != null)
                        {
                            var displayText = $"{song.Title} - {song.Artist}";
                            PlayListBox.Items.Add(displayText);
                        }

                        // İlk şarkı eklemede dalga formunu çiz
                        if (playList.GetList().Count == 1)
                        {
                           DrawWaveform(song.FilePath);
                           
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error adding file: {filePath}\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void Start_Click(object sender, EventArgs e)
        {
            if (!playList.IsPlaying())
            {
                playbackTimer.Start();
                playList.Play();
                DrawWaveform(playList.GetCurrentSong()?.FilePath); // Dalga formunu güncelle
            }
            else
            {
                playbackTimer.Stop();
                playList.Stop();
            }
        }

        private void NextBtn_Click(object sender, EventArgs e)
        {
            if (playList.GetCurrentSong() != null)
            {
                playbackTimer.Stop();
                playList.StopCurrentSong();
                playList.Next();
                playbackTimer.Start();
                playList.Play();
                DrawWaveform(playList.GetCurrentSong()?.FilePath); // Dalga formunu güncelle
            }
        }

        private void PreviusBtn_Click(object sender, EventArgs e)
        {
            if (playList.GetCurrentSong() != null)
            {
                playbackTimer.Stop();
                playList.StopCurrentSong();
                playList.Previous();
                playbackTimer.Start();
                playList.Play();
                DrawWaveform(playList.GetCurrentSong()?.FilePath); // Dalga formunu güncelle
            }
        }

        private void LoopBtn_Click(object sender, EventArgs e)
        {
            playList.ToggleLoopPlaylist();
        }

        private void RandomBtn_Click(object sender, EventArgs e)
        {
            playList.ToggleRandomPlayback();
        }

        private void PlayListBox_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            int index = this.PlayListBox.IndexFromPoint(e.Location);
            if (index != System.Windows.Forms.ListBox.NoMatches)
            {
                if (playList.IsPlaying())
                {
                    playList.Stop();
                }
                playList.ChangeCurrentSong(index);
                playList.Play();

                // Dalga formunu çiz
               DrawWaveform(playList.GetCurrentSong()?.FilePath);
            }
        }

        private void PlayListBox_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right) // Sağ tık kontrolü
            {
                Console.WriteLine("sag tık basıldı");
                ListBox listBox = sender as ListBox;
                if (listBox != null)
                {
                    int index = listBox.IndexFromPoint(e.Location); // Tıklanan öğeyi bul
                    if (index != ListBox.NoMatches)
                    {
                        listBox.SelectedIndex = index; // Sağ tıklanan öğeyi seç
                        listMenu.Show(listBox, e.Location); // Sağ tık menüsünü göster
                    }
                }
            }
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            TrackBar trackBar = sender as TrackBar;
            var currentSong = playList.GetCurrentSong();
            if (currentSong != null && trackBar != null)
            {
                // Kullanıcının seçtiği zamana atla
                currentSong.CurrentTime = TimeSpan.FromSeconds(trackBar.Value);
            }
        }

        private void RewindBtn_Click(object sender, EventArgs e)
        {
            playList.StopCurrentSong();
            playList.Rewind();
            playList.Play();
            DrawWaveform(playList.GetCurrentSong()?.FilePath); // Dalga formunu güncelle
        }

        private void PlayListBox_RemoveSong(object sender, EventArgs e)
        {
            if (PlayListBox.SelectedIndex >= 0)
            {
                playList.RemoveFromList(PlayListBox.SelectedIndex);
                PlayListBox.Items.RemoveAt(PlayListBox.SelectedIndex);

                // Liste boşsa dalga formunu temizle
                if (playList.GetList().Count == 0)
                {
                    DrawPlaceholderWaveform("No song loaded.");
                }
            }
        }

        private void PlayListBox_ClearList(object sender, EventArgs e)
        {
            playList.GetList().Clear();
            PlayListBox.Items.Clear();

            // Liste temizlenince dalga formunu temizle
            DrawPlaceholderWaveform("No song loaded.");
        }

        private void PlayListBox_Upper(object sender, EventArgs e)
        {
            if (PlayListBox.SelectedIndex >= 0)
            {
                if (PlayListBox.SelectedItem != null)
                {
                    int selectedSong = PlayListBox.SelectedIndex;
                    playList.SwapSongOrder(selectedSong, selectedSong - 1);
                    object item = PlayListBox.Items[selectedSong];
                    PlayListBox.Items.RemoveAt(selectedSong);
                    PlayListBox.Items.Insert(selectedSong - 1, item);
                    PlayListBox.SelectedIndex = selectedSong - 1;
                }
            }
        }

        private void PlayListBox_Down(object sender, EventArgs e)
        {
            if (PlayListBox.SelectedIndex >= 0)
            {
                if (PlayListBox.SelectedItem != null)
                {
                    int selectedSong = PlayListBox.SelectedIndex;
                    playList.SwapSongOrder(selectedSong, selectedSong + 1);
                    object item = PlayListBox.Items[selectedSong];
                    PlayListBox.Items.RemoveAt(selectedSong);
                    PlayListBox.Items.Insert(selectedSong + 1, item);
                    PlayListBox.SelectedIndex = selectedSong + 1;
                }
            }
        }

        private void PlayListBox_Save()
        {
                 SaveFileDialog folderBrowserDialog = new SaveFileDialog();
                 if(folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                    playList.SavePlaylist(folderBrowserDialog.FileName);
            }
        }

        private void PlayListBox_Load()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if(openFileDialog.ShowDialog() == DialogResult.OK)
            {
                
                playList.LoadPlaylist(openFileDialog.FileName);
               PlayListBox.Items.Clear();
                var lines  = File.ReadAllLines(openFileDialog.FileName);
                foreach (var filePath in lines)
                {
                    try
                    {
                        var song = new AudioManager(filePath);
                      
                        Console.WriteLine("Album : " + song.AlbumArt);
                        if (PlayListBox != null)
                        {
                            var displayText = $"{song.Title} - {song.Artist}";
                            PlayListBox.Items.Add(displayText);
                        }

                        // İlk şarkı eklemede dalga formunu çiz
                        if (playList.GetList().Count == 1)
                        {
                            DrawWaveform(song.FilePath);

                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error adding file: {filePath}\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

            }
        }

       private void DrawWaveform(string filePath, TimeSpan? currentTime = null)
{
    if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
    {
        DrawPlaceholderWaveform("Invalid or missing file.");
        return;
    }

    using (var reader = new AudioFileReader(filePath))
    {
        // Bitmap oluştur ve grafik objesi al
        var bitmap = new Bitmap(waveformBox.Width, waveformBox.Height);
        var graphics = Graphics.FromImage(bitmap);
        graphics.Clear(Color.Black);

        // Dalga formu özellikleri
        float midY = waveformBox.Height / 2f; // Orta nokta (yatay çizgi)
        float scaleY = waveformBox.Height / 2f; // Genlik ölçeği
        int samplesPerPixel = reader.WaveFormat.SampleRate / 100; // Piksel başına örnek
        float[] buffer = new float[samplesPerPixel];
        float xStep = (float)waveformBox.Width / (reader.Length / samplesPerPixel); // X ekseni ilerlemesi

        float currentX = 0; // Çizim başlangıç noktası
        float playedX = 0;

        if (currentTime.HasValue)
        {
            // Mevcut çalma pozisyonunu hesapla
            long currentSamplePosition = (long)(reader.WaveFormat.SampleRate * currentTime.Value.TotalSeconds);
            reader.Position = currentSamplePosition * reader.WaveFormat.BlockAlign;
            playedX = (float)(currentTime.Value.TotalSeconds / reader.TotalTime.TotalSeconds * waveformBox.Width);
        }

        int bytesRead;
        while ((bytesRead = reader.Read(buffer, 0, buffer.Length)) > 0)
        {
            // Maksimum genlik hesapla
            float maxAmplitude = buffer.Take(bytesRead).Max(Math.Abs);

            // Genlik değerlerini üst ve alt noktaya dönüştür
            float topY = midY - (maxAmplitude * scaleY);
            float bottomY = midY + (maxAmplitude * scaleY);

            // Çizim renklerini ayarla
            Pen pen = currentX <= playedX ? Pens.Green : Pens.Gray;

            // Çizim yap
            graphics.DrawLine(pen, currentX, topY, currentX, bottomY);

            currentX += xStep; // X ekseninde ilerle
            if (currentX > waveformBox.Width) break; // Çizim alanını aştıysa dur
        }

        // Bitmap'i PictureBox'a ata
        waveformBox.Image = bitmap;
    }
}





    }

}



public class AudioManager : IDisposable
{
    private IWavePlayer wavePlayer; // Ses oynatıcı
    private AudioFileReader audioFile; // Ses dosyasını okuyucu
    private TagLib.File tagFile; // Ses dosyasının etiketlerini okumak için TagLib

    public string FilePath { get; private set; } // Ses dosyasının yolu
    public TimeSpan Duration => audioFile?.TotalTime ?? TimeSpan.Zero; // Toplam süre
    public TimeSpan CurrentTime
    {
        get => audioFile?.CurrentTime ?? TimeSpan.Zero;
        set
        {
            if (audioFile != null && value <= Duration)
                audioFile.CurrentTime = value;
        }
    }

    public float Volume
    {
        get => audioFile?.Volume ?? 1.0f;
        set
        {
            if (audioFile != null)
                audioFile.Volume = value;
        }
    }

    // Etiket bilgileri
    public string Title { get; private set; } // Şarkı adı
    public string Artist { get; private set; } // Sanatçı
    public string Album { get; private set; } // Albüm adı
    public System.Drawing.Image AlbumArt { get; private set; } // Albüm kapağı

    // Constructor
    public AudioManager(string filePath)
    {
        FilePath = filePath ?? throw new ArgumentNullException(nameof(filePath));
        audioFile = new AudioFileReader(filePath);
        wavePlayer = new WaveOutEvent();
        wavePlayer.Init(audioFile);

        // Etiket bilgilerini yükle
        LoadTags(filePath);
    }

    private void LoadTags(string filePath)
    {
        try
        {
            tagFile = TagLib.File.Create(filePath);
            Title = tagFile.Tag.Title ?? System.IO.Path.GetFileName(filePath); // Şarkı adı yoksa dosya adı
            Artist = tagFile.Tag.FirstPerformer ?? "Unknown Artist";
            Album = tagFile.Tag.Album ?? "Unknown Album";

            // Albüm kapağını yükle
            if (tagFile.Tag.Pictures.Length > 0)
            {
                var albumArtData = tagFile.Tag.Pictures[0].Data.Data;
                using (var ms = new System.IO.MemoryStream(albumArtData))
                {
                    AlbumArt = System.Drawing.Image.FromStream(ms);
                }
            }
            else
            {
                AlbumArt = null; // Albüm kapağı yoksa
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading tags: {ex.Message}");
            Title = System.IO.Path.GetFileName(filePath);
            Artist = "Unknown Artist";
            Album = "Unknown Album";
            AlbumArt = null;
        }
    }

    // Ses dosyasını çalma
    public void Play()
    {
        if (wavePlayer.PlaybackState != PlaybackState.Playing)
        {
            wavePlayer.Play();
            Console.WriteLine("Playing audio...");
        }
    }

    // Ses dosyasını duraklatma
    public void Pause()
    {
        if (wavePlayer.PlaybackState == PlaybackState.Playing)
        {
            wavePlayer.Pause();
            Console.WriteLine("Audio paused.");
        }
    }

    // Ses dosyasını durdurma ve başa sarma
    public void Stop()
    {
        if (wavePlayer.PlaybackState != PlaybackState.Stopped)
        {
            wavePlayer.Stop();
            audioFile.Position = 0;
            Console.WriteLine("Audio stopped.");
        }
    }

    // Ses dosyasını başa sarma
    public void Rewind()
    {
        if (audioFile != null)
        {
            audioFile.Position = 0;
            Console.WriteLine("Audio rewinded to the beginning.");
        }
    }

    // Ses dalga formu için örnekleri okuma
    public List<float> GetSamples(int samplesPerPixel = 400)
    {
        List<float> samples = new List<float>();
        int bytesRead;
        float[] buffer = new float[samplesPerPixel];

        while ((bytesRead = audioFile.Read(buffer, 0, buffer.Length)) > 0)
        {
            float maxAmplitude = buffer.Take(bytesRead).Max(Math.Abs); // Maksimum genliği al
            samples.Add(maxAmplitude);
        }

        return samples;
    }

    // Kaynakları serbest bırakma
    public void Dispose()
    {
        wavePlayer?.Stop();
        wavePlayer?.Dispose();
        audioFile?.Dispose();
        tagFile?.Dispose();
        Console.WriteLine("Resources disposed.");
    }
}


public class PlayList
{
    private List<AudioManager> queue;
    private int currentIndex;
    private bool isLoopingPlaylist;
    private bool isRandomPlayback;
    private bool isPlaying;

    public PlayList()
    {
        queue = new List<AudioManager>();
        currentIndex = -1; // Başlangıçta bir şarkı seçilmedi
        isLoopingPlaylist = false;
        isRandomPlayback = false;
        isPlaying = false;
    }

    public AudioManager GetCurrentSong()
    {
        return (currentIndex >= 0 && currentIndex < queue.Count) ? queue[currentIndex] : null;
    }

    public void ChangeCurrentSong(int index)
    {
        if (index >= 0 && index < queue.Count)
        {
            currentIndex = index;
        }
    }

    public void Play()
    {
        if (currentIndex != -1 && currentIndex < queue.Count)
        {
            isPlaying = true;
            queue[currentIndex].Play();
        }
        else
        {
            StartPlayList();
        }
    }

    public void Stop()
    {
        if (isPlaying && currentIndex != -1)
        {
            isPlaying = false;
            queue[currentIndex].Pause();
        }
    }

    public bool IsPlaying() => isPlaying;

    public void AddToList(AudioManager song)
    {
        if (song != null && !queue.Contains(song))
        {
            queue.Add(song);
        }
    }

    public bool IsEmpty() => queue.Count == 0;

    public bool RemoveFromList(int songNumber)
    {
        if (songNumber >= 0 && songNumber < queue.Count)
        {
            if (currentIndex == songNumber)
            {
                Stop(); // Mevcut çalan şarkı kaldırılırsa durdur
                currentIndex = -1;
            }

            queue.RemoveAt(songNumber);

            // Şarkı kaldırıldıktan sonra indeksleri güncelle
            if (songNumber <= currentIndex)
            {
                currentIndex--;
            }
            return true;
        }
        return false;
    }

    public List<AudioManager> GetList() => new List<AudioManager>(queue);

    public void StartPlayList()
    {
        if (!IsEmpty())
        {
            currentIndex = 0;
            PlayCurrentSong();
        }
    }

    public void Next()
    {
        if (isRandomPlayback)
        {
            Random random = new Random();
            currentIndex = random.Next(0, queue.Count);
        }
        else
        {
            currentIndex++;
            if (currentIndex >= queue.Count)
            {
                currentIndex = isLoopingPlaylist ? 0 : -1;
            }
        }

        if (currentIndex != -1)
        {
            PlayCurrentSong();
        }
    }

    public void Previous()
    {
        currentIndex--;
        if (currentIndex < 0)
        {
            currentIndex = isLoopingPlaylist ? queue.Count - 1 : -1;
        }

        if (currentIndex != -1)
        {
            PlayCurrentSong();
        }
    }

    public void Rewind()
    {
        if (currentIndex != -1)
        {
            queue[currentIndex].Rewind();
        }
    }

    public void ToggleLoopPlaylist() => isLoopingPlaylist = !isLoopingPlaylist;

    public void ToggleRandomPlayback() => isRandomPlayback = !isRandomPlayback;

    public void SavePlaylist(string filePath)
    {
        using (StreamWriter writer = new StreamWriter(filePath)) 
        {
            foreach (var song in queue)
            {
                writer.WriteLine(song.FilePath);
            }
        }
    }

    public void LoadPlaylist(string filePath)
    {
        if (System.IO.File.Exists(filePath))
        {
            queue.Clear();
            var lines = System.IO.File.ReadAllLines(filePath);
            foreach (var line in lines)
            {
                var song = new AudioManager(line);
                queue.Add(song);
            }
        }
    }

    public void DisplaySongInfo()
    {
        if (currentIndex != -1)
        {
            var song = queue[currentIndex];
            Console.WriteLine($"Playing: {Path.GetFileName(song.FilePath)}");
            Console.WriteLine($"Duration: {song.Duration}");
        }
    }

    private void PlayCurrentSong()
    {
        if (currentIndex != -1 && currentIndex < queue.Count)
        {
            queue[currentIndex].Play();
            DisplaySongInfo();
        }
    }

    public void StopCurrentSong()
    {
        if (currentIndex != -1 && currentIndex < queue.Count)
        {
            queue[currentIndex].Stop();
        }
    }

    public void SwapSongOrder(int firstIndex, int secondIndex)
    {
        if (firstIndex >= 0 && firstIndex < queue.Count && secondIndex >= 0 && secondIndex < queue.Count)
        {
            var temp = queue[firstIndex];
            queue[firstIndex] = queue[secondIndex];
            queue[secondIndex] = temp;
        }
    }
}





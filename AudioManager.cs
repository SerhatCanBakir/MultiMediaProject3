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
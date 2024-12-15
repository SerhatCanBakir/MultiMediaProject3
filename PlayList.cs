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
        if (currentIndex >= 0)
        {
            return queue.ElementAt(currentIndex);
        }
        else
        {
            Console.WriteLine("Current index -1");
            return null;
        }
    }

    public void ChangeCurrentSong(int IndexOFNextSong)
    {
        currentIndex = IndexOFNextSong;
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
        if (isPlaying)
        {
            isPlaying = false;
            queue[currentIndex].Pause();
        }
    }


    public bool IsPlaying()
    {
        return isPlaying;
    }
    // Şarkı ekleme
    public void AddToList(AudioManager song)
    {
        queue.Add(song);
    }

    // Playlist'in boş olup olmadığını kontrol etme
    public bool IsEmpty()
    {
        return queue.Count == 0;
    }

    // Şarkı kaldırma
    public bool RemoveFromList(int songNumber)
    {
        if (songNumber >= 0 && songNumber < queue.Count)
        {
            queue.RemoveAt(songNumber);
            return true;
        }
        return false;
    }

    // Playlist'i alma
    public List<AudioManager> GetList()
    {
        return queue;
    }

    // Playlist'i başlatma
    public void StartPlayList()
    {
        if (!IsEmpty())
        {
            currentIndex = 0;
            PlayCurrentSong();
        }
    }

    // Bir sonraki şarkıya geçme
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

    // Bir önceki şarkıya geçme
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

    // Şarkıyı baştan başlatma
    public void Rewind()
    {
        if (currentIndex != -1)
        {
            queue[currentIndex].Rewind();
        }
    }

    // Playlist'i döngü moduna alma
    public void ToggleLoopPlaylist()
    {
        isLoopingPlaylist = !isLoopingPlaylist;
    }

    // Rastgele oynatma modunu açma/kapatma
    public void ToggleRandomPlayback()
    {
        isRandomPlayback = !isRandomPlayback;
    }

    // Playlist'i kaydetme
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

    // Playlist'i yükleme
    public void LoadPlaylist(string filePath)
    {
        if (File.Exists(filePath))
        {
            queue.Clear();
            var lines = File.ReadAllLines(filePath);
            foreach (var line in lines)
            {
                var song = new AudioManager(line);
                queue.Add(song);
            }
        }
    }

    // Şarkı bilgilerini görüntüleme
    public void DisplaySongInfo()
    {
        if (currentIndex != -1)
        {
            var song = queue[currentIndex];
            Console.WriteLine($"Playing: {Path.GetFileName(song.FilePath)}");
            Console.WriteLine($"Duration: {song.Duration}");
        }
    }

    // Şarkıyı oynatma
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
            DisplaySongInfo();
        }
    }


    public void SwapSongOrder(int firstSong, int SecondSong)
    {
        if (queue.Count != -1)
        {
            if ((firstSong >= 0 && firstSong < queue.Count) && (SecondSong >= 0 && SecondSong < queue.Count))
            {
                var currentSong = GetList()[firstSong];
                GetList().RemoveAt(firstSong);
                GetList().Insert(SecondSong, currentSong);

            }
        }
    }

}
using System;
using System.Media;
using System.Windows.Forms;

namespace Client
{
    public static class AudioManager
    {
        private static SoundPlayer _player;
        public static bool IsMuted { get; private set; } = false; // Mặc định là bật
        private static string _filePath;

        // Hàm khởi tạo nhạc (Gọi 1 lần ở MainForm)
        public static void Init(string filePath)
        {
            try
            {
                _filePath = filePath;
                _player = new SoundPlayer(_filePath);
                Play();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không tìm thấy file nhạc: " + ex.Message);
            }
        }

        public static void Play()
        {
            if (!IsMuted && _player != null)
            {
                try { _player.PlayLooping(); } catch { }
            }
        }

        public static void Stop()
        {
            if (_player != null)
            {
                try { _player.Stop(); } catch { }
            }
        }

        // Hàm bật tắt
        public static void Toggle()
        {
            IsMuted = !IsMuted;
            if (IsMuted) Stop();
            else Play();
        }
    }
}
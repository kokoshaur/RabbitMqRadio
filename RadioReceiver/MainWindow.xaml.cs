using RabbitMQSocket;
using SampleDJ;
using System;
using System.Threading;
using System.Windows;

namespace RadioReceiver
{
    public partial class MainWindow : Window
    {
        IPlayer _player;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void RadioOn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MQMusicSocket socket = new MQMusicSocket("localhost", "mda");
                socket.StartListening(PlaySample);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                PlayRandomMusic();
            }
        }

        private void PlaySample(string format, byte[] sample)
        {
            switch (format)
            {
                case "wav":
                    if (_player == null)
                        _player = new WAVPlayer();
                    break;

                default:
                    if (_player == null)
                        _player = new WAVPlayer();
                    break;
            }

            Play(sample);
        }

        private void Play(byte[] sample)
        {
            if (!_player.IsPlaying())
                _player.PlayAsync();

            _player.Write(sample);
        }

        private void PlayRandomMusic()
        {
            if (_player == null)
                _player = new WAVPlayer();
            Random rnd = new Random();

            _player.PlayAsync();

            while (true)
            {
                var freq = 0.01 * rnd.Next(10);

                for (int i = 0; i < 8000; i++)
                {
                    short v = (short)(Math.Sin(freq * i * Math.PI * 2) * 255);
                    _player.Write((byte)(v >> 8));
                    _player.Write((byte)(v & 0xFF));
                }

                Thread.Sleep(990);
            };
        }
    }
}

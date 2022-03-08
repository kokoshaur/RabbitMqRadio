using NAudio.Wave;

namespace SampleDJ
{
    public class WAVPlayer : IDisposable, IPlayer
    {
        private bool _isPlay = false;
        private SoundStream stream;
        private WaveOutEvent waveOut;
        private WaveFileReader reader;

        public WAVPlayer(int sampleRate = 8000)
        {
            stream = new SoundStream(sampleRate);
            waveOut = new WaveOutEvent();
            _isPlay = false;
        }

        public void Write(params byte[] samples)
        {
            stream.Write(samples);
        }

        public void Write(IEnumerable<byte> samples)
        {
            stream.Write(samples);
        }

        public void PlayAsync()
        {
            ThreadPool.QueueUserWorkItem((_) =>
            {
                reader = new WaveFileReader(stream);
                waveOut.Init(reader);
                waveOut.Play();
            });
            _isPlay = true;
        }

        public void Stop()
        {
            waveOut.Stop();
            _isPlay = false;
        }

        public bool IsPlaying() => _isPlay;

        public float Volume
        {
            get { return waveOut.Volume; }
            set { waveOut.Volume = value; }
        }

        public int Buffered
        {
            get { return stream.Buffered; }
        }

        public void Dispose()
        {
            waveOut.Dispose();
            reader.Dispose();
            stream.Dispose();
        }
    }
}

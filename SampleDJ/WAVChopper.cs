namespace SampleDJ
{
    public class WAVChopper
    {
        private FileStream _file;
        private byte[] _data;
        public WAVChopper(string path, int sampleRate = 8000)
        {
            try
            {
                _file = new FileStream(path, FileMode.Open);
                _file.Seek(44, SeekOrigin.Begin);
            }
            catch (Exception)
            {
                throw new Exception("Файл не найден либо занят");
            }

            _data = new byte[sampleRate];
        }

        public byte[] GetSample()
        {
            _file.Read(_data, 0, _data.Length);
            return _data;
        }

        public void WaitOne()
        {
            Thread.Sleep(_data.Length/10);
        }
    }
}
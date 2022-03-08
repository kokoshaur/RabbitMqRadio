namespace SampleDJ
{
    public interface IPlayer
    {
        void Write(params byte[] samples);
        void Write(IEnumerable<byte> samples);
        void PlayAsync();
        void Stop();
        bool IsPlaying();
        void Dispose();
    }
}

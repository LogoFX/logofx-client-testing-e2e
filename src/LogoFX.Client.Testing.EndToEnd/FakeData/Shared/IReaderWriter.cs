namespace LogoFX.Client.Testing.EndToEnd.FakeData.Shared
{
    public interface IReaderWriter
    {
        void WriteText(string path, string text);
        string ReadText(string path);
    }
}
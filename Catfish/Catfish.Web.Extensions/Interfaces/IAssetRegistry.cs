namespace CatfishWebExtensions.Interfaces
{
    public interface IAssetRegistry
    {
        public void RegisterStylesheet(string pathName);
        public IReadOnlyList<string> GetStylesheets();
        public void RegisterScript(string pathName);
        public IReadOnlyList<string> GetScripts();
    }
}

namespace CatfishWebExtensions.Interfaces
{
    public interface IAssetRegistry
    {
        public void RegisterStylesheet(string productionVersionPathName, string devVersionPathName = null);
        public IReadOnlyList<string> GetStylesheets();
        public void RegisterScript(string productionVersionPathName, string devVersionPathName = null);
        public IReadOnlyList<string> GetScripts();
        public void RegisterModule(string productionVersionPathName, string devVersionPathName = null);
        public IReadOnlyList<string> GetModules();

    }
}

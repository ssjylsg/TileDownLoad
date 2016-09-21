namespace MapDataTools.SystemConfigPlugin
{
    public interface IPlugin
    {
        bool CheckAuth();
    }

    public abstract class PluginBase:IPlugin
    {
        public abstract bool CheckAuth();
        public string ImageUrl { get; set; }
        public string LibUrl { get; set; }
        public string Title { get; set; }
    }
}

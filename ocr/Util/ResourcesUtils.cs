namespace Cocr.Util
{
    internal class ResourcesUtils
    {

        private static string basePath = AppDomain.CurrentDomain.BaseDirectory;

        public static string getResource(string filename)
        {
            string viewPath = "Resources\\" + filename;
            return Path.Combine(basePath, viewPath);
        }
    }
}

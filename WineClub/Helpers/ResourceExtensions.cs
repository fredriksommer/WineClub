using Microsoft.ApplicationModel.Resources;

namespace WineClub.Helpers
{
    internal static class ResourceExtensions
    {
        private static readonly ResourceLoader _resLoader = new();

        public static string GetLocalized(this string resourceKey)
        {
            return _resLoader.GetString(resourceKey);
        }
    }
}

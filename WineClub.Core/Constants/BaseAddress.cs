namespace WineClub.Core.Constants
{
    public static class BaseAddress
    {
        public const string Api = "https://localhost:44358/api/";

        public static string MapUrl(string city, string streetAddress)
        {
            return "https://maps.googleapis.com/maps/api/staticmap?center="
                + city
                + ","
                + streetAddress
                + "&zoom=14&size=400x400&&markers=color:blue%7Clabel:A%7C"
                + streetAddress
                + "%20"
                + city
                + "&key=hidden";
            // Key hidden

        }
    }
}

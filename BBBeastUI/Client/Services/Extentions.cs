namespace BBBeastUI.Services
{
    public static class Extentions
    {
        public static string Truncate(this string str, int first, int end)
        {
            if (string.IsNullOrEmpty(str)) return null;
            var address = $"{str.Substring(0, first)}...{str.Substring(str.Length - end)}";
            return address;
        }
    }
}

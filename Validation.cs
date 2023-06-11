using System.Text.RegularExpressions;

namespace StreamLapse
{
    public class Validation
    {
        public static bool IsValidRtspUrl(string url)
        {
            // Regular expression pattern for validating RTSP URL
            string pattern = @"^rtsp:\/\/(?:\w+:\w+@)?[a-zA-Z0-9\-\.]+(:\d+)?(\/[a-zA-Z0-9\-\._\?\,\'\/\\\+&amp;%\$#\=~]*)?$";

            Regex regex = new Regex(pattern);
            return regex.IsMatch(url);
        }
    }
}

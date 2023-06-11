using CommandLine;

namespace StreamLapse
{
    public class Options
    {
        [Option(Required = true, HelpText = "RTSP url to your IP camera.")]
        public string Stream { get; set; }

        [Option("interval", Default = 5000, HelpText = "Time im milliseconds to wait before save another frame.")]
        public int CaptureInterval { get; set; }

        [Option(HelpText = "Path to store the frames, will be created if not exists.")]
        public string Output { get; set; }

        [Option(Default = "IMG_", HelpText = "Prefix to use with filename, default results in IMG_1, IMG_2")]
        public string Prefix { get; set; }
    }
}

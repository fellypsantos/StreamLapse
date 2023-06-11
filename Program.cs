using CommandLine;
using System;
using System.IO;

namespace StreamLapse
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "..:: StreamLapse ::..";

            try
            {
                Parser.Default.ParseArguments<Options>(args).WithParsed(RunOptions);
            }

            catch (Exception ex) when (ex is FormatException || ex is IOException)
            {
                Console.WriteLine(ex.Message);
            }
        }

        static void RunOptions(Options options)
        {
            if (!Validation.IsValidRtspUrl(options.Stream)) throw new FormatException("Stream url is not valid.");

            StreamHandler streamHandler = new StreamHandler(options.Stream);

            streamHandler.SetCaptureInterval(options.CaptureInterval);

            streamHandler.SetOutputDirectory(options.Output);

            streamHandler.SetImagePrefix(options.Prefix);

            while (true) streamHandler.SaveFrameToJPGFile();
        }
    }
}

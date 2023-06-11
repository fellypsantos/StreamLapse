using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace StreamLapse
{
    public class StreamHandler
    {
        private readonly string _streamURL;
        private string _outputDir;
        private string _prefixImageName;
        private int _frameCount;
        private int _captureIntervalInMilliseconds;
        private long _totalBytesWrited;

        /// <summary>
        /// Handle connection to a camera stream.
        /// </summary>
        /// <param name="rtsp">Real Time Streaming Protocol something like this: rtsp://user:pass@192.168.0.5/live/mpeg4</param>
        public StreamHandler(string rtsp)
        {
            _streamURL = rtsp;
            _frameCount = 0;
            _totalBytesWrited = 0;
        }

        /// <summary>
        /// Setup the capture interval to wait before save each frame.
        /// </summary>
        /// <param name="captureInverval">Time to wait before save another frame.</param>
        public void SetCaptureInterval(int captureInverval)
        {
            _captureIntervalInMilliseconds = captureInverval;
        }

        /// <summary>
        /// Folder to store the saved images.
        /// </summary>
        /// <param name="outputDirectory">Path to store the frames, will be created if not exists.</param>
        public void SetOutputDirectory(string outputDirectory)
        {
            _outputDir = outputDirectory;

            if (string.IsNullOrEmpty(outputDirectory)) return;

            if (!Directory.Exists(_outputDir)) Directory.CreateDirectory(_outputDir);
        }

        /// <summary>
        /// Set prefix to be used as filename with frame count eg IMG_1
        /// </summary>
        /// <param name="prefix">The image prefix</param>
        public void SetImagePrefix(string prefix)
        {
            _prefixImageName = prefix;
        }

        /// <summary>
        /// Takes the filename and concat with output directory to create the full exporting path.
        /// </summary>
        /// <param name="filename">Filename to merge with output path</param>
        /// <returns>Full path to store the image.</returns>
        private string GenerateImageNameOutputPath(string filename)
        {
            if (string.IsNullOrEmpty(_outputDir)) return filename;

            return Path.Combine(_outputDir, filename);
        }

        /// <summary>
        /// Connects to stream and save current frame to file.
        /// </summary>
        /// <exception cref="IOException">In case of the image already exists.</exception>
        public void SaveFrameToJPGFile()
        {
            Stopwatch stopwatch = Stopwatch.StartNew();

            string filename = $"{_prefixImageName}{++_frameCount}.jpg";

            string filenameFullPath = GenerateImageNameOutputPath(filename);

            if (File.Exists(filenameFullPath)) throw new IOException($"Failed to save image {filenameFullPath} because it already exists.");

            stopwatch.Start();

            bool frameWasSaved = false;

            while(!frameWasSaved)
            {
                frameWasSaved = FFMPEG.CaptureStreamFrame(_streamURL, filenameFullPath);

                if (!frameWasSaved) Console.WriteLine($"Failed to save the frame: {filename}. Retrying...");
            }

            stopwatch.Stop();

            RegisterFileSizeInBytes(filenameFullPath);

            double timeSpentToSaveFile = TimeSpan.FromMilliseconds(stopwatch.ElapsedMilliseconds).TotalSeconds;

            PrintStatus(filenameFullPath, timeSpentToSaveFile);

            Thread.Sleep(_captureIntervalInMilliseconds);
        }

        /// <summary>
        /// Accumulate the bytes of all images writed to show while processing.
        /// </summary>
        /// <param name="filename">File to get size and add to previews bytes.</param>
        private void RegisterFileSizeInBytes(string filename)
        {
            FileInfo fileInfo = new FileInfo(filename);

            _totalBytesWrited += fileInfo.Length;
        }

        /// <summary>
        /// Show the main text on screen updating the information on each connection.
        /// </summary>
        /// <param name="filename">Last file saved.</param>
        /// <param name="timeSpent">Time spent in milliseconds to save the last file.</param>
        private void PrintStatus(string filename, double timeSpent)
        {
            Console.Clear();

            Console.WriteLine("..:: StreamLapse ::..\n\n");

            Console.WriteLine("See the world from different perspective.\n");

            Console.WriteLine("created by\n");

            Console.WriteLine("fellypsantos2011@gmail.com\n\n\n");

            Console.WriteLine($"File {filename} saved in {timeSpent} seconds.\n");

            Console.WriteLine("Total frames saved at now: " + _frameCount + '\n');

            Console.WriteLine("Total filesize of images: " + FormatFileSize(_totalBytesWrited));
        }

        /// <summary>
        /// Convert bytes to a more user friendly metric like Bytes, KiloBytes, MegaBytes etc. 
        /// </summary>
        /// <param name="fileSize">File size in bytes to be converted.</param>
        /// <returns>Converted unit with two decimal places.</returns>
        private string FormatFileSize(long fileSize)
        {
            string[] sizeUnits = { "B", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };

            int unitIndex = 0;
            double size = fileSize;

            while (size >= 1024 && unitIndex < sizeUnits.Length - 1)
            {
                size /= 1024;
                unitIndex++;
            }

            return $"{size:0.##} {sizeUnits[unitIndex]}";
        }
    }
}

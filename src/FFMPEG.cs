using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace StreamLapse
{
    public class FFMPEG
    {
        const int MAX_WAIT_TIME_MS = 10 * 1000;

        public static bool CaptureStreamFrame(string streamURL, string filenameFullPath)
        {
            Console.WriteLine("\nRequesting new frame...");

            Task captureOneFrame = Task.Run(() =>
            {
                using (Process p = new Process())
                {
                    p.StartInfo.UseShellExecute = false;
                    p.StartInfo.CreateNoWindow = true;
                    p.StartInfo.FileName = "ffmpeg.exe";
                    p.StartInfo.Arguments = $"-loglevel quiet -rtsp_transport tcp -i {streamURL} -vframes 1 {filenameFullPath}";
                    p.Start();
                    p.WaitForExit();
                }
            });

            Task timeoutTask = Task.Delay(MAX_WAIT_TIME_MS);

            int completedTaskIndex = Task.WaitAny(captureOneFrame, timeoutTask);

            if (completedTaskIndex == 1)
            {
                Console.WriteLine("\n  - Timeout reached.\n");

                Beep();
            }

            return File.Exists(filenameFullPath);
        }

        static void Beep()
        {
            Console.Beep(1000, 100);

            Console.Beep(800, 100);

            Console.Beep(500, 100);
        }
    }
}

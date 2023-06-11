using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace StreamLapse
{
    public class FFMPEG
    {
        public static bool CaptureStreamFrame(string streamURL, string filenameFullPath)
        {
            Console.WriteLine("\nRequesting new frame...");

            using (Process p = new Process())
            {
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.CreateNoWindow = true;
                p.StartInfo.FileName = "ffmpeg.exe";
                p.StartInfo.Arguments = $"-loglevel quiet -rtsp_transport tcp -i {streamURL} -vframes 1 {filenameFullPath}";
                p.Start();
                p.WaitForExit();

                return File.Exists(filenameFullPath);
            }
        }
    }
}

using AmaraVideoClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmaraVideoClientConsole
{
    class Program
    {
        private static VideoClientController vcc = new VideoClientController();
        static void Main(string[] args)
        {
            /*
             Test scenarios: (for each website: youtube, vimeo, dailymotion, lastly for a video uploaded in the sensus web api)
             * A video with the right subtitle gyiTYw4ufYtg en
             * A video with no subtitle https://www.youtube.com/watch?v=RdKAVE0frIM&ab_channel=BBC
             * A video with subtitle in a different langauge gyiTYw4ufYtg dk
             * No video 
             */
            //TestGetInfo("SvoJipcJJH1x","it");
            //TestPostVideo();
            Console.WriteLine("Done");
            Console.ReadLine();
        }

        private static void TestGetInfo(string videoId, string languageCode)
        {
            VideoDetail vd = vcc.GetVideoInfo(videoId);
            if (vd.Languages.Count > 0)
            {
                string vlUri = (from l in vd.Languages where l.Code == languageCode select l).FirstOrDefault().VideoLanguageUri;
                if (!string.IsNullOrWhiteSpace(vlUri))
                {
                    var vlDetails = vcc.GetLanguageDetails(vlUri);
                    Console.WriteLine("Video language Detail: " + vlDetails);
                }
            }
        }

        private static void TestDownloadSubs(string videoId)
        {
            vcc.GetVideoSubtitle(videoId, "en", "txt");
        }

        private static void TestPostVideo()
        {
            VideoSummary vs = new VideoSummary()
            {
                VideoUrl = "http://www.youtube.com/watch?v=cJs7obmEABE"//"https://www.youtube.com/watch?v=RdKAVE0frIM&ab_channel=BBC"
            };
            string subLang = "dk";
            string subFormat = "txt";
            SubtitleInfo si = vcc.RequestVideoSubtitle(vs, subLang, subFormat);
            switch (si.Status)
            {
                case VideoSubtitleStatus.Complete:
                    break;
                case VideoSubtitleStatus.SubtitleRequested:
                    Console.WriteLine("Video exists, but subtitle does not, request for subtitle is made.");
                    break;
                case VideoSubtitleStatus.Error:
                    Console.WriteLine("Error happened!");
                    break;
                case VideoSubtitleStatus.Exists:
                    byte[] res = vcc.GetVideoSubtitle(si.VideoId, subLang, subFormat).SubtitleData;
                    Console.WriteLine("Result: " + Encoding.Default.GetString(res));
                    break;
                case VideoSubtitleStatus.NotComplete: break;
                case VideoSubtitleStatus.Submitted:
                    Console.WriteLine("Video with id {1} has been submitted for manual subtitling. You'll get notified when it's ready by pigeon!", si.VideoId);
                    break;
                default: break;
            }
        }
    }
}

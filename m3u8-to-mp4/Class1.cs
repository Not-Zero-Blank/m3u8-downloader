using System;
using System.IO;
using System.Net;

namespace m3u8_to_mp4
{
    public class Client
    {
        public static void Main()
        {
            Directory.CreateDirectory(Directory.GetCurrentDirectory()+"/Download");
            Download("https://delivery-node-taj.voe-network.net/hls/,6oarm6m35q23cszcryam3ozwvfnhj5xwnknlhqpcmjqpfzsudamwnwgs3u4q,.urlset/master.m3u8", Directory.GetCurrentDirectory() + "/Download", "");
            
        }
        public static void Download(string m3u8Url, string SaveDir, string SaveName)
        {
            string[] master = GetM3u8(m3u8Url).Split('\n');
            foreach (string i in master)
            {
                if (i.StartsWith("#")) continue;
                if (i.EndsWith(".m3u8"))
                {
                    Console.WriteLine("Found m3u8 file: " + i);
                    string baseurl = i.Substring(0, i.LastIndexOf("/") + 1);
                    Console.WriteLine(baseurl);
                    string[] index = GetM3u8(i).Split('\n');
                    foreach (string ts in index)
                    {
                        if (i.StartsWith("#")) continue;
                        if (ts.EndsWith(".ts"))
                        {
                            WebClient client = new WebClient();
                            client.DownloadFile(baseurl+ts, SaveDir +"/"+ ts);
                        }
                    }
                }
            }
        }
        static string GetM3u8(string url)
        {
            var httpRequest = (HttpWebRequest)WebRequest.Create(url);
            var httpResponse = (HttpWebResponse)httpRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                return result;
            }
        }
    }
}

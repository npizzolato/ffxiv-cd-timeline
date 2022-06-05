using System;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Web;
using ICSharpCode.SharpZipLib.GZip;
using Newtonsoft.Json;

public class TimelineSaver
{
    public string SaveTimeline(MitigationTimeline timeline)
    {
        string timelineJson = JsonConvert.SerializeObject(timeline);
        byte[] compressedData = Zip(timelineJson);
        string base64Encoded = System.Convert.ToBase64String(compressedData);
        return HttpUtility.UrlEncode(base64Encoded);
    }

    public MitigationTimeline LoadTimeline(string savedTimeline)
    {
        Console.WriteLine($"Loading {savedTimeline}");
        //string decoded = HttpUtility.UrlDecode(savedTimeline);
        byte[] data = System.Convert.FromBase64String(savedTimeline);
        string uncompressed = Unzip(data);
        //string base64Decoded = System.Text.ASCIIEncoding.ASCII.GetString(data);
        return JsonConvert.DeserializeObject<MitigationTimeline>(uncompressed);
    }

    public static byte[] Zip(string str) {
        var bytes = Encoding.UTF8.GetBytes(str);

        using (var msi = new MemoryStream(bytes))
        using (var mso = new MemoryStream()) {
            GZip.Compress(msi, mso, true);

            return mso.ToArray();
        }
    }

    public static string Unzip(byte[] bytes) {
        using (var msi = new MemoryStream(bytes))
        using (var mso = new MemoryStream()) {
            GZip.Decompress(msi, mso, true);

            return Encoding.UTF8.GetString(mso.ToArray());
        }
    }
    
    private static void CopyTo(Stream src, Stream dest) 
    {
        byte[] bytes = new byte[4096];

        int cnt;

        while ((cnt = src.Read(bytes, 0, bytes.Length)) != 0) 
        {
            dest.Write(bytes, 0, cnt);
        }
    }
}
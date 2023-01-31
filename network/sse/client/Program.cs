using System.Net;
using System.Text;

namespace Client{
    public class Program{
        public static void Main(string[] args){
            Console.WriteLine("Starting client...");
            Process().Wait();
        }

        static async Task Process()
        {
            Console.WriteLine("Sending request...");
            using (var client = new HttpClient())
            {
                var message = new HttpRequestMessage(HttpMethod.Get, "http://localhost:8888/");
                var response = await client.SendAsync(message, HttpCompletionOption.ResponseHeadersRead);
                Console.WriteLine($"Got response headers with status code: {response.StatusCode}");
                Console.WriteLine("Attempting to read stream...");
                using (var streamReader = new StreamReader(response.Content.ReadAsStream()))
                {
                    //ReadLine(streamReader);                    
                    ReadIncremental(streamReader);
                }
            }
        }

        static void ReadLine(StreamReader streamReader)
        {
            Console.WriteLine("Reading line-by-line");
            while (true)
            {
                var line = streamReader.ReadLine();
                if (line == null) break;
                Console.WriteLine($"Got line: {line}");
            }
            Console.WriteLine("Client done");
        }
        static void ReadIncremental(StreamReader streamReader)
        {
            Console.WriteLine("Reading incrementally");
            var buffer = new char[1024];
            var readIdx = 0;
            var curStr = string.Empty;
            while (true)
            {
                if (streamReader.EndOfStream)
                {
                    break;
                }
                Console.WriteLine("Reading again");
                var thing = streamReader.Peek();
                Console.WriteLine(thing);
                var numCharsRead = streamReader.Read(buffer, readIdx, buffer.Length);
                for (var i = 0; i < numCharsRead; i++)
                {
                    Console.WriteLine((int)buffer[i]);
                }
                Console.WriteLine($"Read {numCharsRead} chars");
                var newStr = new string(buffer, 0, numCharsRead);
                Console.WriteLine($"Got new str: {newStr}");
            }
            Console.WriteLine("Client done");
        }

    }
}
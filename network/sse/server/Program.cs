using System.Net;

namespace Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Server starting...");

            Console.WriteLine("Starting HTTP server on port 8888");
            var server = new HttpListener();
            server.Prefixes.Add("http://127.0.0.1:8888/");
            server.Prefixes.Add("http://localhost:8888/");
            server.Start();
            Console.WriteLine("HTTP server started...");

            try
            {
                while (true)
                {
                    var context = server.GetContext();
                    Console.WriteLine("Request received");
                    Console.WriteLine("Sending string \"This\\nis\\na\\ntest\" token-by-token with two seconds between each token");
                    var str = "This\nis\na\ntest";

                    var response = context.Response;
                    response.SendChunked = true;
                    var outputStream = response.OutputStream;
                    using (var streamWriter = new StreamWriter(outputStream))
                    {
                        foreach (var ch in str)
                        {
                            streamWriter.Write(ch);
                            streamWriter.Flush();
                            Console.WriteLine($"Sent {ch}");
                            Thread.Sleep(2000);
                        }
                    }

                    context.Response.Close();
                    Console.WriteLine("Finished writing response");
                }
            }
            catch (Exception)
            {

            }
        }
    }
}
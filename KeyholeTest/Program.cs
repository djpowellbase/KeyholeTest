using System;
using System.IO;
using System.Text;
using System.Net;

namespace KeyholeTest
{
    class Program
    {
        private static char _x;
        private static int _y;
        private static StringBuilder _query;

        static void Main()
        {
            _x = 'a';
            _y = 9;
            _query = new StringBuilder("Q");
            
            CreateWebRequest(_x, _y, _query);
            
            Console.WriteLine("\nYours Truly,\nDeshon Powell\nDeshon@DeshonPowell.com");
            Console.ReadLine();
        }

        private static void CreateWebRequest(char x, int y, StringBuilder query)
        {
            for (int i = 35; i > 0; i--)
            {
                var ltr = x;
                var nbr = y;

                HttpWebRequest request;

                if (i < 10)
                {
                    request =
                        (HttpWebRequest)WebRequest.Create("http://simple-snow-3171.herokuapp.com/?key=" + query + nbr);
                }
                else
                {
                    request =
                        (HttpWebRequest)WebRequest.Create("http://simple-snow-3171.herokuapp.com/?key=" + query + ltr);
                }

                var response = (HttpWebResponse)request.GetResponse();

                var rawUri = response.ResponseUri.Query;

                var stream = response.GetResponseStream();

                if (stream != null)
                {
                    var readStream = new StreamReader(stream, Encoding.UTF8);

                    var html = readStream.ReadToEnd();

                    string[] stringsToTrim = { "<html><body>KEY DENIED: ", "</body></html>" };

                    foreach (var array in stringsToTrim)
                    {
                        html = html.Replace(array, "");
                    }

                    var key = html;
                    var zero = new[] { '0' };
                    bool match = key.IndexOfAny(zero) == -1;

                    if (match && i > 9)
                    {
                        query.Append(ltr);
                        i = 35;
                        x = _x;
                        Console.WriteLine("Current Key - {0}; URI - {1};", key, rawUri);
                    }
                    if (match && i <= 9)
                    {
                        query.Append(nbr);
                        i = 35;
                        y = _y;
                        Console.WriteLine("Current Key - {0}; URI - {1};", key, rawUri);
                    }
                    if (!match && i > 9) x = ++ltr;
                    if (!match && i <= 9) y = --nbr;
                }
            }
        }
    }
}

using System;
using System.IO;
using System.Net;
using Newtonsoft.Json.Linq;

namespace DotnetWebhookExample
{
    public class LatitudeLongitude
    {
        public string Latitude { get; set; }
        public string Longitude { get; set; }
    }

    class Program
    {
        static string username = "<username>";
        static string password = "<password>";
        static string googleApiKey = "<GOOGLE_API_KEY>";
        static string openWeatherMapKey = "<OPEN_WEATHER_MAP_KEY>";

        static void Main(string[] args)
        {
            Console.WriteLine("Hello Bot Webhook!");

            using (HttpListener listener = new HttpListener())
            {
                listener.Prefixes.Add("http://*:8000/dotnet/");
                listener.AuthenticationSchemes = AuthenticationSchemes.Basic;

                while (true)
                {
                    listener.Start();
                    Console.WriteLine("Listening...");
                    // Note: The GetContext method blocks while waiting for a request. 
                    HttpListenerContext context = listener.GetContext();
                    HttpListenerRequest request = context.Request;

                    HttpListenerBasicIdentity identity = (HttpListenerBasicIdentity)context.User.Identity;

                    if (identity.Name != username || identity.Password != password)
                    {
                        context.Response.StatusCode = 401;

                        byte[] buffer = System.Text.Encoding.UTF8.GetBytes("The credentials provided are invalid.");
                        //Get a response stream and write the response to it.
                        context.Response.ContentLength64 = buffer.Length;
                        Stream output = context.Response.OutputStream;
                        output.Write(buffer, 0, buffer.Length);
                        context.Response.Close();
                    }
                    else
                    {
                        Stream body = request.InputStream;
                        System.Text.Encoding encoding = request.ContentEncoding;
                        StreamReader reader = new StreamReader(body, encoding);

                        string requestBody = reader.ReadToEnd();
                        JObject requestObject = JObject.Parse(requestBody);

                        var latLong = getLatLong(requestObject["conversation"]["custom"]["zipCode"].ToString());

                        string forecast = getForecast(latLong.Latitude, latLong.Longitude);
                        Console.WriteLine("forecast: " + forecast);

                        JObject quiqMessage = new JObject();
                        quiqMessage["text"] = forecast;

                        JObject message = new JObject();
                        message["default"] = quiqMessage;

                        JObject sendMessageAction = new JObject();
                        sendMessageAction["action"] = "sendMessage";
                        sendMessageAction["message"] = message;

                        JArray array = new JArray();
                        array.Add(sendMessageAction);

                        JObject response = new JObject();
                        response["actions"] = array;

                        context.Response.StatusCode = 200;

                        Console.WriteLine("Responding to message...");
                        byte[] buffer = System.Text.Encoding.UTF8.GetBytes(response.ToString());
                        //Get a response stream and write the response to it.
                        context.Response.ContentLength64 = buffer.Length;
                        Stream output = context.Response.OutputStream;
                        output.Write(buffer, 0, buffer.Length);
                        context.Response.Close();
                    }
                }
            }
        }

        private static LatitudeLongitude getLatLong(string zipCode) {
            string responseText = string.Empty;
            string url = $"https://maps.googleapis.com/maps/api/geocode/json?key={googleApiKey}&components=postal_code:{zipCode}";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.AutomaticDecompression = DecompressionMethods.GZip;

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                responseText = reader.ReadToEnd();
            }

            JObject geoCodeObject = JObject.Parse(responseText);

            return new LatitudeLongitude
            {
                Latitude = geoCodeObject["results"][0]["geometry"]["location"]["lat"].ToString(),
                Longitude = geoCodeObject["results"][0]["geometry"]["location"]["lng"].ToString(),
            };
        }

        private static string getForecast(string latitude, string longitude) {
            string responseText = string.Empty;
            string url = $"https://api.openweathermap.org/data/2.5/onecall?lat={latitude}&lon={longitude}&exclude=minutely,hourly,daily&appid={openWeatherMapKey}&units=imperial";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.AutomaticDecompression = DecompressionMethods.GZip;

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                responseText = reader.ReadToEnd();
            }

            JObject forecastObject = JObject.Parse(responseText);

            return $"The current weather is {forecastObject["current"]["weather"][0]["description"]} with a temperature of {forecastObject["current"]["temp"]} degrees.";
        }
    }
}

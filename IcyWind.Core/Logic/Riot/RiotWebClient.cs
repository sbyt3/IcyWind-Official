using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using IcyWind.Core.Logic.Riot.Auth;
using Newtonsoft.Json;

namespace IcyWind.Core.Logic.Riot
{
    /// <summary>
    /// This is the RiotWebClient. This is used to post information to Riot's OpenId server
    /// Makes the code a lot cleaner
    /// </summary>
    public static class RiotWebClient
    {
        /// <summary>
        /// Post with the Bearer string
        /// </summary>
        /// <param name="url">The url to the endpoint</param>
        /// <param name="userAgent">The UserAgent <see cref="RiotWebClientUserAgents"/></param>
        /// <param name="token">The bearer token</param>
        /// <param name="contentType">The content type being sent to the servers</param>
        /// <param name="contentAccept">What to accept from the server</param>
        /// <param name="postString">The string to post</param>
        /// <param name="headers">Any headers to be sent the the endpoint</param>
        /// <returns></returns>
        public static RiotWebClientResponse PostWithBearer(string url, string userAgent, string token, string contentType, string contentAccept, string postString, params (string, string)[] headers)
        {
            //Create the Webrequest and make it look like it is coming from the RiotClient
            var client = (HttpWebRequest)WebRequest.Create(url);
            client.Method = WebRequestMethods.Http.Post;
            client.ProtocolVersion = HttpVersion.Version11;
            client.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            client.UserAgent = userAgent;
            client.Headers.Set(HttpRequestHeader.Authorization, "Bearer " + token);
            client.ContentType = contentType;
            client.Accept = contentAccept;
            foreach (var addHeader in headers)
            {
                client.Headers.Add(addHeader.Item1, addHeader.Item2);
            }

            //Seems pointless but this is the string it posts so we will post it too
            var postBytes = Encoding.UTF8.GetBytes(postString);

            //More faking stuff
            client.ContentLength = postBytes.Length;
            client.ServicePoint.Expect100Continue = false;
            client.Headers.Remove(HttpRequestHeader.Pragma);

            //Create the request
            var requestStream = client.GetRequestStream();
            requestStream.Write(postBytes, 0, postBytes.Length);
            requestStream.Close();

            //Retrieve the response
            try
            {
                var response = (HttpWebResponse)client.GetResponse();
                using (var rdr = new StreamReader(response.GetResponseStream() ?? throw new InvalidOperationException()))
                {
                    return new RiotWebClientResponse(true, rdr.ReadToEnd(), null);
                }
            }
            catch (WebException e)
            {
                using (var response = e.Response)
                {
                    using (var data = response.GetResponseStream())
                    using (var reader = new StreamReader(data ?? throw new InvalidOperationException()))
                    {
                        return new RiotWebClientResponse(false, reader.ReadToEnd(), e);
                    }
                }
            }
        }

        /// <summary>
        /// Send a post request to a given endpoint
        /// </summary>
        /// <param name="url">The url to the endpoint</param>
        /// <param name="userAgent">The UserAgent <see cref="RiotWebClientUserAgents"/></param>
        /// <param name="contentType">The content type being sent to the servers</param>
        /// <param name="contentAccept">What to accept from the server</param>
        /// <param name="postString">The string to post</param>
        /// <param name="headers">Any headers to be sent the the endpoint</param>
        /// <returns></returns>
        public static RiotWebClientResponse Post(string url, string userAgent, string contentType, string contentAccept, string postString, params (string, string)[] headers)
        {
            //Create the Webrequest and make it look like it is coming from the RiotClient
            var client = (HttpWebRequest)WebRequest.Create(url);
            client.Method = WebRequestMethods.Http.Post;
            client.ProtocolVersion = HttpVersion.Version11;
            client.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            client.UserAgent = userAgent;
            client.ContentType = contentType;
            client.Accept = contentAccept;
            foreach (var addHeader in headers)
            {
                client.Headers.Add(addHeader.Item1, addHeader.Item2);
            }

            //Seems pointless but this is the string it posts so we will post it too
            var postBytes = Encoding.UTF8.GetBytes(postString);

            //More faking stuff
            client.ContentLength = postBytes.Length;
            client.ServicePoint.Expect100Continue = false;
            client.Headers.Remove(HttpRequestHeader.Pragma);

            //Create the request
            var requestStream = client.GetRequestStream();
            requestStream.Write(postBytes, 0, postBytes.Length);
            requestStream.Close();

            //Retrieve the response
            try
            {
                var response = (HttpWebResponse)client.GetResponse();
                using (var rdr = new StreamReader(response.GetResponseStream() ?? throw new InvalidOperationException()))
                {
                    return new RiotWebClientResponse(true, rdr.ReadToEnd(), null);
                }
            }
            catch (WebException e)
            {
                using (var response = e.Response)
                {
                    using (var data = response.GetResponseStream())
                    using (var reader = new StreamReader(data ?? throw new InvalidOperationException()))
                    {
                        return new RiotWebClientResponse(false, reader.ReadToEnd(), e);
                    }
                }
            }
        }

        /// <summary>
        /// This is a get request with a bearer token
        /// </summary>
        /// <param name="url">The url to the endpoint</param>
        /// <param name="userAgent">The UserAgent <see cref="RiotWebClientUserAgents"/></param>
        /// <param name="contentType">The content type being sent to the servers</param>
        /// <param name="contentAccept">What to accept from the server</param>
        /// <returns></returns>
        public static RiotWebClientResponse GetWithBearer(string url, string userAgent, string token, string contentType, string contentAccept)
        {

            //Create the Webrequest and make it look like it is coming from the RiotClient
            var client = (HttpWebRequest)WebRequest.Create(url);
            client.Method = WebRequestMethods.Http.Get;
            client.ProtocolVersion = HttpVersion.Version11;
            client.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            client.UserAgent = userAgent;
            client.Headers.Set(HttpRequestHeader.Authorization, "Bearer " + token);
            client.ContentType = contentType;
            client.Accept = contentAccept;
            client.Headers.Remove(HttpRequestHeader.Pragma);

            //Holy shit this is so much shorter than all of the other stuff. I love how POST requests are just that much shorter
            try
            {
                var response = (HttpWebResponse)client.GetResponse();
                using (var rdr = new StreamReader(response.GetResponseStream() ?? throw new InvalidOperationException()))
                {
                    return new RiotWebClientResponse(true, rdr.ReadToEnd(), null);
                }
            }
            catch (WebException e)
            {
                using (var response = e.Response)
                {
                    using (var data = response.GetResponseStream())
                    using (var reader = new StreamReader(data ?? throw new InvalidOperationException()))
                    {
                        return new RiotWebClientResponse(false, reader.ReadToEnd(), e);
                    }
                }
            }
        }
    }

    /// <summary>
    /// Contains all of the UserAgents to be used by <see cref="RiotWebClient"/>
    /// </summary>
    public static class RiotWebClientUserAgents
    {
        /// <summary>
        /// The current RiotClient version. Currently this is set to RiotClient/18.0.0
        /// </summary>
        public static string RiotClient => "RiotClient/18.0.0";

        /// <summary>
        /// The UserAgent for the entitlements url
        /// </summary>
        public static string Entitlements => $"{RiotClient} (entitlements)";

        /// <summary>
        /// The UserAgent for the RsoAuth Url
        /// </summary>
        public static string RsoAuth => $"{RiotClient} (rso-auth)";

        /// <summary>
        /// Used for inventory
        /// </summary>
        public static string Inventory => $"{RiotClient} (lol-inventory)";

        /// <summary>
        /// Used for PlayerPrefs
        /// </summary>
        public static string PlayerPref => $"{RiotClient} (lol-player-preferences)";
    }

    public class RiotWebClientResponse
    {
        public RiotWebClientResponse(bool success, string result, WebException exception)
        {
            Success = success;
            Result = result;
            ResultException = exception;
        }
        public bool Success { get; }
        public string Result { get; }
        public WebException ResultException { get; }
    }
}

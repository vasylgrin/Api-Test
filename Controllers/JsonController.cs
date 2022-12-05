using Newtonsoft.Json.Linq;
using System.Net;

namespace Api_Test.Controllers
{
    public class JsonController
    {
        /// <summary>
        /// Gets json data by url.
        /// </summary>
        /// <param name="url">URL</param>
        /// <returns>JObject.</returns>
        public static JToken GetRequest(string url)      
        {
            string Response = "";

            HttpWebRequest? request = WebRequest.Create(url) as HttpWebRequest;
            request.Method = "GET";

            try
            {
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                var stream = response.GetResponseStream();
                if (stream != null) Response = new StreamReader(stream).ReadToEnd();
            }
            catch (Exception)
            {
            }

            if (string.IsNullOrWhiteSpace(Response))
            {
                return new JObject();
            }

            
            return JObject.Parse(Response);
        }
    }
}

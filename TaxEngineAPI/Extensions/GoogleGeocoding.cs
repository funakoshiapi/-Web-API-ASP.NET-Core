using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace TaxEngineAPI.Extensions
{
    public static class GoogleGeocoding
    {
        private static readonly string STATUS_INVALID = "ZERO_RESULTS";

        public static IDictionary<string, string> ForwardGeocoding(this string address, string apiKey)
        {
            var url = string.Format($"https://maps.googleapis.com/maps/api/geocode/json?address={address},&key={apiKey}");

            var req = (HttpWebRequest)WebRequest.Create(url);
            var res = (HttpWebResponse)req.GetResponse();

            using (var streamreader = new StreamReader(res.GetResponseStream()))
            {
                var result = streamreader.ReadToEnd();
                var objAddress = (JObject)JsonConvert.DeserializeObject(result);

                var status = objAddress.SelectToken("status").Value<string>();
                if (status != STATUS_INVALID)
                {
                    var latitude = objAddress.SelectToken("results[0].geometry.location.lat").Value<string>();
                    var longitude = objAddress.SelectToken("results[0].geometry.location.lng").Value<string>();

                    return new Dictionary<string, string>() { {"lat", latitude }, {"lng", longitude } };
                }
            }
            return null;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

namespace FacebookEasyAccess
{
    public static class PageContent
    {
        public static string Get(string url)
        {
            WebRequest request = WebRequest.Create(url);
            var reader = new StreamReader(request.GetResponse().GetResponseStream());
            return reader.ReadToEnd();
        }

        public static IDictionary<string, string> ParseQueryString(string query)
        {
            var result = new Dictionary<string, string>();

            if (String.IsNullOrEmpty(query) || query.Trim().Length == 0)
                return result;

            var pairs = from pair in query.Split('&')
                        where !String.IsNullOrEmpty(pair)
                        let parts = pair.Split('=')
                        select new { Key = parts[0], Value = parts[1] };

            foreach (var pair in pairs)
                result.Add(pair.Key, pair.Value);

            return result;
        }
    }
}

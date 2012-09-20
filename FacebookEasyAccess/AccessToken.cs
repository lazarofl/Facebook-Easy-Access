using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FacebookEasyAccess
{
    public class AccessToken
    {
        public readonly double Expires;
        public readonly string Value;

        public AccessToken(string value, double expires)
        {
            Value = value;
            Expires = expires;
        }

        public AccessToken(string queryString)
        {
            var data = PageContent.ParseQueryString(queryString);
            Expires = Convert.ToDouble(data["expires"]);
            Value = data["access_token"];
        }

        public bool IsEmpty
        {
            get { return Math.Abs(Expires - 0) < 0.001 || string.IsNullOrEmpty(Value); }
        }

        public static implicit operator AccessToken(string input)
        {
            return new AccessToken(input);
        }
    }
}

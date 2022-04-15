using JWT;
using JWT.Algorithms;
using JWT.Serializers;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserRegistration.Authorisation
{
    public class GenrateToken
    {
        private readonly IJwtAlgorithm _algorithm;
        private readonly IJsonSerializer _serializer;
        private readonly IBase64UrlEncoder _base64Encoder;
        private readonly IJwtEncoder _jwtEncoder;

        public GenrateToken()
        {
            // JWT specific initialization.
            _algorithm = new HMACSHA256Algorithm();
            _serializer = new JsonNetSerializer();
            _base64Encoder = new JwtBase64UrlEncoder();
            _jwtEncoder = new JwtEncoder(_algorithm, _serializer, _base64Encoder);

        }

        public string IssuingToken(String Id)
        {
            Dictionary<string, object> claims = new Dictionary<string, object>
            {
                {
                    "UserDetails",
                    Id
                }
            };
            string token = _jwtEncoder.Encode(claims, "ldbudnsvjdnvbsvhsiksncbcvsgwhkaamcbvbdguhrjfkfnmdnbsvwhkmdbbdhjdm dbbdjdkmdnbbjjkdmsnsbdbh");
            return token;

        }


    }
}
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserRegistration.Model
{
    public class UserDetails
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid Id { get; set; } = Guid.NewGuid();


        [JsonProperty(PropertyName = "FirstName")]
        public string FirstName { get; set; }

        [JsonProperty(PropertyName = "LastName")]
        public string LastName { get; set; }

        [JsonProperty(PropertyName = "Email")]
        public string Email { get; set; }

        [JsonProperty(PropertyName = "PassWord")]
        public string Password { get; set; }

        [JsonProperty(PropertyName = "CreatedAt")]
        public DateTime? CreatedAt { get; set; }

        [JsonProperty(PropertyName = "ModifiedAt")]
        public DateTime? ModifiedAt { get; set; }

    }
}

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedBit.CCAdmin
{
    public class Identities
    {
#if WindowsPhone
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "first_name")]
        public string FirstName { get; set; }

        [JsonProperty(PropertyName = "last_name")]
        public string LastName { get; set; }
#else
		public string id { get; set; }

		public string name { get; set; }

		public string first_name { get; set; }

		public string last_name { get; set; }
#endif

    }
}

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedBit.CCAdmin
{
    public class PushChannel
    {
        public int Id { get; set; }

        public string Platform { get; set; }

		public string DeviceId { get; set; }

        [JsonProperty(PropertyName = "uri")]
        public string Uri { get; set; }

    }
}

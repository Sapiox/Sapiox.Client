using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SapioxClient.Models
{
    public class ClientConnectionData
    {
        //JWT Subject == Name
        [Newtonsoft.Json.JsonProperty("sub")]
        public string Sub { get; set; }

        //JWT Audience == Connected Server
        [Newtonsoft.Json.JsonProperty("aud")]
        public string Aud { get; set; }

        //JWT Issuer == Most likely Synapse
        [Newtonsoft.Json.JsonProperty("iss")]
        public string Iss { get; set; }

        [Newtonsoft.Json.JsonProperty("uuid")]
        public string Uuid { get; set; }

        [Newtonsoft.Json.JsonProperty("session")]
        public string Session { get; set; }
    }
}

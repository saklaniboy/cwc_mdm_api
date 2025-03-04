using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cwcerp.Mdm_Models.Response
{
    public class PagingResponse : IDisposable
    {
        [JsonProperty(PropertyName = "draw")]
        public int Draw { get; set; }

        [JsonProperty(PropertyName = "recordsFiltered")]
        public int RecordsFiltered { get; set; }

        [JsonProperty(PropertyName = "recordsTotal")]
        public int RecordsTotal { get; set; }

        [JsonProperty(PropertyName = "responseData")]
        public object responseData { get; set; }

        [JsonProperty(PropertyName = "responseData2")]
        public object responseData2 { get; set; }

        [JsonProperty(PropertyName = "success")]
        public bool success { get; set; }

        [JsonProperty(PropertyName = "message")]
        public string message { get; set; }

        [JsonProperty(PropertyName = "token")]
        public string token { get; set; }



        [JsonProperty(PropertyName = "GrossWeight")]
        public decimal? GrossWeight { get; set; }


        [JsonProperty(PropertyName = "TareWeight")]
        public decimal? TareWeight { get; set; }


        [JsonProperty(PropertyName = "NetWeight")]
        public decimal? NetWeight { get; set; }


        [JsonProperty(PropertyName = "Bags")]
        public Int32? Bags { get; set; }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace cloudconvert_dotnet.Models
{
    class ImportBase64Models
    {
        public string file { get; set; }
        public string filename { get; set; }

        /// <summary>
        /// Base64
        /// </summary>
        /// <param name="file">
        /// The base64 encoded file content. 
        /// </param>
        /// <param name="filename">
        /// The filename of the input file, including extension
        /// </param>
        public ImportBase64Models(string file, string filename)
        {
            this.file = file;
            this.filename = filename;
        }
    }

    class ImportCloudConvertStorageModels
    {
        [JsonProperty(PropertyName = "?xml")]
        public Xml xml { get; set; }
        public Postresponse PostResponse { get; set; }

        public class Xml
        {
            [JsonProperty(PropertyName = "@version")]
            public string version { get; set; }
            [JsonProperty(PropertyName = "@encoding")]
            public string encoding { get; set; }
        }

        public class Postresponse
        {
            public string Bucket { get; set; }
            public string Key { get; set; }
            public string ETag { get; set; }
            public string Location { get; set; }
        }

    }
}

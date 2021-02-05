using cloudconvert_dotnet.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace cloudconvert_dotnet.Models
{
    public class FormData
    {
        public string Name { get; set; }
        public object Content { get; set; }
        public string FileName { get; set; }

        public FormData(string Name, object Content)
        {
            this.Name = Name;
            this.Content = Content;
        }
    }


    public class BodyContentModels
    {
        public MEDIA_SCHEME Scheme { get; set; } = MEDIA_SCHEME.Json;
        public string JsonString { get; set; }
        public List<FormData> FormContent { get; set; }
    }

    public enum MEDIA_SCHEME
    {
        Json, Form
    }
}

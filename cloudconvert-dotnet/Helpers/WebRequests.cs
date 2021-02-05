using cloudconvert_dotnet.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace cloudconvert_dotnet.Helpers
{
    public static class WebRequests
    {
        private const string JSONMEDIA = "application/json";

        public static async Task<HttpResponseMessage> PostAsync(string url, BodyContentModels bodyContent, string accessToken = "")
        {
            var client = new HttpClient();


            client.DefaultRequestHeaders.Clear();

            if(!string.IsNullOrEmpty(accessToken))
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            client.DefaultRequestHeaders
                  .Accept
                  .Add(new MediaTypeWithQualityHeaderValue(JSONMEDIA));//ACCEPT header

            HttpContent content = null;

            if (bodyContent != null)
            {
                if (bodyContent.Scheme == MEDIA_SCHEME.Json)
                    content = new StringContent(bodyContent.JsonString, Encoding.UTF8, JSONMEDIA);
                if (bodyContent.Scheme == MEDIA_SCHEME.Form)
                {
                    var form = new MultipartFormDataContent();

                    foreach (var formItem in bodyContent.FormContent)
                    {
                        if (formItem.Content is Byte[])
                        {
                            var fileBytes = formItem.Content as Byte[];
                            var fileContent = new ByteArrayContent(fileBytes, 0, fileBytes.Length);
                            // Mime types are constantly maintained here
                            // https://github.com/samuelneff/MimeTypeMap
                            // By samuelneff 
                            string mimeType = MimeTypes.MimeTypeMap.GetMimeType(formItem.FileName);
                            fileContent.Headers.Add("Content-Type", mimeType);
                            form.Add(fileContent, formItem.Name, formItem.FileName);
                        }
                        else if (formItem.Content is string)
                            form.Add(new StringContent(formItem.Content as string), formItem.Name);
                    }
                    content = form;
                }
            }

            return await client.PostAsync(url, content);
        }

        public static async Task<HttpResponseMessage> GetAsync(string url, string accessToken)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Clear();

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            return await client.GetAsync(url);
        }
    }
}

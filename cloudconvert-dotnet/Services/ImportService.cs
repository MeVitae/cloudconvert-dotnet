using cloudconvert_dotnet.Helpers;
using cloudconvert_dotnet.Interfaces;
using cloudconvert_dotnet.Models;
using cloudconvert_dotnet_azureclient.models;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml;

namespace cloudconvert_dotnet.Services
{
    public class ImportService : IImportService
    {
        private ICommonService commonService;

        public ImportService(ICommonService commonService)
        {
            this.commonService = commonService;
        }

        public async Task<TaskModels<TaskFileResultModels>> Import(string base64, string fileName)
        {
            return await commonService.Post(new ImportBase64Models(base64, fileName),  @"/v2/import/base64");
        }

        private async Task<TaskModels<TaskFileResultModels>> RetrieveTask(HttpResponseMessage response, TaskModels<TaskFormResultModels> taskModelWithForm)
        {
            var taskModelWithFiles = new TaskModels<TaskFileResultModels>();
            taskModelWithFiles.data = new TaskDataModels<TaskFileResultModels>(taskModelWithForm.data);
            taskModelWithFiles.data.result = new TaskFileResultModels();

            var responseString = await response.Content.ReadAsStringAsync();
            if (!string.IsNullOrEmpty(responseString))
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(responseString);
                string json = JsonConvert.SerializeXmlNode(doc);

                var cloudConvertStorageModels = JsonConvert.DeserializeObject<ImportCloudConvertStorageModels>(json);
                taskModelWithFiles.data.result.files = new TaskFileModels[]
                {
                        new TaskFileModels
                        {
                            filename = cloudConvertStorageModels.PostResponse.Key.Replace($"{taskModelWithFiles.data.id}/", ""),
                            md5 = cloudConvertStorageModels.PostResponse.ETag,
                            url = cloudConvertStorageModels.PostResponse.Location
                        }
                };
            }

            return taskModelWithFiles;
        }

        public async Task<TaskModels<TaskFileResultModels>> Import(Byte[] fileBytes, string fileName)
        {
            var url = commonService.ApiEndpoint + @"/v2/import/upload";
            var response = await WebRequests.PostAsync(url, null, commonService.AccessToken);

            var taskModelWithForm = await commonService.RetrieveTask<TaskFormResultModels>(response);

            long milliseconds = new DateTimeOffset(DateTime.UtcNow.AddDays(1)).ToUnixTimeMilliseconds();
            BodyContentModels bodyContent = new BodyContentModels();
            var parameters = taskModelWithForm.data.result.form.parameters;
            bodyContent.FormContent = new System.Collections.Generic.List<FormData>
            {
                new FormData(nameof(parameters.acl), parameters.acl ?? ""),
                new FormData(nameof(parameters.key), parameters.key ?? ""),
                new FormData(nameof(parameters.Policy), parameters.Policy ?? ""),
                new FormData(nameof(parameters.success_action_status), parameters.success_action_status ?? ""),
                new FormData(StaticMethods.GetJsonPropertyName<TaskParametersModels>(x => x.XAmzAlgorithm), parameters.XAmzAlgorithm ?? ""),
                new FormData(StaticMethods.GetJsonPropertyName<TaskParametersModels>(x => x.XAmzCredential), parameters.XAmzCredential ?? ""),
                new FormData(StaticMethods.GetJsonPropertyName<TaskParametersModels>(x => x.XAmzDate), parameters.XAmzDate ?? ""),
                new FormData(StaticMethods.GetJsonPropertyName<TaskParametersModels>(x => x.XAmzSignature), parameters.XAmzSignature ?? ""),

                new FormData("file", fileBytes)
                {
                   FileName = fileName
                }
            };
            bodyContent.Scheme = MEDIA_SCHEME.Form;

            response = await WebRequests.PostAsync(taskModelWithForm.data.result.form.url, bodyContent);

            if (response.StatusCode == System.Net.HttpStatusCode.OK || response.ReasonPhrase == "Created")
                return await RetrieveTask(response, taskModelWithForm);
            else return null;
        }

        public async Task<TaskModels<TaskFileResultModels>> Import(ImportAzureModels imoportAzure)
        {
            return await commonService.Post(imoportAzure, @"/v2/import/azure/blob");
        }
    }
}

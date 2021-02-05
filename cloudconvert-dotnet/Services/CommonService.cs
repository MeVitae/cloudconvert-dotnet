using cloudconvert_dotnet.Helpers;
using cloudconvert_dotnet.Interfaces;
using cloudconvert_dotnet.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace cloudconvert_dotnet.Services
{
    public class CommonService : ICommonService
    {
        public string AccessToken { get; }
        public string ApiEndpoint { get; }

        public CommonService(string accessToken, string apiEndpoint)
        {
            this.AccessToken = accessToken;
            this.ApiEndpoint = apiEndpoint;
        }

        public async Task<TaskModels<R>> RetrieveTask<R>(HttpResponseMessage response)
        {
            TaskModels<R> taskModel = new TaskModels<R>();
            var responseString = await response.Content.ReadAsStringAsync();
            if (!string.IsNullOrEmpty(responseString))
                taskModel = JsonConvert.DeserializeObject<TaskModels<R>>(responseString);

            return taskModel;
        }

        public async Task<TaskModels<R>> WaitForTaskComplete<R>(string taskId)
        {
            string url = ApiEndpoint + @"/v2/tasks/" + taskId;
            TaskModels<R> taskModel = new TaskModels<R>();
            bool completed = false;
            do
            {
                var response = await WebRequests.GetAsync(url, AccessToken);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    taskModel = await RetrieveTask<R>(response);

                if (taskModel.data?.status == "error" || taskModel.data?.status == "finished")
                    completed = true;
                else await Task.Delay(TimeSpan.FromSeconds(1));

            } while (!completed);

            return taskModel;
        }

        public async Task<TaskModels<TaskFileResultModels>> Post(object model, string query)
        {
            BodyContentModels bodyContent = new BodyContentModels();
            bodyContent.JsonString = JsonConvert.SerializeObject(model);
            bodyContent.Scheme = MEDIA_SCHEME.Json;

            var response = await WebRequests.PostAsync(ApiEndpoint + query, bodyContent, AccessToken);

            if (response.StatusCode == System.Net.HttpStatusCode.OK || response.ReasonPhrase == "Created")
            {
                var task = await RetrieveTask<object>(response);
                return await WaitForTaskComplete<TaskFileResultModels>(task.data.id);
            }
            else return null;
        }
    }
}

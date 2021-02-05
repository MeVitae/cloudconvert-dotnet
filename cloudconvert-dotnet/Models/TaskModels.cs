using cloudconvert_dotnet.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace cloudconvert_dotnet.Models
{
    public class TaskModels<R>
    {
        public TaskDataModels<R> data { get; set; }
    }

    #region Export File Results
    public class TaskFileResultModels
    {
        public TaskFileModels[] files { get; set; }
    }

    public class TaskFileModels
    {
        public string filename { get; set; }
        public string url { get; set; }
        public string md5 { get; set; }
    }
    #endregion

    #region Import Form Results
    public class TaskFormResultModels
    {
        public TaskFormModels form { get; set; }
    }

    public class TaskFormModels
    {
        public string url { get; set; }
        public TaskParametersModels parameters { get; set; }
    }

    public class TaskParametersModels
    {
        public string acl { get; set; }
        public string key { get; set; }
        public string success_action_status { get; set; }
        [JsonProperty(PropertyName = "X-Amz-Credential")]
        public string XAmzCredential { get; set; }
        [JsonProperty(PropertyName = "X-Amz-Algorithm")]
        public string XAmzAlgorithm { get; set; }
        [JsonProperty(PropertyName = "X-Amz-Date")]
        public string XAmzDate { get; set; }
        public string Policy { get; set; }
        [JsonProperty(PropertyName = "X-Amz-Signature")]
        public string XAmzSignature { get; set; }
    }
    #endregion

    public class TaskDataCommonModels
    {
        public string id { get; set; }
        public object job_id { get; set; }
        public string status { get; set; }
        public object credits { get; set; }
        /// <summary>
        /// Error Code
        /// </summary>
        public string code { get; set; }
        public object message { get; set; }
        public int percent { get; set; }
        public string operation { get; set; }
        public DateTime created_at { get; set; }
        public object started_at { get; set; }
        public object ended_at { get; set; }
        public object retry_of_task_id { get; set; }
        public object copy_of_task_id { get; set; }
        public int user_id { get; set; }
        public int priority { get; set; }
        public object host_name { get; set; }
        public object storage { get; set; }
        public object[] depends_on_task_ids { get; set; }
    }

    public class TaskDataModels<R> : TaskDataCommonModels
    {
        public TaskDataModels(TaskDataCommonModels baseModel)
        {
            StaticMethods.PopulateBaseProperties(this, baseModel);
        }
        public TaskDataModels()
        {

        }

        public R result { get; set; }
        public TaskLinksModels links { get; set; }
    }

    public class TaskLinksModels
    {
        public string self { get; set; }
    }
}

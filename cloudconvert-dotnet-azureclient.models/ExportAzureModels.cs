using System;
using System.Collections.Generic;
using System.Text;

namespace cloudconvert_dotnet_azureclient.models
{
    public class ExportAzureModels : CommonAzureModels
    {
        /// <summary>
        /// The ID of the task to export. Multiple task IDs can be provided as an array. 
        /// </summary>
        public string input { get; set; }
    }
}

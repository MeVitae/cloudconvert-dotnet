using cloudconvert_dotnet.Models;
using cloudconvert_dotnet_azureclient.models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace cloudconvert_dotnet.Interfaces
{
    public interface IExportService
    {
        Task<TaskModels<TaskFileResultModels>> Export(string taskId);
        Task<TaskModels<TaskFileResultModels>> Export(ExportAzureModels exportAzure);
    }
}

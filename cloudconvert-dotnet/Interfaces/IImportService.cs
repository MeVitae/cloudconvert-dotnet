using cloudconvert_dotnet.Models;
using cloudconvert_dotnet_azureclient.models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace cloudconvert_dotnet.Interfaces
{
    public interface IImportService
    {
        Task<TaskModels<TaskFileResultModels>> Import(string base64, string fileName);
        Task<TaskModels<TaskFileResultModels>> Import(Byte[] fileBytes, string fileName);
        Task<TaskModels<TaskFileResultModels>> Import(ImportAzureModels imoportAzure);
    }
}

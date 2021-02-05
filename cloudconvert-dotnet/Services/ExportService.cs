using cloudconvert_dotnet.Helpers;
using cloudconvert_dotnet.Interfaces;
using cloudconvert_dotnet.Models;
using cloudconvert_dotnet_azureclient.models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace cloudconvert_dotnet.Services
{
    public class ExportService : IExportService
    {
        private ICommonService commonService;

        public ExportService(ICommonService commonService)
        {
            this.commonService = commonService;
        }

        public async Task<TaskModels<TaskFileResultModels>> Export(string taskId)
        {
            return await commonService.Post(new ExportURLModels(taskId), @"/v2/export/url");
        }

        public async Task<TaskModels<TaskFileResultModels>> Export(ExportAzureModels exportAzure)
        {
            return await commonService.Post(exportAzure, @"/v2/export/azure/blob");
        }
    }
}

using cloudconvert_dotnet.Helpers;
using cloudconvert_dotnet.Interfaces;
using cloudconvert_dotnet.Models;
using cloudconvert_dotnet.Services;
using cloudconvert_dotnet_azureclient.models;
using System;
using System.Threading.Tasks;

namespace cloudconvert_dotnet
{
    public class CloudConvertClient
    {
        public readonly string accessToken;
        public readonly string apiEndpoint;

        public readonly IImportService importService;
        public readonly IConvertService convertService;
        public readonly IExportService exportService;

        public CloudConvertClient(string accessToken) :
            this(accessToken, @"https://api.cloudconvert.com")
        {
        }

        public CloudConvertClient(string accessToken, string apiEndpoint)
        {
            this.accessToken = accessToken;
            this.apiEndpoint = apiEndpoint;

            ICommonService commonService = new CommonService(accessToken, apiEndpoint);

            importService = new ImportService(commonService);
            convertService = new ConvertService(commonService);
            exportService = new ExportService(commonService);
        }

        #region Import

        public async Task<TaskModels<TaskFileResultModels>> Import(string base64, string fileName)
        {
            return await importService.Import(base64, fileName);
        }

        public async Task<TaskModels<TaskFileResultModels>> Import(Byte[] fileBytes, string fileName)
        {
            return await importService.Import(fileBytes, fileName);
        }

        public async Task<TaskModels<TaskFileResultModels>> Import(string storage_account, string sas_token, string container, string blob, string filename)
        {
            ImportAzureModels importAzure = new ImportAzureModels();
            importAzure.storage_account = storage_account;
            importAzure.sas_token = sas_token;
            importAzure.container = container;
            importAzure.blob = blob;
            importAzure.filename = filename;

            return await importService.Import(importAzure);
        }

        public async Task<TaskModels<TaskFileResultModels>> Import(CommonAzureModels commonAzure, string filename)
        {
            ImportAzureModels importAzure = new ImportAzureModels();
            StaticMethods.PopulateBaseProperties(importAzure, commonAzure);
            importAzure.filename = filename;

            return await importService.Import(importAzure);
        }

        #endregion

        #region Convert

        public async Task<TaskModels<TaskFileResultModels>> Convert(string taskId, string toFormat)
        {
            return await convertService.Convert(taskId, toFormat);
        }

        #endregion

        #region Export

        public async Task<TaskModels<TaskFileResultModels>> Export(string taskId)
        {
            return await exportService.Export(taskId);
        }

        public async Task<TaskModels<TaskFileResultModels>> Export(string input, string storage_account, string sas_token, string container, string blob)
        {
            ExportAzureModels exportAzure = new ExportAzureModels();
            exportAzure.input = input;
            exportAzure.storage_account = storage_account;
            exportAzure.sas_token = sas_token;
            exportAzure.container = container;
            exportAzure.blob = blob;

            return await exportService.Export(exportAzure);
        }

        public async Task<TaskModels<TaskFileResultModels>> Export(string input, CommonAzureModels commonAzure)
        {
            ExportAzureModels exportAzure = new ExportAzureModels();
            StaticMethods.PopulateBaseProperties(exportAzure, commonAzure);
            exportAzure.input = input;

            return await exportService.Export(exportAzure);
        }

        #endregion
    }
}

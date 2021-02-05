using Azure.Storage;
using Azure.Storage.Sas;
using cloudconvert_dontnet_azureclient;
using cloudconvert_dotnet.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace cloudconvert_dotnet.Tests
{
    [TestClass()]
    public class CloudConvertClientTests
    {
        CloudConvertClient cloudConvertClient;
        AzureCloudConvertClient azureCloudConvertClient;

        [TestInitialize]
        public void TestInitialise()
        {
            // Hash the files to sandbox test cloudconvert
            // Fake data cloudconvert-dotnet\cloudconvert-dotnetTests\Source
            // Windows 10: CertUtil -hashfile Resume10.docx MD5 
            // Hash is 7802cbd4bce6a39e9f7882d64d8672c8

            // string key folder path
            string keyFolderPath = @"D:\MeVitaeWorkspace\MeVitaeNotes\MeVitaeKeys\";

            // File Path to the JWT Token 
            var tokenPath = Path.Combine(keyFolderPath, @"cloudconvert.txt");
            string accessTokenSandbox = File.ReadAllText(tokenPath);
            string apiEndpointSandbox = @"https://api.sandbox.cloudconvert.com";

            cloudConvertClient = new CloudConvertClient(accessTokenSandbox, apiEndpointSandbox);

            // Azure blob storage connection string
            string azureConnectionStringPath = Path.Combine(keyFolderPath, @"storageconnectionstring.txt");
            string azureConnectionString = File.ReadAllText(azureConnectionStringPath);
            string containerName = @"cloudconvert";

            azureCloudConvertClient = new AzureCloudConvertClient(azureConnectionString, containerName);
        }

        [TestMethod()]
        public async Task ImportBase64Test()
        {
            var filename = @"Resume11.pdf";
            var filepath = @"Source/";
            var fileBytes = File.ReadAllBytes(Path.Combine(filepath, filename));
            var base64 = Convert.ToBase64String(File.ReadAllBytes(Path.Combine(filepath, filename)));

            var taskModel = await cloudConvertClient.Import(base64, filename);

            Assert.IsNotNull(taskModel, taskModel.data.code);

            var file_md5 = StaticMethods.CreateMD5(fileBytes);

            foreach (var file in taskModel.data.result.files)
                Assert.AreEqual(file.md5.Trim('"'), file_md5.ToLower());

            Console.WriteLine(taskModel.data.id);
        }

        [TestMethod()]
        public async Task ImportUploadFileTest()
        {
            var filename = @"Resume11.pdf";
            var filepath = @"Source/";
            var fileBytes = File.ReadAllBytes(Path.Combine(filepath, filename));

            var taskModel = await cloudConvertClient.Import(fileBytes, filename);

            Assert.IsNotNull(taskModel, taskModel.data.code);

            var file_md5 = StaticMethods.CreateMD5(fileBytes);

            foreach (var file in taskModel.data.result.files)
                Assert.AreEqual(file.md5.Trim('"'), file_md5.ToLower());

            Console.WriteLine(taskModel.data.id);
        }

        [TestMethod()]
        public async Task ImportAzureFileTest()
        {
            var filename = @"Resume11.pdf";
            var filepath = @"Source/";
            var fileBytes = File.ReadAllBytes(Path.Combine(filepath, filename));

            var commonAzure = await azureCloudConvertClient.Upload(fileBytes);

            var taskModel = await cloudConvertClient.Import(commonAzure, filename);

            Assert.IsNotNull(taskModel, taskModel.data.code);

            var file_md5 = StaticMethods.CreateMD5(fileBytes);

            foreach (var file in taskModel.data.result.files)
                Assert.AreEqual(file.md5.Trim('"'), file_md5.ToLower());

            Console.WriteLine(taskModel.data.id);
            Console.WriteLine(commonAzure.blob);
        }

        [TestMethod()]
        public async Task ConvertTest()
        {
            // Use the task id from the UploadFiles test
            var taskid = @"47ba87a5-4e47-4692-bc6b-9194bab58c2a";
            var toFormat = @"docx";

            var taskModel = await cloudConvertClient.Convert(taskid, toFormat);

            Assert.IsNotNull(taskModel, taskModel.data.code);

            Console.WriteLine(taskModel.data.id);
        }

        [TestMethod()]
        public async Task ExportURLTest()
        {
            // Use the task id from the UploadFiles test
            var taskid = @"ac5b2d6f-bec5-4982-b8ee-af7e17768608";

            //https://cloudconvert.com/blog/changes-in-default-behaviour-for-file-storage

            var taskModel = await cloudConvertClient.Export(taskid);

            Assert.IsNotNull(taskModel, taskModel.data.code);

            bool allPass = true;
            foreach (var file in taskModel.data.result.files)
            {
                var base64 = Convert.ToBase64String(new WebClient().DownloadData(file.url));
                if (string.IsNullOrEmpty(base64))
                {
                    allPass = false;
                    taskModel.data.code += " " + file.filename;
                }
            }

            Assert.IsTrue(allPass, taskModel.data.code);

            Console.WriteLine(taskModel.data.id);
        }

        [TestMethod()]
        public async Task ExportAzureFileTest()
        {
            var taskid = @"ac5b2d6f-bec5-4982-b8ee-af7e17768608";
            var commonAzure = azureCloudConvertClient.GetCommonAzureModels();

            var taskModel = await cloudConvertClient.Export(taskid, commonAzure);

            Assert.IsNotNull(taskModel, taskModel.data.code);

            bool allPass = true;
            foreach (var file in taskModel.data.result.files)
            {
                var base64 = await azureCloudConvertClient.Download(file.filename);
                if (string.IsNullOrEmpty(base64))
                {
                    allPass = false;
                    taskModel.data.code += " " + file.filename;
                }
            }

            Assert.IsTrue(allPass, taskModel.data.code);

            Console.WriteLine(taskModel.data.id);
        }
    }
}
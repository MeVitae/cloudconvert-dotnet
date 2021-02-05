using Microsoft.VisualStudio.TestTools.UnitTesting;
using cloudconvert_dotnet.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace cloudconvert_dotnet.Services.Tests
{
    [TestClass()]
    public class CommonServiceTests
    {
        CommonService commonService;


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

            commonService = new CommonService(accessTokenSandbox, apiEndpointSandbox);
        }

        [TestMethod()]
        public async Task WaitForTaskCompleteTest()
        {
            // Use the task id from the UploadFiles test
            var taskid = @"1ecafb7e-a864-4dd0-bded-1b8ed107a4a4";

            var taskModel = await commonService.WaitForTaskComplete<object>(taskid);

            Assert.IsNotNull(taskModel, taskModel.data.code);
        }
    }
}
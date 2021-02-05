using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using cloudconvert_dotnet_azureclient.models;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cloudconvert_dontnet_azureclient
{
    public class AzureCloudConvertClient
    {
        private readonly BlobContainerClient azureContainerClient;
        private readonly string azureAccountKey = "";

        public AzureCloudConvertClient(string azureConnectionString, string containerName)
        {
            // Create a BlobServiceClient object which will be used to create a container client
            var azureblobServiceClient = new BlobServiceClient(azureConnectionString);
            azureAccountKey = azureConnectionString.Split(';')
                .Where(m => m.StartsWith("AccountKey="))
                .FirstOrDefault().Replace("AccountKey=", "");

            // Create the container and return a container client object
            azureContainerClient = azureblobServiceClient.GetBlobContainerClient(containerName);
        }

        private string GetAzureContainerSASToken()
        {
            // Check whether this BlobContainerClient object has been authorized with Shared Key.
            if (azureContainerClient.CanGenerateSasUri)
            {
                // Create a SAS token that's valid for one hour.
                BlobSasBuilder sasBuilder = new BlobSasBuilder()
                {
                    BlobContainerName = azureContainerClient.Name,
                    Resource = "c"
                };

                sasBuilder.ExpiresOn = DateTimeOffset.UtcNow.AddHours(1);
                sasBuilder.SetPermissions(permissions: BlobAccountSasPermissions.Read | BlobAccountSasPermissions.Create);

                return sasBuilder.ToSasQueryParameters(new StorageSharedKeyCredential(azureContainerClient.AccountName, azureAccountKey)).ToString();
            }
            else
            {
                Console.WriteLine(@"BlobContainerClient must be authorized with Shared Key 
                          credentials to create a service SAS.");
                return null;
            }
        }

        public CommonAzureModels GetCommonAzureModels(string blobName = "")
        {
            CommonAzureModels commonAzure = new CommonAzureModels();

            if (string.IsNullOrEmpty(blobName))
                blobName = Guid.NewGuid().ToString();

            commonAzure.blob = blobName;

            commonAzure.storage_account = azureContainerClient.AccountName;
            commonAzure.sas_token = GetAzureContainerSASToken();
            commonAzure.container = azureContainerClient.Name;

            return commonAzure;
        }

        public async Task<CommonAzureModels> Upload(byte[] fileBytes, string blobName = "")
        {
            var commonAzure = GetCommonAzureModels(blobName);

            var uploadedBlob = await azureContainerClient.UploadBlobAsync(blobName, new MemoryStream(fileBytes));

            var uploadStatus = uploadedBlob.GetRawResponse().Status;
            // Error Handling
            if (uploadStatus >= 200 && uploadStatus <= 300)
            {

            }

            return commonAzure;
        }

        private string ConvertToBase64(Stream stream)
        {
            byte[] bytes;
            using (var memoryStream = new MemoryStream())
            {
                stream.CopyTo(memoryStream);
                bytes = memoryStream.ToArray();
            }

            return Convert.ToBase64String(bytes);
        }

        public async Task<string> Download(string blobName)
        {
            BlobClient blobClient = azureContainerClient.GetBlobClient(blobName);
            var blob = await blobClient.DownloadAsync();
            return ConvertToBase64(blob.Value.Content);
        }
    }
}

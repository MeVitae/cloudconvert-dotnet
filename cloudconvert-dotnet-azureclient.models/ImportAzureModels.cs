using System;

namespace cloudconvert_dotnet_azureclient.models
{
    public class ImportAzureModels : CommonAzureModels
    {

        /// <summary>
        /// The filename of the input file, including extension. If none provided we will use the blob parameter as the filename for the file. 
        /// </summary>
        public string filename { get; set; }
    }
}

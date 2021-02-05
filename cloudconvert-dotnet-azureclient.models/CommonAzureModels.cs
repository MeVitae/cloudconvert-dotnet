using System;
using System.Collections.Generic;
using System.Text;

namespace cloudconvert_dotnet_azureclient.models
{
    public class CommonAzureModels
    {
        /// <summary>
        /// The name of the Azure storage account (This is the string before .blob.core.windows.net). 
        /// </summary>
        public string storage_account { get; set; }
        /// <summary>
        /// The Azure SAS token. 
        /// </summary>
        public string sas_token { get; set; }
        /// <summary>
        /// Azure container name. 
        /// </summary>
        public string container { get; set; }
        /// <summary>
        /// blob
        /// </summary>
        public string blob { get; set; }
    }
}

using cloudconvert_dotnet.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace cloudconvert_dotnet.Interfaces
{
    public interface ICommonService
    {
        public string AccessToken { get; }
        public string ApiEndpoint{ get; }

        Task<TaskModels<R>> RetrieveTask<R>(HttpResponseMessage response);

        Task<TaskModels<TaskFileResultModels>> Post(object model, string query);
    }
}

using cloudconvert_dotnet.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace cloudconvert_dotnet.Interfaces
{
    public interface IConvertService
    {
        Task<TaskModels<TaskFileResultModels>> Convert(string taskId, string toFormat);
    }
}

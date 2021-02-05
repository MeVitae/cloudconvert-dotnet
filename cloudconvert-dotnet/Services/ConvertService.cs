using cloudconvert_dotnet.Helpers;
using cloudconvert_dotnet.Interfaces;
using cloudconvert_dotnet.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace cloudconvert_dotnet.Services
{
    public class ConvertService : IConvertService
    {
        private ICommonService commonService { get; }

        public ConvertService(ICommonService commonService)
        {
            this.commonService = commonService;
        }

        public async Task<TaskModels<TaskFileResultModels>> Convert(string taskId, string toFormat)
        {
            return await commonService.Post(new ConvertModels(taskId, toFormat), @"/v2/convert");
        }
    }
}

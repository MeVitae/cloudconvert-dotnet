using System;
using System.Collections.Generic;
using System.Text;

namespace cloudconvert_dotnet.Models
{
    class ExportURLModels
    {
        public string input { get; set; }

        /// <summary>
        /// Export URL
        /// </summary>
        /// <param name="input">
        /// The ID of the task to create temporary URLs for. Multiple task IDs can be provided as an array. 
        /// </param>
        public ExportURLModels(string input)
        {
            this.input = input;
        }
    }
}

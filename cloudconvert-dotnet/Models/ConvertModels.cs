using System;
using System.Collections.Generic;
using System.Text;

namespace cloudconvert_dotnet.Models
{
    class ConvertModels
    {
        public string input { get; set; }
        public string output_format { get; set; }



        /// <summary>
        /// Convert
        /// </summary>
        /// <param name="input">
        /// The ID of the input task for the conversion, normally the import task. Multiple task IDs can be provided as an array. 
        /// </param>
        /// <param name="output_format">
        /// The target format to convert to. 
        /// </param>
        public ConvertModels(string input, string output_format)
        {
            this.input = input;
            this.output_format = output_format;
        }
    }
}

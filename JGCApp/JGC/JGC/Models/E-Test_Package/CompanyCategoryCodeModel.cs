using System;
using System.Collections.Generic;
using System.Text;

namespace JGC.Models.E_Test_Package
{
    public class CompanyCategoryCodeModel
    {
        public override string ToString()
        {
            return text.ToString();
        }
        public string text { get; set; }
        public string value { get; set; }

        public CompanyCategoryCodeModel(string textInput, string valueInput)
        {
            text = textInput;
            value = valueInput;
        }
    }
}

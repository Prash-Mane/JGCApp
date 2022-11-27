using System;
using System.Collections.Generic;
using System.Text;

namespace JGC.Models.Work_Pack
{
    public class IWPCompanyCategoryCodeModel
    {
        public override string ToString()
        {
            return text.ToString();
        }
        public string text { get; set; }
        public string value { get; set; }

        public IWPCompanyCategoryCodeModel(string textInput, string valueInput)
        {
            text = textInput;
            value = valueInput;
        }
    }
}

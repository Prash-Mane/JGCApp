using System;
using System.Collections.Generic;
using System.Text;

namespace JGC.Models.E_Test_Package
{
    public abstract class TestPackageImage 
    {
        public override string ToString()
        {
            return DisplayName.ToString();
        }

        public string DisplayName { get; set; }
        public string FileName { get; set; }
        public string Extension { get; set; }
        public int FileSize { get; set; }
        public string FileBytes { get; set; }

        public abstract string InsertQuery(int projectID, int etestpackageID); // created so the classes that inherit this one, can have their own overrides.

    }
}

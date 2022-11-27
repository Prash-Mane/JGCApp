using System;
using System.Collections.Generic;
using System.Text;

namespace JGC.Models.E_Test_Package
{
    public class PunchImage : TestPackageImage
    {
        public string PunchID { get; set; }
        public Boolean Live { get; set; }
        
        public override string InsertQuery(int projectID, int etestpackageID)
        {
            string sql = "INSERT INTO [T_PunchImage] ([ProjectID],[ETestPackageID],[PunchID],[DisplayName],[FileName],[Extension],[FileSize],[FileBytes],[Live]) VALUES " +
                        "('"+ projectID + "','"+ etestpackageID + "','"+ PunchID + "','"+ DisplayName + "','"+ FileName + "','"+ Extension + "','"+ FileSize + "','" +
                          FileBytes + "',0)";
                        
            return sql;
        }
    }
}

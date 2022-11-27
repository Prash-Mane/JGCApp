using JGC.DataBase.DataTables;
using System;
using System.Collections.Generic;
using System.Text;

namespace JGC.Models.E_Test_Package
{
    public class TestRecordImage : TestPackageImage
    {
        public override string InsertQuery(int projectID, int etestpackageID)
        {
            string sql = "INSERT INTO [T_TestRecordImage] ([ProjectID],[ETestPackageID],[DisplayName],[FileName],[Extension],[FileSize],[FileBytes]) VALUES " +
                                                    "('"+ projectID + "','"+ etestpackageID + "','"+ DisplayName + "','"+ FileName + "','"+ Extension + "','"+ FileSize + "','"+ FileBytes + "')";
           



            return sql;
        }
    }
}

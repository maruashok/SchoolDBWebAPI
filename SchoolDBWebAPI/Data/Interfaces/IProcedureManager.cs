using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace SchoolDBWebAPI.Data.DBHelper
{
    public interface IProcedureManager
    {
        void Dispose();

        bool ExecStoreProcedure(string StoreProcedure, List<DBSQLParameter> SQLParameters);

        bool ExecStoreProcedure(string StoreProcedure, object StoreProcedureModel);

        List<T> ExecStoreProcedure<T>(string StoreProcedure, List<DBSQLParameter> SQLParameters);

        List<T> ExecStoreProcedure<T>(string StoreProcedure, object StoreProcedureModel);

        DataTable ExecStoreProcedureDT(string StoreProcedure, List<DBSQLParameter> SQLParameters);

        List<DBSQLParameter> ExecStoreProcedureOut(string StoreProcedure, List<DBSQLParameter> SQLParameters);

        DataTable ExecuteSelect(string Command, params SqlParameter[] SQLParameters);

        List<T> ExecuteSelect<T>(string Command, params SqlParameter[] SQLParameters) where T : new();

        List<DBSQLParameter> GenerateParams(object objModel, bool AddNull = false);
    }
}
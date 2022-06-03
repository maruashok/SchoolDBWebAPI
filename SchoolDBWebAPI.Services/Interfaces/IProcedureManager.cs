using SchoolDBWebAPI.Services.SPHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace SchoolDBWebAPI.Services.Interfaces
{
    public interface IProcedureManager
    {
        void Dispose();

        bool ExecStoreProcedure(string StoreProcedure, List<DBSQLParameter> SQLParameters);

        bool ExecStoreProcedure(string StoreProcedure, object StoreProcedureModel);

        List<T> ExecStoreProcedure<T>(string StoreProcedure, List<DBSQLParameter> SQLParameters);

        List<T> ExecStoreProcedure<T>(string StoreProcedure, object StoreProcedureModel);

        Tuple<List<TFirst>, List<TSecond>> ExecStoreProcedureMulResults<TFirst, TSecond>(string StoreProcedure, List<DBSQLParameter> SQLParameters);

        List<DBSQLParameter> ExecStoreProcedureOut(string StoreProcedure, List<DBSQLParameter> SQLParameters);

        List<DBSQLParameter> GenerateParams(object objModel, bool AddNull = false);
    }
}
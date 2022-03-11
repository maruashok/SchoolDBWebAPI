using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SchoolDBWebAPI.DBModels;
using SchoolDBWebAPI.Helpers;
using Serilog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace SchoolDBWebAPI.Data.DBHelper
{
    public interface IProcedureManager : IDisposable
    {
        bool ExecStoreProcedure(string StoreProcedure, List<DBSQLParameter> SQLParameters);

        bool ExecStoreProcedure(string StoreProcedure, object StoreProcedureModel);

        int ExecStoreProcedure(string StoreProcedure, params object[] SQLParameters);

        List<T> ExecStoreProcedure<T>(string StoreProcedure, List<DBSQLParameter> SQLParameters);

        List<T> ExecStoreProcedure<T>(string StoreProcedure, object StoreProcedureModel);

        DataTable ExecStoreProcedureDT(string StoreProcedure, List<DBSQLParameter> SQLParameters);

        DataTable ExecStoreProcedureDT(string StoreProcedure, object StoreProcedureModel);

        List<DBSQLParameter> ExecStoreProcedureOut(string StoreProcedure, List<DBSQLParameter> SQLParameters);

        List<DBSQLParameter> GenerateParams(object objModel, bool AddNull = false);
    }

    public class ProcedureManager : IProcedureManager
    {
        private bool disposed = false;
        private SchoolDBContext dbContext;
        private ILogger logger = Log.ForContext(typeof(ProcedureManager));

        public ProcedureManager()
        {
            dbContext = new SchoolDBContext();
        }

        private bool OpenConnection(SqlConnection connection)
        {
            bool Result = false;
            try
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                    Result = connection.State == ConnectionState.Open;
                }
            }
            catch (Exception Ex)
            {
                logger.Error(Ex, Ex.Message);
            }

            return Result;
        }

        private void CloseConnection(SqlConnection connection)
        {
            try
            {
                if (connection.State != ConnectionState.Closed)
                {
                    connection.Close();
                }
            }
            catch (Exception Ex)
            {
                logger.Error(Ex, Ex.Message);
            }
        }

        public bool ExecStoreProcedure(string StoreProcedure, object StoreProcedureModel)
        {
            bool Result = true;

            try
            {
                List<DBSQLParameter> SQLParameters = GenerateParams(StoreProcedureModel);
                using (SqlConnection connection = new SqlConnection(dbContext.Database.GetConnectionString()))
                {
                    using (SqlCommand sqlCommand = new SqlCommand(StoreProcedure, connection))
                    {
                        if (SQLParameters != null)
                        {
                            sqlCommand.CommandType = CommandType.StoredProcedure;

                            foreach (DBSQLParameter curParam in SQLParameters)
                            {
                                sqlCommand.Parameters.AddWithValue(curParam.Name, curParam.Value ?? DBNull.Value);
                            }
                        }

                        if (OpenConnection(connection))
                        {
                            sqlCommand.ExecuteNonQuery();
                            CloseConnection(connection);
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                logger.Error(Ex, Ex.Message);
            }

            return Result;
        }

        public List<DBSQLParameter> GenerateParams(object objModel, bool AddNull = false)
        {
            List<DBSQLParameter> paramList = new List<DBSQLParameter>();

            try
            {
                foreach (PropertyInfo item in objModel.GetType().GetProperties())
                {
                    if (item.GetValue(objModel) == null)
                    {
                        if (AddNull)
                        {
                            paramList.Add(new DBSQLParameter($"@{item.Name}", DBNull.Value));
                        }
                    }
                    else
                    {
                        paramList.Add(new DBSQLParameter($"@{item.Name}", item.GetValue(objModel)));
                    }
                }
            }
            catch (Exception Ex)
            {
                logger.Error(Ex, Ex.Message);
            }

            return paramList;
        }

        public int ExecStoreProcedure(string StoreProcedure, params object[] SQLParameters)
        {
            int RowsAffected = -1;
            try
            {
                RowsAffected = dbContext.Database.ExecuteSqlRaw(StoreProcedure, SQLParameters);
            }
            catch (Exception Ex)
            {
                logger.Error(Ex, Ex.Message);
            }
            return RowsAffected;
        }

        public List<T> ExecStoreProcedure<T>(string StoreProcedure, object StoreProcedureModel)
        {
            List<T> objResult = default;
            DataTable _table = new DataTable();

            try
            {
                List<DBSQLParameter> SQLParameters = GenerateParams(StoreProcedureModel);

                using (SqlConnection connection = new SqlConnection(dbContext.Database.GetConnectionString()))
                {
                    using (SqlCommand sqlCommand = new SqlCommand(StoreProcedure, connection))
                    {
                        if (SQLParameters != null)
                        {
                            sqlCommand.CommandType = CommandType.StoredProcedure;

                            foreach (DBSQLParameter curParam in SQLParameters)
                            {
                                sqlCommand.Parameters.AddWithValue(curParam.Name, curParam.Value ?? DBNull.Value);
                            }
                        }

                        using (var dataAdapter = new SqlDataAdapter(sqlCommand))
                        {
                            dataAdapter.Fill(_table);
                        }
                    }
                }

                if (_table != null)
                {
                    objResult = _table.ToList<T>();
                }
            }
            catch (Exception Ex)
            {
                logger.Error(Ex, Ex.Message);
            }

            return objResult;
        }

        public DataTable ExecStoreProcedureDT(string StoreProcedure, object StoreProcedureModel)
        {
            DataTable dataTable = new DataTable();

            try
            {
                List<DBSQLParameter> SQLParameters = GenerateParams(StoreProcedureModel);

                using (SqlConnection connection = new SqlConnection(dbContext.Database.GetConnectionString()))
                {
                    using (SqlCommand sqlCommand = new SqlCommand(StoreProcedure, connection))
                    {
                        if (SQLParameters != null)
                        {
                            sqlCommand.CommandType = CommandType.StoredProcedure;

                            foreach (DBSQLParameter curParam in SQLParameters)
                            {
                                sqlCommand.Parameters.AddWithValue(curParam.Name, curParam.Value ?? DBNull.Value);
                            }
                        }

                        using (var dataAdapter = new SqlDataAdapter(sqlCommand))
                        {
                            dataAdapter.Fill(dataTable);
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                logger.Error(Ex, Ex.Message);
            }

            return dataTable;
        }

        public bool ExecStoreProcedure(string StoreProcedure, List<DBSQLParameter> SQLParameters)
        {
            bool Result = true;

            try
            {
                using (SqlConnection connection = new SqlConnection(dbContext.Database.GetConnectionString()))
                {
                    using (SqlCommand sqlCommand = new SqlCommand(StoreProcedure, connection))
                    {
                        if (SQLParameters != null)
                        {
                            sqlCommand.CommandType = CommandType.StoredProcedure;

                            foreach (DBSQLParameter curParam in SQLParameters)
                            {
                                sqlCommand.Parameters.AddWithValue(curParam.Name, curParam.Value ?? DBNull.Value);
                            }
                        }

                        if (OpenConnection(connection))
                        {
                            sqlCommand.ExecuteNonQuery();
                            CloseConnection(connection);
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                logger.Error(Ex, Ex.Message);
            }

            return Result;
        }

        public List<T> ExecStoreProcedure<T>(string StoreProcedure, List<DBSQLParameter> SQLParameters)
        {
            List<T> objResult = default;
            DataTable _table = new DataTable();

            try
            {
                Log.Information(StoreProcedure);
                using (SqlConnection connection = new SqlConnection(dbContext.Database.GetConnectionString()))
                {
                    using (SqlCommand sqlCommand = new SqlCommand(StoreProcedure, connection))
                    {
                        if (SQLParameters != null)
                        {
                            sqlCommand.CommandType = CommandType.StoredProcedure;

                            foreach (DBSQLParameter curParam in SQLParameters)
                            {
                                sqlCommand.Parameters.AddWithValue(curParam.Name, curParam.Value ?? DBNull.Value);
                            }
                        }

                        using (var dataAdapter = new SqlDataAdapter(sqlCommand))
                        {
                            dataAdapter.Fill(_table);
                        }
                    }
                }

                if (_table != null)
                {
                    objResult = _table.ToList<T>();
                }
            }
            catch (Exception Ex)
            {
                logger.Error(Ex, Ex.Message);
            }

            return objResult;
        }

        public DataTable ExecStoreProcedureDT(string StoreProcedure, List<DBSQLParameter> SQLParameters)
        {
            DataTable dataTable = new DataTable();

            try
            {
                Log.Information(StoreProcedure);
                using (SqlConnection connection = new SqlConnection(dbContext.Database.GetConnectionString()))
                {
                    using (SqlCommand sqlCommand = new SqlCommand(StoreProcedure, connection))
                    {
                        if (SQLParameters != null)
                        {
                            sqlCommand.CommandType = CommandType.StoredProcedure;

                            foreach (DBSQLParameter curParam in SQLParameters)
                            {
                                sqlCommand.Parameters.AddWithValue(curParam.Name, curParam.Value ?? DBNull.Value);
                            }
                        }

                        using (var dataAdapter = new SqlDataAdapter(sqlCommand))
                        {
                            dataAdapter.Fill(dataTable);
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                logger.Error(Ex, Ex.Message);
            }

            return dataTable;
        }

        public List<DBSQLParameter> ExecStoreProcedureOut(string StoreProcedure, List<DBSQLParameter> SQLParameters)
        {
            List<DBSQLParameter> OutSQLParameters = default;

            try
            {
                using (SqlConnection connection = new SqlConnection(dbContext.Database.GetConnectionString()))
                {
                    using (SqlCommand sqlCommand = new SqlCommand(StoreProcedure, connection))
                    {
                        if (SQLParameters != null)
                        {
                            sqlCommand.CommandType = CommandType.StoredProcedure;
                            foreach (DBSQLParameter curParam in SQLParameters)
                            {
                                sqlCommand.Parameters.AddWithValue(curParam.Name, curParam.Value ?? DBNull.Value);
                            }
                        }

                        sqlCommand.ExecuteNonQuery();

                        if (SQLParameters != null)
                        {
                            foreach (DBSQLParameter curParam in SQLParameters.Where(param => param.IsOutParam).ToList())
                            {
                                OutSQLParameters.Add(new DBSQLParameter(curParam.Name, sqlCommand.Parameters[curParam.Name].Value.ChangeType<object>()));
                            }
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                logger.Error(Ex, Ex.Message);
            }

            return OutSQLParameters;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    dbContext.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
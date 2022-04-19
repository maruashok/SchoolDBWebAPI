using Microsoft.EntityFrameworkCore;
using SchoolDBWebAPI.Services.DBModels;
using SchoolDBWebAPI.Services.Extensions;
using SchoolDBWebAPI.Services.Interfaces;
using Serilog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;

namespace SchoolDBWebAPI.Services.SPHelper
{
    public class ProcedureManager : IProcedureManager
    {
        private bool disposed = false;
        private readonly SchoolDBContext dbContext;
        private readonly ILogger logger = Log.ForContext(typeof(ProcedureManager));

        public ProcedureManager(SchoolDBContext _dBContext)
        {
            dbContext = _dBContext;
        }

        private bool OpenConnection(SqlConnection connection)
        {
            bool Result = false;
            try
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }

                Result = connection.State == ConnectionState.Open;
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
                                if (curParam.Name.StartsWith('@'))
                                {
                                    sqlCommand.Parameters.AddWithValue(curParam.Name, curParam.Value ?? DBNull.Value);
                                }
                                else
                                {
                                    sqlCommand.Parameters.AddWithValue($"@{curParam.Name}", curParam.Value ?? DBNull.Value);
                                }
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

        public DataTable ExecuteSelect(string Command, params SqlParameter[] SQLParameters)
        {
            DataTable dataTable = new DataTable();

            try
            {
                using (SqlConnection connection = new SqlConnection(dbContext.Database.GetConnectionString()))
                {
                    using (SqlCommand sqlCommand = new SqlCommand(Command, connection))
                    {
                        if (SQLParameters != null)
                        {
                            foreach (object curParam in SQLParameters)
                            {
                                sqlCommand.Parameters.Add(curParam);
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

        public List<T> ExecuteSelect<T>(string Command, params SqlParameter[] SQLParameters) where T : new()
        {
            List<T> objResult = default;

            try
            {
                using (SqlConnection connection = new SqlConnection(dbContext.Database.GetConnectionString()))
                {
                    using (SqlCommand sqlCommand = new SqlCommand(Command, connection))
                    {
                        sqlCommand.CommandType = CommandType.Text;

                        if (SQLParameters != null)
                        {
                            foreach (SqlParameter curParam in SQLParameters)
                            {
                                sqlCommand.Parameters.Add(curParam);
                            }
                        }

                        if (OpenConnection(connection))
                        {
                            using (SqlDataReader sqlDataReader = sqlCommand.ExecuteReader())
                            {
                                objResult = sqlDataReader.MapToList<T>();
                            }

                            CloseConnection(connection);
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                logger.Error(Ex, Ex.Message);
            }

            return objResult;
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
                                if (curParam.Name.StartsWith('@'))
                                {
                                    sqlCommand.Parameters.AddWithValue(curParam.Name, curParam.Value ?? DBNull.Value);
                                }
                                else
                                {
                                    sqlCommand.Parameters.AddWithValue($"@{curParam.Name}", curParam.Value ?? DBNull.Value);
                                }
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
                                if (curParam.Name.StartsWith('@'))
                                {
                                    sqlCommand.Parameters.AddWithValue(curParam.Name, curParam.Value ?? DBNull.Value);
                                }
                                else
                                {
                                    sqlCommand.Parameters.AddWithValue($"@{curParam.Name}", curParam.Value ?? DBNull.Value);
                                }
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
                                if (curParam.Name.StartsWith('@'))
                                {
                                    sqlCommand.Parameters.AddWithValue(curParam.Name, curParam.Value ?? DBNull.Value);
                                }
                                else
                                {
                                    sqlCommand.Parameters.AddWithValue($"@{curParam.Name}", curParam.Value ?? DBNull.Value);
                                }
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
                                if (curParam.Name.StartsWith('@'))
                                {
                                    sqlCommand.Parameters.AddWithValue(curParam.Name, curParam.Value ?? DBNull.Value);
                                }
                                else
                                {
                                    sqlCommand.Parameters.AddWithValue($"@{curParam.Name}", curParam.Value ?? DBNull.Value);
                                }
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
                                if (curParam.Name.StartsWith('@'))
                                {
                                    sqlCommand.Parameters.AddWithValue(curParam.Name, curParam.Value ?? DBNull.Value);
                                }
                                else
                                {
                                    sqlCommand.Parameters.AddWithValue($"@{curParam.Name}", curParam.Value ?? DBNull.Value);
                                }
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
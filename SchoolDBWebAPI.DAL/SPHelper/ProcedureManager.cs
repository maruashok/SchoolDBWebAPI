using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SchoolDBWebAPI.DAL.DBModels;
using SchoolDBWebAPI.DAL.Extensions;
using SchoolDBWebAPI.DAL.Interfaces;
using Serilog;
using System.Data;
using System.Reflection;

namespace SchoolDBWebAPI.DAL.SPHelper
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
                        sqlCommand.AddParams(GenerateParams(StoreProcedureModel));

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
                        sqlCommand.AddParams(GenerateParams(StoreProcedureModel));

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
                        sqlCommand.AddParams(SQLParameters);

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
                        sqlCommand.AddParams(SQLParameters);

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

        public List<DBSQLParameter> ExecStoreProcedureOut(string StoreProcedure, List<DBSQLParameter> SQLParameters)
        {
            List<DBSQLParameter> OutSQLParameters = default;

            try
            {
                using (SqlConnection connection = new SqlConnection(dbContext.Database.GetConnectionString()))
                {
                    using (SqlCommand sqlCommand = new SqlCommand(StoreProcedure, connection))
                    {
                        sqlCommand.AddParams(SQLParameters);
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

        public Tuple<List<TFirst>, List<TSecond>> ExecStoreProcedureMulResults<TFirst, TSecond>(string StoreProcedure, List<DBSQLParameter> SQLParameters)
        {
            List<TFirst> firstResult = new();
            List<TSecond> secondResult = new();

            try
            {
                using (SqlConnection connection = new SqlConnection(dbContext.Database.GetConnectionString()))
                {
                    if (OpenConnection(connection))
                    {
                        using (SqlCommand sqlCommand = new SqlCommand(StoreProcedure, connection))
                        {
                            sqlCommand.AddParams(SQLParameters);

                            using (var reader = sqlCommand.ExecuteReader())
                            {
                                firstResult = reader.MapToList<TFirst>();
                                reader.NextResult();
                                secondResult = reader.MapToList<TSecond>();
                                reader.Close();
                            }
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                logger.Error(Ex, Ex.Message);
            }

            return new Tuple<List<TFirst>, List<TSecond>>(firstResult, secondResult);
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
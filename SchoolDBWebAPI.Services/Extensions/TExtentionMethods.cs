using SchoolDBWebAPI.Services.SPHelper;
using Serilog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using ILogger = Serilog.ILogger;

namespace SchoolDBWebAPI.Services.Extensions
{
    public static class TExtentionMethods
    {
        private static ILogger logger = Log.ForContext(typeof(TExtentionMethods));

        private static T GetItem<T>(DataRow dr)
        {
            Type temp = typeof(T);
            T obj = Activator.CreateInstance<T>();
            PropertyInfo[] properties = temp.GetProperties(BindingFlags.Instance | BindingFlags.Public);

            foreach (DataColumn column in dr.Table.Columns)
            {
                PropertyInfo propertyInfo = properties.Where(prop => prop.Name.ToLower() == column.ColumnName.ToLower()).FirstOrDefault();

                if (propertyInfo != null && dr[column.ColumnName] != DBNull.Value)
                {
                    propertyInfo.SetValue(obj, dr[column.ColumnName], null);
                }
            }
            return obj;
        }

        public static T ChangeType<T>(this object args)
        {
            try
            {
                return (T)Convert.ChangeType(args, typeof(T));
            }
            catch (Exception)
            {
                return default(T);
            }
        }

        public static List<T> ToList<T>(this DataTable dt)
        {
            List<T> data = new List<T>();
            foreach (DataRow row in dt.Rows)
            {
                data.Add(GetItem<T>(row));
            }
            return data;
        }

        public static T MapToSingle<T>(this SqlDataReader dr)
        {
            Type Entity = typeof(T);
            T RetVal = Activator.CreateInstance<T>();
            Dictionary<string, PropertyInfo> PropDict = new Dictionary<string, PropertyInfo>();

            try
            {
                if (dr != null && dr.HasRows)
                {
                    PropertyInfo[] Props = Entity.GetProperties(BindingFlags.Instance | BindingFlags.Public);
                    PropDict = Props.ToDictionary(item => item.Name.ToUpper(), prop => prop);
                    dr.Read();

                    for (int Index = 0; Index < dr.FieldCount; Index++)
                    {
                        if (PropDict.ContainsKey(dr.GetName(Index).ToUpper()))
                        {
                            PropertyInfo Info = PropDict[dr.GetName(Index).ToUpper()];
                            if ((Info != null) && Info.CanWrite)
                            {
                                var Val = dr.GetValue(Index);
                                Info.SetValue(RetVal, (Val == DBNull.Value) ? null : Val, null);
                            }
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                logger.Error(Ex, Ex.Message);
            }
            return RetVal;
        }

        public static List<T> MapToList<T>(this SqlDataReader dr)
        {
            List<T> RetVal = null;
            Type Entity = typeof(T);
            Dictionary<string, PropertyInfo> PropDict = new Dictionary<string, PropertyInfo>();

            try
            {
                if (dr != null && dr.HasRows)
                {
                    RetVal = new List<T>();
                    PropertyInfo[] Props = Entity.GetProperties(BindingFlags.Instance | BindingFlags.Public);
                    PropDict = Props.ToDictionary(p => p.Name.ToUpper(), p => p);

                    while (dr.Read())
                    {
                        T newObject = Activator.CreateInstance<T>();
                        for (int Index = 0; Index < dr.FieldCount; Index++)
                        {
                            if (PropDict.ContainsKey(dr.GetName(Index).ToUpper()))
                            {
                                var Info = PropDict[dr.GetName(Index).ToUpper()];
                                if ((Info != null) && Info.CanWrite)
                                {
                                    var Val = dr.GetValue(Index);
                                    Info.SetValue(newObject, (Val == DBNull.Value) ? null : Val, null);
                                }
                            }
                        }
                        RetVal.Add(newObject);
                    }
                }
            }
            catch (Exception Ex)
            {
                logger.Error(Ex, Ex.Message);
            }
            return RetVal;
        }

        public static IEnumerable<TFirst> Map<TFirst, TSecond, TKey>
        (
            this List<TFirst> parent,
            List<TSecond> child,
            Func<TFirst, TKey> firstKey,
            Func<TSecond, TKey> secondKey,
            Action<TFirst, IEnumerable<TSecond>> addChildren
        )
        {
            var childMap = child
                .GroupBy(secondKey)
                .ToDictionary(g => g.Key, g => g.AsEnumerable());

            foreach (var item in parent)
            {
                if (childMap.TryGetValue(firstKey(item), out IEnumerable<TSecond> children))
                {
                    addChildren(item, children);
                }
            }

            return parent;
        }

        public static SqlCommand AddParams(this SqlCommand sqlCommand, List<DBSQLParameter> SQLParameters)
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

            return sqlCommand;
        }
    }
}
using Serilog;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using ILogger = Serilog.ILogger;

namespace SchoolDBWebAPI.Extensions
{
    public static class TExtentionMethods
    {
        private static ILogger logger = Log.ForContext(typeof(TExtentionMethods));

        private static T GetItem<T>(DataRow dr)
        {
            Type temp = typeof(T);
            T obj = Activator.CreateInstance<T>();

            foreach (DataColumn column in dr.Table.Columns)
            {
                foreach (PropertyInfo pro in temp.GetProperties())
                {
                    if (pro.Name == column.ColumnName)
                        pro.SetValue(obj, dr[column.ColumnName], null);
                    else
                        continue;
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

        public static T MapToSingle<T>(this DbDataReader dr) where T : new()
        {
            T RetVal = new();
            Type Entity = typeof(T);
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

        public static List<T> MapToList<T>(this DbDataReader dr) where T : new()
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
                        T newObject = new T();
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

        public static T GetDataTableColumnValue<T>(this DataRow row, string columnname)
        {
            try
            {
                object _objColumn = row[columnname];
                return _objColumn.ChangeType<T>();
            }
            catch (Exception)
            {
                try
                {
                    var _objColumn = row.Field<object>(columnname);
                    return ChangeType<T>(_objColumn);
                }
                catch (Exception)
                {
                    return default(T);
                }
            }
        }
    }
}
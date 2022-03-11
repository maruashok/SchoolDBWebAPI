using Serilog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using ILogger = Serilog.ILogger;

namespace SchoolDBWebAPI.Helpers
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
                T item = GetItem<T>(row);
                data.Add(item);
            }
            return data;
        }

        public static dynamic ChangeType(this object args, string typesFullName)
        {
            try
            {
                if (args == DBNull.Value)
                {
                    return default(object);
                }
                else
                {
                    Type t = Type.GetType(typesFullName);
                    return Convert.ChangeType(args, t);
                }
            }
            catch (Exception Ex)
            {
                logger.Error(Ex, Ex.Message);
                return default;
            }
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
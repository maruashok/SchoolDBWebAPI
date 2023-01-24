using Microsoft.Data.SqlClient;
using SchoolDBWebAPI.DAL.Helpers;
using SchoolDBWebAPI.DAL.SPHelper;
using Serilog;
using System.Collections;
using System.Data;
using System.Linq.Expressions;
using System.Reflection;
using ILogger = Serilog.ILogger;

namespace SchoolDBWebAPI.DAL.Extensions
{
    public static class ExtensionMethods
    {
        private static readonly ILogger logger = Log.ForContext(typeof(ExtensionMethods));

        public static bool HasProperty(this object obj, string propertyName)
        {
            return obj.GetType().GetProperty(propertyName) != null;
        }

        public static bool HasProperty(this object obj, string propertyName, out PropertyInfo? propertyInfo)
        {
            propertyInfo = null;
            if (HasProperty(obj, propertyName))
            {
                propertyInfo = obj.GetType().GetProperty(propertyName);
                return true;
            }
            else
            {
                return false;
            }
        }

        public static IList? CreateListOfType(this Type type)
        {
            Type listType = typeof(List<>).MakeGenericType(type);
            return Activator.CreateInstance(listType) as IList;
        }

        public static PropertyInfo? GetProperty(this object obj, string propertyName)
        {
            PropertyInfo? property = default;

            try
            {
                if (obj.HasProperty(propertyName))
                {
                    property = obj.GetType().GetProperty(propertyName);
                }
            }
            catch (Exception Ex)
            {
                logger.Error(Ex.Message, Ex);
            }

            return property;
        }

        public static object? GetPropertyValue(this object obj, string propertyName)
        {
            try
            {
                if (obj.HasProperty(propertyName, out PropertyInfo? property) && property != null)
                {
                    return property.GetValue(obj);
                }
            }
            catch (Exception Ex)
            {
                logger.Error(Ex.Message, Ex);
            }

            return null;
        }

        public static bool GetPropertyValue(this object obj, string propertyName, out object? value)
        {
            value = default;

            try
            {
                if (obj.HasProperty(propertyName, out PropertyInfo? property) && property != null)
                {
                    value = property.GetValue(obj);
                    return true;
                }
            }
            catch (Exception Ex)
            {
                logger.Error(Ex.Message, Ex);
            }

            return false;
        }

        public static void SetPropertyValue(this object obj, string propertyName, object value)
        {
            if (obj.HasProperty(propertyName))
            {
                PropertyInfo? property = obj.GetProperty(propertyName);

                property?.SetValue(obj, value);
            }
        }

        public static string GetPropertyName<T>(this Expression<Func<T, object>> property)
        {
            LambdaExpression lambda = property;
            MemberExpression? memberExpression = null;

            if (lambda.Body is UnaryExpression unaryExpression)
            {
                if (unaryExpression != null)
                {
                    memberExpression = (MemberExpression)(unaryExpression.Operand);
                }
            }
            else
            {
                memberExpression = (MemberExpression)(lambda.Body);
            }

            return (memberExpression?.Member as PropertyInfo)?.Name ?? string.Empty;
        }
    }
}
using SchoolDBWebAPI.DAL.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SchoolDBWebAPI.DAL.SPHelper
{
    public abstract class MultipleResultSet
    {
        public Type GetInnerType(PropertyInfo propertyInfo)
        {
            return propertyInfo.PropertyType.GetGenericArguments().FirstOrDefault();
        }

        public IList CreateListType(PropertyInfo propertyInfo)
        {
            Type? innerType = propertyInfo.PropertyType.GetGenericArguments().FirstOrDefault();

            if (innerType != null)
            {
                return innerType.CreateListOfType();
            }
            else
            {
                return null;
            }
        }
    }
}
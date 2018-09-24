﻿using System;
using System.Linq;
using System.Reflection;
using lib12.Extensions;

namespace lib12.Reflection
{
    /// <summary>
    /// TypeExtension
    /// </summary>
    public static class TypeExtension
    {
        /// <summary>
        /// Determines whether the specified type is numeric or nullable numeric
        /// </summary>
        /// <param name="type">The type to check</param>
        /// <returns></returns>
        public static bool IsTypeNumericOrNullableNumeric(this Type type)
        {
            return type.IsTypeNumeric() || type.IsNullable() && Nullable.GetUnderlyingType(type).IsTypeNumeric();
        }

        /// <summary>
        /// Determines whether the specified type is numeric
        /// </summary>
        /// <param name="type">The type to check</param>
        /// <returns></returns>
        public static bool IsTypeNumeric(this Type type)
        {
            return type.GetTypeInfo().IsPrimitive || type.FullName == "System.Decimal";
        }

        /// <summary>
        /// Determines whether the specified type nullable
        /// </summary>
        /// <param name="type">The type to check</param>
        /// <returns></returns>
        public static bool IsNullable(this Type type)
        {
            return type.GetTypeInfo().IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
        }

        /// <summary>
        /// Determines whether the specified type is static
        /// </summary>
        /// <param name="type">The type to check</param>
        /// <returns></returns>
        public static bool IsStatic(this Type type)
        {
            //according to https://stackoverflow.com/questions/1175888/determine-if-a-type-is-static
            var typeInfo = type.GetTypeInfo();
            return typeInfo.IsAbstract && typeInfo.IsSealed;
        }

        /// <summary>
        /// Gets the attribute decorating given type
        /// </summary>
        /// <param name="type">The type to check</param>
        /// <returns></returns>
        public static T GetAttribute<T>(this Type type) where T : Attribute
        {
            var attribute = type.GetTypeInfo().GetCustomAttributes(typeof(T), false).SingleOrDefault();
            return attribute != null ? (T)attribute : default(T);
        }

        /// <summary>
        /// Gets the default of given type
        /// </summary>
        /// <param name="type">The type to operate</param>
        /// <returns></returns>
        public static object GetDefault(this Type type)
        {
            if (type.GetTypeInfo().IsValueType)
                return Activator.CreateInstance(type);
            else
                return null;
        }

        /// <summary>
        /// Gets the default, parameterless constructor of given type or null if this not exists
        /// </summary>
        /// <param name="type">The type to operate</param>
        /// <returns></returns>
        public static object GetDefaultConstructor(this Type type)
        {
            return type.GetTypeInfo().GetConstructor(Type.EmptyTypes);
        }

        /// <summary>
        /// Gets the property value
        /// </summary>
        /// <param name="type">The source type</param>
        /// <param name="source">The source object to get value from</param>
        /// <param name="propertyName">Name of the property to get value from</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Provided property name cannot be null or empty</exception>
        /// <exception cref="lib12Exception"></exception>
        public static object GetPropertyValue(this Type type, object source, string propertyName)
        {
            if (propertyName.IsNullOrEmpty())
                throw new ArgumentException("Provided property name cannot be null or empty", propertyName);

            var prop = type.GetTypeInfo().GetDeclaredProperty(propertyName);
            if (prop == null)
                throw new lib12Exception(string.Format("Type {0} don't have property named {1}", type.Name, propertyName));

            return prop.GetValue(source, null);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Opsins
{
    /// <summary>
    /// Type扩展
    /// </summary>
    public static class TypeExtensions
    {
        #region 类型判断

        /// <summary>
        /// 数字类型
        /// </summary>
        readonly static List<Type> NumericalType = new List<Type>(){  
            #region 收缩
            typeof(byte),
            typeof(Int16),
            typeof(Int32),
            typeof(Int64),
            typeof(UInt16),
            typeof(UInt32),
            typeof(UInt64),
            typeof(int?),
            typeof(int),
            typeof(float),
            typeof(long),
            typeof(double),
            typeof(decimal)
            #endregion
        };

        /// <summary>
        /// 简单类型
        /// </summary>
        readonly static List<Type> SimpleType = new List<Type>(){
            #region 收缩                                    
            typeof(byte),
            typeof(Int16),
            typeof(Int32),
            typeof(Int64),
            typeof(UInt16),
            typeof(UInt32),
            typeof(UInt64),
            typeof(int?),
            typeof(int),
            typeof(float),
            typeof(long),
            typeof(double),
            typeof(decimal),
            typeof(String),
            typeof(DateTime),
            typeof(DateTime?),
            #endregion
        };

        /// <summary>
        /// 判断是否为数字类型
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns>
        /// 	<c>true</c> 如果是数字类型; 否则, <c>false</c>.
        /// </returns>
        public static bool IsNumericType(this Type type)
        {
            return NumericalType.Contains(type);
        }

        /// <summary>
        /// 判断是否为简单类型[包括：数字、日期、字符、字符串]
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsSimpleType(this Type type)
        {
            return SimpleType.Contains(type) || type.IsEnum;
        }

        /// <summary>
        /// 是否为整数兼容类型
        /// </summary>
        /// <param name="targerType"></param>
        /// <returns></returns>
        public static bool IsIntegerCompatibleType(this Type targerType)
        {
            if ((targerType == typeof(int)) || (targerType == typeof(uint)) || (targerType == typeof(short)) || (targerType == typeof(ushort))
                || (targerType == typeof(long)) || (targerType == typeof(ulong)) || (targerType == typeof(byte)) || (targerType == typeof(sbyte)))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 类型长度是否固定
        /// </summary>
        /// <param name="targetType"></param>
        /// <returns></returns>
        public static bool IsFixLength(this Type targetType)
        {
            if (IsNumericType(targetType)) { return true; }
            if (targetType == typeof(byte[])) { return true; }
            if ((targetType == typeof(DateTime)) || (targetType == typeof(bool))) { return true; }

            return false;
        }

        #endregion

        #region 创建对象

        /// <summary>
        /// 创建类型对象
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="args">参数</param>
        /// <returns>返回一个类型对象</returns>
        public static object CreateInstance(this Type type,params object[] args)
        {
            return Activator.CreateInstance(type, args);
        }

        /// <summary>
        /// 创建类型对象
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="args">参数</param>
        /// <returns>返回一个类型对象</returns>
        public static T CreateInstance<T>(this Type type,params object[] args)
        {
            return (T)type.CreateInstance(args);
        }

        #endregion

        #region 类型获取

        /// <summary>
        /// 获取类型默认值
        /// </summary>
        /// <param name="targetType"></param>
        /// <returns></returns>
        public static object GetDefaultValue(this Type targetType)
        {
            if (IsNumericType(targetType)) { return 0; }
            if (targetType == typeof(string)) { return ""; }
            if (targetType == typeof(bool)) { return false; }
            if (targetType == typeof(DateTime)) { return DateTime.Now; }
            if (targetType == typeof(Guid)) { return System.Guid.NewGuid(); }
            if (targetType == typeof(TimeSpan)) { return System.TimeSpan.Zero; }
            return null;
        }

        /// <summary>
        /// 获取类型默认字符值
        /// </summary>
        /// <param name="targetType"></param>
        /// <returns></returns>
        public static string GetDefaultValueString(this Type targetType)
        {
            if (IsNumericType(targetType)) { return "0"; }
            if (targetType == typeof(string)) { return "\"\""; }
            if (targetType == typeof(bool)) { return "false"; }
            if (targetType == typeof(DateTime)) { return "DateTime.Now"; }
            if (targetType == typeof(Guid)) { return "System.Guid.NewGuid()"; }
            if (targetType == typeof(TimeSpan)) { return "System.TimeSpan.Zero"; }

            return string.Empty;
        }

        /// <summary>
        /// 判断是否有默认的构造函数
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns>
        /// <c>true</c> if parameter less constructor exists; otherwise, <c>false</c>.
        /// </returns>
        [DebuggerStepThrough]
        public static bool HasDefaultConstructor(this Type instance)
        {
            //Checker.Argument.IsNotNull(instance, "instance");

            return instance.GetConstructors(BindingFlags.Instance | BindingFlags.Public).Any(ctor => ctor.GetParameters().Length == 0);
        }

        /// <summary>
        /// 获取所有子类型
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns></returns>
        public static IEnumerable<Type> GetAllChildTypes(this Type type)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies()
                .Where(a => !a.GlobalAssemblyCache)
                .ToList();
            var types = new List<Type>();
            foreach (var assembly in assemblies)
            {
                try
                {
                    types.AddRange(assembly.GetTypes());
                }
                catch { }
            }
            var targetTypes = types.Where(p => type.IsAssignableFrom(p) && type != p);
            return targetTypes;
        }

        /// <summary>
        /// 获取无版本类型名称
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns></returns>
        public static string GetTypeNameWithoutVersion(this Type type)
        {
            if (type.AssemblyQualifiedName != null)
            {
                string[] str = type.AssemblyQualifiedName.Split(',');
                return string.Format("{0},{1}", str[0], str[1]);
            }
            return string.Empty;
        }

        #endregion

        #region 属性操作

        /// <summary>
        /// 获取属性值
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="propertyName">属性名</param>
        /// <param name="o">The o.</param>
        /// <returns></returns>
        public static object GetPropertyValue(this Type type, string propertyName, object o)
        {
            var propertyInfo = type.GetProperty(propertyName, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
            if (propertyInfo != null)
            {
                var value = propertyInfo.GetValue(o, new object[0]);
                return value;
            }
            return null;
        }

        /// <summary>
        /// 设置属性值
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="propertyName">属性名</param>
        /// <param name="o">The o.</param>
        /// <param name="value">属性值</param>
        public static void SetPropertyValue(this Type type, string propertyName, object o, object value)
        {
            var propertyInfo = type.GetProperty(propertyName, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
            propertyInfo.SetValue(o, value, new object[0]);
        }

        #endregion

        #region 对System.Convert.ChangeType进行了增强

        /// <summary>
        /// 对System.Convert.ChangeType进行了增强，支持(0,1)到bool的转换，字符串->枚举、int->枚举、字符串->Type
        /// </summary>
        /// <param name="targetType"></param>
        /// <param name="val"></param>
        /// <returns></returns>

        public static object ChangeType(this Type targetType, object val)
        {
            #region null
            if (val == null)
            {
                return null;
            }
            #endregion

            #region Same Type
            if (targetType == val.GetType())
            {
                return val;
            }
            #endregion

            #region bool 1,0
            if (targetType == typeof(bool))
            {
                return val.ToString() == "1" ? true : false;
            }
            #endregion

            #region Enum
            if (targetType.IsEnum)
            {
                int intVal = 0;
                bool suc = int.TryParse(val.ToString(), out intVal);
                if (!suc)
                {
                    return Enum.Parse(targetType, val.ToString());
                }
                return val;
            }
            #endregion

            #region Type
            if (targetType == typeof(Type))
            {
                return ReflectionHelper.GetType(val.ToString());
            }
            #endregion

            //将double赋值给数值型的DataRow的字段是可以的，但是通过反射赋值给object的非double的其它数值类型的属性，却不行        
            return System.Convert.ChangeType(val, targetType);
        }

        #endregion

        #region 获取类型的完全名称

        /// <summary>
        /// 获取类型的完全名称，如"Assembly.Helpers.TypeHelper,Assembly"
        /// </summary>
        /// <param name="targetType"></param>
        /// <returns></returns>
        public static string GetTypeRegularName(this Type targetType)
        {
            string assName = targetType.Assembly.FullName.Split(',')[0];

            return string.Format("{0},{1}", targetType.ToString(), assName);

        }

        /// <summary>
        /// 获取对象的类型完全名称，如"Assembly.Helpers.TypeHelper,Assembly"
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string GetTypeRegularNameOf(this object obj)
        {
            Type destType = obj.GetType();
            return GetTypeRegularName(destType);
        }
        #endregion

        #region 通过类型的完全名称获取类型

        /// <summary>
        /// 通过类型的完全名称获取类型，regularName如"Assembly.Helpers.TypeHelper,Assembly"
        /// </summary>       
        public static Type GetTypeByRegularName(this string regularName)
        {
            return ReflectionHelper.GetType(regularName);
        }

        #endregion

        #region 其它操作

        public static object[] BuildTypeConstructorParametersFromContainer(this Type type, Func<ParameterInfo, object> getParameterObject)
        {
            ConstructorInfo constructor = type.GetLargestConstructor();
            ParameterInfo[] constructorParameters = constructor.GetParameters();
            List<object> parameters = new List<object>(constructorParameters.Length);

            foreach (ParameterInfo parameter in constructorParameters)
                parameters.Add(getParameterObject(parameter));

            return parameters.ToArray();
        }

        /// <summary>
        /// 获取构造函数
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static ConstructorInfo GetLargestConstructor(this Type type)
        {
            return type.GetLargestConstructor(BindingFlags.Public | BindingFlags.Instance);
        }

        /// <summary>
        /// 获取构造函数
        /// </summary>
        /// <param name="type"></param>
        /// <param name="bindings"></param>
        /// <returns></returns>
        public static ConstructorInfo GetLargestConstructor(this Type type, BindingFlags bindings)
        {
            ConstructorInfo foundConstructor = null;

            foreach (ConstructorInfo constructor in type.GetConstructors(bindings))
                if (foundConstructor == null || constructor.GetParameters().Length > foundConstructor.GetParameters().Length)
                    foundConstructor = constructor;

            return foundConstructor;
        }

        #endregion
    }
}

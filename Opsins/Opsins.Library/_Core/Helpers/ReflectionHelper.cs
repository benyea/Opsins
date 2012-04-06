using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Opsins
{
    /// <summary>
    /// Reflection反射帮助
    /// </summary>
    public static class ReflectionHelper
    {
        #region 获取类型

        /// <summary>
        /// 通过完全限定的类型名来加载对应的类型。typeAndAssemblyName如"iWas.Helper.ReflectionHelper,iWas"。
        /// 如果为系统简单类型，则可以不带程序集名称。
        /// </summary>
        /// <param name="typeAndAssemblyName">类型和程序集名</param>
        /// <returns></returns>
        public static Type GetType(string typeAndAssemblyName)
        {
            string[] names = typeAndAssemblyName.Split(',');
            if (names.Length < 2)
            {
                return Type.GetType(typeAndAssemblyName);
            }

            return GetType(names[0].Trim(), names[1].Trim());
        }

        /// <summary>
        /// 加载assemblyName程序集中的名为typeFullName的类型。assemblyName不用带扩展名，如果目标类型在当前程序集中，assemblyName传入null
        /// </summary>
        /// <param name="typeFullName">类型全名</param>
        /// <param name="assemblyName">程序集名</param>
        /// <returns></returns>
        public static Type GetType(string typeFullName, string assemblyName)
        {
            if (assemblyName == null)
            {
                return Type.GetType(typeFullName);
            }

            //搜索当前域中已加载的程序集
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assembly in assemblies)
            {
                String[] names = assembly.FullName.Split(',');
                if (names[0].Trim() == assemblyName.Trim())
                {
                    return assembly.GetType(typeFullName);
                }
            }

            //加载目标程序集
            Assembly targetAssembly = Assembly.Load(assemblyName);
            if (targetAssembly != null)
            {
                return targetAssembly.GetType(typeFullName);
            }

            return null;
        }

        #endregion

        #region 获取类型完整名

        /// <summary>
        /// 获取类型全名
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetTypeFullName(Type type)
        {
            return string.Format("{0},{1}", type.FullName, type.Assembly.FullName.Split(',')[0]);
        }

        #endregion

        #region 加载继承类实例

        /// <summary>
        /// 将程序集中所有继承自TBase的类型实例化
        /// </summary>
        /// <typeparam name="TBase">基础类型（或接口类型）</typeparam>
        /// <param name="assembly">目标程序集</param>
        /// <returns>TBase实例列表</returns>
        public static IList<TBase> LoadDerivedInstance<TBase>(Assembly assembly)
        {
            Type baseType = typeof(TBase);

            return (from item in assembly.GetTypes()
                    where baseType.IsAssignableFrom(item) && (!item.IsAbstract) && (!item.IsInterface)
                    select (TBase)Activator.CreateInstance(item)).ToList();
        }

        #endregion

        #region LoadDerivedType

        /// <summary>
        /// LoadDerivedType 加载directorySearched目录下所有程序集中的所有派生自baseType的类型
        /// </summary>
        /// <param name="baseType">基类（或接口）类型</param>
        /// <param name="directorySearched">搜索的目录</param>
        /// <param name="searchChildFolder">是否搜索子目录中的程序集</param>
        /// <param name="config">高级配置，可以传入null采用默认配置</param>        
        /// <returns>所有从BaseType派生的类型列表</returns>
        public static IList<Type> LoadDerivedType(Type baseType, string directorySearched, bool searchChildFolder, TypeLoadConfig config)
        {
            if (config == null)
            {
                config = new TypeLoadConfig();
            }

            IList<Type> derivedTypeList = new List<Type>();
            if (searchChildFolder)
            {
                ReflectionHelper.LoadDerivedTypeInAllFolder(baseType, derivedTypeList, directorySearched, config);
            }
            else
            {
                ReflectionHelper.LoadDerivedTypeInOneFolder(baseType, derivedTypeList, directorySearched, config);
            }

            return derivedTypeList;
        }

        #region 类型加载配置

        /// <summary>
        /// 类型加载配置
        /// </summary>
        public class TypeLoadConfig
        {
            #region 构造函数
            public TypeLoadConfig()
            {
                CopyToMemory = false;
                LoadAbstractType = false;
                TargetFilePostfix = ".dll";

            }
            public TypeLoadConfig(bool copyToMem, bool loadAbstract, string postfix)
            {
                CopyToMemory = copyToMem;
                LoadAbstractType = loadAbstract;
                TargetFilePostfix = postfix;
            }
            #endregion

            /// <summary>
            /// CopyToMem 是否将程序集拷贝到内存后加载
            /// </summary>
            public bool CopyToMemory { get; set; }

            /// <summary>
            /// LoadAbstractType 是否加载抽象类型
            /// </summary>
            public bool LoadAbstractType { get; set; }

            /// <summary>
            /// TargetFilePostfix 搜索的目标程序集的后缀名
            /// </summary>
            public string TargetFilePostfix { get; set; }
        }

        #endregion

        #region LoadDerivedTypeInAllFolder
        private static void LoadDerivedTypeInAllFolder(Type baseType, IList<Type> derivedTypeList, string folderPath, TypeLoadConfig config)
        {
            ReflectionHelper.LoadDerivedTypeInOneFolder(baseType, derivedTypeList, folderPath, config);
            string[] folders = Directory.GetDirectories(folderPath);
            if (folders != null)
            {
                foreach (string nextFolder in folders)
                {
                    ReflectionHelper.LoadDerivedTypeInAllFolder(baseType, derivedTypeList, nextFolder, config);
                }
            }
        }
        #endregion

        #region LoadDerivedTypeInOneFolder
        private static void LoadDerivedTypeInOneFolder(Type baseType, IList<Type> derivedTypeList, string folderPath, TypeLoadConfig config)
        {
            string[] files = Directory.GetFiles(folderPath);
            foreach (string file in files)
            {
                if (config.TargetFilePostfix != null)
                {
                    if (!file.EndsWith(config.TargetFilePostfix))
                    {
                        continue;
                    }
                }

                Assembly asm = null;

                #region Asm
                try
                {
                    if (config.CopyToMemory)
                    {
                        //byte[] addinStream = FileHelper.ReadFileReturnBytes(file);
                        //asm = Assembly.Load(addinStream);
                    }
                    else
                    {
                        asm = Assembly.LoadFrom(file);
                    }
                }
                catch (Exception ee)
                {
                    throw;
                }

                if (asm == null)
                {
                    continue;
                }
                #endregion

                Type[] types = asm.GetTypes();

                foreach (Type t in types)
                {
                    if (t.IsSubclassOf(baseType) || baseType.IsAssignableFrom(t))
                    {
                        bool canLoad = config.LoadAbstractType ? true : (!t.IsAbstract);
                        if (canLoad)
                        {
                            derivedTypeList.Add(t);
                        }
                    }
                }
            }

        }
        #endregion
        #endregion

        #region 设置对象属性

        /// <summary>
        /// 如果list中的object具有指定的propertyName属性，则设置该属性的值为propertyValue
        /// </summary>
        /// <param name="objs">对象集合</param>
        /// <param name="propertyName">属性名</param>
        /// <param name="propertyValue">属性值</param>
        public static void SetProperty(IList<object> objs, string propertyName, object propertyValue)
        {
            object[] args = { propertyValue };
            foreach (var o in objs)
            {
                SetProperty(o, propertyName, propertyValue);
            }
        }

        /// <summary>
        /// 如果object具有指定的propertyName属性，则设置该属性的值为propertyValue
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="propertyName">属性名</param>
        /// <param name="propertyValue">属性值</param>
        /// <param name="ignoreError">是否忽略错误，默认为真</param>
        public static void SetProperty(object obj, string propertyName, object propertyValue, bool ignoreError = true)
        {
            Type type = obj.GetType();
            PropertyInfo propertyInfo = type.GetProperty(propertyName);
            if ((propertyInfo == null) || (!propertyInfo.CanWrite))
            {
                if (!ignoreError)
                {
                    string msg = string.Format("The setter of property named '{0}' not found in '{1}'", propertyName, type);
                    throw new Exception(msg);
                }

                return;
            }

            #region 尝试转换类型
            try
            {
                propertyValue = propertyInfo.PropertyType.ChangeType(propertyValue);
            }
            catch
            { }
            #endregion

            object[] args = { propertyValue };
            type.InvokeMember(propertyName,
                              BindingFlags.Public | BindingFlags.IgnoreCase | BindingFlags.Instance |
                              BindingFlags.SetProperty, null, obj, args);
        }

        #endregion

        #region 获取对象属性值

        /// <summary>
        /// 根据指定的属性名获取目标对象该属性的值
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="propertyName">属性名</param>
        /// <returns></returns>
        public static object GetProperty(object obj, string propertyName)
        {
            Type t = obj.GetType();

            return t.InvokeMember(propertyName, BindingFlags.Default | BindingFlags.GetProperty, null, obj, null);
        }

        #endregion

        #region 获取对象字段值

        /// <summary>
        /// 取得目标对象的指定field的值，field可以是private
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="fieldName">字段名</param>
        /// <returns></returns>

        public static object GetFieldValue(object obj, string fieldName)
        {
            Type t = obj.GetType();
            FieldInfo field = t.GetField(fieldName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.Instance);
            if (field == null)
            {
                string msg = string.Format("The field named '{0}' not found in '{1}'.", fieldName, t);
                throw new Exception(msg);
            }

            return field.GetValue(obj);
        }

        #endregion

        #region 设置对象字段值

        /// <summary>
        /// 设置目标对象的指定field的值，field可以是private
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="fieldName">字段名</param>
        /// <param name="val">值</param>
        public static void SetFieldValue(object obj, string fieldName, object val)
        {
            Type t = obj.GetType();
            FieldInfo field = t.GetField(fieldName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.SetField | BindingFlags.Instance);
            if (field == null)
            {
                string msg = string.Format("The field named '{0}' not found in '{1}'.", fieldName, t);
                throw new Exception(msg);
            }

            field.SetValue(obj, val);
        }

        #endregion

        #region 复制对象属性

        /// <summary>
        /// 将source中的属性的值赋给target上想匹配的属性，匹配关系通过propertyMapItemList确定
        /// 当propertyMapItemList=null时，将source中的属性的值赋给target上同名的属性，方便的实现拷贝构造函数
        /// </summary>
        /// <param name="source">源对象</param>
        /// <param name="target">目标对象</param>
        /// <param name="propertyMapItemList">属性映射项列表</param>
        public static void CopyProperty(object source, object target, IList<MapItem<string, string>> propertyMapItemList = null)
        {
            Type sourceType = source.GetType();
            Type targetType = target.GetType();
            PropertyInfo[] sourcePros = sourceType.GetProperties();

            if (propertyMapItemList != null)
            {
                foreach (var item in propertyMapItemList)
                {
                    object val = ReflectionHelper.GetProperty(source, item.Source);
                    ReflectionHelper.SetProperty(target, item.Target, val);
                }
            }
            else
            {
                foreach (PropertyInfo sourceProperty in sourcePros)
                {
                    object val = ReflectionHelper.GetProperty(source, sourceProperty.Name);
                    ReflectionHelper.SetProperty(target, sourceProperty.Name, val);
                }
            }
        }

        #endregion

        #region 获取方法

        /// <summary>
        /// GetAllMethods 获取接口的所有方法信息，包括继承的
        /// </summary>       
        public static IList<MethodInfo> GetAllMethods(params Type[] interfaceTypes)
        {
            foreach (Type interfaceType in interfaceTypes)
            {
                if (!interfaceType.IsInterface)
                {
                    throw new Exception("Target Type must be interface!");
                }
            }

            IList<MethodInfo> list = new List<MethodInfo>();
            foreach (Type interfaceType in interfaceTypes)
            {
                ReflectionHelper.DistillMethods(interfaceType, ref list);
            }

            return list;
        }

        /// <summary>
        /// 提取接口方法
        /// </summary>
        /// <param name="interfaceType">接口类型</param>
        /// <param name="methodList">提取的方法列表</param>
        private static void DistillMethods(Type interfaceType, ref IList<MethodInfo> methodList)
        {
            foreach (MethodInfo meth in interfaceType.GetMethods())
            {
                bool isExist = false;
                foreach (MethodInfo temp in methodList)
                {
                    if ((temp.Name == meth.Name) && (temp.ReturnType == meth.ReturnType))
                    {
                        ParameterInfo[] para1 = temp.GetParameters();
                        ParameterInfo[] para2 = meth.GetParameters();
                        if (para1.Length == para2.Length)
                        {
                            bool same = true;
                            for (int i = 0; i < para1.Length; i++)
                            {
                                if (para1[i].ParameterType != para2[i].ParameterType)
                                {
                                    same = false;
                                }
                            }

                            if (same)
                            {
                                isExist = true;
                                break;
                            }
                        }
                    }
                }

                if (!isExist)
                {
                    methodList.Add(meth);
                }
            }

            foreach (Type superInterfaceType in interfaceType.GetInterfaces())
            {
                ReflectionHelper.DistillMethods(superInterfaceType, ref methodList);
            }
        }

        /// <summary>
        /// 搜索指定类型定义的泛型方法，不包括继承的。
        /// </summary>
        /// <param name="originType"></param>
        /// <param name="methodName"></param>
        /// <param name="argTypes"></param>
        /// <returns></returns>
        public static MethodInfo SearchGenericMethodInType(Type originType, string methodName, Type[] argTypes)
        {
            foreach (MethodInfo method in originType.GetMethods())
            {
                if (method.ContainsGenericParameters && method.Name == methodName)
                {
                    bool succeed = true;
                    ParameterInfo[] paras = method.GetParameters();
                    if (paras.Length == argTypes.Length)
                    {
                        for (int i = 0; i < paras.Length; i++)
                        {
                            if (!paras[i].ParameterType.IsGenericParameter) //跳过泛型参数
                            {
                                if (paras[i].ParameterType.IsGenericType) //如果参数本身就是泛型类型，如IList<T>
                                {
                                    if (paras[i].ParameterType.GetGenericTypeDefinition() != argTypes[i].GetGenericTypeDefinition())
                                    {
                                        succeed = false;
                                        break;
                                    }
                                }
                                else //普通类型的参数
                                {
                                    if (paras[i].ParameterType != argTypes[i])
                                    {
                                        succeed = false;
                                        break;
                                    }
                                }
                            }
                        }
                        if (succeed)
                        {
                            return method;
                        }
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// 查询包括被继承的所有方法，也包括泛型方法
        /// </summary>
        /// <param name="originType"></param>
        /// <param name="methodName"></param>
        /// <param name="argTypes"></param>
        /// <returns></returns>
        public static MethodInfo SearchMethod(Type originType, string methodName, Type[] argTypes)
        {
            MethodInfo meth = originType.GetMethod(methodName, argTypes);
            if (meth != null)
            {
                return meth;
            }

            meth = ReflectionHelper.SearchGenericMethodInType(originType, methodName, argTypes);
            if (meth != null)
            {
                return meth;
            }

            //搜索基类 
            Type baseType = originType.BaseType;
            if (baseType != null)
            {
                while (baseType != typeof(object))
                {
                    MethodInfo target = baseType.GetMethod(methodName, argTypes);
                    if (target != null)
                    {
                        return target;
                    }

                    target = ReflectionHelper.SearchGenericMethodInType(baseType, methodName, argTypes);
                    if (target != null)
                    {
                        return target;
                    }

                    baseType = baseType.BaseType;
                }
            }

            //搜索基接口
            if (originType.GetInterfaces() != null)
            {
                IList<MethodInfo> list = ReflectionHelper.GetAllMethods(originType.GetInterfaces());
                foreach (MethodInfo theMethod in list)
                {
                    if (theMethod.Name != methodName)
                    {
                        continue;
                    }
                    ParameterInfo[] args = theMethod.GetParameters();
                    if (args.Length != argTypes.Length)
                    {
                        continue;
                    }

                    bool correctArgType = true;
                    for (int i = 0; i < args.Length; i++)
                    {
                        if (args[i].ParameterType != argTypes[i])
                        {
                            correctArgType = false;
                            break;
                        }
                    }

                    if (correctArgType)
                    {
                        return theMethod;
                    }
                }
            }

            return null;
        }

        #endregion
    }
}

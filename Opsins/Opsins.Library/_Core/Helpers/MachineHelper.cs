using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Text;

namespace Opsins
{
    /// <summary>
    /// 计算机辅助方法
    /// </summary>
    public static class MachineHelper
    {
        private static PerformanceCounter CpuPerformanceCounter = new PerformanceCounter("Process", "% Processor Time", "Idle");
        private static PerformanceCounter MemPerformanceCounter = new PerformanceCounter("Memory", "% Committed Bytes In Use", "");//Available MBytes

        #region GetMacAddress

        /// <summary>
        /// 获取网卡mac地址
        /// </summary>        
        public static IList<string> GetMacAddress()
        {
            ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
            ManagementObjectCollection moc = mc.GetInstances();
            IList<string> strArr = new List<string>();

            foreach (ManagementObject mo in moc)
            {
                if ((bool)mo["IPEnabled"])
                {
                    strArr.Add(mo["MacAddress"].ToString().Replace(":", ""));
                }
                mo.Dispose();
            }

            return strArr;
        }

        #endregion

        /// <summary>
        /// 是否为当前计算机
        /// </summary>
        /// <param name="macAddress"></param>
        /// <returns></returns>
        public static bool IsCurrentMachine(string macAddress)
        {
            IList<string> addList = MachineHelper.GetMacAddress();
            return addList.Contains(macAddress);
        }

        /// <summary>
        /// 获取计算机性能使用情况
        /// </summary>
        /// <param name="cpuAvailable"></param>
        /// <param name="memoryUsage"></param>
        public static void GetPerformanceUsage(out float cpuAvailable, out float memoryUsage)
        {
            cpuAvailable = MachineHelper.CpuPerformanceCounter.NextValue();
            memoryUsage = MachineHelper.MemPerformanceCounter.NextValue();
        }
    }
}

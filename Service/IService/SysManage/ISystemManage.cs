using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Service.IService
{
    /// <summary>
    /// Service 授权验证模块对应接口
    /// add yuangang by 2016-05-19
    /// </summary>
    public interface ISystemManage : IRepository<Domain.SYS_SYSTEM>
    {
        /// <summary>
        /// 获取系统ID、NAME
        /// </summary>
        /// <param name="systems">用户拥有操作权限的系统</param>
        /// <returns></returns>
        dynamic LoadSystemInfo(List<string> systems);
    }
}
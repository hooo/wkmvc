using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Service.IService;

namespace Service.ServiceImp
{
    class SystemManage: RepositoryBase<Domain.SYS_SYSTEM>, IService.ISystemManage
    {
        /// <summary>
        /// 获取系统ID、NAME
        /// </summary>
        /// <param name="systems">用户拥有操作权限的系统</param>
        /// <returns></returns>
        public dynamic LoadSystemInfo(List<string> systems)
        {
            return Common.JsonConverter.JsonClass(this.LoadAll(p => systems.Any(e => e == p.ID)).Select(p => new { p.ID, p.NAME }).ToList());
        }
        //如果你就一套系统
/// <summary>
/// 获取系统ID、NAME
/// </summary>
/// <param name="systems">用户拥有操作权限的系统</param>
/// <returns></returns>
//public dynamic LoadSystemInfo(List<string> systems)
//        {
//            return Common.JsonConverter.JsonClass(this.LoadAll(null).Select(p => new { p.ID, p.NAME }).ToList());
//        }
    }
}

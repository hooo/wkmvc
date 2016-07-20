using Service.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebPage.Controllers;

namespace WebPage.Areas.SysManage.Controllers
{
    public class ModuleController : BaseController
    {
        #region 声明容器
        /// <summary>
        /// 模块管理 
        /// </summary>
        IModuleManage ModuleManage { get; set; }
        /// <summary>
        /// 权限管理 
        /// </summary>
        IPermissionManage PermissionManage { get; set; }
        /// <summary>
        /// 系统管理 
        /// </summary>
        ISystemManage SystemManage { get; set; }

        #endregion
        // GET: SysManage/Module
        /// <summary>
        /// 模块管理首页加载
        /// </summary>
        [UserAuthorizeAttribute(ModuleAlias ="Module", OperaAction ="View")]
        public ActionResult Index()
        {
            try
            {
                #region 处理查询数据
                string systems = Request.QueryString["System"];
                ViewBag.Search = base.keywords;
                ViewData["System"] = systems;
                ViewData["Systemlist"] = this.SystemManage.LoadSystemInfo(CurrentUser.System_Id);
                #endregion

                return View(BindList(systems));

            }

            catch(Exception e)
            {
                //WriteLog();
                throw e.InnerException;
            }
            
        }

        /// <summary>
        /// 查询模块列表
        /// </summary>
        private object BindList(string systems)
        {
            //预加载所有模块(二级缓存)
            var query = this.ModuleManage.LoadAll(null);

            //系统ID
            if (!string.IsNullOrEmpty(systems))
            {
                query = query.Where(p => p.FK_BELONGSYSTEM == systems);
            }
            else
            {
                //选择全部 查询所有用户所属系统的模块
                query = query.Where(p => this.CurrentUser.System_Id.Any(e => e == p.FK_BELONGSYSTEM));
            }

            //递归排序（无分页）
            var entity = this.ModuleManage.RecursiveModule(query.ToList())
                .Select(p => new
                {
                    p.ID,
                    MODULENAME = GetModuleName(p.NAME, p.LEVELS),
                    p.ALIAS,
                    p.MODULEPATH,
                    p.SHOWORDER,
                    p.ICON,
                    MODULETYPE = ((Common.Enums.enumModuleType)p.MODULETYPE).ToString(),
                    ISSHOW = p.ISSHOW ? "显示" : "隐藏",
                    p.NAME,
                    SYSNAME = p.SYS_SYSTEM.NAME,
                    p.FK_BELONGSYSTEM
                });
            //查询关键字
            if(!string.IsNullOrEmpty(base.keywords))
            {
                entity = entity.Where(p => p.NAME.Contains(keywords));
            }

            return Common.JsonConverter.JsonClass(entity);

        }

        private object GetModuleName(string name, decimal? level)
        {
            if (level > 0)
            {
                string nbsp = "&nbsp;&nbsp;";
                for (int i=0;i<level;i++)
                {
                    nbsp += "&nbsp;&nbsp;";

                }

                name = nbsp + "  |--" + name;
            }
            return name;
        }

    }
}
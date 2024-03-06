using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using informix.BLL;
using informix.DBUtility;

namespace informix.Web.Controllers
{
    public class HomeController : Controller
    {
        #region 首页视图
        //主页面
        public ActionResult main()
        {
            return View();
        }

        //左侧目录
        public ActionResult left()
        {
            return View();
        }
        //页头
        public ActionResult top()
        {          
            ViewData["UserName"] = new Linq2xmlHelper().GetChildElValueByElIdValue("session", "temp", "UserName");
            return View();
        }
        //页尾
        public ActionResult footer()
        {
            return View();
        }
        //主内容页
        public ActionResult index(string id)
        {
            if (id == null)
            {
                id = "2";
            }
            ViewData["UserName"] =new  Linq2xmlHelper().GetChildElValueByElIdValue("session", "temp", "UserName");
            //ViewData["lastTime"] = new Online().GetOnline(new Linq2xmlHelper().GetChildElValueByElIdValue("session", "temp", "UserId"))[0].LoginTime.ToString("yyyy-MM-dd HH:mm:ss");
            ViewData["location"] = new Shared().GetLocationById(id);
           
            return View();
        }
        //页面不存在
        public ActionResult error()
        {
            return View();
        }
        #endregion
        /// <summary>
        /// 获取模块列表
        /// </summary>
        public void ModuleList()
        {
            string json = new Module().GetModuleList();
            Response.Write(json);
            Response.Flush();
            Response.End();
        }
    }
}
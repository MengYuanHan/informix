using informix.BLL;
using informix.DBUtility;
using System;
using System.IO;
using System.Text;
using System.Web.Mvc;

namespace informix.Web.Controllers {
    public class SharedController : Controller {

        private static string errorLogPath = new ConfigHelper().GetConfigPath("ErrorLog");

        //当前用户
        public string CurrentUserId() => Session["CurrentUserId"].ToString();
        //当前登录用户是否为超级管理员
        public bool IsSuperPower() => (bool)Session["IsSuperPower"];
        public ActionResult location()
        {
            return View();
        }
        //验证权限有效性
        public string GetIsVaild(string node,string id)
        {
            Linq2xmlHelper lx = new Linq2xmlHelper();
            return lx.GetXmlAttributeValueById(node,id, "used");
        }
        /// <summary>
        /// 验证当前用户是否有修改用户信息权限
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public string IsVerifyAuthority(string userId)
        {
            string result = new BLL.User().IsVerifyAuthority(userId).ToString().ToLower();
            return result;
        }
        /// <summary>
        /// 获取当前登录用户UserId
        /// </summary>
        /// <returns></returns>
        //public string GetCurrentUserId()
        //{
        //    return new BLL.User().GetCurrentUserId();
        //}
        /// <summary>
        /// 将操作写入日志
        /// </summary>
        /// <param name="title"></param>
        /// <param name="contents"></param>
        /// <param name="type"></param>
        public void WriteOperLog(string title, string contents, string type)
        {
            OperateLog operLog = new OperateLog();
            operLog.CreateOperateLog(title, contents, type);
        }

        //将异常写入记事本
        public static void WriteErorrLog(Exception ex)
        {
            string msg = "";
            try
            {
                if (ex == null)
                    return;                         //ex = null 返回

                DateTime dt = DateTime.Now;         // 设置日志时间  
                string time = dt.ToString("yyyy-mm-dd HH:mm:ss"); //年-月-日 时：分：秒  
                string logName = time + ".log";       //日志名称  
                string logPath = errorLogPath;     //日志存放路径  
                string log = logPath + logName;    //路径+名称  

                StringBuilder sb = new StringBuilder();
                sb.Append(time);
                sb.Append(ex.Message);
                sb.Append("异常信息：" + ex.ToString());
                sb.Append("异常堆栈：" + ex.StackTrace.ToString());
                sb.Append("/r/n-----------------");

                msg = sb.ToString();

                if (Directory.Exists(logPath) == false)
                {
                    Directory.CreateDirectory(logPath);
                }
                FileStream fs = new FileStream(log, FileMode.Append);
                StreamWriter sw = new StreamWriter(fs);
                sw.Write(msg);
                //清空缓冲区               
                sw.Flush();
                //关闭流               
                sw.Close();
                sw.Dispose();
                fs.Close();
                fs.Dispose();
            }
            catch (Exception e)
            {
                string datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                msg = "[" + datetime + "]写入日志出错：" + e.Message;
                FileStream fs = new FileStream(errorLogPath+""+datetime+".log", FileMode.Append);
                StreamWriter sw = new StreamWriter(fs);
                sw.Write(msg);
                //清空缓冲区               
                sw.Flush();
                //关闭流               
                sw.Close();
                sw.Dispose();
                fs.Close();
                fs.Dispose();
            }
        }
    }
}
using informix.BLL;
using informix.DBUtility;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Web; 
using System.Web.Mvc;

namespace informix.Web.Controllers
{
    public class SystemMaintenanceController : Controller
    {
        private static readonly string appLogPath = new ConfigHelper().GetConfigPath("ErrorLog");
        public static Linq2xmlHelper lx =new Linq2xmlHelper();
        #region 系统维护视图
        //系统运行状态
        public ActionResult RunningState(string id)
        {
            if (id == null)
            {
                id = "2";
            }

            ViewData["location"] = new Shared().GetLocationById(id);
            return View();
        }
        //日志分析
        public ActionResult Analytics(string  id)
        {
            if (id == null)
            {
                id = "2";
            }
            ViewData["location"] = new Shared().GetLocationById(id);
            return View();
        }
        //系统出错日志
        public ActionResult ErrorLog(string id)
        {
            if (id == null)
            {
                id = "2";
            }

            ViewData["location"] = new Shared().GetLocationById(id);
            return View();
        }
        //系统环境配置
        public ActionResult EnvironConfig(string id)
        {
            if (id == null)
            {
                id = "2";
            }

            ViewData["location"] = new Shared().GetLocationById(id);
            return View();
        }
        #endregion

        #region 日志分析

        
        public string GetFileInfo()
        {
            //FileInfo.Exists：获取指定文件是否存在；
            //FileInfo.Name，FileInfo.Extension：获取文件的名称和扩展名；
            //FileInfo.FullName：获取文件的全限定名称（完整路径）；
            //FileInfo.Directory：获取文件所在目录，返回类型为DirectoryInfo；
            //FileInfo.DirectoryName：获取文件所在目录的路径（完整路径）；
            //FileInfo.Length：获取文件的大小（字节数）；
            //FileInfo.IsReadOnly：获取文件是否只读；
            //FileInfo.Attributes：获取或设置指定文件的属性，返回类型为FileAttributes枚举，可以是多个值的组合
            DirectoryInfo folder = new DirectoryInfo(appLogPath);
            string json = null;
            StringBuilder sb = new StringBuilder();
            foreach (FileInfo file in folder.GetFiles("*.log"))
            {
                sb.Append("{\"FileName\":\"" + file.Name + "\",\"FileType\":\"" + file.Extension + "\",FileDir:\"" + file.FullName.Replace("\\", "/") + "\",\"FileLen\":" + file.Length + ",\"CreateTime\":\"" + file.CreationTime.ToString("yyyy-MM-dd HH:mm:ss") + "\",\"LaseTime\":\"" + file.LastWriteTime.ToString("yyyy-MM-dd HH:mm:ss") + "\",\"AccessTime\":\"" + file.LastAccessTime.ToString("yyyy-MM-dd HH:mm:ss") + "\"},");
            }
            if (sb.ToString() != "")
            {
                json = sb.ToString().Substring(0, sb.ToString().Length - 1);
            }
            else
            {
                json = "]";
            }
            return json;
        }
        /// <summary>
        /// 删除日志文件
        /// </summary>
        /// <returns></returns>
        public string DeleteLog()
        {
            string dir = Request.Params["dir"].Trim();
            dir = dir.Replace("/", "\\");
            //调用批处理
            ExecBatCommand(p => { p(@"del " + dir + ""); p("exit 0"); });
            return "删除完成！";
        }
        /// <summary>
        /// 打开日志文件
        /// </summary>
        public void OpenLog()
        {
            string dir = Request.Params["dir"].Trim();
            dir = dir.Replace("/", "\\");
            //调用批处理
            ExecBatCommand(p => { p(@"start notepad " + dir + ""); p("exit 0"); });
        }
        /// <summary>
        /// 打开控制台执行拼接完成的批处理命令字符串
        /// </summary>
        /// <param name="inputAction">需要执行的命令委托方法：每次调用 <paramref name="inputAction"/> 中的参数都会执行一次</param>
        private static void ExecBatCommand(Action<Action<string>> inputAction)
        {
            Process pro = null;
            StreamWriter sIn = null;
            StreamReader sOut = null;

            try
            {
                pro = new Process();
                pro.StartInfo.FileName = "cmd.exe";
                pro.StartInfo.UseShellExecute = false;
                pro.StartInfo.CreateNoWindow = true;
                pro.StartInfo.RedirectStandardInput = true;
                pro.StartInfo.RedirectStandardOutput = true;
                pro.StartInfo.RedirectStandardError = true;

                pro.OutputDataReceived += (sender, e) => Console.WriteLine(e.Data);
                pro.ErrorDataReceived += (sender, e) => Console.WriteLine(e.Data);

                pro.Start();
                sIn = pro.StandardInput;
                sIn.AutoFlush = true;

                pro.BeginOutputReadLine();
                inputAction(value => sIn.WriteLine(value));

                pro.WaitForExit();
            }
            finally
            {
                if (pro != null && !pro.HasExited)
                    pro.Kill();

                if (sIn != null)
                    sIn.Close();
                if (sOut != null)
                    sOut.Close();
                if (pro != null)
                    pro.Close();
            }
        }
        #endregion

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using informix.BLL;
using informix.DBUtility;
using System.Xml;
using System.Collections;
using informix.Web.Dto;

namespace informix.Web.Controllers {
    public class AccountController : SharedController {

        public static ResponseHelper rh = new ResponseHelper();
        public static Linq2xmlHelper lx = new Linq2xmlHelper();
        private string configPath = new ConfigHelper().GetConfigPath("config");


        // GET: Login
        public ActionResult Login()
        {
            return View();
        }
        /// <summary>
        /// 登陆或退出
        /// </summary>
        /// <param name="record"></param>
        /// <returns></returns>
        public string LogInOrOut(UserDto record)
        {
            //请求前台参数
            string userName = record.userName;
            string password = record.password;
            string result = null;
            string type = "用户登录";

            string attValue = lx.GetXmlAttributeValue("UserId", "used");

            rh.m_values = new User().GetUserInfoByUserNameAndPwd(userName, password);

            //如果存在该用户
            if (rh.m_values != null)
            {
                string userId = rh.m_values["UserId"].ToString();
                bool IsEnable = rh.m_values["State"].ToString() == "0" ? true : false;
                if (attValue == "true")
                {
                    //存储用户编号和用户名
                    lx.UpdateItemAttribute("UserId", "UserId", "", userId, 1);
                    lx.UpdateItemAttribute("UserName", "UserName", "", userName, 1);
                }

                if (IsEnable)
                {
                    result = "1";
                    Online online = new Online();
                    if (online.IsExist(userName, "true") == true)
                    {
                        //修改为false
                        online.Update(userName);
                    }

                    Session[userName] = Guid.NewGuid().ToString();
                    Session["CurrentUserId"] = userId;
                    //添加一条在线记录
                    online.Onlined(DateTime.Now, userId, userName, "true", Session[userName].ToString());

                    WriteOperLog("用户登录成功", string.Concat("用户:", userName, ",登录成功"), type);
                }
                else
                {
                    result = "2";

                    WriteOperLog("用户登录失败", string.Concat("用户:", userName, ",登录失败,帐号已停用,请联系管理员解除"), type);
                }
            }
            else
            {
                result = "0";
                WriteOperLog("用户登录失败", string.Concat("用户:", userName, ",登录失败,用户名或密码错误"), type);
            }
            return result;
        }
    }
}
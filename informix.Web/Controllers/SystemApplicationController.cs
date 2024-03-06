using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using informix.BLL;
using informix.DBUtility;
using System.Data;
using System.Web.Script.Serialization;
using informix.Model.Entities;
using System.EnterpriseServices;
using System.Collections;
using System.Xml;
using System.IO;
using System.Text;
using System.Diagnostics;
using informix.Web.Dto;

namespace informix.Web.Controllers {
    public class SystemApplicationController : SharedController {
        public static Linq2xmlHelper lx = new Linq2xmlHelper();

        #region 系统应用视图

        //应用列表
        public ActionResult AppList(string id)
        {
            if (id == null)
            {
                id = "2";
            }

            ViewData["location"] = new Shared().GetLocationById(id);
            return View();
        }
        //应用模块
        public ActionResult AppModule(string id)
        {
            if (id == null)
            {
                id = "2";
            }
            ViewData["location"] = new Shared().GetLocationById(id);
            return View();
        }
        //用户资料
        public ActionResult UserInfo(string id)
        {
            if (id == null)
            {
                id = "2";
            }
            ViewData["location"] = new Shared().GetLocationById(id);
            return View();
        }
        //用户修改
        public ActionResult UserUpdate()
        {
            return View();
        }
        ///应用字段
        public ActionResult AppField(string id)
        {
            if (id == null)
            {
                id = "2";
            }
            ViewData["location"] = new Shared().GetLocationById(id);
            return View();
        }
        //部门资料
        public ActionResult DeptInfo(string id)
        {
            if (id == null)
            {
                id = "2";
            }
            ViewData["location"] = new Shared().GetLocationById(id);
            return View();
        }
        //角色资料
        public ActionResult RoleInfo(string id)
        {
            if (id == null)
            {
                id = "2";
            }
            ViewData["location"] = new Shared().GetLocationById(id);
            return View();
        }
        //消息日志
        public ActionResult OperateLog(string id)
        {
            if (id == null)
            {
                id = "2";
            }
            ViewData["location"] = new Shared().GetLocationById(id);
            return View();
        }
        //在线用户
        public ActionResult UserOnline(string id)
        {
            if (id == null)
            {
                id = "2";
            }
            ViewData["location"] = new Shared().GetLocationById(id);
            return View();
        }
        #endregion

        #region 用户资料
        /// <summary>
        /// 获取所有用户列表
        /// </summary>
        /// <returns>json</returns>
        public string GetUser()
        {
            string json = null;
            int currentPage = Convert.ToInt32(Request.Params["currentPage"].Trim());
            int pageCount = Convert.ToInt32(Request.Params["pageCount"].Trim());

            string userId = this.CurrentUserId();
            try
            {
                //序列化列表
                JavaScriptSerializer js = new JavaScriptSerializer();
                json = js.Serialize(new BLL.User().GetUser(currentPage, pageCount));
            }
            catch (Exception ex)
            {
                json = ex.Message;
            }

            return json;
        }
        public string UpdateState()
        {
            string userId = Request.Params["userId"].ToString();
            string state = Request.Params["state"].ToString();
            string result = null;

            //userId
            string UserId =lx.GetChildElValueByElIdValue("session", "temp", "UserId");
            //authority节点属性id的值
            string authorityId_startup = lx.GetXmlAttributeValueById("authority", "startup", "value");
            string authorityId_blockup = lx.GetXmlAttributeValueById("authority", "blockup", "value");
            //判断是否有权限
            string authority_startup = new Authority().GetAuthByUserId(UserId, authorityId_startup);
            string authority_blockup = new Authority().GetAuthByUserId(UserId, authorityId_blockup);
            if (authority_startup != "0" && authority_blockup != "0")
            {
                result = new BLL.User().UpdateState(userId, state);
                if (state == "0")
                {
                    WriteOperLog("用户操作成功", string.Concat("用户id=", userId, ",已经被启用"), "用户启用或停用");
                }
                else
                {
                    WriteOperLog("用户操作成功", string.Concat("用户id=", userId, ",已经被停用"), "用户启用或停用");
                }
                result = state;
            }
            else
            {
                result = "2";
                WriteOperLog("用户操作失败", string.Concat("当前用户没有权限"), "用户启用或停用");
            }
            return result;
        }
        /// <summary>
        /// 获取用户记录数
        /// </summary>
        /// <returns></returns>
        public string GetUserCount()
        {
            return new User().GetUserCount().ToString();
        }
        /// <summary>
        /// 添加用户
        /// </summary>
        /// <returns></returns>
        public string AddUser(UserDto record)
        {
            bool result;
            //添加用户资料
            Sys_User obj = new Sys_User();
            obj.UserName = record.userName;
            obj.Password = record.password;
            obj.DeptId = Convert.ToInt32(record.deptId);
            obj.Sex = record.sex;
            obj.RealName = record.realName;
            obj.Title = record.title;
            obj.Phone = record.phone;
            obj.Fax = record.fax;
            obj.Email = record.email;
            obj.Province = record.province;
            obj.City = record.city;
            obj.Address = record.address;
            obj.QQ = record.QQ;
            obj.NickName = record.nickName;
            //string UserId = lx.GetChildElValueByElIdValue("session", "temp", "UserId");
            string userId=CurrentUserId();
            obj.CreateUserId = Convert.ToInt32(userId);
            obj.CreateDate = DateTime.Now;
            obj.State = "0";
            bool result_user = new BLL.User().AddUser(obj);

            //添加角色
            Sys_User_Role sur = new Sys_User_Role();
            sur.RoleId = Convert.ToInt32(record.roleId);
            sur.UserId = new BLL.User().GetMaxUserId();
            string role = new Role().GetRoleByRoleId(sur.RoleId);
            sur.Descript = "用户" + obj.UserName + "被分配为角色" + role + "";

            bool result_userrole = new UserRole().AddUserRole(sur);

            if (result_user == true && result_userrole == true)
            {
                result = true;
                WriteOperLog("添加用户成功", string.Concat("用户:", obj.UserName, ",添加成功," + sur.Descript), "添加用户");
            }
            else
            {
                result = false;
                WriteOperLog("添加用户失败", string.Concat("用户:", obj.UserName, ",添加失败," + sur.Descript), "添加用户");
            }
            return result.ToString().ToLower();
        }
        public string GetUserByUserId()
        {
            string json = null;
            string userId = Request.Params["UserId"].Trim();
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            json = serializer.Serialize(new User().GetUserByUserId(Convert.ToInt32(userId)));

            return json;
        }
        public string GetDepartment()
        {
            string json = null;
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            json = serializer.Serialize(new Department().GetDepartment());

            return json;
        }
        public string UpdateUser()
        {
            bool result = false;

            Sys_User su = new Sys_User();
            string UserId = Request.Params["UserId"].Trim();
            su.UserId = Convert.ToInt32(UserId);
            su.UserName = Request.Params["UserName"].Trim();
            su.Password = Request.Params["Password"].Trim();
            su.DeptId = Convert.ToInt32(Request.Params["DeptId"].Trim());
            su.Sex = Request.Params["Sex"].Trim();
            su.RealName = Request.Params["RealName"].Trim();
            su.Title = Request.Params["Title"].Trim();
            su.Phone = Request.Params["Phone"].Trim();
            su.Fax = Request.Params["Fax"].Trim();
            su.Email = Request.Params["Email"].Trim();
            su.Province = Request.Params["Province"].Trim();
            su.City = Request.Params["City"].Trim();
            su.Address = Request.Params["Address"].Trim();
            su.QQ = Request.Params["QQ"].Trim();
            su.NickName = Request.Params["NickName"].Trim();
           // string CurrentUserId =lx.GetChildElValueByElIdValue("session", "temp", "UserId");
            string userId = CurrentUserId();
            su.CreateUserId = Convert.ToInt32(userId);
            su.CreateDate = DateTime.Now;
            su.State = "0";

            bool result_updateuser = new BLL.User().UpdateUser(su);

            string roleId = Request.Params["RoleId"].Trim();
            bool result_role = false;

            StringBuilder sb = new StringBuilder();
            //判断用户角色
            if (roleId != "]")
            {
                //反序列化 将json字符串转换成list对象
                JavaScriptSerializer Serializer = new JavaScriptSerializer();
                List<Hashtable> htList = Serializer.Deserialize<List<Hashtable>>(roleId);

                //循环选中数组权限
                foreach (Hashtable list in htList)
                {
                    //当选中添加权限时，则添加到权限模块表中
                    if (list.Contains("add") == true)
                    {
                        //判断是否存在该权限
                        if (new UserRole().IsUserRole(UserId, (string)list["add"]) == 0)
                        {
                            Sys_User_Role sur = new Sys_User_Role();
                            sur.UserId = Convert.ToInt32(UserId);
                            sur.RoleId = Convert.ToInt32((string)list["add"]);
                            string role = new Role().GetRoleByRoleId(sur.RoleId);
                            sur.Descript = "用户" + su.UserName + "被分配为角色" + role + "";
                            new UserRole().AddUserRole(sur);
                            sb.Append(sur.Descript);
                        }
                        result_role = true;
                    }
                    else
                    {
                        new UserRole().DelUserRoleByUR(UserId, (string)list["del"]);
                        result_role = true;
                        sb.Append("用户Id=" + UserId + ",删除角色Id=" + (string)list["del"]);
                    }
                }
            }
            else
            {
                result_role = true;
            }

            if (result_updateuser == true && result_role == true)
            {
                result = true;
                WriteOperLog("修改用户成功", string.Concat("用户:", su.UserName, ",修改成功," + sb.ToString()), "修改用户");
            }
            else
            {
                result = false;
                WriteOperLog("修改用户失败", string.Concat("用户:", su.UserName, ",修改失败"), "修改用户");
            }
            return result.ToString().ToLower() ;
        }
        #endregion

        #region 角色权限
        public void GetRoleAuth()
        {
            string json = new Role().GetRoleAuth();
            Response.Write(json);
            Response.Flush();
            Response.End();
        }
        public string GetRole()
        {
            string json = null;
            int currentPage = Convert.ToInt32(Request.Params["currentPage"].Trim());
            int pageCount = Convert.ToInt32(Request.Params["pageCount"].Trim());
            try
            {
                //序列化列表
                JavaScriptSerializer js = new JavaScriptSerializer();
                json = js.Serialize(new Role().GetRole(currentPage, pageCount));
            }
            catch (Exception ex)
            {
                json = ex.Message;
            }
            return json;
        }
        public string GetRoleCount()
        {
            return new Role().GetUserCount().ToString();
        }
        public string AddRole(Sys_Role record)
        {
            bool result;

            bool result_r = new Role().AddRole(record);

            Sys_Authority sa = new Sys_Authority();
            sa.Authority = record.Role;
            sa.Type = "系统权限";
            sa.Remark = "";
            bool result_a = new Authority().AddAuthority(sa);

            Sys_Auth_Role sar = new Sys_Auth_Role();
            sar.AuthorityId = new Authority().GetMaxAuthorityId();
            sar.RoleId = new Role().GetMaxRoleId();
            bool result_ar = new AuthRole().AddAuthRole(sar);
            if (result_r == true && result_ar == true && result_a == true)
            {
                result = true;
                WriteOperLog("添加角色成功", string.Concat("添加角色:" + record.Role + "成功"), "角色权限");
            }
            else
            {
                result = false;
                WriteOperLog("添加角色失败", string.Concat("添加角色:" + record.Role + "失败"), "角色权限");
            }
            return result.ToString().ToLower();
        }
        public string GetModule()
        {
            string json = null;
            try
            {
                JavaScriptSerializer js = new JavaScriptSerializer();
                json = js.Serialize(new Module().GetModule());
            }
            catch (Exception ex)
            {
                json = ex.Message;
            }
            return json;
        }
        public string GetAuthByRoleId()
        {
            string roleId = Request.Params["roleId"].Trim();
            string json = new Module().GetAuthByRoleId(roleId);
            return json;
        }
        public string DeleteRole()
        {
            bool result;
            string type = "角色权限";
            string roleId = Request.Params["roleId"].Trim();
            string role = Request.Params["role"].Trim();
            bool result_role = new Role().DelRole(roleId);
            bool result_userrole = new UserRole().DelUserRole(roleId);
            bool result_authrole = new AuthRole().DelAuthRole(roleId);
            //获取权限编号
            int authorityId = new Authority().GetAuthorityId(role);
            bool result_authmodule = new AuthModule().DelAuthModuleByAuthId(authorityId);
            bool result_authority = new Authority().DeleteAuthority(role);

            if (result_role == true && result_userrole == true && result_authrole == true && result_authority == true && result_authmodule == true)
            {
                result = true;
                WriteOperLog("删除角色成功", string.Concat("添加角色:" + role + "成功"), type);
            }
            else
            {
                result = false;
                WriteOperLog("删除角色失败", string.Concat("添加角色:" + role + "失败"), type);
            }
            return result.ToString().ToLower();
        }
        /// <summary>
        /// 添加或删除所属角色的权限
        /// </summary>
        /// <returns></returns>
        public string ConfirmAuthority()
        {
            bool result = false;
            string roleId = Request.Params["roleId"].Trim();
            string authority = Request.Params["authority"].Trim();
            string type = "角色权限";

            //反序列化 将json字符串转换成list对象
            JavaScriptSerializer Serializer = new JavaScriptSerializer();
            List<Hashtable> htList = Serializer.Deserialize<List<Hashtable>>(authority);

            //循环选中数组权限
            foreach (Hashtable list in htList)
            {
                int authorityId = new AuthRole().GetAuthorityIdByRoleId(Convert.ToInt32(roleId));
                //当选中添加权限时，则添加到权限模块表中
                if (list.Contains("add") == true)
                {
                    //判断是否存在该权限
                    if (new AuthRole().IsAuthRole(roleId, (string)list["add"]) == 0)
                    {
                        Sys_Auth_Module sam = new Sys_Auth_Module();
                        sam.AuthorityId = authorityId;
                        sam.ModuleId = Convert.ToInt32((string)list["add"]);
                        new AuthModule().Add(sam);
                        WriteOperLog("角色授予权限成功", string.Concat("添加角色Id=" + roleId + "授予权限Id=" + (string)list["add"] + "成功"), type);
                    }
                    result = true;
                }
                else
                {
                    new AuthModule().DelAuthModuleByModuleId(authorityId, Convert.ToInt32((string)list["del"]));
                    result = true;
                    WriteOperLog("角色授予权限成功", string.Concat("删除角色Id=" + roleId + "授予权限Id=" + (string)list["add"] + "成功"), type);
                }
            }
            return result.ToString().ToLower();
        }
        public string GetRoles()
        {
            string json = null;
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            json = serializer.Serialize(new Role().GetRole());
            return json;
        }
        public string GetSurplusRole()
        {
            string UserId = Request.Params["UserId"].Trim();
            string json = new Role().GetSurplusRole(UserId);
            return json;
        }
        public string GetCurrentUserRole()
        {
            string UserId = Request.Params["UserId"].Trim();
            string json = new Role().GetCurrentUserRole(UserId);
            return json;
        }
        #endregion

        #region 应用列表
        public string GetAppList()
        {
            string json = null;
            IList<Sys_Application> list = new Application().GetApp();

            JavaScriptSerializer js = new JavaScriptSerializer();
            json = js.Serialize(list);
            return json;
        }
        //获取应用列表
        public string GetApp()
        {
            string json = null;
            int currentPage = Convert.ToInt32(Request.Params["currentPage"].Trim());
            int pageCount = Convert.ToInt32(Request.Params["pageCount"].Trim());
            IList<Sys_Application> list = new Application().GetApp(currentPage, pageCount);

            JavaScriptSerializer js = new JavaScriptSerializer();
            json = js.Serialize(list);
            return json;
        }

        /// <summary>
        /// 添加应用
        /// </summary>
        /// <returns>添加应用结果</returns>
        public string AddApp()
        {
            string AppName = Request.Params["appName"].ToString().Trim();
            string decription = Request.Params["decription"].ToString().Trim();
            string url = Request.Params["appurl"].ToString().Trim();
            string orders = Request.Params["apporders"].ToString().Trim();
            string icon = Request.Params["icon"].ToString().Trim();

            string result = null;
            string type = "应用列表";
            //添加到应用表
            Sys_Application sa = new Sys_Application();
            sa.AppName = AppName;
            sa.AppDesc = decription;
            if (string.IsNullOrEmpty(url))
            {
                sa.Url = "http://";
            }
            else
            {
                sa.Url = url;
            }
            if (string.IsNullOrEmpty(orders))
            {
                sa.OrderBy = 1;
            }
            else
            {
                sa.OrderBy = Convert.ToInt32(orders);
            }
            bool result_app = new Application().AddApp(sa);

            //模块中加入根节点
            Sys_Module sm = new Sys_Module();
            sm.AppId = new Application().GetMaxApplicationId();
            sm.ParentId = 0;
            sm.Module = AppName;
            sm.Directory = "/Home/index";
            sm.Code = new Module().GetCode();
            sm.Issystem = 0;
            sm.Close = 0;
            sm.Status = "inactive";
            sm.Icon = icon;
            bool result_module = new Module().AddAppModule(sm);

            //添加到权限模块
            Sys_Auth_Module sam = new Sys_Auth_Module();
            sam.AuthorityId = 125;
            sam.ModuleId = new Module().GetMaxModuleId();
            bool result_auth_mod = new AuthModule().Add(sam);

            if (result_app == true && result_module == true && result_auth_mod == true)
            {
                result = "1";
                WriteOperLog("添加应用成功", string.Concat("添加应用" + AppName + "成功"), type);
            }
            else
            {
                result = "0";
                WriteOperLog("添加应用失败", string.Concat("添加应用" + AppName + "失败"), type);
            }
            return result;
        }
        /// <summary>
        /// 删除应用
        /// </summary>
        /// <returns>删除结果标识</returns>
        public string DeleteApp()
        {
            string applicationId = Request.Params["ApplicationId"].ToString();
            string result = null;
            string type = "应用列表";

            //确认配置是否有效
            string val = lx.GetXmlAttributeValue("delapp", "used");
            //userId
            string userId =lx.GetChildElValueByElIdValue("session", "temp", "UserId");
            //authority节点属性id的值
            string authorityId = lx.GetXmlAttributeValue("delapp", "value");
            //判断是否有权限
            string authority = new Authority().GetAuthByUserId(userId, authorityId);

            if (authority != "0")
            {
                bool result_app = new Application().DelAppById(applicationId);
                int moduleId = new Module().GetModuleIdByAppId(applicationId);
                bool result_authmodule = new AuthModule().DeleteAuthModule(moduleId);
                bool result_module = new Module().DelByAppId(applicationId);
                if (result_app == true && result_authmodule == true && result_module == true)
                {
                    result = "1";
                    WriteOperLog("删除应用成功", string.Concat("删除应用Id=" + applicationId + "成功"), type);
                }
                else
                {
                    result = "2";
                    WriteOperLog("删除应用失败", string.Concat("删除应用Id=" + applicationId + "失败"), type);
                }
            }
            else
            {
                result = "0";
                WriteOperLog("删除应用失败", string.Concat("没有权限"), type);
            }
            return result;
        }
        /// <summary>
        /// 按应用编号查询应用
        /// </summary>
        /// <param name="applicationId">应用编号</param>
        /// <returns>查询结果集[应用名称、应用模块、路径]</returns>
        public string GetAppById(string applicationId)
        {
            string json = new Application().GetAppById(applicationId);
            return json;
        }
        /// <summary>
        /// 查询应用记录数
        /// </summary>
        /// <returns></returns>
        public string GetAppCount()
        {
            return new Application().GetAppCount().ToString();
        }
        #endregion

        #region 应用模块
        /// <summary>
        /// 获取模块记录数
        /// </summary>
        /// <returns></returns>
        public string GetAppModuleCount()
        {
            return new Module().GetAppModuleCount().ToString();
        }
        /// <summary>
        /// 更新状态
        /// </summary>
        /// <returns></returns>
        public string UpdateAppModuleStatus()
        {

            string moduleId = Request.Params["moduleId"].ToString();
            string close = Request.Params["close"].ToString();
            string result = new Module().UpdateAppModuleStatus(moduleId, close);

            if (result == "0")
            {
                WriteOperLog("应用模块启用", string.Concat("应用模块已启用"), "应用模块");
            }
            else
            {
                WriteOperLog("应用模块禁用", string.Concat("应用模块已禁用"), "应用模块");
            }
            return result;
        }
        /// <summary>
        /// 添加应用模块
        /// </summary>
        /// <returns></returns>
        public string AddAppModule()
        {
            string code = Request.Params["code"].ToString().Trim();
            string application = Request.Params["application"].ToString().Trim();
            string module = Request.Params["module"].ToString().Trim();
            string subModule = Request.Params["subModule"].ToString().Trim();
            string directory = Request.Params["directory"].ToString().Trim();
            string close = Request.Params["close"].ToString().Trim();
            string IsSystem = Request.Params["IsSystem"].ToString().Trim();
            //string icon = Request.Params["icon"].ToString().Trim();
            string status = Request.Params["status"].ToString().Trim();
            string type = "应用模块";
            //模块中加入根节点
            Sys_Module sm = new Sys_Module();

            sm.ParentId = Convert.ToInt32(module);
            sm.AppId = Convert.ToInt32(application);
            sm.Module = subModule;
            sm.Directory = directory;
            sm.Code = code;
            sm.Issystem = Convert.ToInt32(IsSystem);
            sm.Close = Convert.ToInt32(close);
            sm.Status = status;
            sm.Icon = "";

            bool result = new Module().AddAppModule(sm);
            if (result == true)
            {
                WriteOperLog("应用模块添加成功", string.Concat("应用模块" + subModule + "已添加"), type);
            }
            else
            {
                WriteOperLog("应用模块添加失败", string.Concat("应用模块" + subModule + "添加失败"), type);
            }

            return result.ToString().ToLower();
        }
        /// <summary>
        /// 删除模块
        /// </summary>
        /// <returns></returns>
        public string deleteAppModuleByModuleId()
        {
            string moduleId = Request.Params["ModuleId"].ToString().Trim();
            return new Module().DeleteAppModuleByModuleId(moduleId).ToString().ToLower();
        }
        /// <summary>
        /// 根据Id查询模块信息
        /// </summary>
        /// <returns></returns>
        public string GetAppModuleByAppId()
        {
            string json = null;
            string moduleId = Request.Params["ModuleId"].ToString().Trim();
            DataTable dt = new Module().GetAppModuleById(moduleId);

            JavaScriptSerializer js = new JavaScriptSerializer();
            List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();
            foreach (DataRow dr in dt.Rows)//每一行信息，新建一个Dictionary<string,object>,将该行的每列信息加入到字典
            {
                Dictionary<string, object> result = new Dictionary<string, object>();
                foreach (DataColumn dc in dt.Columns)
                {
                    result.Add(dc.ColumnName, dr[dc].ToString());
                }
                list.Add(result);
            }
            json = js.Serialize(list);
            return json;
        }
        public string UpdateAppModule()
        {
            string moduleId = Request.Params["ModuleId"].ToString().Trim();
            string code = Request.Params["code"].ToString().Trim();
            string application = Request.Params["AppId"].ToString().Trim();
            string module = Request.Params["ParentId"].ToString().Trim();
            string subModule = Request.Params["subModule"].ToString().Trim();
            string directory = Request.Params["Directory"].ToString().Trim();
            string close = Request.Params["Close"].ToString().Trim();
            string IsSystem = Request.Params["IsSystem"].ToString().Trim();
            //string icon = Request.Params["icon"].ToString().Trim();
            string status = Request.Params["Status"].ToString().Trim();
            string type = "应用模块";
            //模块中加入根节点
            Sys_Module sm = new Sys_Module();
            sm.ModuleId = Convert.ToInt32(moduleId);
            sm.ParentId = Convert.ToInt32(module);
            sm.AppId = Convert.ToInt32(application);
            sm.Module = subModule;
            sm.Directory = directory;
            sm.Code = code;
            sm.Issystem = Convert.ToInt32(IsSystem);
            sm.Close = Convert.ToInt32(close);
            sm.Status = status;
            sm.Icon = "";

            bool result = new Module().UpdateAppModule(sm);
            if (result == true)
            {
                WriteOperLog("应用模块更新成功", string.Concat("应用模块" + subModule + "更新成功"), type);
            }
            else
            {
                WriteOperLog("应用模块更新失败", string.Concat("应用模块" + subModule + "更新失败"), type);
            }
            return result.ToString().ToLower();
        }
        #endregion

        #region 部门资料

        /// <summary>
        /// 获取部门记录数
        /// </summary>
        /// <returns></returns>
        public string GetDeptCount()
        {
            return new Department().GetDeptCount().ToString();
        }
        public string GetDept(string currentPage, string pageCount)
        {
            string json = null;
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            json = serializer.Serialize(new Department().GetDepartment(Convert.ToInt32(currentPage), Convert.ToInt32(pageCount)));

            return json;
        }
        public string AddDept()
        {
            bool result;
            string type = "部门资料";
            Sys_Department sd = new Sys_Department();
            sd.Dept = Request.Params["Dept"].Trim();
            sd.DeptLevel = Convert.ToInt32(Request.Params["deptlevel"].Trim());
            sd.ChildCount = Convert.ToInt32(Request.Params["childcount"].Trim());
            result = new Department().AddDept(sd);
            if (result == true)
            {
                WriteOperLog("部门添加成功", string.Concat("部门:" + sd.Dept + "添加成功"), type);
            }
            else
            {
                WriteOperLog("部门添加失败", string.Concat("部门:" + sd.Dept + "添加失败"), type);
            }
            return result.ToString().ToLower();
        }
        public string deleteDept()
        {
            bool result;
            string type = "部门资料";
            string DeptId = Request.Params["DeptId"].Trim();
            result = new Department().deleteDept(DeptId);
            if (result == true)
            {
                WriteOperLog("部门删除成功", string.Concat("部门Id=" + DeptId + "删除成功"), type);
            }
            else
            {
                WriteOperLog("部门删除失败", string.Concat("部门Id=" + DeptId + "删除失败"), type);
            }
            return result.ToString().ToLower();
        }
        #endregion

        #region 在线用户
        public string GetOnline()
        {
            string json = null;
            int currentPage = Convert.ToInt32(Request.Params["currentPage"].Trim());
            int pageCount = Convert.ToInt32(Request.Params["pageCount"].Trim());
            try
            {
                //序列化列表
                JavaScriptSerializer js = new JavaScriptSerializer();
                json = js.Serialize(new Online().GetOnline(currentPage, pageCount));
            }
            catch (Exception ex)
            {
                json = ex.Message;
            }

            return json;
        }
        /// <summary>
        /// 获取在线用户记录数
        /// </summary>
        /// <returns></returns>
        public string GetOnlineCount()
        {
            return new Online().GetOnlineCount().ToString();
        }
        /// <summary>
        /// 下线
        /// </summary>
        /// <returns></returns>
        public string Offline()
        {
            string result = "0";
            string guid = Request.Params["guid"].Trim();

            string userId = SessionHelper.GetSession("CurrentUserId").ToString();
            //获取当前登录用户的GUID
            IList<Sys_User> user = new User().GetUserByUserId(Convert.ToInt32(userId));
            if (user.Count > 0)
            {
                foreach (var p in user)
                {
                    //如果guid值相等,则强制下线并且删除session
                    if (guid == SessionHelper.GetSession(p.UserName).ToString())
                    {                        
                        Session.Remove(p.UserName);
                        result = "1";
                    }
                }
            }
            new Online().Offline(guid);
            return result;
        }
        #endregion

        #region 日志消息
        /// <summary>
        /// 获取操作日志并分页
        /// </summary>
        /// <returns></returns>
        public string GetOperlog()
        {
            string json = null;
            int currentPage = Convert.ToInt32(Request.Params["currentPage"].Trim());
            int pageCount = Convert.ToInt32(Request.Params["pageCount"].Trim());

            try
            {
                //序列化列表
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                json = serializer.Serialize(new OperateLog().GetOperlog(currentPage, pageCount));
            }
            catch (Exception ex)
            {
                json = ex.Message;
            }
            return json;
        }
        /// <summary>
        /// 获取操作日志记录数
        /// </summary>
        /// <returns></returns>
        public string GetOperlogCount()
        {
            return new OperateLog().GetOperlogCount().ToString();
        }
        #endregion


    }
}
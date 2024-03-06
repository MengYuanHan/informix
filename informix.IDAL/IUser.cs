using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using informix.Model.Entities;

namespace informix.IDAL
{
    public interface IUser
    {
        /// <summary>
        /// 检查密码
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="userPwd">密码</param>
        /// <returns></returns>
        decimal ValidateUserInfo(string userName, string userPwd);
        /// <summary>
        /// 查询用户是否启用
        /// </summary>
        /// <param name="userId">用户编号</param>
        /// <returns></returns>
        string IsEnable (string userId);
        /// <summary>
        /// 根据用户名和密码获取userId
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="userPwd">密码</param>
        /// <returns></returns>
        int GetUserId(string userName, string userPwd);
        /// <summary>
        /// 根据用户名和密码获取用户信息
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="userPwd">密码</param>
        /// <returns></returns>
        SortedDictionary<string, string> GetUserInfoByUserNameAndPwd(string userName, string userPwd);
        /// <summary>
        /// 根据用户编号更新用户状态
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        string UpdateState(string userId,string state);
        /// <summary>
        /// 用户分页
        /// </summary>
        /// <param name="currentPage">当前页</param>
        /// <param name="pageCount">数量</param>
        /// <returns></returns>
        IList<Sys_User> GetUser(int currentPage,int pageCount);
        /// <summary>
        /// 获取用户数量
        /// </summary>
        /// <returns></returns>
        int GetUserCount();
        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="user">用户对象</param>
        /// <returns></returns>
        bool AddUser(Sys_User user);
        /// <summary>
        /// 获取用户编号最大值
        /// </summary>
        /// <returns></returns>
        int GetMaxUserId();
        /// <summary>
        /// 根据用户编号获取用户信息
        /// </summary>
        /// <param name="userId">用户编号</param>
        /// <returns></returns>
        IList<Sys_User> GetUserByUserId(int userId);
        /// <summary>
        /// 修改用户信息
        /// </summary>
        /// <param name="user">用户对象</param>
        /// <returns></returns>
        bool UpdateUser(Sys_User user);
        /// <summary>
        /// 判断当前用户是否拥有修改其他用户权限
        /// </summary>
        /// <param name="userId">用户编号</param>
        /// <returns></returns>
        bool IsVerifyAuthority(string userId);
        /// <summary>
        /// 获取当前登录用户权限
        /// </summary>
        /// <returns></returns>
        string GetCurrentUserId();
        /// <summary>
        /// 查询用户信息
        /// </summary>
        /// <param name="userName">登录名</param>
        /// <param name="userPwd">密码</param>
        /// <returns></returns>
        IList<Sys_User> GetUserInfo(string userName, string userPwd);
        /// <summary>
        /// 获取当前登录用户信息
        /// </summary>
        /// <returns></returns>
        IList<Sys_User> GetCurrentUser();
    }
}

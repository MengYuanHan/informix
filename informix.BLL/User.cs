using informix.IDAL;
using informix.Model.Entities;
using System.Collections.Generic;

namespace informix.BLL
{
    public class User {
        private readonly IUser _dal = DALFactory.DataAccess.CreateUser();
        /// <summary>
        /// 根据登录名和密码验证用户
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="userPwd"></param>
        /// <returns></returns>
        public decimal ValidateUserInfo(string userName, string userPwd) => _dal.ValidateUserInfo(userName, userPwd);
        /// <summary>
        /// 根绝UserId检验用户是否启用
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public string IsEnable(string userId) => _dal.IsEnable(userId);

        /// <summary>
        /// 根据登录名和密码获取UserId
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="userPwd">密码</param>
        /// <returns></returns>
        public int GetUserId(string userName, string userPwd) => _dal.GetUserId(userName, userPwd);
        /// <summary>
        /// 根据登录名和密码获取用户信息
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="userPwd">密码</param>
        /// <returns></returns>
        public SortedDictionary<string, string> GetUserInfoByUserNameAndPwd(string userName, string userPwd) => _dal.GetUserInfoByUserNameAndPwd(userName, userPwd);
        /// <summary>
        /// 获取用户信息并分页
        /// </summary>
        /// <param name="currentPage"></param>
        /// <param name="pageCount"></param>
        /// <returns></returns>
        public IList<Sys_User> GetUser(int currentPage, int pageCount) => _dal.GetUser(currentPage, pageCount);
        /// <summary>
        /// 根据UserId更新用户状态
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public string UpdateState(string userId, string state) => _dal.UpdateState(userId, state);
        /// <summary>
        /// 获取用户数
        /// </summary>
        /// <returns></returns>
        public int GetUserCount() => _dal.GetUserCount();
        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool AddUser(Sys_User user) => _dal.AddUser(user);

        /// <summary>
        /// 获取UserId最大值
        /// </summary>
        /// <returns></returns>
        public int GetMaxUserId() => _dal.GetMaxUserId();
        /// <summary>
        /// 根据UserId获取用户信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public IList<Sys_User> GetUserByUserId(int userId) => _dal.GetUserByUserId(userId);
        /// <summary>
        /// 更新用户信息
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool UpdateUser(Sys_User user) => _dal.UpdateUser(user);

        /// <summary>
        /// 验证当前用户是否有修改其他账户权限
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public bool IsVerifyAuthority(string userId) => _dal.IsVerifyAuthority(userId);
        /// <summary>
        /// 获取当前用户UserId
        /// </summary>
        /// <returns></returns>
        public string GetCurrentUserId() => _dal.GetCurrentUserId();
        /// <summary>
        /// 根据登录名和密码查询用户信息
        /// </summary>
        /// <param name="userName">登录名</param>
        /// <param name="userPwd">密码</param>
        /// <returns></returns>
        public IList<Sys_User> GetUserInfo(string userName, string userPwd) => _dal.GetUserInfo(userName, userPwd);
        /// <summary>
        /// 获取当前登录用户信息
        /// </summary>
        /// <returns></returns>
        public IList<Sys_User> GetCurrentUser() => _dal.GetCurrentUser();
    }
}

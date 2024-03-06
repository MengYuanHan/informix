using informix.IDAL;
using informix.Model.Entities;

namespace informix.BLL
{
    public class UserRole{
        private readonly IUserRole _dal = DALFactory.DataAccess.CreateUserRole();
        /// <summary>
        /// 添加用户角色
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public  bool AddUserRole(Sys_User_Role userRole) => _dal.AddUserRole(userRole);
        /// <summary>
        /// 删除用户角色
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public bool DelUserRole(string roleId) => _dal.DelUserRole(roleId);
        /// <summary>
        /// 判断用户角色
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public int IsUserRole(string userId, string roleId) => _dal.IsUserRole(userId, roleId);
        /// <summary>
        /// 删除用户角色
        /// </summary>
        /// <param name="userId">用户编号</param>
        /// <param name="roleId">角色编号</param>
        /// <returns></returns>
        public bool DelUserRoleByUR(string userId, string roleId) => _dal.DelUserRoleByUR(userId, roleId);
    }
}

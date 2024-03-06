using informix.IDAL;
using informix.Model.Entities;

namespace informix.BLL
{
    public class AuthRole{
        private readonly IAuthRole _dal = DALFactory.DataAccess.CreateAuthRole();
        /// <summary>
        /// 删除权限角色
        /// </summary>
        /// <param name="roleId">角色编号</param>
        /// <returns></returns>
        public bool DelAuthRole(string roleId) => _dal.DelAuthRole(roleId);
        /// <summary>
        /// 判断是否拥有权限角色
        /// </summary>
        /// <param name="roleId">角色编号</param>
        /// <param name="moduleId">模块编号</param>
        /// <returns></returns>
        public int IsAuthRole(string roleId,string moduleId) => _dal.IsAuthRole(roleId, moduleId);
        /// <summary>
        /// 添加权限角色
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool AddAuthRole(Sys_Auth_Role authRole) => _dal.AddAuthRole(authRole);
        /// <summary>
        /// 根据角色编号获取权限
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public int GetAuthorityIdByRoleId(int roleId) => _dal.GetAuthorityIdByRoleId(roleId);
    }
}

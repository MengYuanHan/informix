using informix.IDAL;
using informix.Model.Entities;

namespace informix.BLL
{
    public class Authority{
        private readonly IAuthority _dal = DALFactory.DataAccess.CreateAuthority();
        /// <summary>
        /// 根据用户编号、权限编号获取权限
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="authorityId"></param>
        /// <returns></returns>
        public string GetAuthByUserId(string userId, string authorityId) => _dal.GetAuthByUserId(userId, authorityId);
        /// <summary>
        /// 添加权限
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool AddAuthority(Sys_Authority authority) => _dal.AddAuthority(authority);
        /// <summary>
        /// 删除权限
        /// </summary>
        /// <param name="authority">权限名称</param>
        /// <returns></returns>
        public bool DeleteAuthority(string authority) => _dal.DeleteAuthority(authority);
        /// <summary>
        /// 获取权限编号最大值
        /// </summary>
        /// <returns></returns>
        public int GetMaxAuthorityId() => _dal.GetMaxAuthorityId();
        /// <summary>
        /// 获取权限编号
        /// </summary>
        /// <param name="authority">权限名称</param>
        /// <returns></returns>
        public int GetAuthorityId(string authority) => _dal.GetAuthorityId(authority);
    }
}

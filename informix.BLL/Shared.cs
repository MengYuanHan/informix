using informix.IDAL;

namespace informix.BLL
{
    public class Shared{
        private readonly IShared _dal = DALFactory.DataAccess.CreateShared();
        /// <summary>
        /// 获取当前位置
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string GetLocationById(string id) => _dal.GetLocationById(id);
    }
}

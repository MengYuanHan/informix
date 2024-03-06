using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using informix.IDAL;

namespace informix.BLL {
    public class Log {
        private readonly ILog dal = informix.DALFactory.DataAccess.CreateLog();


    }
}

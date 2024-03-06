using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace informix.Model.Entities {
    public class Sys_Auth_Module {
        public virtual int AuthModuleId { get; set; }
        public virtual int AuthorityId { get; set; }
        public virtual int ModuleId { get; set; }
    }
}

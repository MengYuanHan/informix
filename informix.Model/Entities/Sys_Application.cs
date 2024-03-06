using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace informix.Model.Entities {
    public class Sys_Application {
        /// <summary>
        /// 应用编号
        /// </summary>
        public virtual int AppId { get; set; }
        /// <summary>
        /// 应用名称
        /// </summary>
        public virtual string AppName { get; set; }
        /// <summary>
        /// 应用描述
        /// </summary>
        public virtual string AppDesc { get; set; }
        /// <summary>
        /// 应用url
        /// </summary>
        public virtual string Url { get; set; }
        /// <summary>
        /// 应用排序
        /// </summary>
        public virtual int OrderBy { get; set; }

        //N个Module属于一个Application
        //public virtual IList<Sys_Module> Module { get; set; }
        //public Sys_Application()
        //{
        //    Modules = new List<Sys_Module>();
        //}
    }

}

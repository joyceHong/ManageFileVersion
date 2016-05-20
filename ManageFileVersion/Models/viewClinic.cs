using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ManageFileVersion.Models
{
    public class viewClinic
    {
        /// <summary>
        /// 診所索引編號
        /// </summary>
        public string clinicIndex
        {
            get;
            set;
        }

        /// <summary>
        /// 診所名稱
        /// </summary>
        public string clinicName
        {
            get;
            set;
        }

        public string clinicAddr
        {
            get;
            set;
        }
    }
}
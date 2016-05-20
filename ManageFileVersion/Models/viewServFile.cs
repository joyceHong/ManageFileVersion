using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ManageFileVersion.Models
{
    public class viewServFile
    {
        /// <summary>
        /// 流水編號
        /// </summary>
        public int ikey
        {
            get;
            set;
        }

        /// <summary>
        /// 檔案名稱
        /// </summary>
        public string FileName
        {
            get;
            set;
        }
      

        public string FilePath
        {
            get;
            set;
        }

        /// <summary>
        /// 版本
        /// </summary>
        public string FileVersion
        {
            get;
            set;
        }

        public string FileType
        {
            get;
            set;
        }


        /// <summary>
        /// 檔案大小
        /// </summary>
        public int FileSize
        {
            get;
            set;
        }

        public DateTime? FileDate
        {
            get;
            set;
        }        
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ManageFileVersion.Models
{
    public class viewAnnounce
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
        public string fileName
        {
            get;
            set;
        }

        /// <summary>
        /// 檔案版本
        /// </summary>
        public string fileVersion
        {
            get;
            set;
        }

        /// <summary>
        /// 更新時間
        /// </summary>
        public DateTime updateTime
        {
            get;
            set;
        }

        /// <summary>
        /// 更新時間文字型態
        /// </summary>
        public string strUpdateTime
        {
            get;
            set;
        }


        /// <summary>
        /// 主旨
        /// </summary>
        public string context
        {
            get;
            set;
        }

        /// <summary>
        /// 下戴文件
        /// </summary>
        public string downloadFile
        {
            get;
            set;
        }

        /// <summary>
        /// 測試檔案的路徑
        /// </summary>
        public string testFilePath
        {
            get;
            set;
        }

        /// <summary>
        /// 是否釋出版本0:未釋出 1:已釋出
        /// </summary>
        public int isRelease
        {
            get;
            set;
        }

        /// <summary>
        /// 產品分類
        /// </summary>
        public string productType
        {
            get;
            set;
        }
    }
}
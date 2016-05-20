using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ManageFileVersion.Models
{
    public class viewBug
    {

        /// <summary>
        /// 唯一流水編號
        /// </summary>
        public int ikey
        {
            get;
            set;
        }

        /// <summary>
        /// test_note 的唯一碼
        /// </summary>
        public int announcementIkey
        {
            get;
            set;
        }

        /// <summary>
        /// 提報者
        /// </summary>
        public string uid
        {
            get;
            set;
        }

        /// <summary>
        /// 提報者
        /// </summary>
        public string userName
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

        /// <summary>
        /// 產品類別
        /// </summary>
        public string productType
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
        /// 版本
        /// </summary>
        public string fileVersion
        {
            get;
            set;
        }

        /// <summary>
        /// 回報測試的訊息
        /// </summary>
        public string bugContext
        {
            get;
            set;
        }

        /// <summary>
        /// 測試檔案路徑
        /// </summary>
        public string bugDocumentFile
        {
            get;
            set;
        }

        /// <summary>
        /// 診所編號
        /// </summary>
        public string clinicIndex
        {
            get;
            set;
        }


        /// <summary>
        /// 建立日期時間
        /// </summary>
        public string createdTime
        {
            get;
            set;
        }

        /// <summary>
        /// 預估完成日
        /// </summary>
        public string estimatedDate
        {
            get;
            set;
        }

        /// <summary>
        /// 實際完成日期
        /// </summary>
        public string completeDate
        {
            get;
            set;
        }

        public string status
        {
            get;
            set;
        }


        /// <summary>
        /// 備註
        /// </summary>
        public string remark
        {
            get;
            set;
        }


        /// <summary>
        /// 等級
        /// </summary>
        public int  grade
        {
            get;
            set;
        }

        /// <summary>
        /// 統計筆數值
        /// </summary>
        public int totalCount
        {
            get;
            set;
        }
    }
}
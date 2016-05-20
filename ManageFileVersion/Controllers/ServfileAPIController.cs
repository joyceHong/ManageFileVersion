using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ManageFileVersion.Models
{
    
    public class ServfileAPIController : ApiController
    {

        ServfileModel objModel = new ServfileModel();
       
        /// <summary>
        /// 顯示所有檔案版本名稱
        /// </summary>
        /// <returns></returns>
        public IEnumerable<viewServFile>Get()
        {
            try
            {
                return objModel.getAllFiles();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 只顯示最近日期
        /// </summary>
        /// <param name="showLastWeekDate"></param>
        /// <returns></returns>
        public IEnumerable<viewServFile> Get(string showLastWeekDate)
        {
            try
            {
                return objModel.getLastNewWeek();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ManageFileVersion.Models
{
    public class ServfileModel
    {
        I_cpnewsEntities entityObj = new I_cpnewsEntities();
        TCSEntities tcsDB = new TCSEntities();
        /// <summary>
        /// 取得所有版本的檔案名稱
        /// </summary>
        /// <returns></returns>
        public IEnumerable<viewServFile> getAllFiles()
        {
            try
            {
                var files = from c in entityObj.servfile_notice                          
                            join d in entityObj.servfile on c.檔名 equals d.檔名
                            select new viewServFile()
                            {
                                ikey = c.Skey,
                                FileName = c.顯示檔名,
                                FileSize = d.大小,
                                FileType = c.類別,
                                FileVersion = d.版本,
                                FilePath = c.存放路徑,
                            };
                //var files = from c in entityObj.servfile
                //            join d in entityObj.servfile_notice
                //            on c.檔名.Trim() equals d.檔名 into g
                //            from e in g.DefaultIfEmpty()
                //            select new viewServFile()
                //            {
                //                 ikey = e.Skey,
                //                 FileName =e.顯示檔名,
                //                 FileSize = c.大小,
                //                 FileType=e.類別,
                //                 FileVersion = c.版本,
                //                 FilePath = e.存放路徑,
                //            };

                return files.ToList(); 
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 取得兩週內公告的檔案版本
        /// </summary>
        /// <returns></returns>
        public IEnumerable<viewServFile> getLastNewWeek()
        {
            try
            {
                var baselineDate = DateTime.Now.AddDays(-14);

                var files = from c in entityObj.servfile
                            join d in entityObj.servfile_notice
                            on c.檔名.Trim() equals d.檔名 into g
                            from e in g.DefaultIfEmpty()
                            where e.更新日期.Value >= baselineDate
                            select new viewServFile()
                            {
                                FileName = e.顯示檔名,
                                FileSize = c.大小,
                                FileType = e.類別,
                                FileVersion = c.版本,
                                FilePath=e.存放路徑,
                                FileDate = e.更新日期
                            };

                //var files = from c in entityObj.servfile_notice
                //            where c.更新日期.Value >= baselineDate
                //            select new tempServFile()
                //            {
                //                FileName = c.檔名,
                //                FileDate = c.更新日期,
                //                FileType = c.類別,
                //                FileVersion = c.版本
                //            };

                return files.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 檢查帳號是否通過
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public int isPassLogin(string userName, string password)
        {
            try
            {
                object objResult = tcsDB.IdentifyLogin3(userName, password, "").FirstOrDefault();
                return (int)objResult;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 黃總要儘快只想要針對ICD10的部份直接回報
        /// </summary>
        /// <returns></returns>
        public viewServFile getICD10Type()
        {
            try
            {
                //var query = (i_cpNewDb.servfile_notice).Where(q => q.檔名.Contains("COOPER.EXE")).Select();
                var query = from q in entityObj.bug_type
                            where q.type.Contains("ICD10")
                            select new viewServFile()
                            {
                                ikey = q.ikey,
                                FileName = q.type,
                                FileVersion = q.fileversion,
                            };

                return query.AsQueryable<viewServFile>().FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public string getFileVersion(string productType)
        {
            try
            {
                var query = (from q in entityObj.bug_type
                            where q.type.Contains(productType)
                            select new viewServFile()
                            {
                                FileVersion = q.fileversion,
                            }).FirstOrDefault();

                return query.FileVersion;
            }
            catch (Exception ex)
            {
                return null;
                //throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 列出所有的產品類別
        /// </summary>
        /// <returns></returns>
        public IEnumerable<viewServFile> getProductType()
        {
            try
            {

                var query = from q in entityObj.bug_type
                            select new viewServFile()
                            {
                                ikey = q.ikey,
                                FileName = q.type,
                                FileVersion = q.fileversion,
                            };
                  
                return query.AsQueryable<viewServFile>();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public viewClinic getClinicName(string clinicIndex)
        {
            try
            {
                 TCSEntities tcsDB = new TCSEntities();
                viewClinic viewClinicObj = (from clinic in tcsDB.asuser
                            where clinic.索引編號.Contains(clinicIndex)
                            select new viewClinic()
                            {
                                clinicIndex = clinic.索引編號,
                                clinicName = clinic.客戶名稱
                            }).AsQueryable().FirstOrDefault();

                return viewClinicObj;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 列出所有診所名單
        /// </summary>
        /// <param name="clinicName"></param>
        /// <returns></returns>
        public IEnumerable<viewClinic> listClinics(string clinicName)
        {
            try
            {
                TCSEntities tcsDB = new TCSEntities();
                var query = from clinic in tcsDB.asuser
                            where clinic.客戶名稱.Contains(clinicName)
                            select new viewClinic()
                            {
                                clinicIndex = clinic.索引編號,
                                clinicName = clinic.客戶名稱,
                                clinicAddr= clinic.客戶地址,
                            };

                return query.AsQueryable();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public viewBug readRelease(int ikey)
        {
            try
            {
                var data = entityObj.servfile_notice.Where(a => a.Skey == ikey).Select(x => new
                {
                    x.Skey,
                    x.版本,
                    x.顯示檔名,
                }).FirstOrDefault();
                
                viewBug viewBugObj = new viewBug();
                viewBugObj.ikey = data.Skey;
                viewBugObj.fileName = data.顯示檔名;
                viewBugObj.fileVersion = data.版本;
                return viewBugObj;                
            }
            catch (Exception ex)
            {
                
                throw new Exception(ex.Message);
            }
        }

    }
}
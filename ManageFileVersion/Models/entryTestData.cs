using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using WriteEvent;


namespace ManageFileVersion.Models
{
    public class entityTestData
    {
        double paging = 5; //設定分頁=5

        I_cpnewsEntities i_cpNewDb = new I_cpnewsEntities();
        WrittingEventLog writeObj = new WrittingEventLog();

        #region 後台管理介面

        /// <summary>
        /// 新增資料
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="fileVersion"></param>
        /// <param name="context"></param>
        /// <param name="documentFile"></param>
        /// <returns></returns>
        public async Task<bool> add(string produceType, string fileName, string fileVersion, string context, string testFilePath, string documentFile)
        {
            try
            {

                test_notice testNoticeObj = new test_notice();
                testNoticeObj.產品類別 = produceType;
                testNoticeObj.檔案名稱 = fileName;
                testNoticeObj.版本 = fileVersion;
                testNoticeObj.主旨 = context;
                testNoticeObj.測試檔案路徑 = testFilePath;
                testNoticeObj.測試文件路徑 = documentFile;
                testNoticeObj.修改日期 = DateTime.Now;
                i_cpNewDb.test_notice.Add(testNoticeObj);
                i_cpNewDb.SaveChanges();

                if (testNoticeObj.ikey <= 0)
                {
                    throw new Exception("entity sql ikey  is 0");
                }

                return true;
            }
            catch (Exception ex)
            {
                //throw new Exception(ex.Message);
                writeObj.writeToFile("ManageFileVersion_" + DateTime.Now.ToString("yyyyMMdd"), HttpContext.Current.Server.MapPath("~"), "add error: " + ex.Message);
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 修改資料
        /// </summary>
        /// <param name="ikey"></param>
        /// <param name="fileName"></param>
        /// <param name="fileVersion"></param>
        /// <param name="context"></param>
        /// <param name="downloadFile"></param>
        /// <returns></returns>
        public async Task<bool> edit(int editIkey, string productType, string fileName, string fileVersion, string context, string testFilePath, string documentdFile, int isRease)
        {
            try
            {
                //修改

                test_notice testNoticeObj = i_cpNewDb.test_notice.Find(editIkey);
                if (testNoticeObj == null)
                    return false;

                testNoticeObj.產品類別 = productType;
                testNoticeObj.檔案名稱 = fileName;
                testNoticeObj.版本 = fileVersion;
                testNoticeObj.主旨 = context;
                testNoticeObj.修改日期 = DateTime.Now;
                testNoticeObj.測試文件路徑 = documentdFile;
                testNoticeObj.測試檔案路徑 = testFilePath;
                testNoticeObj.釋出正式版 = isRease;
                i_cpNewDb.SaveChanges();
                return true;


            }
            catch (Exception ex)
            {
                writeObj.writeToFile("ManageFileVersion_" + DateTime.Now.ToString("yyyyMMdd"), HttpContext.Current.Server.MapPath("~"), "edit error: " + ex.Message);
                throw new Exception(ex.Message);

            }
        }

        /// <summary>
        /// 刪除後台的測試資料
        /// </summary>
        /// <param name="ikey"></param>
        /// <returns></returns>
        public async Task<bool> delete(int ikey)
        {
            try
            {

                test_notice testNoticeObj = i_cpNewDb.test_notice.Find(ikey);
                if (testNoticeObj == null)
                    return false;

                i_cpNewDb.test_notice.Remove(testNoticeObj);
                i_cpNewDb.SaveChanges();
                return true;

            }
            catch (Exception ex)
            {
                writeObj.writeToFile("ManageFileVersion_" + DateTime.Now.ToString("yyyyMMdd"), HttpContext.Current.Server.MapPath("~"), "delete error: " + ex.Message);
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 讀取單筆的資料
        /// </summary>
        /// <param name="ikey"></param>
        /// <returns></returns>
        public viewAnnounce read(int ikey)
        {
            try
            {

                viewAnnounce viewAnnounceObj = new viewAnnounce();
                var data = i_cpNewDb.test_notice.Where(b => b.ikey == ikey).FirstOrDefault();
                viewAnnounceObj.ikey = data.ikey;
                viewAnnounceObj.productType = data.產品類別;
                viewAnnounceObj.fileName = data.檔案名稱;
                viewAnnounceObj.fileVersion = data.版本;
                viewAnnounceObj.context = data.主旨;
                viewAnnounceObj.downloadFile = data.測試文件路徑;
                viewAnnounceObj.testFilePath = data.測試檔案路徑;
                viewAnnounceObj.isRelease = data.釋出正式版;
                return viewAnnounceObj;

            }
            catch (Exception ex)
            {
                writeObj.writeToFile("ManageFileVersion_" + DateTime.Now.ToString("yyyyMMdd"), HttpContext.Current.Server.MapPath("~"), "read error: " + ex.Message);
                throw new Exception(ex.Message);
            }
        }


        /// <summary>
        /// 查詢
        /// </summary>
        /// <param name="productType"></param>
        /// <param name="fileName"></param>
        /// <param name="fileVersion"></param>
        /// <returns></returns>
        public IQueryable<viewAnnounce> search(int currentPage, string productType, string fileName, string fileVersion)
        {
            try
            {
                int intPagging = 0;
                intPagging = Convert.ToInt32(paging);


                var query = from q in i_cpNewDb.test_notice
                            orderby q.ikey descending
                            select new viewAnnounce()
                            {
                                ikey = q.ikey,
                                productType = q.產品類別,
                                fileName = q.檔案名稱,
                                fileVersion = q.版本,
                                context = q.主旨,
                                updateTime = q.修改日期,
                                testFilePath = q.測試檔案路徑,
                                downloadFile = q.測試文件路徑,
                                isRelease = q.釋出正式版
                            };

                if (!string.IsNullOrEmpty(productType))
                {
                    query = query.Where(q => q.productType.Contains(productType.Trim()));
                }

                if (!string.IsNullOrEmpty(fileName))
                {
                    query = query.Where(q => q.fileName.Contains(fileName.Trim()));
                }

                if (!string.IsNullOrEmpty(fileVersion))
                {
                    query = query.Where(q => q.fileVersion.Contains(fileVersion.Trim()));
                }

                IQueryable<viewAnnounce> liResult = query.AsEnumerable<viewAnnounce>().Skip((currentPage - 1) * intPagging).Take(intPagging).AsQueryable();

                return liResult;

            }
            catch (Exception ex)
            {
                writeObj.writeToFile("ManageFileVersion_" + DateTime.Now.ToString("yyyyMMdd"), HttpContext.Current.Server.MapPath("~"), "search error: " + ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<int> totalCount()
        {
            try
            {
                return i_cpNewDb.test_notice.Count();
            }
            catch (Exception ex)
            {
                writeObj.writeToFile("ManageFileVersion_" + DateTime.Now.ToString("yyyyMMdd"), HttpContext.Current.Server.MapPath("~"), "totalCount error: " + ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<int> pageCount(int totalCount)
        {
            try
            {
                double pageCount = Math.Ceiling(totalCount / paging);
                return Convert.ToInt32(pageCount);
            }
            catch (Exception ex)
            {
                writeObj.writeToFile("ManageFileVersion_" + DateTime.Now.ToString("yyyyMMdd"), HttpContext.Current.Server.MapPath("~"), "pageCount error: " + ex.Message);
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 檔案上傳
        /// </summary>
        /// <param name="addDownloadFile"></param>
        /// <returns></returns>
        public async Task<string> fileUpload(IEnumerable<HttpPostedFileBase> addDownloadFile, string path)
        {
            try
            {
                var fileName = "";
                var httpPath = HttpContext.Current.Server.MapPath("~/" + path);
                Directory.CreateDirectory(httpPath);
                //路徑不存在自已建立

                if (addDownloadFile.Count() > 0)
                {
                    foreach (HttpPostedFileBase file in addDownloadFile)
                    {
                        fileName += Path.GetFileName(file.FileName) + ",";
                        var pathFileName = Path.Combine(httpPath, file.FileName);
                        file.SaveAs(pathFileName);
                    }
                    fileName = fileName.Substring(0, fileName.Length - 1);
                }
                return fileName;
            }
            catch (Exception ex)
            {
                writeObj.writeToFile("ManageFileVersion_" + DateTime.Now.ToString("yyyyMMdd"), HttpContext.Current.Server.MapPath("~"), "fileUpload error: " + ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<string> fileUpload(int ikey, IEnumerable<HttpPostedFileBase> addDownloadFile, string path )
        {
            var fileName = "";
            try
            {
                var httpPath = HttpContext.Current.Server.MapPath("~/" + path);
                Directory.CreateDirectory(httpPath);
                //路徑不存在自已建立
                if (addDownloadFile.Count() > 0)
                {
                    foreach (HttpPostedFileBase file in addDownloadFile)
                    {
                        string[] fileNameSplit = file.FileName.Split('.'); //切開檔案名稱
                        string newFileName = fileNameSplit[0] + "_" + ikey +"."+ fileNameSplit[1];
                        fileName += Path.GetFileName(newFileName) + ",";
                        var pathFileName = Path.Combine(httpPath, newFileName); //加上編號
                        file.SaveAs(pathFileName);
                    }
                    fileName = fileName.Substring(0, fileName.Length - 1);
                }
                return fileName;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> moveFile(string fromPathFileName, string moveFilePathName)
        {
            try
            {
                var httpServerFromPath = HttpContext.Current.Server.MapPath("~/" + fromPathFileName);
                var httpServerMovePath = HttpContext.Current.Server.MapPath("~/" + moveFilePathName);
                string fromFile = Path.GetFileName(httpServerFromPath);
                if (String.IsNullOrEmpty(fromFile) == false)
                {
                    Directory.CreateDirectory(httpServerMovePath);
                    System.IO.File.Copy(httpServerFromPath, httpServerMovePath + "/" + fromFile, true);
                }
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> deleteFile(string fromPathFileName)
        {
            try
            {
                 var httpServerFromPath = HttpContext.Current.Server.MapPath("~/" + fromPathFileName);
                 string fromFile = Path.GetFileName(httpServerFromPath);
                 if (String.IsNullOrEmpty(fromFile) == false)
                 {
                     File.Delete(httpServerFromPath);   
                 }
                 return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #endregion


        /// <summary>
        /// 顯示最新公告列表(最近二星期內); 尚未釋出正式版
        /// </summary>
        /// <returns></returns>
        public IEnumerable<viewAnnounce> listNewNotice()
        {
            try
            {
                var baselineDate = DateTime.Now.AddDays(-14);


                var query = from p in i_cpNewDb.test_notice
                            orderby p.修改日期 descending
                            where p.釋出正式版 == 0
                                 && p.修改日期 >= baselineDate
                            select new viewAnnounce()
                            {
                                ikey = p.ikey,
                                productType = p.產品類別,
                                fileName = p.檔案名稱,
                                fileVersion = p.版本,
                                updateTime = p.修改日期,
                                context = p.主旨,
                                testFilePath = p.測試檔案路徑,
                                downloadFile = p.測試文件路徑,
                            };
                return query.AsEnumerable<viewAnnounce>();

            }
            catch (Exception ex)
            {
                writeObj.writeToFile("ManageFileVersion_" + DateTime.Now.ToString("yyyyMMdd"), HttpContext.Current.Server.MapPath("~"), "listNewNotice error: " + ex.Message);
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 新增bug 回報
        /// </summary>
        /// <param name="userID">登入者的uid</param>
        /// <param name="announcementIkey">回報錯誤的流水號</param>
        /// <param name="bugContext">回報內文</param>
        /// <param name="bugFile">回報的檔案</param>
        /// <param name="bugType">0:測試 1:正式釋出</param>
        /// <returns></returns>
        public async Task<bool> addBug(string userID, string clinicIndex, int announcementIkey, string bugContext, string bugFile, int bugType)
        {
            try
            {

                bug_notice bugNoticeObj = new bug_notice();
                bugNoticeObj.uid = userID;
                bugNoticeObj.announcement_ikey = announcementIkey;
                bugNoticeObj.回報結果 = bugContext;
                bugNoticeObj.測試檔案 = bugFile;
                bugNoticeObj.測試類別 = bugType; //測試版
                bugNoticeObj.診所索引編號 = clinicIndex;
                i_cpNewDb.bug_notice.Add(bugNoticeObj);
                i_cpNewDb.SaveChanges();

                if (bugNoticeObj.ikey <= 0)
                {
                    throw new Exception("entity sql ikey  is 0");
                }

                return true;
            }
            catch (Exception ex)
            {
                writeObj.writeToFile("ManageFileVersion_" + DateTime.Now.ToString("yyyyMMdd"), HttpContext.Current.Server.MapPath("~"), "addBug error: " + ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> editBugFileName(int ikey, string fileName)
        {
            try
            {
                 bug_notice bugNoticeObj = i_cpNewDb.bug_notice.Find(ikey);
                if (bugNoticeObj == null)
                    return false;

                bugNoticeObj.ikey = ikey;
                bugNoticeObj.測試檔案 = fileName;
                i_cpNewDb.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                writeObj.writeToFile("ManageFileVersion_" + DateTime.Now.ToString("yyyyMMdd"), HttpContext.Current.Server.MapPath("~"), "editBugFileName error: " + ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<int> addReleaseBug(string userID, string clinicIndex, int announcementIkey, string fileVersion, string bugContext, string bugFile, int bugType, int bugGrade)
        {
            try
            {

                bug_notice bugNoticeObj = new bug_notice();
                bugNoticeObj.uid = userID;
                bugNoticeObj.announcement_ikey = announcementIkey;
                bugNoticeObj.回報結果 = bugContext;
                bugNoticeObj.測試檔案 = bugFile;
                bugNoticeObj.測試類別 = bugType; //測試版
                bugNoticeObj.診所索引編號 = clinicIndex;
                bugNoticeObj.版本 = fileVersion;
                bugNoticeObj.建立日期 = DateTime.Now;                
                bugNoticeObj.最後狀態 = "未完成";
                bugNoticeObj.回報等級 = bugGrade;
                 
                i_cpNewDb.bug_notice.Add(bugNoticeObj);
                i_cpNewDb.SaveChanges();

                if (bugNoticeObj.ikey <= 0)
                {
                    throw new Exception("entity sql ikey  is 0");
                }

                return bugNoticeObj.ikey;
            }
            catch (Exception ex)
            {
                writeObj.writeToFile("ManageFileVersion_" + DateTime.Now.ToString("yyyyMMdd"), HttpContext.Current.Server.MapPath("~"), "addBug error: " + ex.Message);
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 列出所有回覆bug的列表 ready 
        /// </summary>
        /// <param name="announceIkey"></param>
        /// <param name="bugType"></param>
        /// <returns></returns>
        public IEnumerable<viewBug> listBugs(int announceIkey, int bugType)
        {
            try
            {
                //因為有跨資料表，才需要特別使用
                string eSQL = @"select a.ikey as ikey, a.announcement_ikey as announcementIkey,a.uid as uid,b.產品類別 as productType, a.測試檔案 as bugDocumentFile , a.回報結果 as bugContext,b.檔案名稱 as fileName,b.版本 as fileVersion,c.name as userName,d.客戶名稱 as clinicName from bug_notice  a 
                                    left join test_notice b on a.announcement_ikey = b.ikey
                                    left join TCS.dbo.alluser c on (a.uid = c.uid)
                                    left join TCS.dbo.asuser d on (a.診所索引編號 = d.索引編號)
                                    where a.announcement_ikey=@Announcement and a.測試類別=@bugType";

                SqlParameter spAnnounceIkey = new SqlParameter("Announcement", announceIkey);
                spAnnounceIkey.SqlDbType = System.Data.SqlDbType.Int;


                SqlParameter spBugType = new SqlParameter("bugType", bugType);
                spAnnounceIkey.SqlDbType = System.Data.SqlDbType.Int;


                object[] parameters = { spAnnounceIkey, spBugType };
                var results = i_cpNewDb.Database.SqlQuery<viewBug>(eSQL, parameters).AsEnumerable();

               
               

                return results.AsEnumerable<viewBug>();
            }
            catch (SqlException sqlEx)
            {
                throw;
            }
            catch (Exception ex)
            {
                writeObj.writeToFile("ManageFileVersion_" + DateTime.Now.ToString("yyyyMMdd"), HttpContext.Current.Server.MapPath("~"), "listBugs error: " + ex.Message);
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 列出所有問題清單
        /// </summary>
        /// <param name="announcementIkey">產品分類</param>
        /// <param name="userID">登入帳號</param>
        /// <param name="clinicIndex">診所編號 index</param>
        /// <param name="startDate">開始日期</param>
        /// <param name="endDate">結束日期</param>
        /// <param name="bugContext">問題描述</param>      
        /// <param name="bugContext">狀態</param>
        /// <returns></returns>
        public IEnumerable<viewBug> listAllBugs(string announcementIkey, string userID, string clinicName, string reportName, string bugContext, DateTime? startDate, DateTime? endDate, string status, int? grade)
        {
            try
            {
                string eSQL = @"
                   select a.ikey as ikey,
                        a.announcement_ikey as announcementIkey,
                        a.uid as uid,
                        a.測試檔案 as bugDocumentFile,
                        a.回報結果 as bugContext,                        
                        b.type as fileName,
                        a.版本 as fileVersion,
                        c.name as userName,
                        d.客戶名稱 as clinicName,
                        CONVERT(char(10), a.建立日期,126) as createdTime,
                        CONVERT(char(10), a.預計完成日,126) as estimatedDate,
                        CONVERT(char(10), a.完成日,126) as completeDate,
                        ISNULL(a.退回,0) as reject,
                        ISNULL(a.需討論,0) as dispute,
                        a.備註 as remark,
                        a.最後狀態 as status,
                        a.回報等級 as grade
                   from bug_notice  a 
                   left join bug_type b on a.announcement_ikey = b.ikey
                   left join TCS.dbo.alluser c on (a.uid = c.uid)
                   left join TCS.dbo.asuser d on (a.診所索引編號 = d.索引編號)
                   where  a.測試類別='1' "; //測試類別 0: 測試 1:正式

                List<object> parameters = new List<object>();

                #region 條件設定

                //產品分類
                if (!string.IsNullOrEmpty(announcementIkey))
                {
                    eSQL += " and a.announcement_ikey=@announcementIkey";
                    SqlParameter spAnnouncementIkey = new SqlParameter("@announcementIkey", announcementIkey);
                    spAnnouncementIkey.SqlDbType = System.Data.SqlDbType.VarChar;
                    parameters.Add(spAnnouncementIkey);
                }

                //建立bug 起始日
                if (!string.IsNullOrEmpty(userID))
                {   
                    eSQL += "  and a.uid=@uid";
                    SqlParameter spUserID = new SqlParameter("@uid", userID);
                    spUserID.SqlDbType = System.Data.SqlDbType.VarChar;
                    parameters.Add(spUserID);
                }

                //診所分類
                if (!string.IsNullOrEmpty(clinicName))
                {
                    eSQL += "  and d.客戶名稱 like '%" + clinicName + "%'";
                }

                if (!string.IsNullOrEmpty(reportName))
                {
                    eSQL += " and c.name like  '%" + reportName + "%'";
                }

                if (!string.IsNullOrEmpty(bugContext))
                {
                    eSQL += " and  a.回報結果 like  '%" + bugContext + "%'";
                }

                //起始日
                if (startDate != null)
                {
                    eSQL += "   and  a.建立日期>=@startDate";
                    SqlParameter spStartDate = new SqlParameter("@startDate", startDate);
                    spStartDate.SqlDbType = System.Data.SqlDbType.Date;
                    parameters.Add(spStartDate);
                }

                //結束日
                if (endDate != null)
                {
                    eSQL += "   and  a.建立日期<=@endDate";

                    SqlParameter spEndDate = new SqlParameter("@endDate", endDate);
                    spEndDate.SqlDbType = System.Data.SqlDbType.Date;
                    parameters.Add(spEndDate);
                }

                //最後狀態
                if (!string.IsNullOrEmpty(status))
                {
                    eSQL += "   and  a.最後狀態=@status";
                    SqlParameter spStatus = new SqlParameter("@status", status);
                    spStatus.SqlDbType = System.Data.SqlDbType.VarChar;
                    parameters.Add(spStatus);
                }

                //回報等級

                if (grade != null)
                {
                    eSQL += "   and  a.回報等級=@grade";
                    SqlParameter spStatus = new SqlParameter("@grade", grade);
                    spStatus.SqlDbType = System.Data.SqlDbType.Int;
                    parameters.Add(spStatus);
                }

                eSQL += "  order by a.建立日期 desc ";

                #endregion

                var results = i_cpNewDb.Database.SqlQuery<viewBug>(eSQL, parameters.ToArray()).AsEnumerable();
                return results;
            }
            catch (SqlException sqlEx)
            {
                writeObj.writeToFile("ManageFileVersion_" + DateTime.Now.ToString("yyyyMMdd"), HttpContext.Current.Server.MapPath("~"), "listBugs error: " + sqlEx.Message);
                throw;
            }
            catch (Exception ex)
            {
                writeObj.writeToFile("ManageFileVersion_" + DateTime.Now.ToString("yyyyMMdd"), HttpContext.Current.Server.MapPath("~"), "listBugs error: " + ex.Message);
                throw;
            }
        }


        /// <summary>
        /// 由bug notify 提供id 讓rd上網查詢
        /// </summary>
        /// <param name="ikey"></param>
        /// <returns></returns>
        public IEnumerable<viewBug> listAllBugs(int? ikey)
        {
            try
            {
                if (ikey == null)
                    return null;

                string eSQL = @"
                   select a.ikey as ikey,
                        a.announcement_ikey as announcementIkey,
                        a.uid as uid,
                        a.測試檔案 as bugDocumentFile,
                        a.回報結果 as bugContext,                        
                        b.type as fileName,
                        a.版本 as fileVersion,
                        c.name as userName,
                        d.客戶名稱 as clinicName,
                        CONVERT(char(10), a.建立日期,126) as createdTime,
                        CONVERT(char(10), a.預計完成日,126) as estimatedDate,
                        CONVERT(char(10), a.完成日,126) as completeDate,
                        ISNULL(a.退回,0) as reject,
                        ISNULL(a.需討論,0) as dispute,
                        a.備註 as remark,
                        a.最後狀態 as status,
                        a.回報等級 as grade
                   from bug_notice  a 
                   left join bug_type b on a.announcement_ikey = b.ikey
                   left join TCS.dbo.alluser c on (a.uid = c.uid)
                   left join TCS.dbo.asuser d on (a.診所索引編號 = d.索引編號)
                   where  a.測試類別='1' "; //測試類別 0: 測試 1:正式

                List<object> spParameterArray = new List<object>();
                eSQL += " and a.ikey=@ikey";
                SqlParameter spIkey = new SqlParameter("@ikey", ikey);
                spIkey.SqlDbType = System.Data.SqlDbType.Int;
                spParameterArray.Add(spIkey);
                var results = i_cpNewDb.Database.SqlQuery<viewBug>(eSQL, spParameterArray.ToArray()).AsEnumerable();
                return results;
            }
            catch (SqlException sqlEx)
            {
                writeObj.writeToFile("ManageFileVersion_" + DateTime.Now.ToString("yyyyMMdd"), HttpContext.Current.Server.MapPath("~"), "listBugs error: " + sqlEx.Message);
                throw;
            }
            catch (Exception ex)
            {
                writeObj.writeToFile("ManageFileVersion_" + DateTime.Now.ToString("yyyyMMdd"), HttpContext.Current.Server.MapPath("~"), "listBugs error: " + ex.Message);
                throw;
            }
        }


        public IEnumerable<viewBug> statistics(string announcementIkey, string userID, string clinicName, string reportName, string bugContext, DateTime? startDate, DateTime? endDate, string status, int? grade)
        {
            try
            {
                string eSQL = @"select                                        
                        b.type as fileName,
                        a.回報等級 as grade, 
                        a.最後狀態 as status,
                        count(*)  as totalCount                    
                   from bug_notice  a 
                   left join bug_type b on a.announcement_ikey = b.ikey
                   left join TCS.dbo.alluser c on (a.uid = c.uid)
                   left join TCS.dbo.asuser d on (a.診所索引編號 = d.索引編號)
                   where  a.測試類別='1'";

                List<object> parameters = new List<object>();

                #region 條件設定

                //產品分類
                if (!string.IsNullOrEmpty(announcementIkey))
                {
                    eSQL += " and a.announcement_ikey=@announcementIkey";
                    SqlParameter spAnnouncementIkey = new SqlParameter("@announcementIkey", announcementIkey);
                    spAnnouncementIkey.SqlDbType = System.Data.SqlDbType.VarChar;
                    parameters.Add(spAnnouncementIkey);
                }

                //建立bug 起始日
                if (!string.IsNullOrEmpty(userID))
                {
                    eSQL += "  and a.uid=@uid";
                    SqlParameter spUserID = new SqlParameter("@uid", userID);
                    spUserID.SqlDbType = System.Data.SqlDbType.VarChar;
                    parameters.Add(spUserID);
                }

                //診所分類
                if (!string.IsNullOrEmpty(clinicName))
                {
                    eSQL += "  and d.客戶名稱 like '%" + clinicName + "%'";
                }

                if (!string.IsNullOrEmpty(reportName))
                {
                    eSQL += " and c.name like  '%" + reportName + "%'";
                }

                if (!string.IsNullOrEmpty(bugContext))
                {
                    eSQL += " and  a.回報結果 like  '%" + bugContext + "%'";
                }

                //起始日
                if (startDate != null)
                {
                    eSQL += "   and  a.建立日期>=@startDate";
                    SqlParameter spStartDate = new SqlParameter("@startDate", startDate);
                    spStartDate.SqlDbType = System.Data.SqlDbType.Date;
                    parameters.Add(spStartDate);
                }

                //結束日
                if (endDate != null)
                {
                    eSQL += "   and  a.建立日期<=@endDate";

                    SqlParameter spEndDate = new SqlParameter("@endDate", endDate);
                    spEndDate.SqlDbType = System.Data.SqlDbType.Date;
                    parameters.Add(spEndDate);
                }

                //最後狀態
                if (!string.IsNullOrEmpty(status))
                {
                    eSQL += "   and  a.最後狀態=@status";
                    SqlParameter spStatus = new SqlParameter("@status", status);
                    spStatus.SqlDbType = System.Data.SqlDbType.VarChar;
                    parameters.Add(spStatus);
                }

                //回報等級

                if (grade != null)
                {
                    eSQL += "   and  a.回報等級=@grade";
                    SqlParameter spGrade = new SqlParameter("@grade", grade);
                    spGrade.SqlDbType = System.Data.SqlDbType.Int;
                    parameters.Add(spGrade);
                }

                eSQL += " group by  b.type,a.回報等級,a.最後狀態";

                #endregion

                var results = i_cpNewDb.Database.SqlQuery<viewBug>(eSQL, parameters.ToArray()).AsEnumerable();
                return results;
            }
            catch (SqlException sqlEx)
            {
                writeObj.writeToFile("ManageFileVersion_" + DateTime.Now.ToString("yyyyMMdd"), HttpContext.Current.Server.MapPath("~"), "statistics error: " + sqlEx.Message);
                throw;
            }
            catch (Exception ex)
            {
                writeObj.writeToFile("ManageFileVersion_" + DateTime.Now.ToString("yyyyMMdd"), HttpContext.Current.Server.MapPath("~"), "statistics error: " + ex.Message);
                throw;
            }
        }



        public viewBug readBug(int editIkey)
        {
            try
            {
                string eSQL = @"
                   select a.ikey as ikey,
                        a.announcement_ikey as announcementIkey,
                        a.uid as uid,
                        a.測試檔案 as bugDocumentFile,
                        a.回報結果 as bugContext,       
                        a.診所索引編號 as clinicIndex,   
                        CONVERT(char(10), a.建立日期,126) as createdTime,
                        CONVERT(char(10), a.預計完成日,126) as estimatedDate,
                        CONVERT(char(10), a.完成日,126) as completeDate,              
                        b.type as fileName,
                        a.版本 as fileVersion,
                        c.name as userName,
                        d.客戶名稱 as clinicName,
                        a.備註 as remark,
                        a.最後狀態 as status,
                        a.回報等級 as grade
                   from bug_notice  a 
                   left join bug_type b on a.announcement_ikey = b.ikey
                   left join TCS.dbo.alluser c on (a.uid = c.uid)
                   left join TCS.dbo.asuser d on (a.診所索引編號 = d.索引編號)
                   where  a.測試類別='1' and a.ikey=@ikey"; //測試類別 0: 測試 1:正式

                List<object> parameters = new List<object>();
                SqlParameter spEditIkey = new SqlParameter("@ikey", editIkey);
                spEditIkey.SqlDbType = System.Data.SqlDbType.Int;
                parameters.Add(spEditIkey);

                var results = i_cpNewDb.Database.SqlQuery<viewBug>(eSQL, parameters.ToArray()).AsEnumerable().FirstOrDefault<viewBug>();
                return results;
            }
            catch (Exception ex)
            {
                writeObj.writeToFile("ManageFileVersion_" + DateTime.Now.ToString("yyyyMMdd"), HttpContext.Current.Server.MapPath("~"), "readBug error: " + ex.Message);
                throw new Exception(ex.Message);
            }
        }

        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="editIkey"></param>
        /// <param name="bugContext"></param>
        /// <param name="bugFile"></param>
        /// <returns></returns>
        public async Task<bool> editBug(int editIkey, string bugContext, string bugFile, int bugType)
        {
            try
            {
                bug_notice bugNoticeObj = i_cpNewDb.bug_notice.Find(editIkey);
                if (bugNoticeObj == null)
                    return false;

                bugNoticeObj.ikey = editIkey;
                if (!string.IsNullOrEmpty(bugContext))
                    bugNoticeObj.回報結果 = bugContext;

                if (!string.IsNullOrEmpty(bugFile))
                    bugNoticeObj.測試檔案 = bugFile;

                bugNoticeObj.測試類別 = bugType;
                i_cpNewDb.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                writeObj.writeToFile("ManageFileVersion_" + DateTime.Now.ToString("yyyyMMdd"), HttpContext.Current.Server.MapPath("~"), "editBut error: " + ex.Message);
                throw new Exception(ex.Message);
            }
        }
        
        public async Task<bool> editReleaseBug(int editIkey,int productTypeID, string clinicIndex, string fileVersion ,string bugContext, string bugFile, int bugType, int bugGrade)
        {
            try
            {
                bug_notice bugNoticeObj = i_cpNewDb.bug_notice.Find(editIkey);
                if (bugNoticeObj == null)
                    return false;

                string[] fileNames = bugNoticeObj.測試檔案.ToString().Split(',');

                //如改變產品目錄時，需連同檔案一起搬移
                if (bugNoticeObj.announcement_ikey != productTypeID)
                {
                    foreach (string filename in fileNames)
                    {
                        string fromPathFileName = "bugFile/" + bugNoticeObj.announcement_ikey + "/" + DateTime.Now.Year + "/" + filename;
                        string movePath = "bugFile/" + productTypeID + "/" + DateTime.Now.Year;
                        await moveFile(fromPathFileName, movePath); //搬移目錄
                        await deleteFile(fromPathFileName);
                    }
                }

                bugNoticeObj.ikey = editIkey;                
                bugNoticeObj.回報結果 = bugContext;                
                bugNoticeObj.診所索引編號 = clinicIndex;                
                bugNoticeObj.版本 = fileVersion;                
                bugNoticeObj.announcement_ikey = productTypeID;
                bugNoticeObj.回報等級 = bugGrade;
                if (!string.IsNullOrEmpty(bugFile))
                    bugNoticeObj.測試檔案 = bugFile;

                bugNoticeObj.測試類別 = bugType;
                i_cpNewDb.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                writeObj.writeToFile("ManageFileVersion_" + DateTime.Now.ToString("yyyyMMdd"), HttpContext.Current.Server.MapPath("~"), "editReleaseBug error: " + ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> deleteBug(int editIkey)
        {
            try
            {
                bug_notice bugNoticeObj = i_cpNewDb.bug_notice.Find(editIkey);
                if (bugNoticeObj == null)
                    return false;

                i_cpNewDb.bug_notice.Remove(bugNoticeObj);
                i_cpNewDb.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                writeObj.writeToFile("ManageFileVersion_" + DateTime.Now.ToString("yyyyMMdd"), HttpContext.Current.Server.MapPath("~"), "deleteBug error: " + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// 改變提報bug的問題狀態
        /// </summary>
        /// <param name="editIkey"></param>
        /// <param name="estimateDate">預估時間</param>
        /// <param name="completeDate">完成時間</param>
        /// <param name="status">最後狀態</param>
        /// <param name="remark">備註</param>
        /// <returns></returns>
        public async Task<bool> changeStatus(int editIkey, int editType, DateTime? estimateDate, DateTime? completeDate, string status,string remark, int bugGrade)
        {
            try
            {
                bug_notice bugNoticeObj = i_cpNewDb.bug_notice.Find(editIkey);
                if (bugNoticeObj == null)
                    return false;

                string[] fileNames = bugNoticeObj.測試檔案.ToString().Split(',');

                //如改變產品目錄時，需連同檔案一起搬移
                if (bugNoticeObj.announcement_ikey != editType)
                {
                    foreach (string filename in fileNames)
                    {
                        string fromPathFileName = "bugFile/" + bugNoticeObj.announcement_ikey + "/" + DateTime.Now.Year + "/" + filename;
                        string movePath = "bugFile/" + editType + "/" + DateTime.Now.Year;                       
                        await moveFile(fromPathFileName, movePath); //搬移目錄
                        await deleteFile(fromPathFileName);
                    }
                }

                bugNoticeObj.ikey = editIkey;
                bugNoticeObj.announcement_ikey = editType;
                bugNoticeObj.預計完成日 = estimateDate;
                bugNoticeObj.完成日 = completeDate;
                bugNoticeObj.最後狀態 = status;
                bugNoticeObj.備註 = remark;
                bugNoticeObj.修改時間 = DateTime.Now;
                bugNoticeObj.回報等級 = bugGrade;

                i_cpNewDb.SaveChanges();
                return true;
            }
            catch (System.Data.Entity.Infrastructure.DbUpdateException updatedError)
            {
                writeObj.writeToFile("ManageFileVersion_" + DateTime.Now.ToString("yyyyMMdd"), HttpContext.Current.Server.MapPath("~"), "changeStatus error: " + updatedError.Message);
                throw;
            }
            catch (Exception ex)
            {
                writeObj.writeToFile("ManageFileVersion_" + DateTime.Now.ToString("yyyyMMdd"), HttpContext.Current.Server.MapPath("~"), "changeStatus error: " + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int maxIkey()
        {
            try
            {
                bug_notice bugNoticeObj= i_cpNewDb.bug_notice.OrderByDescending(u => u.ikey).FirstOrDefault();
                return bugNoticeObj.ikey;
            }
            catch (Exception ex)
            {
                writeObj.writeToFile("ManageFileVersion_" + DateTime.Now.ToString("yyyyMMdd"), HttpContext.Current.Server.MapPath("~"), "maxIkey error: " + ex.Message);
                throw new Exception(ex.Message);
            }
        }
    }
}
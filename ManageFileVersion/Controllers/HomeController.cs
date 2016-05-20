using ManageFileVersion.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ManageFileVersion.Controllers
{
    public class viewBugStatus
    {
        public string status
        {
            get;
            set;
        }
    }

    public class viewBugGrade
    {
        public int key
        {
            get;
            set;
        }

        public string value
        {
            get;
            set;
        }
    }

    public class viewStatics
    {
        /// <summary>
        /// 產品名稱
        /// </summary>
        public string ProductName
        {
            get;
            set;
        }

        /// <summary>
        /// bug等級(建議/bug)
        /// </summary>
        public int bugGrade
        {
            get;
            set;
        }

        /// <summary>
        /// 尚未完成
        /// </summary>
        public int notComplete
        {
            get;
            set;
        }

        /// <summary>
        /// 已完成
        /// </summary>
        public int complete
        {
            get;
            set;
        }

        /// <summary>
        /// 暫不處理
        /// </summary>
         public int notDeal
        {
            get;
            set;
        }
    }

    public class HomeController : Controller
    {
        entityTestData entityTestObj = new entityTestData(); //呼叫實體層

        string[] options = new string[6] { "未完成","處理中","暫援","已完成","會議討論","退回"};//狀態

        List<viewBugGrade> bugGrades = new List<viewBugGrade>()
        {
            new viewBugGrade(){
                 key=0,
                  value="建議事項"
            },
            new viewBugGrade(){
                key=1,
                value="BUG回報"
            },  
            new viewBugGrade(){
                key=2,
                value="法規整理"
            }
        };
        


        public void getUserNameAndPassword(string username, string userPassword, out string outUserName, out string outUserPassword)
        {
            try
            {
                if (string.IsNullOrEmpty(username) && string.IsNullOrEmpty(userPassword) )
                {
                    
                    if (!string.IsNullOrEmpty(Session["username"].ToString()) && !string.IsNullOrEmpty(Session["password"].ToString()))
                    {
                        username = Session["username"].ToString();
                        userPassword = Session["password"].ToString();
                    }
                }
                else if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(userPassword) )
                {
                   
                    userPassword = Encoding.Default.GetString(Convert.FromBase64String(userPassword));

                    /*寫入session  不用重新登入*/
                    Session["username"] = username;
                    Session["password"] = userPassword;
                }

                outUserName = username;
                outUserPassword = userPassword;
            }
            catch (Exception ex)
            {   
                throw new Exception(ex.Message);
            }
        }


        /// <summary>
        /// 由回報系統直接串接而來
        /// </summary>
        /// <param name="username">使用登入帳號</param>
        /// <param name="password">使用登入密碼</param>
        /// <param name="clinicIndex">診所索引編號</param>
        /// <returns></returns>
        public ActionResult Index(string username, string password,string clinicIndex)
        {
            //檢查邏輯是否為客管人員
            try
            {
                string strDomainURL = Request.Url.Authority;
                string strOutUsername;
                string strOutPassword;
                getUserNameAndPassword(username, password, out strOutUsername, out strOutPassword);

                ServfileModel objModel = new ServfileModel();
                if (objModel.isPassLogin(strOutUsername, strOutPassword) != 1)
                {
                    ViewData["msg"] = "帳號/密碼 輸入錯誤";
                    Response.Redirect("noAuthority");
                }
                ViewBag.username = strOutUsername;
                ViewBag.clinicIndex = clinicIndex;
                return View(ViewBag);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public ActionResult Add(string username, string password, string clinicIndex,string fromICD10)
        {
            try
            {
                string strDomainURL = Request.Url.Authority;
                string strOutUsername;
                string strOutPassword;

                getUserNameAndPassword(username, password, out strOutUsername, out strOutPassword);
                ServfileModel objModel = new ServfileModel();

                //檢查邏輯是否為公司員工
                if (objModel.isPassLogin(strOutUsername, strOutPassword) != 1)
                {
                    ViewData["msg"] = "帳號/密碼 輸入錯誤";
                    Response.Redirect("noAuthority");
                }

                //取得診所名稱
                if (string.IsNullOrEmpty(clinicIndex) == false)
                {
                    viewClinic viewClinicObj = objModel.getClinicName(clinicIndex); 
                    ViewBag.clinicName = viewClinicObj.clinicName;
                }

                //產品類別
                IEnumerable<viewServFile> productTypes = objModel.getProductType(); 
                ViewBag.username = strOutUsername;
                ViewBag.clinicIndex = clinicIndex;

                viewServFile defaultServFile = (viewServFile)(productTypes).Where(q => q.FileName.Contains("Cooper")).FirstOrDefault();

                //預設值
                if (string.IsNullOrEmpty(fromICD10))
                {
                    ViewBag.announceIkey = defaultServFile.ikey;
                    ViewBag.fileName = defaultServFile.FileName;
                    ViewBag.fileVersion = defaultServFile.FileVersion;
                }
                else
                {
                    viewServFile viewIcd10 = objModel.getICD10Type();  //預設取得icd10 的預設值                   
                    ViewBag.announceIkey = viewIcd10.ikey;
                    ViewBag.fileName = viewIcd10.FileName;
                    ViewBag.fileVersion = viewIcd10.FileVersion;
                }
                ViewBag.bugGrades = bugGrades;
                return View(ViewBag);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public ActionResult ListBugs()
        {
            try
            {
                string username = "";
                if (Session != null)
                {
                    if (string.IsNullOrEmpty(Session["username"].ToString()))
                    {
                        return RedirectToAction("Login", new
                        {
                            username = "",
                            password = ""
                        });
                    }
                }
                username = Session["username"].ToString();
                ViewBag.username = username;
                ViewBag.options = options;
                ViewBag.bugGrades = bugGrades;
                return View(ViewBag);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpPost]
        public ActionResult ListBugs(string announceIkey, string userID, string clinicName,string reportName,string bugContext ,string startDate, string endDate, string status)
        {
            try
            {
                DateTime? sDTime;
                DateTime? eDTime;
                convertToTime(startDate, endDate, out sDTime, out eDTime);
                var result1 = entityTestObj.listAllBugs(announceIkey, userID, clinicName, reportName, bugContext, sDTime, eDTime, status,null);
                return Json(result1);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public ActionResult RDListBugs(int? intIkey)
        {
            try
            {
              ViewBag.options = options;
              ViewBag.bugGrades = bugGrades;
              ViewBag.listBugs = "";
              if (intIkey != null )
              {
                  var listBugs = entityTestObj.listAllBugs(intIkey);
                  ViewBag.listBugs = Json(listBugs);
              }

              return View(ViewBag);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpPost]
        public ActionResult RDListBugs(string announceIkey, string userID, string clinicName,string startDate, string endDate, string status,string context,int? bugGrade)
        {
            try
            {
                DateTime? sDTime;
                DateTime? eDTime;
                convertToTime(startDate, endDate, out sDTime, out eDTime);
                var result1 = entityTestObj.listAllBugs(announceIkey, userID, clinicName,"",context, sDTime, eDTime, status, bugGrade);
                return Json(result1);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpPost]
        public ActionResult statistics(string announceIkey, string userID, string clinicName, string reportName, string startDate, string endDate, string status, string context, int? bugGrade)
        {
            try
            {
                DateTime? sDTime;
                DateTime? eDTime;
                convertToTime(startDate, endDate, out sDTime, out eDTime);
                List<viewBug> result1 = entityTestObj.statistics(announceIkey, userID, clinicName, reportName, context, sDTime, eDTime, status, bugGrade).ToList();

                //找出產品的分類
                var productCategories = from p in result1
                                             group p by new
                                             {
                                                 p.fileName
                                             } into g
                                             select new
                                             {
                                                 Product = g.Key.fileName,
                                                 QTY = g.Sum(p => p.totalCount)
                                             };

                List<viewStatics> viewStaticsObj = new List<viewStatics>();
                foreach (var productObj in productCategories)
                {
                   
                    //找出產品 --> BUG等級
                    var gradeCategories = from grade in result1
                                          where grade.fileName == productObj.Product
                                          group grade by new
                                            {
                                                grade.grade
                                            } into g
                                            select new
                                            {
                                                Grade = g.Key.grade,
                                                QTY = g.Sum(p => p.totalCount)
                                            };
                    //找出產品bug的分類等級
                    foreach (var gradeObj in gradeCategories)
                    {
                        //已完成筆數
                        var completeStatics = from complete in result1
                                              where complete.fileName == productObj.Product && complete.grade ==
                                              gradeObj.Grade && complete.status == "已完成"
                                              group complete by new
                                              {
                                                  complete.fileName,
                                                  complete.grade
                                              } into g
                                              select new
                                              {
                                                  QTY = g.Sum(p => p.totalCount)
                                              };

                        var noDealStatics = from complete in result1
                                              where complete.fileName == productObj.Product && complete.grade == gradeObj.Grade
                                              && complete.status == "暫援"
                                              group complete by new
                                              {
                                                  complete.fileName,
                                                  complete.grade
                                              } into g
                                              select new
                                              {
                                                  QTY = g.Sum(p => p.totalCount)
                                              };

                        var noCompleteStatics = from complete in result1
                                            where complete.fileName == productObj.Product && complete.grade == gradeObj.Grade
                                            && (complete.status != "暫援" && complete.status != "已完成" && complete.status != "退回")
                                            group complete by new
                                            {
                                                complete.fileName,
                                                complete.grade
                                            } into g
                                            select new
                                            {
                                                QTY = g.Sum(p => p.totalCount)
                                            };

                        int intNotComplete = (noCompleteStatics.FirstOrDefault() == null) ? 0 : noCompleteStatics.First().QTY;
                        int intComplete = (completeStatics.FirstOrDefault() == null) ? 0 : completeStatics.First().QTY;
                        int intNotDeal = (noDealStatics.FirstOrDefault() == null) ? 0 : noDealStatics.First().QTY;

                        viewStaticsObj.Add(new viewStatics()
                        {
                            ProductName = productObj.Product,
                            bugGrade = gradeObj.Grade,
                            complete = intComplete,
                            notComplete = intNotComplete,
                            notDeal = intNotDeal
                        });
                    }
                }


                var totalComplete = viewStaticsObj.Sum(x=>x.complete);
                var totalNotComplete = viewStaticsObj.Sum(x => x.notComplete);
                var totalNotDeal = viewStaticsObj.Sum(x => x.notDeal);

                var summaryStatics = from summary in viewStaticsObj
                                     group summary by new
                                     {
                                         summary.ProductName,
                                         summary.bugGrade
                                     } into PName
                                     select new
                                     {
                                         ProductName = PName.Key.ProductName,
                                         BugGrade = bugGrades.Where(q=>q.key == PName.Key.bugGrade).FirstOrDefault().value,
                                         //BugGrade =( PName.Key.bugGrade==0)?"建議事項" : "BUG",
                                         Complete = PName.Sum(p => p.complete),
                                         NotComplete = PName.Sum(p => p.notComplete),
                                         NotDeal = PName.Sum(p => p.notDeal),
                                         TotalComplete = totalComplete,
                                         TotalNotComplete = totalNotComplete,
                                         TotalNotDeal = totalNotDeal
                                     };

                return Json(summaryStatics);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        

        [HttpPost]
        public async Task<bool> RDChangeBugStatus(int editIkey,int editType ,string estimateDate, string completeDate, string bugStatus, string remark, int editGrade)
        {
            try
            {
                StringWriter writer = new StringWriter();
                HttpUtility.HtmlDecode(remark, writer);
                string decodeText = writer.ToString();

                DateTime? estimateDTime;
                DateTime? completeDTime;
                convertToTime(estimateDate, completeDate, out estimateDTime, out completeDTime);
                return await entityTestObj.changeStatus(editIkey, editType, estimateDTime, completeDTime, bugStatus, decodeText, editGrade);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private static void convertToTime(string estimateDate, string completeDate, out DateTime? estimateDTime, out DateTime? completeDTime)
        {

            if (!string.IsNullOrEmpty(estimateDate))
            {
                estimateDTime = DateTime.Parse(estimateDate);
            }
            else
            {
                estimateDTime = null;
            }

            if (!string.IsNullOrEmpty(completeDate))
            {
                completeDTime = DateTime.Parse(completeDate);
            }
            else
            {
                completeDTime = null;
            }
        }     
               
        public ActionResult Print(string productType, string clinicName, string startDate, string endDate, string status)
        {
            ViewBag.currentYear = DateTime.Now.Year;
            DateTime? sDTime;
            DateTime? eDTime;
            productType = (productType == "null") ? "" : productType;
            clinicName = (clinicName == "null") ? "" : clinicName;
            convertToTime(startDate, endDate, out sDTime, out eDTime);
            IEnumerable<viewBug> Result = entityTestObj.listAllBugs(productType, "", clinicName,"","", sDTime, eDTime, status,null);
            return View(Result);
        }

        public ActionResult noAuthority()
        {
            try
            {
                 return View();
            }
            catch (Exception ex)
            {   
                throw new Exception(ex.Message);
            }
        }

        public ActionResult Login()
        {
            try
            {
                
                return View();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            try
            {
                Session["username"] = username;
                Session["password"] = password;

                return RedirectToAction("index", new
                {
                    username = "",
                    password = ""
                });
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public ActionResult Official()
        {
             return View();
        }
     
    }
}
using ManageFileVersion.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ManageFileVersion.Controllers
{
    public class TestController : Controller
    {
        // GET: Test
        entityTestData entityTestObj = new entityTestData(); //呼叫實體層

        public ActionResult Index()
        {
            string strDomainURL = Request.Url.Authority;

            string username="";
            string password = "";
            if (!string.IsNullOrEmpty(Session["username"].ToString()) && !string.IsNullOrEmpty(Session["password"].ToString()))
            {
                username = Session["username"].ToString();
                password = Session["password"].ToString();
            }
            else
            {
                Response.Redirect("../Home/login");
            }

            ViewBag.username = username;
            return View(ViewBag);
        }

        #region 後台測試管理
        public async Task<ActionResult> Announce()
        {
            int totalCount = await entityTestObj.totalCount();           
            ViewBag.totalCount = await entityTestObj.pageCount(totalCount);
            return View(ViewBag);
        }


        [HttpPost]
        public async Task<bool> Announce(string addProduceType, string addFileName, string addFileVersion, string addContext, string addFileTestPath, IEnumerable<HttpPostedFileBase> addDocumentFile)
        {
            //var path = HttpContext.Current.Server.MapPath("~/documentFile");
            string path = "documentFile";
            string saveFileName = await entityTestObj.fileUpload(addDocumentFile, path);
            return await entityTestObj.add(addProduceType, addFileName, addFileVersion, addContext, addFileTestPath, saveFileName);
            //return View();
        }

        [HttpPost]
        public async Task<bool> EditAnnounce(int editIkey, string editProductType, string editFileName, string editFileVersion, string editContext, string editFileTestPath, IEnumerable<HttpPostedFileBase> editDocumentFile, int editIsRease)
        {
            string path = "documentFile";
            string saveFileName = await entityTestObj.fileUpload(editDocumentFile, path);
            return await entityTestObj.edit(editIkey, editProductType, editFileName, editFileVersion, editContext, editFileTestPath, saveFileName, editIsRease);
            //return View();
        }

        [HttpPost]
        public async Task<bool> DeleteAnnounce(int deleteIkey)
        {
            return await entityTestObj.delete(deleteIkey);
            //return View();
        }


        [HttpPost]
        public ActionResult SearchAnnounce(int currentPage, string searchProductType, string searchFileName, string searchFileVersion)
        {
            var result = entityTestObj.search(currentPage, searchProductType, searchFileName, searchFileVersion);
            return Json(result);
        } 
        #endregion


        //[HttpPost]
        //public async Task<bool> AddBug(string addBugUid, int addBugAnnounceIkey, string addBugContext, IEnumerable<HttpPostedFileBase> addBugDocumentFile, int addBugType)
        //{
        //    string path = "bugFile";
        //    string saveFileName = await entityTestObj.fileUpload(addBugDocumentFile, path);
        //    //return await entityTestObj.addBug(addBugUid, "", addBugAnnounceIkey, addBugContext,saveFileName, addBugType);
        //}

        [HttpPost]
        public async Task<bool> AddReleaseBug(string addBugUid, string addClinicIndex, int addBugAnnounceIkey, string addBugfileVersion, string addBugContext, int addBugType, int addBugGrade, IEnumerable<HttpPostedFileBase>[] photo)
        {
            StringWriter writer = new StringWriter();
            HttpUtility.HtmlDecode(addBugContext, writer);
            string decodeText = writer.ToString();

            //依序分為產品別，年度
            string path = "bugFile/" + addBugAnnounceIkey+"/" + DateTime.Now.Year;
            int ikey = await entityTestObj.addReleaseBug(addBugUid, addClinicIndex, addBugAnnounceIkey, addBugfileVersion, decodeText, "", addBugType, addBugGrade);

            string strMultiFiles = "";
            try
            {
                if (photo != null)
                {
                    foreach (IEnumerable<HttpPostedFileBase> uploadImage in photo)
                    {
                        strMultiFiles += await entityTestObj.fileUpload(ikey,uploadImage, path) + ",";
                    }
                    strMultiFiles = strMultiFiles.Substring(0, strMultiFiles.Length - 1);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return await entityTestObj.editBugFileName(ikey, strMultiFiles);
        }


        [HttpPost]
        public async Task<bool> EditBug(int editIkey, string editBugContext, IEnumerable<HttpPostedFileBase> editBugDocumentFile, int editBugType)
        {
            string path = "bugFile";
            string saveFileName = await entityTestObj.fileUpload(editBugDocumentFile, path);
            return await entityTestObj.editBug(editIkey, editBugContext, saveFileName, editBugType);
        }

        [HttpPost]
        public async Task<bool> EditReleaseBug(int editIkey, int productTypeID, string clinicIndex, string fileVersion, string editBugContext, IEnumerable<HttpPostedFileBase>[] editBugDocumentFile, int editBugType, int editBugGrade)
        {

            StringWriter writer = new StringWriter();
            HttpUtility.HtmlDecode(editBugContext, writer);
            string decodeText = writer.ToString();

            string path = "bugFile/" + productTypeID + "/" + DateTime.Now.Year;   
            //string path = "bugFile";
            string strMultiFiles = "";

            try
            {
                if (editBugDocumentFile != null)
                {
                    foreach (IEnumerable<HttpPostedFileBase> uploadImage in editBugDocumentFile)
                    {
                        strMultiFiles += await entityTestObj.fileUpload(editIkey,uploadImage, path) + ",";
                    }
                    strMultiFiles = strMultiFiles.Substring(0, strMultiFiles.Length - 1);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return await entityTestObj.editReleaseBug(editIkey, productTypeID, clinicIndex, fileVersion, decodeText, strMultiFiles, editBugType, editBugGrade);
        }

        [HttpPost]
        public async Task<bool> DeleteBug(int deleteIkey)
        {
            return await entityTestObj.deleteBug(deleteIkey);
        }
    }
}
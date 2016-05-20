using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ManageFileVersion;
using ManageFileVersion.Controllers;
using ManageFileVersion.Models;
using System.Threading.Tasks;

namespace ManageFileVersion.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {
        entityTestData data = new entityTestData();

        [TestMethod]
        public void  Index()
        {
            // Arrange
            HomeController controller = new HomeController();
            test();
        }

        public async void test()
        {
            entityTestData data = new entityTestData();
            int result = await data.pageCount(30);
        }

         [TestMethod]
        public void listAllBug()
        {
            IEnumerable<viewBug> query = data.listAllBugs("1", "0137", "", "","", null, null, "1",null);
            Assert.IsNotNull(query);
        }

         [TestMethod]
         public void readBugsHaveData()
         {
             int ikey = data.maxIkey();
             viewBug viewBugObj = data.readBug(ikey);
             Assert.IsNotNull(viewBugObj);
         }

         [TestMethod]
         public void readBugsNoData()
         {
             viewBug viewBugObj = data.readBug(0);
             Assert.IsNull(viewBugObj);
         }

         [TestMethod]
         public void readBugsEditData()
         {
             int ikey = data.maxIkey();
             Task<bool> result = data.editBug(ikey, "this is a test", "", 1);
            
         }

         [TestMethod]
         public void editReleaseBugData()
         {
             int ikey = data.maxIkey();
             Task<bool> result = data.editReleaseBug(ikey, 2, "00001", "1.0.0.0", "this is a test editRelease", "", 1,1);
         }


         [TestMethod]
         public void deleteReleaseBugData()
         {
             int ikey = data.maxIkey();
             Task<bool> result = data.deleteBug(ikey);
         }

         [TestMethod]
         public void addReleaseBugData()
         {
             Task<int> result = data.addReleaseBug("0137","00001",1,"1.0.0.0","this is test for add releaseBug","",1,1);             
         }

         [TestMethod]
         public void changeBugStatus()
         {
             int ikey = data.maxIkey();
             Task<bool> result = data.changeStatus(ikey,1, DateTime.Today, DateTime.Today, "已完成", "this is test for change Bug status",0);
         }
      
    }
}

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ManageFileVersion.Models;
using System.Collections.Generic;

namespace ManageFileVersion.Tests.Controllers
{
    [TestClass]
    public class UnitTest1
    {
        entityTestData data = new entityTestData();

        [TestMethod]
        public async void TestMethod1()
        {
           
            await data.pageCount(30);

            //Console.Write(result);
        }

        [TestMethod]
        public void TestListAllBugs()
        {            
             IEnumerable<viewBug>  query = data.listBugs(1,1);             

        }
    }
}

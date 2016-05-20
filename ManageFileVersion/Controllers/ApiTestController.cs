using ManageFileVersion.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace ManageFileVersion.Controllers
{
    public class ApiTestController : ApiController
    {
        entityTestData entityTestObj = new entityTestData(); //呼叫實體層
        ServfileModel servFile = new ServfileModel();

        // GET api/<controller>
        public IEnumerable<viewAnnounce> Get()
        {
            return entityTestObj.listNewNotice();
        }

        //[ActionName("readTestNote")]
        [HttpGet]
        [Route("api/ApiTest/readTestNote/{announceIkey}")]
        public viewAnnounce Get(int announceIkey)
        {
            return entityTestObj.read(announceIkey);
        }

        //[ActionName("listBugNote")]
        [HttpGet]
        [Route("api/ApiTest/listBugNote/{announceIkey}/bugType/{bugType}")]
        public IEnumerable<viewBug> listBugNote(int announceIkey, int bugType)
        {
            return entityTestObj.listBugs(announceIkey, bugType);
        }

        [HttpGet]
        [Route("api/ApiTest/readBugRelease/{ikey}")]
        public viewBug readBugRelease(int ikey)
        {
            ServfileModel servFile = new ServfileModel();
            return servFile.readRelease(ikey);
        }

        [HttpGet]
        [Route("api/ApiTest/listClinics/{clinicName}")]
        public IEnumerable<viewClinic> listClinics(string clinicName)
        {
            ServfileModel servFile = new ServfileModel();
            return servFile.listClinics(clinicName);
        }


        [HttpGet]
        [Route("api/ApiTest/getProductTypes")]
        public IEnumerable<viewServFile> getProductTypes()
        {
            return servFile.getProductType();
        }

        [HttpGet]
        [Route("api/ApiTest/getProductVersion/{productType}")]
        public string getProductVersion(string productType)
        {
            return servFile.getFileVersion(productType);
        }

        [HttpGet]
        [Route("api/ApiTest/readEditBug/{editIkey}")]
        public viewBug readEditBug(int editIkey)
        {
            return entityTestObj.readBug(editIkey);
        }


      

        // POST api/<controller>
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}
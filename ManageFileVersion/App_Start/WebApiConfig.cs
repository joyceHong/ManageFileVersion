using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace ManageFileVersion
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API 設定和服務

            // Web API 路由
            config.MapHttpAttributeRoutes();

            //config.Routes.MapHttpRoute(
            //    name: "DefaultApi",
            //    routeTemplate: "api/{controller}/{currentPage}/{order}/{orderType}",
            //    defaults: new { id = RouteParameter.Optional }
            //);

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new
                {
                    id = RouteParameter.Optional
                }
            );

           //  config.Routes.MapHttpRoute(
           //    name: "ActionApi",
           //    routeTemplate: "api/{controller}/{action}/{ikey}",
           //    defaults: new {action = "Get"}
           //);

            config.Routes.MapHttpRoute(
             name: "ActionApi",
             routeTemplate: "api/{controller}/readTestNote/{announceIkey}",
             defaults: new
             {
                 action = "readTestNote"
             });


            config.Routes.MapHttpRoute(
             name: "BugNoteApi",
             routeTemplate: "api/{controller}/listBugNote/{announceIkey}/bugType/{bugType}",
             defaults: new
             {
                 action = "listBugNote"
             });

            config.Routes.MapHttpRoute(
            name: "BugReleaseApi",
            routeTemplate: "api/{controller}/readBugRelease/{ikey}",
            defaults: new
            {
                action = "readBugRelease"
            });

            config.Routes.MapHttpRoute(
            name: "listClinics",
            routeTemplate: "api/{controller}/listClinics/{clinicName}",
            defaults: new
            {
                action = "listClinics"
            });

            config.Routes.MapHttpRoute(
            name: "getProductTypes",
            routeTemplate: "api/ApiTest/getProductTypes",
            defaults: new
            {
                action = "getProductTypes"
            });

            config.Routes.MapHttpRoute(
            name: "getProductVersion",
            routeTemplate: "api/ApiTest/getProductVersion/{productType}",
            defaults: new
            {
                action = "getProductVersion"
            });

            config.Routes.MapHttpRoute(
            name: "readEditBug",
            routeTemplate: "api/ApiTest/readEditBug/{editIkey}",
            defaults: new
            {
                action = "readEditBug"
            });
        }
    }
}

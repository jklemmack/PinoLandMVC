using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace PinoLandMVC4
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            // **************************************************
            // Routes to handle mapping actions
            // **************************************************
            routes.MapRoute(
                name: "FullMapScaled",
                url: "Tile/FullMap/{id}/{height}/{width}",
                defaults: new { controller = "Tile", action = "GetFullMap" }
            );

            routes.MapRoute(
                name: "Tiles",
                url: "Tile/{id}/{z}/{x}/{y}",
                defaults: new { controller = "Tile", action = "Get" }
            );


            routes.MapRoute(
                name: "MarkerPoints",
                url: "Tile/Points/{id}/{skip}/{take}",
                defaults: new { controller = "Tile", action = "Points" }
            );

            // **************************************************
            // Routes for the student controller
            // **************************************************
            routes.MapRoute(
                name: "StudentIndex",
                url: "Student/{action}",
                defaults: new { controller = "Student", action="Index" }
            );

            routes.MapRoute(
                name: "StudentActions",
                url: "Student/{economy}/{team}/{action}",
                defaults: new { controller = "Student" }
            );

            // **************************************************
            // Routes for the manager controller
            // **************************************************
            routes.MapRoute(
                name: "MangerDetails",
                url: "Manager/Details/{id}/{tab}",
                defaults: new { controller = "Manager", action = "Details", tab = UrlParameter.Optional }
            );


            //Fallback, default route
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
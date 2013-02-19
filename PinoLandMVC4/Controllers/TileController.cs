using System;
using System.Collections.Generic;
//using System.Data.Spatial;
using System.Drawing;
//using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Microsoft.MapPoint;
using MG = Fuqua.CompetativeAnalysis.MarketGame;
using PinoLandMVC4.Models;


namespace PinoLandMVC4.Controllers
{
    public class TileController : Controller
    {
        static PinolandBounds LLBOUNDS = new PinolandBounds() { top = 1, right = 1, bottom = -1, left = -1 };
        static Dictionary<int, PinolandBounds> PINOLAND_BOUNDS;
        static Dictionary<int, Rectangle> PINOLAND_RECTS;

        

        static TileController()
        {


            //Pre-cache some calculations
            PINOLAND_BOUNDS = new Dictionary<int, PinolandBounds>();
            PINOLAND_RECTS = new Dictionary<int, Rectangle>();
            for (int zoom = 0; zoom <= 18; zoom++)
            {
                int x1, y1, x2, y2;
                TileSystem.LatLongToPixelXY(LLBOUNDS.top, LLBOUNDS.left, zoom, out x1, out y1);
                TileSystem.LatLongToPixelXY(LLBOUNDS.bottom, LLBOUNDS.right, zoom, out x2, out y2);

                PINOLAND_BOUNDS.Add(zoom, new PinolandBounds() { left = x1, right = x2, top = y1, bottom = y2 });
                PINOLAND_RECTS.Add(zoom, new Rectangle(x1, y1, x2 - x1, y2 - y1));
            }
        }

        [HttpGet]
        public FileResult Get(int id, int z, int x, int y)
        {
            try
            {
                int nwX;
                int nwY;

                TileSystem.TileXYToPixelXY(x, y, out nwX, out nwY);

                double nwLatitude, nwLongitude;
                double seLatitude, seLongitude;

                TileSystem.PixelXYToLatLong(nwX, nwY, z, out nwLatitude, out nwLongitude);
                TileSystem.PixelXYToLatLong(nwX + 256, nwY + 256, z, out seLatitude, out seLongitude);

                // The target image to render out
                var tile = new Bitmap(256, 256, PixelFormat.Format32bppArgb);

                // Create a drawing surface on the target
                Graphics g = Graphics.FromImage(tile);

                // Fill it with blue by default
                g.FillRectangle(new SolidBrush(Color.Blue), new Rectangle(0, 0, 256, 256));

                //Test if our tile intersects with PinoLand map.   Do this for efficiency so we don't do
                // a bitmap copy unless necessary
                if (PINOLAND_RECTS[z].IntersectsWith(new Rectangle(nwX, nwY, 256, 256)))
                {
                    // TODO: 
                    // pinolandImage is single-threaded, which is why we load it dynamically each call
                    // it barfs on the g.DrawImage with multi-threaded access 

                    Image pinolandImage = Image.FromFile(Server.MapPath("~/Content/PinoLand1000x1000.png"));
                    //Source is the entire Pinoland image - get the rectangle for it
                    Rectangle src = new Rectangle(0, 0, pinolandImage.Size.Width, pinolandImage.Size.Height);

                    //Dest is our location, zoom-adjusted, in the 256x256 tile
                    Rectangle dest = new Rectangle(PINOLAND_BOUNDS[z].left - nwX,
                                                    PINOLAND_BOUNDS[z].top - nwY,
                                                    PINOLAND_BOUNDS[z].right - PINOLAND_BOUNDS[z].left,
                                                    PINOLAND_BOUNDS[z].bottom - PINOLAND_BOUNDS[z].top);


                    //And draw Pinoland
                    lock (pinolandImage)    // Avoid multi-threaded access, which gives Image conniptions
                        g.DrawImage(pinolandImage, dest, src, GraphicsUnit.Pixel);
                }
                var ms = new MemoryStream();
                tile.Save(ms, ImageFormat.Png);

                //string fileName = string.Format("~/Content/Tiles/map.{0}.{1}.{2}.png", x, y, z);
                //using (FileStream fs = new FileStream(Server.MapPath(fileName), FileMode.Create))
                //{
                //    fs.Write(ms.ToArray(), 0, (int)ms.Length);
                //}

                ms.Position = 0;
                return File(ms.ToArray(), "image/png");
            }
            catch (Exception ex)
            {

            }
            return null;
        }

        [HttpGet]
        public FileResult GetFullMap(int id, int height, int width)
        {
            Image pinolandImage = Image.FromFile(Server.MapPath("~/Content/PinoLand1000x1000.png"));
            Image targetImage = new Bitmap(width, height);

            Graphics g = Graphics.FromImage(targetImage);
            //g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.
            //g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            lock (pinolandImage)
                g.DrawImage(pinolandImage, new Rectangle(0, 0, width, height));

            var ms = new MemoryStream();
            targetImage.Save(ms, ImageFormat.Png);

            ms.Position = 0;
            return File(ms.ToArray(), "image/png");
        }

        public ActionResult Index()
        {
            return View(LLBOUNDS);
        }

        [HttpGet]
        public dynamic Points(int id, int skip, int take)
        {

            using (MG.GameDataObjectContext context = ContextHelper.GetObjectContext())
            {
                var points = context.Households.Where(h => h.EconomyId == id)
                    .OrderBy(h => h.HouseholdId)
                    .Skip(skip).Take(take)
                    .Select(h => new { Lat = (float)h.Latitude, Lng = (float)h.Longitude, Id = h.Identifier }).ToList();
                return Json(points, JsonRequestBehavior.AllowGet);

            }
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using PinoLandMVC4;
using MG = Fuqua.CompetativeAnalysis.MarketGame;

namespace PinoLandMVC4.Controllers
{
    public class ManagerController : Controller
    {
        //
        // GET: /Manager/

        [Authorize(Roles = "Manager")]
        public ActionResult Index()
        {
            IList economies = null;
            using (MG.GameDataObjectContext context = ContextHelper.GetObjectContext())
            {
                economies = context.Economies.ToList();
            }
            return View(economies);
        }

        [Authorize(Roles = "Manager")]
        public ActionResult Details(int id, string selectedTab)
        {
            MG.Economy economy = null;
            using (MG.GameDataObjectContext context = ContextHelper.GetObjectContext())
            {
                economy = context.Economies
                    //.Include("Companies")
                    //.Include("Industries")
                    .Where(e => e.EconomyId == id).SingleOrDefault();

                using (System.IO.TextWriter tw = new System.IO.StringWriter())
                {
                    new Newtonsoft.Json.JsonSerializer().Serialize(tw, economy);
                    ViewBag.Economy = tw.ToString();
                }

            }
            // Query to load the economy
            return View(economy);
        }

        [Authorize(Roles = "Manager")]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Manager")]
        public ActionResult Create(PinoLandMVC4.Models.EconomyModel model)
        {
            if (ModelState.IsValid)
            {
                using (MG.GameDataObjectContext context = ContextHelper.GetObjectContext())
                {

                    MG.Economy economy = new MG.Economy();
                    economy.Name = model.Name;
                    economy.DateCreated = DateTime.UtcNow;
                    economy.Reference = model.Reference;

                    #region Create default Age & Wealth bins and their distribution profiles

                    economy.Ages.Add(new MG.Age() { Name = "Young", DisplayOrder = 0 });
                    economy.Ages.Add(new MG.Age() { Name = "Middle", DisplayOrder = 1 });
                    economy.Ages.Add(new MG.Age() { Name = "Old", DisplayOrder = 2 });
                    economy.Wealths.Add(new MG.Wealth() { Name = "Poor", DisplayOrder = 0 });
                    economy.Wealths.Add(new MG.Wealth() { Name = "Middle", DisplayOrder = 1 });
                    economy.Wealths.Add(new MG.Wealth() { Name = "Rich", DisplayOrder = 2 });


                    //And a map profiles for Urban, suburban, and Rural
                    MG.Profile profileU = economy.CreateProfile("Urban", new double[] { .1, .2, .1, .1, .1, .1, .1, .1, .1 });
                    MG.Profile profileS = economy.CreateProfile("Suburban", new double[] { .1, .39, .01, .1, .3, .1, 0, 0, 0 });
                    MG.Profile profileR = economy.CreateProfile("Rural", new double[] { .3, .25, .05, .2, .1, .1, 0, 0, 0 });


                    #endregion

                    #region Locations
                    //These are lat/lng coords of the bounding rectangle.  Perhaps replace with a simple map in the future...
                    //14.2, -77.5, 1.0, 1.0, 10, 10
                    double _top = 14.2;
                    double _left = -77.5;
                    double _height = 1.0;
                    double _width = 1.0;
                    int cols = 10;
                    int rows = 10;

                    for (int x = 1; x <= 10; x++)
                        for (int y = 1; y <= 10; y++)
                        {
                            MG.Location loc = new MG.Location() { CenterX = x, CenterY = y, Profile = profileU, TotalPopulation = 50 };
                            loc.Identifier = string.Format("{0}{1}", (char)('A' + x - 1), y);

                            economy.Locations.Add(loc);

                            double lat1 = _top - ((int)loc.CenterY - 1) * _height / rows;
                            double lng1 = _left + ((int)loc.CenterX - 1) * _width / cols;

                            double lat2 = _top - ((int)loc.CenterY) * _height / rows;
                            double lng2 = _left + ((int)loc.CenterX - 1) * _width / cols;

                            double lat3 = _top - ((int)loc.CenterY) * _height / rows;
                            double lng3 = _left + ((int)loc.CenterX) * _width / cols;

                            double lat4 = _top - ((int)loc.CenterY - 1) * _height / rows;
                            double lng4 = _left + ((int)loc.CenterX) * _width / cols;

                            context.SetLocationShape(loc.LocationId, lng1, lat1, lng2, lat2, lng3, lat3, lng4, lat4);
                        }

                    #endregion

                    #region  Create the default food industry
                    MG.Food_Industry foodIndustry = new MG.Food_Industry()
                    {
                        CanHoldInventory = false,
                        CapacityCostMean = 5000,
                        CapacityCostStdDev = 1000,
                        CapacityDecayRate = 0.0,
                        CapacityResaleRate = 0.5,
                        MaintenanceCostMean = 500,
                        MaintenanceCostStdDev = 100,
                        InventoryCostMean = 10,
                        InventoryCostStdDev = 5,
                        Economy = economy,
                        Elasticity = 0.5,
                        EntryCostMean = 50000,
                        EntryCostStdDev = 5000,
                        MarginalCostMean = 10,
                        MarginalCostStdDev = 2,
                        Name = "Restaurant",
                        Type = "Food"
                    };


                    foodIndustry.Food_Industry_Good_Type.Add(new MG.Food_Industry_Good_Type() { Name = "Junk" });
                    foodIndustry.Food_Industry_Good_Type.Add(new MG.Food_Industry_Good_Type() { Name = "Ethnic" });
                    foodIndustry.Food_Industry_Good_Type.Add(new MG.Food_Industry_Good_Type() { Name = "Healthy" });

                    // Basis for the half-normal curve distribution
                    foodIndustry.AddProfileSettings(profileU, "Sigma", new double[] { 50, 100, 200, 50, 100, 200, 55, 150, 350 });
                    //foodIndustry.AddProfileSettings(profileS, "Sigma", new double[] { 50, 100, 200, 55, 150, 350 });
                    //foodIndustry.AddProfileSettings(profileR, "Sigma", new double[] { 50, 100, 200, 55, 150, 350 });

                    //The "cost" for choosing a type different that what is preferred
                    foodIndustry.AddProfileSettings(profileU, "SensitivityType", new double[] { 1, 20, 50, 1, 20, 50, 20, 50, 200 });
                    //foodIndustry.AddProfileSettings(profileS, "SensitivityType", new double[] { 1, 20, 50, 20, 50, 200 });
                    //foodIndustry.AddProfileSettings(profileR, "SensitivityType", new double[] { 1, 20, 50, 20, 50, 200 });

                    //This is cost per "mile".  
                    foodIndustry.AddProfileSettings(profileU, "SensitivityDistance", new double[] { 20, 10, 5, 25, 12, 3, 30, 15, 2 });
                    //foodIndustry.AddProfileSettings(profileS, "SensitivityDistance", new double[] { 20, 10, 5, 30, 15, 2 });
                    //foodIndustry.AddProfileSettings(profileR, "SensitivityDistance", new double[] { 20, 10, 5, 30, 15, 2 });

                    #endregion

                    #region Create a frozen food manufacturing industry

                    //MG.Food_Industry frozenFood = new MG.Food_Industry()
                    //{
                    //    CanHoldInventory = true,
                    //    CapacityCostMean = 10000,
                    //    CapacityCostStdDev = 1000,
                    //    CapacityDecayRate = 0.0,
                    //    CapacityResaleRate = 0.5,
                    //    MaintenanceCostMean = 500,
                    //    MaintenanceCostStdDev = 100,
                    //    InventoryCostMean = 10,
                    //    InventoryCostStdDev = 5,
                    //    Economy = economy,
                    //    Elasticity = 0.2,
                    //    EntryCostMean = 200000,
                    //    EntryCostStdDev = 25000,
                    //    MarginalCostMean = 10,
                    //    MarginalCostStdDev = 2,
                    //    Name = "Frozen Food",
                    //    Type = "Food"
                    //};

                    #endregion

                    context.AddToEconomies(economy);    //Add our root object to the context manager, so it will save
                    context.SaveChanges();
                }
                return RedirectToAction("Index", "Manager");
            }
            return View();
        }
    }
}

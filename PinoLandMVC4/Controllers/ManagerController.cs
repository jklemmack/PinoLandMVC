using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.EntityClient;
using System.Data.Objects;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using PinoLandMVC4;
using MG = Fuqua.CompetativeAnalysis.MarketGame;
using Microsoft.Samples.EntityDataReader;



namespace PinoLandMVC4.Controllers
{
    public class ManagerController : Controller
    {
        object lockObject = new object();
        Bitmap pinoLandImage;
        Dictionary<MG.Profile, List<Point>> profilePoints = new Dictionary<MG.Profile, List<Point>>();


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

            ViewBag.EconomyId = economy.EconomyId;
            // Query to load the economy
            return View(new PinoLandMVC4.Helpers.PageModel<MG.Economy>(economy, ""));
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
                    MG.Profile profileU = economy.CreateProfile("Urban", 20000, new double[] { .1, .2, .1, .1, .1, .1, .1, .1, .1 });
                    MG.Profile profileS = economy.CreateProfile("Suburban", 20000, new double[] { .1, .39, .01, .1, .3, .1, 0, 0, 0 });
                    MG.Profile profileR = economy.CreateProfile("Rural", 10000, new double[] { .3, .25, .05, .2, .1, .1, 0, 0, 0 });


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

        [HttpGet]
        [Authorize(Roles = "Manager")]
        public ActionResult StartGame(int id)
        {
            using (MG.GameDataObjectContext context = ContextHelper.GetObjectContext())
            {
                MG.Economy economy = context.Economies.Single(e => e.EconomyId == id);
                MG.Economy.InitializeCallbacks callbacks = new MG.Economy.InitializeCallbacks();
                callbacks.PositionHousehold = PlaceHousehold;
                economy.Initialize(callbacks);

                context.SaveChanges();

                #region bulk copy Household & Food_Household_Company_Round

                List<ObjectStateEntry> entityStatesHouseholds =
                    context.ObjectStateManager.GetObjectStateEntries(System.Data.EntityState.Added).Where(es => es.EntitySet.Name == "Households").ToList();

                var households = (from es in entityStatesHouseholds
                                  select es.Entity as MG.Household).ToList();

                EntityConnectionStringBuilder connString = new EntityConnectionStringBuilder(context.Connection.ConnectionString);

                SqlBulkCopy bc = new SqlBulkCopy(connString.ProviderConnectionString);
                bc.BulkCopyTimeout = 120;
                bc.BatchSize = 5000;
                bc.DestinationTableName = "Household";
                bc.WriteToServer(households.AsDataReader<MG.Household>());

                //Remove the state-changes for Household
                foreach (ObjectStateEntry entityState in entityStatesHouseholds)
                    entityState.Delete();

                #endregion

                context.SaveChanges();
            }

            Dictionary<string, string> values = new Dictionary<string, string>();
            values.Add("id", id.ToString());
            values.Add("selectedTab", "");
            RedirectToAction("Details", values);
            return null;
        }

        [HttpGet]
        [Authorize(Roles = "Manager")]
        public ActionResult ProcessRound(int id)
        {
            using (RoundProcessor.ProcessRoundServiceClient client = new RoundProcessor.ProcessRoundServiceClient())
            {
                bool? status = client.ProcessRound(id);
            }

            //Dictionary<string, string> values = new Dictionary<string, string>();
            //values.Add("id", id.ToString());
            //values.Add("selectedTab", "");
            RedirectToAction("Index");
            return null;
        }

        /// <summary>
        /// Process a round for an economy.  We never want to process directly - always route through 
        /// the ProcessRound workflow WCF service internally so we can pick up the 
        /// Workflow Engine running on AppFabric
        /// </summary>
        /// <param name="economyId"></param>
        [NonAction]
        public void ProcessRoundInternal(int economyId)
        {
            MG.Economy economy = null;
            using (MG.GameDataObjectContext context = ContextHelper.GetObjectContext())
            {
                economy = context.Economies.Single(e => e.EconomyId == economyId);
                economy.ProcessRound();

                #region bulk copy
                //Food_Household_Company_Round
                var entitySates = context.ObjectStateManager.GetObjectStateEntries(System.Data.EntityState.Added).Where(es => es.EntitySet.Name == "Food_Household_Company_Round");
                var fhcr = (from es in entitySates
                            select es.Entity as MG.Food_Household_Company_Round).ToList();

                //Remove the state-changes for HRM
                foreach (var entityState in entitySates)
                    entityState.Delete();

                EntityConnectionStringBuilder connString = new EntityConnectionStringBuilder(context.Connection.ConnectionString);

                SqlBulkCopy bc = new SqlBulkCopy(connString.ProviderConnectionString);
                bc.BulkCopyTimeout = 60;
                bc.BatchSize = 5000;

                bc.DestinationTableName = "Food_Household_Company_Round";
                bc.WriteToServer(fhcr.AsDataReader<MG.Food_Household_Company_Round>());
                #endregion

                #region bulk copy

                //Food_Industry_Household_Company_Round
                entitySates = context.ObjectStateManager.GetObjectStateEntries(System.Data.EntityState.Added).Where(es => es.EntitySet.Name == "Food_Industry_Household_Company_Round");
                var fihcr = (from es in entitySates
                             select es.Entity as MG.Food_Industry_Household_Company_Round).ToList();

                //Remove the state-changes for HRM
                foreach (var entityState in entitySates)
                    entityState.Delete();

                bc = new SqlBulkCopy(connString.ProviderConnectionString);
                bc.BulkCopyTimeout = 60;
                bc.BatchSize = 5000;

                bc.DestinationTableName = "Food_Industry_Household_Company_Round";
                bc.WriteToServer(fihcr.AsDataReader<MG.Food_Industry_Household_Company_Round>());
                #endregion

                context.SaveChanges();
            }

        }

        /// <summary>
        /// Universal manager update handler to persist UI-driven changes in the database
        /// </summary>
        /// <param name="id"></param>
        /// <param name="collection"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = "Manager")]
        public dynamic Update(int id, FormCollection collection)
        {
            string type = collection["type"];

            try
            {
                if (type == "Profile_Age_Wealth")
                {
                    string age = collection["Age"];
                    string wealth = collection["Wealth"];
                    string value = collection["Value"];
                    string profile = collection["Profile"];
                    double probability = 0.0;

                    using (MG.GameDataObjectContext context = ContextHelper.GetObjectContext())
                    {
                        MG.Profile_Age_Wealth paw = context.Profile_Age_Wealth.SingleOrDefault(x => x.EconomyId == id &&
                                string.Compare(x.Profile.Name, profile) == 0 &&
                                string.Compare(x.Age.Name, age) == 0 &&
                                string.Compare(x.Wealth.Name, wealth) == 0);

                        if (paw != null && double.TryParse(value, out probability))
                            paw.Probability = probability;
                        context.SaveChanges();
                    }
                    return probability;

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return null;
        }

        /// <summary>
        /// Handle upload of a team file
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UploadTeam(int id, HttpPostedFileBase file)
        {

            if (file.ContentLength > 0)
            {
                PinoLandMVC4.Helpers.ExcelHelper.UploadTeamFile(id, file.InputStream);
            }

            return RedirectToAction("Index");
        }

        private MG.Economy.GeoPosition PlaceHousehold(MG.Household h, Dictionary<System.Drawing.Color, MG.Profile> mapping)
        {

            //Singleton gate - for the lifetime of the controller instance (NOT a static variable!!!)
            if (pinoLandImage == null)
            {
                lock (lockObject)
                {
                    if (pinoLandImage == null)
                    {
                        foreach (MG.Profile p in mapping.Values)
                            if (!profilePoints.ContainsKey(p))
                                profilePoints.Add(p, new List<Point>());
                        List<Color> colors = new List<Color>();

                        pinoLandImage = (Bitmap)Image.FromFile(Server.MapPath("~/Content/PinoLand1000x1000.png"));
                        for (int x = 1; x < pinoLandImage.Width; x++)
                        {
                            for (int y = 1; y < pinoLandImage.Height; y++)
                            {
                                Color c = pinoLandImage.GetPixel(x, y);
                                if (!colors.Contains(c))
                                    colors.Add(c);
                                MG.Profile profile = mapping.FirstOrDefault(kvp => kvp.Key.ToArgb() == c.ToArgb()).Value;
                                if (profile != null)
                                {
                                    profilePoints[profile].Add(new Point(x, y));
                                }
                            }
                        }

                    }
                }
            }

            double sample = Fuqua.CompetativeAnalysis.MarketGame.Statistics.Instance.Sample();
            if (profilePoints[h.Profile].Count > 0)
            {
                Point loc = profilePoints[h.Profile][(int)(sample * profilePoints[h.Profile].Count)];

                return new MG.Economy.GeoPosition()
                {
                    Latitude = (float)(1.0 - loc.Y / (pinoLandImage.Height / 2.0)),
                    Longitude = (float)(-1.0 + loc.X / (pinoLandImage.Width / 2.0))
                };
            }


            return new MG.Economy.GeoPosition()
            {
                Latitude = 0.0f,
                Longitude = 0.0f
            };
        }
    }
}

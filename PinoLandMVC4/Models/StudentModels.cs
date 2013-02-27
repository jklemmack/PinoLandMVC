using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PinoLandMVC4.Models
{
    public class StudentModel
    {
        public PinolandBounds MapBounds = PinolandBounds.LLBOUNDS;
        public List<NewsModel> News;
        public List<FoodTeamAction> FoodActions;
    }

    public class StudentGame
    {
        public int EconomyId;
        public string GameName;
        public string GameReference;
        public int TeamId;
        public string TeamName;
    }

    public class NewsModel
    {
        [Display(Name = "Date Posted")]
        public DateTime DatePosted;

        [Display(Name = "Posted By")]
        public string PostedBy;

        [Display(Name = "Subject")]
        public string Subject;

        [Display(Name = "News Item")]
        public string Body;
    }

    public class FoodTeamAction
    {
        public int EconomyId;
        public int RoundId;
        public int TeamId;

        public string Team;

        public int TypeId;
        public string Type;

        public double Latitude;
        public double Longitude;

        public double CapacityNow;
        public double Price;

        // Team input
        public double PriceNext;
        public double ProductionNext;
        public double CapacityChange;

        public bool IsMine;
        public bool IsNew;
    }

}
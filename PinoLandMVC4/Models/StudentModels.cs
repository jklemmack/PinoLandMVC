using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PinoLandMVC4.Models
{
    public class StudentModels
    {

    }

    public class MapModel
    {
        
    }

    public class NewsModel
    {
        [Display(Name="Date Posted")]
        public DateTime DatePosted;

        [Display(Name="Posted By")]
        public string PostedBy;

        [Display(Name = "Subject")]
        public string Subject;

        [Display(Name = "News Item")]
        public string Body;
    }

}
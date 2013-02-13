using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PinoLandMVC4.Models
{
    public class EconomyModel
    {
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "The unique name for this game")]
        public string Name { get; set; }


        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "An easy reference for this game")]
        public string Reference { get; set; }
    }
}
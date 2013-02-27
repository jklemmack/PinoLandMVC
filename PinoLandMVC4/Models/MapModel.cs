using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PinoLandMVC4.Models
{

    public class PinolandBounds
    {
        public int left;
        public int right;
        public int top;
        public int bottom;

        public static PinolandBounds LLBOUNDS = new PinolandBounds() { top = 1, right = 1, bottom = -1, left = -1 };
    }

}
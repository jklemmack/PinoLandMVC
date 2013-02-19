using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PinoLandMVC4.Helpers
{
    public class TabQueue : Queue<int>
    {
        public TabQueue(string querystring)
        {
            //process
            this.Enqueue(1);
            this.Enqueue(2);
        }

        public override string ToString()
        {
            return this.ToList().ToString();
        }
    }

    class PageModel<T> where T : class
    {
        public TabQueue Tabs;
        public T Data;

        public PageModel(T data, string tabs)
        {
            Data = data;
            Tabs = new TabQueue(tabs);
        }

    }
}
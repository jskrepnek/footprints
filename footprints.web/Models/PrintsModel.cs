using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace footprints.web.Models
{
    [Serializable()]
    public class PrintsModel
    {
        public PrintsModel()
        {
            list = new List<PrintModel>();
        }

        public void Add(PrintModel print)
        {
            list.Add (print);
        }

        public List<PrintModel> List
        {
            get
            {
                return list;
            }
        }

        public List<PrintModel> list { get; set; }
    }
}
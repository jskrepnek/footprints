using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace footprints.web.Models
{
    [Serializable()]
    public class PrintsModel
    {
        List<PrintModel> _list;

        public PrintsModel()
        {
            _list = new List<PrintModel>();
        }

        public void Add(PrintModel print)
        {
            List.Add (print);
        }

        public List<PrintModel> List
        {
            get
            {
                return _list;
            }
        }        
    }
}
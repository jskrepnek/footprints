using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace footprints.test.runner
{
    class Program
    {
        static void Main(string[] args)
        {
            var test = new FootprintsControllerTests();

            test.ViewPrintsAction_OnGet_ReturnsViewPrints();
        }
    }
}

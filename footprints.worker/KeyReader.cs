using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace footprints.worker
{
    public class KeyReader : IKeyReader
    {
        public bool KeyAvailable
        {
            get
            {
                return Console.KeyAvailable;
            }
        }
    }
}

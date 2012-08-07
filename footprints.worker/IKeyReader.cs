using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace footprints.worker
{
    public interface IKeyReader
    {
        bool KeyAvailable { get; }
    }
}

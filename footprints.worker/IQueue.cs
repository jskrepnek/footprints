using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace footprints.worker
{
    public interface IQueue
    {
        string GetMessage(out string msgId, out string popReceipt);
        void DeleteMessage(string msgId, string popReceipt);
    }
}

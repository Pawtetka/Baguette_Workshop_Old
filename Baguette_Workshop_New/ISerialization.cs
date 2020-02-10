using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baguette_Workshop_New
{
    interface ISerialization
    {
        void Serialization(Object obj, string path);
        Object Deserialization(string path);
    }
}

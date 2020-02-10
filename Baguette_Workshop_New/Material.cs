using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Baguette_Workshop_New
{
    [DataContract]
    abstract class Material
    {
        [DataMember]
        public int price;
        [DataMember]
        public string materialName;
    }
}

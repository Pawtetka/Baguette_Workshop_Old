using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Baguette_Workshop_New
{
    [DataContract]
    class BaguetteMoulee : Baguette
    {
        public BaguetteMoulee() : base()
        {

        }
        public BaguetteMoulee(int polishLayers, int width, int height, string type) : base(width, height, type)
        {
            baguetteInfo.polishLayers = polishLayers;
        }

    }
}

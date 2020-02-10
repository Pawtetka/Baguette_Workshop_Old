using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Baguette_Workshop_New
{
    [DataContract]
    class BaguetteFarinee : Baguette
    {
        public BaguetteFarinee() : base()
        {

        }
        public BaguetteFarinee(int width, int height, string type) : base(width, height, type)
        {

        }
    }
}

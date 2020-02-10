using System.Runtime.Serialization;

namespace Baguette_Workshop_New
{
    [DataContract]
    class Wood : Material
    {
        public Wood()
        {
            price = 5;
            materialName = "Wood:";
        }
    }
}

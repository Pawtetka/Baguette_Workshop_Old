using System.Runtime.Serialization;

namespace Baguette_Workshop_New
{
    [DataContract]
    class Polish : Material
    {
        public Polish()
        {
            price = 10;
            materialName = "Polish:";
        }
    }
}

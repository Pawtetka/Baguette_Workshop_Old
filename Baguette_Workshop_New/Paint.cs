using System.Runtime.Serialization;

namespace Baguette_Workshop_New
{
    [DataContract]
    class Paint : Material
    {
        public Paint()
        {
            price = 15;
            materialName = "Paint:";
        }
    }
}

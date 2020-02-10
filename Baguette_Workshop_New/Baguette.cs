using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Baguette_Workshop_New
{
    [DataContract]
    [KnownType(typeof(Paint))]
    [KnownType(typeof(Polish))]
    [KnownType(typeof(Wood))]
    abstract class Baguette : IComparable
    {
        /*[DataMember]
        public Dictionary<Material, int> materials = new Dictionary<Material, int>(); //Maybe I need Container here?
        [DataMember]
        public readonly string baguetteType; //I really need it?
        [DataMember]
        public double TimeToDo { get; set; }*/

        [DataMember]
        public BaguetteInfo baguetteInfo;
        public double Size
        {
            get { return this.baguetteInfo.width * this.baguetteInfo.height; } 
            set { if (value <= 0) throw new ArgumentException("Size must be greater than 0"); }
        }

        /*[DataMember]
        public int width;
        [DataMember]
        public int height;
        [DataMember]
        public int polishLayers = 1; */


        public Baguette() { }
        public Baguette(int width, int height, string type)
        {
            baguetteInfo = ReaderFromFile.ReadBaguetteInfo(type);
            baguetteInfo.SetSize(width, height);
        }

        /*private int CountPriceForOne()
        {
            int priceForOne = 0;
            foreach (KeyValuePair<Material, int> keyValue in materials)
            {
                priceForOne += keyValue.Key.price * keyValue.Value;
            }
            return priceForOne;
        }

        public int GetPrice()
        {
            return CountPriceForOne();
        }*/

        public double CountPrice(ICounter counter)
        {
            return counter.GetPrice();
        }

        public int CompareTo(object obj)
        {
            return this.Size.CompareTo((obj as Baguette).Size);
        }

        ~Baguette() { }
    }

    [DataContract]
    class BaguetteInfo
    {
        [DataMember]
        public Dictionary<Material, int> materials = new Dictionary<Material, int>();
        [DataMember]
        public readonly string baguetteType;
        [DataMember]
        public double TimeToDo { get; set; }
        [DataMember]
        public double width;
        [DataMember]
        public double height;
        [DataMember]
        public int polishLayers = 1;

        public BaguetteInfo(string baguetteType, Dictionary<Material, int> materials, double timeToDo)
        {
            this.baguetteType = baguetteType;
            this.materials = materials;
            this.TimeToDo = timeToDo;
        }

        public void SetSize(double width, double height)
        {
            this.width = width;
            this.height = height;
        }
        //public BaguetteInfo(string baguetteType, double width, double height, int polishLayers)
        //{
        //    this.baguetteType = baguetteType;
        //    this.width = width;
        //    this.height = height;
        //    this.polishLayers = polishLayers;
        //}
    }

    interface ICounter
    {
        double GetPrice();
        void CountPrice();
    }

    class BaguettePriceCounter : ICounter
    {
        private double price;
        private Dictionary<Material, int> materials;
        private double size;
        private int polishLayers;

        public BaguettePriceCounter(Baguette baguette)
        {
            this.materials = baguette.baguetteInfo.materials;
            this.size = baguette.Size;
            this.polishLayers = baguette.baguetteInfo.polishLayers;
        }
        public void CountPrice()
        {
            foreach (KeyValuePair<Material, int> material in materials)
            {
                price += material.Key.price * material.Value;
            }
        }

        public double GetPrice()
        {
            CountPrice();
            price *= size * polishLayers;
            return price;
        }
    }


}

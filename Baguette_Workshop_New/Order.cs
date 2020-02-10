using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Baguette_Workshop_New
{
    [DataContract]
    [KnownType(typeof(BaguetteFarinee))]
    [KnownType(typeof(BaguetteMoulee))]
    class Order
    {
        [DataMember]
        public Baguette baguette;
        [DataMember]
        public DateTime readyBy = DateTime.Now;
        [DataMember]
        public int count;
        [DataMember]
        public bool ready = false;
        [DataMember]
        public string surname;
        public Order()
        {

        }
        public Order(int count, Baguette baguette, string surname)
        {
            this.baguette = baguette;
            this.count = count;
            this.surname = surname;
        }
        public Order(int count, Baguette baguette, string surname, DateTime readyBy)
        {
            this.baguette = baguette;
            this.count = count;
            this.surname = surname;
            this.readyBy = readyBy;
        }
        public void OrderReady()
        {
            ready = true;
        }

        public double GetPrice(ICounterOrder counter)
        {
            return counter.CountPrice(baguette.CountPrice(new BaguettePriceCounter(baguette)), count);
        }

        ~Order() { }
    }


    interface ICounterOrder
    {
        double CountPrice(double priceForOne, int count);
    }
    public class OrderPriceCounter : ICounterOrder
    {
        public double CountPrice(double priceForOne, int count)
        {
            return priceForOne * count;
        }
    }
}

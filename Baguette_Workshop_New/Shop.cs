using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;

namespace Baguette_Workshop_New
{
    [DataContract]
    class Shop
    {
        [DataMember]
        public Dictionary<string, double> materialAvailability = new Dictionary<string, double>();//Maybe I need container?
        [DataMember]
        public List<Order> orders = new List<Order>();//I need container...
        private Timer timer;//Class timer? Becouse Single Responsibility
        //private TimerCheck timerCheck;
        /*[DataMember]
        IOrderProcessor processor;
        [DataMember]
        IOrderAdder adder;
        [DataMember]
        IPrinter printer;
        [DataMember]
        ShopInfo shopInfo;*/

        public Shop()
        {
            //StartTimer();
        }
        /*public Shop(IOrderProcessor processor, IOrderAdder adder, IPrinter printer, ShopInfo info)
        {
            this.processor = processor;
            this.adder = adder;
            this.printer = printer;
            this.shopInfo = info;
        }*/
        public void AddMaterials(int wood, int polish, int paint)
        {
            materialAvailability.Add("Wood:", wood);
            materialAvailability.Add("Polish:", polish);
            materialAvailability.Add("Paint:", paint);
        }

        public List<string> ProcessOrder()
        {
            List<string> rezult = new List<string>();
            foreach (Order order in orders)
            {
                rezult.Add("Next Order:\n" + order.baguette.baguetteInfo.baguetteType + "\n");
                rezult.Add("Count: " + Convert.ToString(order.count) + "\n");
                rezult.Add("Price: " + Convert.ToString(CountPrice(order)) + "\n");
                if (order.ready)
                {
                    rezult.Add("This order is ready\n\n");
                }
                else
                {
                    rezult.Add("Be ready on " + Convert.ToString(order.readyBy) + "\n\n");
                }
            }
            return rezult;
        }
        private void CheckMatAvailability(Order order)
        {
            foreach (KeyValuePair<Material, int> keyValue in order.baguette.baguetteInfo.materials)
            {
                if (materialAvailability[keyValue.Key.materialName] < (keyValue.Value * order.baguette.Size))
                {
                    MoveOrder(order);
                    return;
                }
            }
            foreach (Material key in order.baguette.baguetteInfo.materials.Keys)
            {
                materialAvailability[key.materialName] -= order.baguette.baguetteInfo.materials[key] * order.baguette.Size;
            }

        }

        private double CountPrice(Order order)
        {
            return order.GetPrice(new OrderPriceCounter());
        }

        private void MoveOrder(Order order)
        {
            order.readyBy = order.readyBy.AddDays(7);
        }

        public void AddNewOrder(Order order)
        {
            orders.Add(order);
            CheckMatAvailability(order);
            order.readyBy = order.readyBy.AddHours(order.baguette.baguetteInfo.TimeToDo * order.count);
            //order.readyBy = order.readyBy.AddSeconds(15);
        }

        private void CheckOrders()
        {
            foreach (Order order in orders)
            {
                if (order.readyBy < DateTime.Now)
                {
                    order.OrderReady();
                }
            }
        }
        void timer_Tick(object sender)
        {
            CheckOrders();
        }
        public void StartTimer()
        {
            TimerCallback tm = new TimerCallback(timer_Tick);
            timer = new Timer(tm, null, 0, 1000);
        }
    }

    [DataContract]
    [KnownType(typeof(Order))]
    [KnownType(typeof(ShopInfo))]
    [KnownType(typeof(OrderAdder))]
    [KnownType(typeof(Printer))]
    [KnownType(typeof(OrderCreator))]
    [KnownType(typeof(TimeCalculator))]
    [KnownType(typeof(Changer))]
    [KnownType(typeof(MaterialChecker))]
    class NewShop
    {
        [DataMember]
        private ShopInfo ShopInfo { get; set; }
        [DataMember]
        private IOrderAdder Adder { get; set; }
        [DataMember]
        private IPrinter Printer { get; set; }


        public NewShop() { }
        public NewShop(IOrderAdder adder, IPrinter printer, ShopInfo info)
        {
            this.Adder = adder;
            this.Printer = printer;
            this.ShopInfo = info;
        }

        public void AddNewOrder(Dictionary<string, string> orderInfo)
        {
            Adder.AddNewOrder(ShopInfo, orderInfo);
        }

        public List<string> PrintOrders(string id)
        {
            return Printer.PrintOrders(id, ShopInfo.orders);
        }
    }


    [DataContract]
    class ShopInfo
    {
        [DataMember]
        public Dictionary<string, double> materialAvailability = new Dictionary<string, double>();
        [DataMember]
        public List<Order> orders = new List<Order>();

        public ShopInfo() { }
        public ShopInfo(double value)
        {
            materialAvailability.Add("Wood:", value);
            materialAvailability.Add("Paint:", value);
            materialAvailability.Add("Polish:", value);
        }
    }

    class TimerCheck
    {
        private Timer timer;
        List<Order> orders;
        public TimerCheck(Shop shop)
        {
            this.orders = shop.orders;
            StartTimer();
        }
        void timer_Tick(object sender)
        {
            CheckOrders();
        }
        public void StartTimer()
        {
            TimerCallback tm = new TimerCallback(timer_Tick);
            timer = new Timer(tm, null, 0, 1000);
        }
        private void CheckOrders()
        {
            foreach (Order order in orders)
            {
                if (order.readyBy < DateTime.Now)
                {
                    order.OrderReady();
                }
            }
        }
    }
    interface IOrderAdder
    {
        Order CreateOrder(Dictionary<string, string> orderInfo);
        void AddNewOrder(ShopInfo shop, Dictionary<string, string> orderInfo);
    }
    [DataContract]
    class OrderAdder : IOrderAdder
    {
        private Order order;
        [DataMember]
        private IReadyTimeCalculator TimeCalculator { get; set; }
        [DataMember]
        private IOrderCreator Creator { get; set; }
        public OrderAdder() { }
        public OrderAdder(IReadyTimeCalculator calculator, IOrderCreator creator)
        {
            this.TimeCalculator = calculator;
            this.Creator = creator;
        }
        
        public Order CreateOrder(Dictionary<string, string> orderInfo)
        {
            return Creator.CreateOrder(orderInfo);
        }

        public void AddNewOrder(ShopInfo shop, Dictionary<string, string> orderInfo)
        {
            order = CreateOrder(orderInfo);
            order.readyBy = TimeCalculator.CalculateReadyTime(shop, order);
            shop.orders.Add(order);
        }
    }
    interface IOrderCreator
    {
        Order CreateOrder(Dictionary<string, string> orderInfo);
    }
    [DataContract]
    class OrderCreator : IOrderCreator
    {
        public OrderCreator() { }
        public Order CreateOrder(Dictionary<string, string> orderInfo)
        {
            Baguette baguette;
            if (orderInfo["Baguette_type:"].Equals("Farinee"))
            {
                baguette = new BaguetteFarinee(Convert.ToInt32(orderInfo["Width:"]), Convert.ToInt32(orderInfo["Height:"]), orderInfo["Baguette_type:"]);
            }
            else if (orderInfo["Baguette_type:"].Equals("Moulee"))
            {
                baguette = new BaguetteMoulee(Convert.ToInt32(orderInfo["Amount_of_coats:"]), Convert.ToInt32(orderInfo["Width:"]), Convert.ToInt32(orderInfo["Height:"]), orderInfo["Baguette_type:"]);
            }
            else throw new ArgumentException("Wrong Baguette Type");
            return new Order(Convert.ToInt32(orderInfo["Count:"]), baguette, orderInfo["Surname:"]);
        }
    }

    interface IShopChanger
    {
        void ChangeShop(ShopInfo shop, Order order);
    }
    [DataContract]
    class Changer : IShopChanger
    {
        public Changer() { }

        public void ChangeShop(ShopInfo shop, Order order)
        {
            foreach (Material key in order.baguette.baguetteInfo.materials.Keys)
            {
                shop.materialAvailability[key.materialName] -= order.baguette.baguetteInfo.materials[key] * order.baguette.Size;
            }
        }
    }

    interface IPrinter
    {
        void Print(Order order);
        List<string> PrintOrders(string id, List<Order> orders);
    }
    [DataContract]
    class Printer : IPrinter
    {
        private List<string> printText;
        public Printer() { }

        public void Print(Order order)
        {
            printText.Add("Next Order:\n" + order.baguette.baguetteInfo.baguetteType + "\n");
            printText.Add("Count: " + Convert.ToString(order.count) + "\n");
            printText.Add("Price: " + Convert.ToString(order.GetPrice(new OrderPriceCounter())) + "\n");
            if (order.ready)
            {
                printText.Add("This order is ready\n\n");
            }
            else
            {
                printText.Add("Be ready on " + Convert.ToString(order.readyBy) + "\n\n");
            }
        }

        public List<string> PrintOrders(string id, List<Order> orders)
        {
            printText = new List<string>();
            foreach (Order order in orders)
            {
                if (id.Equals("White_Horse"))  Print(order); 
                else { if (order.surname.Equals(id)) Print(order); }
            }
            return printText;
        }
    }
    interface IMaterialsChecker
    {
        bool CheckShopMaterials(ShopInfo shop, Order  order);
        void ChangeShop(ShopInfo shop, Order order);
    }
    [DataContract]
    class MaterialChecker : IMaterialsChecker
    {
        [DataMember]
        private IShopChanger Changer { get; set; }
        public MaterialChecker() { } 
        public MaterialChecker(IShopChanger changer)
        {
            this.Changer = changer;
        }

        public bool CheckShopMaterials(ShopInfo shop, Order order)
        {
            foreach (KeyValuePair<Material, int> keyValue in order.baguette.baguetteInfo.materials)
            {
                if (shop.materialAvailability[keyValue.Key.materialName] < (keyValue.Value * order.baguette.Size))
                {
                    return false;
                }
            }
            ChangeShop(shop, order);
            return true;
        }
        public void ChangeShop(ShopInfo shop, Order order)
        {
            Changer.ChangeShop(shop, order);
        }
    }

    interface IReadyTimeCalculator
    {
        DateTime CalculateReadyTime(ShopInfo shop, Order order);
        bool CheckMaterials(ShopInfo shop, Order order);

    }
    [DataContract]
    class TimeCalculator : IReadyTimeCalculator
    {
        [DataMember]
        private IMaterialsChecker Checker { get; set; }
        public TimeCalculator() { }
        public TimeCalculator(IMaterialsChecker checker)
        {
            this.Checker = checker;
        }

        public DateTime CalculateReadyTime(ShopInfo shop, Order order)
        {
            if (CheckMaterials(shop, order)) return order.readyBy.AddHours(order.baguette.baguetteInfo.TimeToDo * order.count);
            else return order.readyBy.AddDays(7);
        }

        public bool CheckMaterials(ShopInfo shop, Order order)
        {
            return Checker.CheckShopMaterials(shop, order);
        }

        
    }

}

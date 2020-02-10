using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Baguette_Workshop_New
{
    static class ReaderFromFile
    {

        //public static readonly string orderPath = @"C:\Users\PashkaPustik\Desktop\OOP\Labs\Baguette\Order.txt";
        //public static readonly string shopPath = @"C:\Users\PashkaPustik\Desktop\OOP\Labs\Baguette\Shop.txt";
        private static List<Dictionary<string, string>> ReadFile(string path)
        {
            Dictionary<string, string> array = new Dictionary<string, string>();
            List<Dictionary<string, string>> listArr = new List<Dictionary<string, string>>();
            using (StreamReader sr = new StreamReader(path))
            {
                string myString;
                while ((myString = sr.ReadLine()) != null)
                {
                    if (myString.Equals("New Order:"))
                    {
                        listArr.Add(array);
                        array = new Dictionary<string, string>();
                        continue;
                    }
                    array.Add(myString.Split(' ')[0], myString.Split(' ')[1]);
                }
                if (array.Count != 0) { listArr.Add(array); }
            }
            return listArr;
        }

        private static void WriteFile(string path, List<Dictionary<string, string>> rezult)
        {
            using (StreamWriter sw = new StreamWriter(path, false))
            {
                foreach (Dictionary<string, string> pairs in rezult)
                {
                    foreach (KeyValuePair<string, string> pair in pairs)
                    {
                        sw.WriteLine(pair.Key + pair.Value);
                    }
                }
            }
        }

        public static void ReadOrder(string path, Shop shop)
        {
            Baguette baguette = null;
            List<Dictionary<string, string>> listArr = ReadFile(path);
            Dictionary<string, string> array = new Dictionary<string, string>();
            for (int i = 0; i < listArr.Count; i++)
            {
                array = listArr[i];
                if(array["Baguette_type:"].Equals("Farinee"))
                {
                    baguette = new BaguetteFarinee(Convert.ToInt32(array["Width:"]), Convert.ToInt32(array["Height:"]), array["Baguette_type:"]);
                }
                else if(array["Baguette_type:"].Equals("Moulee"))
                {
                    baguette = new BaguetteMoulee(Convert.ToInt32(array["Amount_of_coats:"]), Convert.ToInt32(array["Width:"]), Convert.ToInt32(array["Height:"]), array["Baguette_type:"]);
                }
                shop.AddNewOrder(new Order(Convert.ToInt32(array["Count:"]), baguette, array["Surname:"]));
            }
        }

        public static void WriteOrder(string path, List<Order> orders)
        {
            List<Dictionary<string, string>> rezult = new List<Dictionary<string, string>>();
            Dictionary<string, string> pairs = new Dictionary<string, string>();
            foreach (Order order in orders)
            {
                pairs.Add("Baguette_type: ", order.baguette.baguetteInfo.baguetteType);
                pairs.Add("Count: ", Convert.ToString(order.count));
                pairs.Add("Width: ", Convert.ToString(order.baguette.baguetteInfo.width));
                pairs.Add("Height: ", Convert.ToString(order.baguette.baguetteInfo.height));
                if (order.baguette.baguetteInfo.baguetteType.Equals("Moulee"))
                {
                    pairs.Add("Amount_of_coats: ", Convert.ToString(order.baguette.baguetteInfo.polishLayers));
                }
                pairs.Add("Surname: ", order.surname);
                pairs.Add("New Order:", null);
                rezult.Add(pairs);
                pairs = new Dictionary<string, string>();
            }
            WriteFile(path, rezult);
        }

        public static Shop ReadShop(string path)
        {
            List<Dictionary<string,string>> array = ReadFile(path);
            Dictionary<string, string> arr = new Dictionary<string, string>();
            for(int i = 0; i < array.Count; i++)
            {
                arr = array[i];
                Shop shop = new Shop();
                shop.AddMaterials(Convert.ToInt32(arr["Wood:"]),
                                Convert.ToInt32(arr["Paint:"]),
                                Convert.ToInt32(arr["Polish:"]));
                return shop;

            }
            return null;
        }

        public static void WriteShop(string path, Shop shop)
        {
            List<Dictionary<string, string>> rezult = new List<Dictionary<string, string>>();
            Dictionary<string, string> pairs = new Dictionary<string, string>();
            foreach (KeyValuePair<string, double> pair in shop.materialAvailability)
            {
                pairs.Add(pair.Key + " ", Convert.ToString(pair.Value));
            }
            rezult.Add(pairs);
            WriteFile(path, rezult);
        }

        public static BaguetteInfo ReadBaguetteInfo(string type)
        {
            Dictionary<string, string> paths = new Dictionary<string, string>();
            Dictionary<Material, int> materials = new Dictionary<Material, int>();
            List<Dictionary<string, string>> files;
            paths.Add("Farinee", @"C:\Users\PashkaPustik\Desktop\OOP\Labs\Baguette\Farinee.txt");
            paths.Add("Moulee", @"C:\Users\PashkaPustik\Desktop\OOP\Labs\Baguette\Moulee.txt");
            files = ReadFile(paths[type]);
            foreach(Dictionary<string, string> info in files)
            {
                materials.Add(new Wood(), Convert.ToInt32(info["Wood:"]));
                materials.Add(new Paint(), Convert.ToInt32(info["Paint:"]));
                materials.Add(new Polish(), Convert.ToInt32(info["Polish:"]));
                return new BaguetteInfo(type, materials, Convert.ToDouble(info["Time:"]));
            }
            return null;
        }
        
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Baguette_Workshop_New
{
    public partial class WhiteHorse : Form
    {
        public static readonly string orderPath = @"C:\Users\PashkaPustik\Desktop\OOP\Labs\Baguette\Order.txt";
        public static readonly string shopPath = @"C:\Users\PashkaPustik\Desktop\OOP\Labs\Baguette\Shop.txt";
        public static readonly string xmlPath = @"C:\Users\PashkaPustik\Desktop\OOP\Labs\Baguette\Shop.xml";
        public static readonly string jsonPath = @"C:\Users\PashkaPustik\Desktop\OOP\Labs\Baguette\Shop.json";

        static Shop myshop;
        static NewShop myshop2 = new NewShop(new OrderAdder(new TimeCalculator(new MaterialChecker(new Changer())), 
                                                            new OrderCreator()), 
                                             new Printer(),
                                             new ShopInfo(10000));
        public WhiteHorse()
        {
            InitializeComponent();
        }

        private void Btn_ReadOrder_Click(object sender, EventArgs e)
        {
            myshop = ReaderFromFile.ReadShop(shopPath);
            ReaderFromFile.ReadOrder(orderPath, myshop);
            txt_ReadOrders.Text = "Інформацію зчитано";
        }


        private void Btn_serialize_Click(object sender, EventArgs e)
        {
            Serializator<NewShop> serializator = new Serializator<NewShop>(new JsonSerializator(typeof(NewShop)));
            serializator.Serialize(myshop2, jsonPath);
            //ReaderFromFile.WriteShop(shopPath, myshop);
            //ReaderFromFile.WriteOrder(orderPath, myshop.orders);
            txt_ReadOrders.Text = "Інформацію збережено";
        }


        private void Btn_AddOrder_Click(object sender, EventArgs e)
        {
            myshop2.AddNewOrder(ReadOrderInfo());
        }

        private Dictionary<string, string> ReadOrderInfo()
        {
            Dictionary<string, string> orderInfo = new Dictionary<string, string>();
            orderInfo.Add("Baguette_type:", txt_Baguette_Type.Text);
            orderInfo.Add("Width:", txt_Width.Text);
            orderInfo.Add("Height:", txt_Height.Text);
            orderInfo.Add("Count:", txt_Count.Text);
            orderInfo.Add("Surname:", txt_Surname.Text);
            orderInfo.Add("Amount_of_coats:", txt_Amount_of_coats.Text);
            return orderInfo;

        }


        private void Btn_deserialize_Click_1(object sender, EventArgs e)
        {
            Serializator<NewShop> serializator = new Serializator<NewShop>(new JsonSerializator(typeof(NewShop)));
            myshop2 = serializator.Deserialize(jsonPath);
            //myshop.StartTimer();
            txt_ReadOrders.Text = "Інформацію зчитано";
        }

        private void Btn_ProcessOrders_Click_1(object sender, EventArgs e)
        {
            txt_Rezult.Text = null;
            foreach (string str in myshop2.PrintOrders("White_Horse"))
            {
                txt_Rezult.Text += str;
            }
        }

        private void Btn_PrintOrdersSurname_Click_1(object sender, EventArgs e)
        {
            txtBx_SurnameOrders.Text = null;
            foreach (string str in myshop2.PrintOrders(txt_SurnamePrint.Text))
            {
                txtBx_SurnameOrders.Text += str;
            }
        }

        /*private void Form1_FormClosing(Object sender, FormClosingEventArgs e)
{
Serializator serializator = new Serializator(new XMLSerializator(typeof(Shop)));
serializator.Serialize(myshop, xmlPath);
MessageBox.Show("Успішно збережено");
}*/
    }
}

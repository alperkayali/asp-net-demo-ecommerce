using AspNetCore_WebAPP_MVC_PROJE.Models.DbSets;
using AspNetCore_WebAPP_MVC_PROJE.Models.DbViews;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;

namespace AspNetCore_WebAPP_MVC_PROJE.Models.MVVM
{
    public class cls_Order
    {
        KayaliContext context = new KayaliContext();

        #region -- PROPS --
        public int ProductID { get; set; }
        public int Quantity { get; set; }
        public string? MyCart { get; set; }
        public string? ProductName { get; set; }
        public int KDV { get; set; }
        public decimal UnitPrice { get; set; }
        public string? PhotoPath { get; set; }
        public string? tckimlik_or_vergino { get; set; }
        #endregion

        #region ADD TO CART
        public bool AddtoMyCart(string id) //ID is string because cookies will take it as string. Read explanation in CartProcess for more.
        {
            bool exists = false;

            if (MyCart == "")
            {
                MyCart = id + "=1";
            }

            else
            {                           // 10=1&20=3&30=7...
                string[] MyCartArray = MyCart.Split('&');

                for (int i = 0; i < MyCartArray.Length; i++)
                {
                    //first loop for for it has 10=1, then 20=3...
                    //we need another string array
                    string[] MyCartArrayLoop = MyCartArray[i].Split('=');

                    if (MyCartArrayLoop[0] == id)
                    {
                        //this item is in the Cart already.
                        exists = true;
                        //add 1 more of it.
                        MyCartArrayLoop[1] += MyCartArrayLoop[1]; //bu patlayabilir ???
                    }
                }
                if (exists == false)
                {
                    MyCart = MyCart + "&" + id + "=1";
                }
            }

            return exists;
        }
        #endregion

        #region PICK FROM CART
        //this methow will work when clicked the "Cart" from the Header Area.
        public List<cls_Order> SelectMyCart()
        {
            //Each products details will be saved in a list.
            List<cls_Order > list = new List<cls_Order>();

            string[] MyCartArray = MyCart.Split('&');

            if (MyCartArray[0] != "")
            {
                for (int i = 0; i < MyCartArray.Length; i++)
                {
                    //We will pick the each product in MyCartArray, find them each in DB, take it's properties and assign them into the cls_Orders.

                    string[] MyCartArrayLoop = MyCartArray[i].Split('=');
                    int ProductID = Convert.ToInt32(MyCartArrayLoop[0]);

                    Product? prd = context.Products.FirstOrDefault(p=>p.ProductID == ProductID);

                    cls_Order ord = new cls_Order();
                    ord.ProductID = prd.ProductID;
                    ord.Quantity = Convert.ToInt32(MyCartArrayLoop[1]);
                    ord.UnitPrice = prd.UnitPrice;
                    ord.ProductName = prd.ProductName;
                    ord.KDV = prd.Kdv;
                    ord.PhotoPath = prd.PhotoPath;
                    list.Add(ord);

                }
            }
            return list;
        }
        #endregion

        #region DELETE FROM CART
        public void DeleteFromMyCart(string id)
        {
            string[] MyCartArray = MyCart.Split('&');
            string NewMyCart = "";
            int count = 1;

            for (int i = 0; i < MyCartArray.Length; i++)
            {
                //split the ProductID and quantity
                string[] MyCartArrayLoop = MyCartArray[i].Split('=');
                //0. index assigned to the MyCartID with the each for loop
                string MyCartID = MyCartArrayLoop[0];

                if (MyCartID != id)
                {   //this is be the product WON'T BE DELETED.
                    if (count == 1)
                    {
                        NewMyCart = MyCartArrayLoop[0] + "=" + MyCartArrayLoop[1];
                        count++;
                    }
                    else
                    {
                        NewMyCart += "&" + MyCartArrayLoop[0] + "=" + MyCartArrayLoop[1];
                    }                    
                }
            }
            MyCart = NewMyCart;
        }
        #endregion

        #region ORDER CREATE
        public string OrderCreate(string Email)
        {
            List<cls_Order> orderList = SelectMyCart();

            string OrderGroupGUID = DateTime.Now.ToString().Replace(":", "").Replace(" ", "").Replace(".", "");
            DateTime OrderDate = DateTime.Now;

            foreach (var item in orderList)
            {
                Order order = new Order();

                order.OrderDate = OrderDate;
                order.OrderGroupGUID = OrderGroupGUID;
                order.UserID = context.Users.FirstOrDefault(u => u.Email == Email).UserID;
                order.ProductID = item.ProductID;
                order.Quantity = item.Quantity;

                context.Orders.Add(order);
                context.SaveChanges();
            }
            return OrderGroupGUID;
        }
        #endregion

        #region INVOICE CREATE
        public void InvoiceCreate()
        {
            //DigitalPlanet'e gönderilmek üzere fatura oluşturan (xml format) method kodları.
            //Daha önceki derste işlediğimiz yerden kopyala.

            //tckimlik_or_vergino
        }
        #endregion

        public List<vw_MyOrders> SelectMyOrders(string Email)
        {
            int UserID = context.Users.FirstOrDefault(u => u.Email == Email).UserID;

            List<vw_MyOrders> myOrders = context.vw_MyOrders.Where(o=>o.UserID == UserID).ToList();
            return myOrders;
        }
    }
}

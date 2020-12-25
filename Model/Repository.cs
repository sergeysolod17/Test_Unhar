using System.Collections.Generic;
using System.Linq;
using Test_Unhar.Interfaces;
using Test_Unhar.Models;

namespace Test_Unhar.Services
{
    public class OrderRepository : IOrderRepository
    {
        private List<Order> _orderList;
        private int _count;
        public OrderRepository()
        {
            InitializeData();
        }

        public IEnumerable<Order> All
        {
            get { return _orderList; }
        }

        public bool DoesItemExist(string id)
        {
            return _orderList.Any(item => item.Id == id);
        }

        public Order Find(string id)
        {
            return _orderList.FirstOrDefault(item => item.Id == id);
        }
        public IEnumerable<Order.Response> FindPageReponse(int start=0, int quantity=int.MaxValue, string status="")
        {
            IEnumerable<Order.Response> orders;
            if(status=="")
            {
                orders = (_orderList.Skip(start-1).Take(quantity)).Select(t=>t.response);                
            }
            else
            {
                orders = (from t in _orderList where (t.status==status) select t.response).Skip(start).Take(quantity);
            }
            return orders;
        }

        public void Insert(Order item)
        {
            _orderList.Add(item);
        }
        public string Insert(Order.Request item)
        {
            _orderList.Add(new Order((_count+1).ToString(),item.dimension,item.pickup,item.dropOff,StatusOrder.Status1.ToString()));
            _count = _count+1;
            return _count.ToString();
        }
        public void Update(Order item)
        {
            var orderItem = this.Find(item.Id);
            var index = _orderList.IndexOf(orderItem);
            _orderList.RemoveAt(index);
            _orderList.Insert(index, item);
        }

        public void Delete(string id)
        {
            _orderList.Remove(this.Find(id));
        }
        public int Quantity{get{return _orderList.Count;}}
        private void InitializeData()
        {
            _orderList = new List<Order>();

            var order1 = new Order
            (
                "0001", //id
                "200x100x120",                          //dimension
                new Location(50.1250000,12.055668),     //pickup
                new Location(50.1250000,12.055668),     //dropoff
                StatusOrder.Status1.ToString()                    //status
            );
            var order2 = new Order
            (
                "0002", //id
                "210x100x150",                          //dimension
                new Location(50.1250000,47.055668),     //pickup
                new Location(50.1250000,12.055668),     //dropoff
                StatusOrder.Status1.ToString()                    //status
            );
            var order3 = new Order
            (
                "0003", //id
                "300x150x120",                          //dimension
                new Location(50.1250000,30.055668),     //pickup
                new Location(50.1250000,12.055668),     //dropoff
                StatusOrder.Status3.ToString()                    //status
            );

            _orderList.Add(order1);
            _orderList.Add(order2);
            _orderList.Add(order3);
            _count = 3;
        }
    }
}
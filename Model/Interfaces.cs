using System.Collections.Generic;
using Test_Unhar.Models;


namespace Test_Unhar.Interfaces
{
    public interface IOrderRepository
    {
        bool DoesItemExist(string id);
        public IEnumerable<Order.Response> FindPageReponse(int start=0, int quantity=int.MaxValue, string status="");
        IEnumerable<Order> All { get; }
        Order Find(string id);
        void Insert(Order item);
        public string Insert(Order.Request item);
        void Update(Order item);
        int Quantity {get;}
    }
    
}
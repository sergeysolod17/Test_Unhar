using System.Collections.Generic;
using Test_Unhar.Models;


namespace Test_Unhar.Interfaces
{
    public interface I
    {
        bool DoesItemExist(string id);
        IEnumerable<ToDoItem> All { get; }
        ToDoItem Find(string id);
        void Insert(ToDoItem item);
        void Update(ToDoItem item);
        
    }
}
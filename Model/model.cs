using System;


namespace Test_Unhar.Models
{
    public struct Location
    {   
        public double latitude { get; set; }

        public int longitude { get; set; }
        
    }

    public class Error
    {
        public int code{ get; set; }
        public String message{ get; set; }
        public String detail { get; set; }
    }

    public class Order
    {
        public struct Request
        {
           public String dimension { get; set;}
           public Test_Unhar.Location pickup{ get; set;}
           public Test_Unhar.Location dropOff{ get; set;}
        }

        public class Response
        {
           public String id {get;  set;}
           public String dimension { get; set;}
           public enum Status
           {
               Status1,
               Status2,
               Status3
           }

           Status status;
           public Test_Unhar.Location pickup{ get; set;}
           public Test_Unhar.Location dropOff{ get; set;}
        }

    }

}

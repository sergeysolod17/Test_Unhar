using System;


namespace Test_Unhar.Models
{    
    public enum StatusOrder
           {
               Status1,
               Status2,
               Status3
           }
    public class Location
    {   
        public double latitude { get; set; }
        public double longitude { get; set; }
        
        public Location(double lat, double lon)
        {
            latitude = lat;
            longitude = lon;
        }
    }
    public class Error
    {
        public int code{ get; set; }
        public String message{ get; set; }
        public String detail { get; set; }
        public Error(int code_, String message_, String detail_)
        {
            code = code_;
            message = message_;
            detail = detail_;
        }
    }
    
    public class Order
    {
        public class Request
        {
           public String dimension { get; set;}
           public Location pickup{ get; set;}
           public Location dropOff{ get; set;}
           public Request(String dimen, Location pickup, Location dropOff)
           {
               this.dimension = dimen;
               this.pickup = pickup;
               this.dropOff = dropOff;
           }
        }
        public class Response
        {
           public String id {get;  set;}
           public String dimension { get; set;}
           public String status{ get; set;}
           public Location pickup{ get; set;}
           public Location dropOff{ get; set;}
            public Response(String dimen, Location pickup, Location dropOff, String status, String id)
           {
               this.dimension = dimen;
               this.pickup = pickup;
               this.dropOff = dropOff;
               this.status = status;
               this.id = id;
           }
        }
        public Order(String id, String dimen, Location pickup, Location dropOff, String status)
        {
            _id = id;
            _dimension = dimen;
            _pickup = pickup;
            _dropOff = dropOff;
            _status = status;
        }
        private String _status;
        private String _id;
        private  String _dimension;
        private Location _pickup;
        private Location _dropOff;
        public Order.Request request
        {get{return new Order.Request(_dimension, _pickup, _dropOff);}}
        public Order.Response response
        {get{return new Order.Response(_dimension, _pickup, _dropOff, _status, _id);}}
        public String Id{get{return _id;}} 
        public String status{get{return _status;}}       
    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Test_Unhar.Models;
using Test_Unhar.Interfaces;
using Test_Unhar.Services;


namespace Test_Unhar.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ordersController : ControllerBase
    {        
        ordersController(OrderRepository orderRepository)
        {
            //_orderRepository = orderRepository;
        }

        private readonly ILogger<ordersController> _logger;

        public ordersController(ILogger<ordersController> logger)
        {
            _logger = logger;
        }

        private static readonly IOrderRepository _orderRepository = new OrderRepository();

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public ActionResult<Order> Get([FromQuery] int start=0, [FromQuery] int quantity=int.MaxValue, [FromQuery] String status="")
        {
            IEnumerable<Order.Response> orders;
            if(!ValidateToken())
            {
                var err401 = new Test_Unhar.Models.Error(1105,"Invalid token","This token alredy expired");
                return new UnauthorizedObjectResult(err401);
            }            
            if(_orderRepository.Quantity==0)
                    {return NoContent();}                 
            orders = _orderRepository.FindPageReponse(start, quantity, status);
            if(orders.Count() == 0)
                    {return NoContent();}
            return new OkObjectResult(orders);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Order> Get(String id)//return JSON
        {
            if(!ValidateToken())
            {
                var err401 = new Test_Unhar.Models.Error(1105,"Invalid token","This token alredy expired");
                return new UnauthorizedObjectResult(err401);
            }
            var order = _orderRepository.Find(id);
            if (order == null)
            {
                var err404 = new Test_Unhar.Models.Error(135,"Order not found","Cannot find order with id: "+id);
                return new NotFoundObjectResult(err404);
            }
            return new OkObjectResult(order.response);
        }
        [HttpPost]
        //[Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public ActionResult<Order.Response> Create([FromBody] Order.Request request)
        {            
            if(!ValidateToken())
            {
                var err401 = new Test_Unhar.Models.Error(1105,"Invalid token","This token alredy expired");
                return new UnauthorizedObjectResult(err401);
            };
            if (request == null)
            {
                var err422 = new Test_Unhar.Models.Error(1050,"Invalid data","Invalid location field in incoming object");
                return new UnprocessableEntityObjectResult(err422);                
            };
            string _id = _orderRepository.Insert(request);
            return new ObjectResult(_orderRepository.Find(_id).response) {StatusCode = StatusCodes.Status201Created};
            /*var baseUrl = Request.Path;
            return new CreatedResult(baseUrl+"/" + _id.ToString(),_orderRepository.Find(_id).response);*/
        }
        private bool ValidateToken()
    {   
        var simplePrinciple = GetPrincipal();        
        if (simplePrinciple == null)
            return false;
        var identity = simplePrinciple.Identity as ClaimsIdentity;
        if (!identity.IsAuthenticated)
            return false;
        // More validate to check whether username exists in system
        return true;
     }
       public ClaimsPrincipal GetPrincipal()
    {
        try
        {
            //String jwt = ((String)this.Request.Headers["Authorization"]). Replace("Bearer ", string.Empty);
            String authHeader = Request.Headers["Authorization"];
            authHeader = authHeader.Replace("Bearer ", "");
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = new System.IdentityModel.Tokens.Jwt.JwtSecurityToken(authHeader);                
            if (token == null)
                return null;     
            var validationParameters = new TokenValidationParameters()
            {
               RequireExpirationTime = true,
               ValidateIssuer = false,
               ValidateAudience = false,
               IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey()
            };
            SecurityToken securityToken;            
            var principal = tokenHandler.ValidateToken(authHeader, validationParameters, out securityToken);
            if(securityToken.ValidTo < DateTime.UtcNow)
            {return null;}
            return principal;
        }
        catch (Exception)
        {            
            return null;
        }
    }
}
    public class AuthOptions
    {
        public const string ISSUER = "MyTestUnharServer"; // издатель токена
        public const string AUDIENCE = "MyAuthClient"; // потребитель токена
        const string KEY = "new_key!3.1415$PI";   // ключ для шифрации
        public const int LIFETIME = 14400; // время жизни токена - 14400 минута = 10 суток
        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
        }
    }
    /*public class AccountController : Controller
    {
        // тестовые данные вместо использования базы данных
        
 
        [HttpPost("/token")]
        public IActionResult Token(string username, string password)
        {
            var identity = GetIdentity(username, password);
            if (identity == null)
            {
                return BadRequest(new { errorText = "Invalid username or password." });
            }
 
            var now = DateTime.UtcNow;
            // создаем JWT-токен
            var jwt = new JwtSecurityToken(
                    issuer: AuthOptions.ISSUER,
                    audience: AuthOptions.AUDIENCE,
                    notBefore: now,
                    claims: identity.Claims,
                    expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
 
            var response = new
            {
                access_token = encodedJwt,
                username = "default"
            };
 
            return Json(response);
        }
         private ClaimsIdentity GetIdentity(string username, string password)
        {                     {             
            return new ClaimsIdentity();            
        }
    }
}*/
}

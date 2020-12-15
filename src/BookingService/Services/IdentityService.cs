using BookingServiceNS.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;

namespace BookingServiceNS.Services
{
    public class IdentityService : IIdentityService
    {
        private IHttpContextAccessor _context;

        public IdentityService(IHttpContextAccessor context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public IdentityModel GetIdentity()
        {
            string authorizationHeader = _context.HttpContext.Request.Headers["Authorization"];

            if (authorizationHeader != null)
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var token = authorizationHeader.Split(" ")[1];
                var parsedToken = tokenHandler.ReadJwtToken(token);

                var Customerid = parsedToken.Claims
                    .Where(c => c.Type == "Customerid")
                    .FirstOrDefault();

                var CustomerName = parsedToken.Claims
                    .Where(c => c.Type == "CustomerName")
                    .FirstOrDefault();

                var Email = parsedToken.Claims
                    .Where(c => c.Type == "Email")
                    .FirstOrDefault();

                return new IdentityModel()
                {
                    Customerid = Customerid.ToString(),
                    CustomerName = CustomerName.ToString(),
                    Email = Email.ToString()
                };
            }

            throw new ArgumentNullException("CustomerID");
        }
    }
}

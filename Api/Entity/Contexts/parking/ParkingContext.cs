using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Entity.Contexts.parking
{
    public class ParkingContext : IParkingContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ParkingContext(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public int? ParkingId
        {
            get
            {
                var user = _httpContextAccessor.HttpContext?.User;
                var claim = user?.FindFirst("parkingId");
                return claim != null ? int.Parse(claim.Value) : 0;
            }
        }
    }

}
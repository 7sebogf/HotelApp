using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Data
{
    public static class AppConnect
    {
        
        public static HotelBookingDbEntities1 model0db = new HotelBookingDbEntities1();

        public static int CurrentUserId { get; set; }
        public static string UserName { get; set; } = "Гость";
        public static string UserRole { get; set; } = "Guest";
    }
}

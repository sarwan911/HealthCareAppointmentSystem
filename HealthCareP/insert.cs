using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace HealthCareAppointmentSystem
{
    public class insert
    {
        HealthCareContext _context=new HealthCareContext();

        public bool insertUser(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
            return true;
        }
    }
}

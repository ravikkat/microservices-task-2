using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
namespace AssesmentTask2.Models
{
    public class BookingDataContext : DbContext
    {
        public BookingDataContext(DbContextOptions<BookingDataContext> options)
          : base(options)
        {
        }

        public DbSet<Booking> TodoItems { get; set; }
    }
}

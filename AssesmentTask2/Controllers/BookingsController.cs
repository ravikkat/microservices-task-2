using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AssesmentTask2.Models;

namespace AssesmentTask2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        private readonly BookingDataContext _context;

        public BookingsController(BookingDataContext context)
        {
            _context = context;
        }

        // GET: api/Bookings
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Booking>>> GetTodoItems()
        {
            return await _context.TodoItems.ToListAsync();
        }

        // GET: api/Bookings/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Booking>> GetBooking(long id)
        {
            var booking = await _context.TodoItems.FindAsync(id);

            if (booking == null)
            {
                return NotFound();
            }

            return booking;
        }

        // PUT: api/Bookings/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBooking(long id, Booking booking)
        {
            if (id != booking.Id)
            {
                return BadRequest();
            }

            _context.Entry(booking).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookingExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Bookings
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Booking>> PostBooking(Booking booking)
        {
            if(booking != null && !string.IsNullOrEmpty(booking.Currency))
            {
                double? sgdamount = (booking.Amount) * GetSGDCurrency(booking.Currency.ToLower());
                if( (sgdamount ?? 0) > 0)
                {
                    booking.SGDAmount = sgdamount.ToString();
                }
               
            }
           
            _context.TodoItems.Add(booking);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBooking", new { id = booking.Id }, booking);
        }

        // DELETE: api/Bookings/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBooking(long id)
        {
            var booking = await _context.TodoItems.FindAsync(id);
            if (booking == null)
            {
                return NotFound();
            }

            _context.TodoItems.Remove(booking);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BookingExists(long id)
        {
            return _context.TodoItems.Any(e => e.Id == id);
        }

        private double? GetSGDCurrency(string  sourcecurrency)
        {
            double? sgdCurrency = 0;
            string surl = "https://cdn.jsdelivr.net/gh/fawazahmed0/currency-api@1/latest/currencies/" + sourcecurrency + "/sgd.json";
            using (var client = new System.Net.Http.HttpClient())
            {
                
                client.BaseAddress = new Uri("https://cdn.jsdelivr.net/gh/fawazahmed0/currency-api@1/latest/currencies/" + sourcecurrency+ "/sgd.json");

                //HTTP GET
                try
                {
                    var responseTask = client.GetAsync(surl);
                    responseTask.Wait();

                    var result = responseTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        var readTask = result.Content.ReadAsStringAsync();
                        readTask.Wait();

                        var alldata = readTask.Result;
                        CurrencyPM roles = Newtonsoft.Json.JsonConvert.DeserializeObject<CurrencyPM>(alldata);
                        sgdCurrency= roles.sgd;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("{0} Exception caught.", e);
                }

            }
            return sgdCurrency;
          
        }
    }
}

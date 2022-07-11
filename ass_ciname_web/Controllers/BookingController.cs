using ass_ciname_web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ass_ciname_web.Controllers
{
    public class BookingController : Controller
    {
        private readonly CinemaContext _context;

        public BookingController(CinemaContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var show = await _context.Shows
                    .Include(show => show.Film)
                    .Include(show => show.Room)
                    .FirstOrDefaultAsync(m => m.ShowId == id);
            if (show == null)
            {
                return NotFound();
            }

            ViewBag.Show = show;

            var cinameContext = _context
                        .Bookings.Where(booking => booking.ShowId == id)
                        .Include(show => show.Show);
            var bookings = await cinameContext.ToListAsync();
            int col = show.Room.NumberCols ?? default(int);
            int row = show.Room.NumberRows ?? default(int);
            char[] charArray = new char[col * row];
            Array.Fill(charArray, '0');
            foreach (Booking booking in bookings)
            {
                for (int i = 0; i < booking.SeatStatus.Length; i++)
                {
                    if (booking.SeatStatus[i] == '1')
                    {
                        charArray[i] = '1';
                    }
                }
            }
            ViewBag.bookings = bookings;
            ViewBag.result = charArray;
            return View();
        }


        public async Task<IActionResult> Detail(int? id)
        {
            var booking = await _context.Bookings
               .Include(booking => booking.Show)
                   .ThenInclude(show => show.Room)
               .FirstOrDefaultAsync(m => m.BookingId == id);
            int col = booking.Show.Room.NumberCols ?? default(int);
            int row = booking.Show.Room.NumberRows ?? default(int);
            char[] charArray = new char[col * row];
            Array.Fill(charArray, '0');
            for (int i = 0; i < booking.SeatStatus.Length; i++)
            {
                if (booking.SeatStatus[i] == '1')
                {
                    charArray[i] = '1';
                }
            }
            ViewBag.col = col;
            ViewBag.row = row;
            ViewBag.result = charArray;
            ViewBag.booking = booking;

            return View(booking);
        }

        [Route("/Booking/Index/{id?}/Create")]
        public async Task<IActionResult> Create(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var show = await _context.Shows
                    .Include(show => show.Film)
                    .Include(show => show.Room)
                    .FirstOrDefaultAsync(m => m.ShowId == id);
            if (show == null)
            {
                return NotFound();
            }

            ViewBag.Show = show;

            var cinameContext = _context
                        .Bookings.Where(booking => booking.ShowId == id)
                        .Include(show => show.Show);
            var bookings = await cinameContext.ToListAsync();
            int col = show.Room.NumberCols ?? default(int);
            int row = show.Room.NumberRows ?? default(int);
            char[] charArray = new char[col * row];
            Array.Fill(charArray, '0');
            foreach (Booking booking in bookings)
            {
                for (int i = 0; i < booking.SeatStatus.Length; i++)
                {
                    if (booking.SeatStatus[i] == '1')
                    {
                        charArray[i] = '1';
                    }
                }
            }
            ViewBag.bookings = bookings;
            ViewBag.result = charArray;
            ViewBag.col = col;
            ViewBag.row = row;
            ViewBag.Show = show;
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("/Booking/Index/{id?}/Create")]
        public async Task<IActionResult> Create(int? id, [Bind("seats")] int[] seats, [Bind("name")] string name)
        {
            if (id == null)
            {
                return NotFound();
            }

            var show = await _context.Shows
                    .Include(show => show.Film)
                    .Include(show => show.Room)
                    .FirstOrDefaultAsync(m => m.ShowId == id);
            if (show == null)
            {
                return NotFound();
            }

            int col = show.Room.NumberCols ?? default(int);
            int row = show.Room.NumberRows ?? default(int);

            if (seats==null || seats.Length <= 0 || col * row != seats.Length)
            {
                return BadRequest();
            }


            string seatStatus = "";
            int count = 0;
            for (int i = 0; i < col * row; i++)
            {
                if (seats[i]==1)
                {
                    count++;
                }
                seatStatus+=seats[i];
            }
          
            Booking booking = new Booking();
            booking.SeatStatus = seatStatus;
            booking.ShowId = show.ShowId;
            booking.Amount = count * show.Price;
            booking.Name = name;
            if (count == 0)
            {
                char[] charArray = new char[col * row];
                Array.Fill(charArray, '0');

                var cinameContext = _context
                            .Bookings.Where(booking => booking.ShowId == id)
                            .Include(show => show.Show);
                var bookings = await cinameContext.ToListAsync();
                foreach (Booking item in bookings)
                {
                    for (int i = 0; i < item.SeatStatus.Length; i++)
                    {
                        if (item.SeatStatus[i] == '1')
                        {
                            charArray[i] = '1';
                        }
                    }
                }
                ViewBag.bookings = bookings;
                ViewBag.result = charArray;
                ViewBag.col = col;
                ViewBag.row = row;
                ViewBag.Show = show;

                ViewBag.error = "Please choose seat!";
                return View(booking);
            }
            _context.Bookings.Add(booking);
            _context.SaveChanges();
            return Redirect("~/Booking/Index/"+show.ShowId);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            var booking = await _context.Bookings
               .Include(booking => booking.Show)
                   .ThenInclude(show => show.Room)
               .FirstOrDefaultAsync(m => m.BookingId == id);
            int col = booking.Show.Room.NumberCols ?? default(int);
            int row = booking.Show.Room.NumberRows ?? default(int);
            char[] charArray = new char[col * row];
            Array.Fill(charArray, '0');
            for (int i = 0; i < booking.SeatStatus.Length; i++)
            {
                if (booking.SeatStatus[i] == '1')
                {
                    charArray[i] = '1';
                }
            }
            ViewBag.col = col;
            ViewBag.row = row;
            ViewBag.result = charArray;
            ViewBag.booking = booking;

            return View(booking);
        }

        public async Task<IActionResult> ConfirmDelete(int? id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            _context.Bookings.Remove(booking);
            await _context.SaveChangesAsync();
            return Redirect("~/Booking/Index/" + booking.ShowId);
        }
    }
}

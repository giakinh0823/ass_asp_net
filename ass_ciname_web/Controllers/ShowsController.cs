using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ass_ciname_web.Models;
using Microsoft.AspNetCore.Http;

namespace ass_ciname_web.Controllers
{
    public class ShowsController : Controller
    {
        private readonly CinemaContext _context;

        public ShowsController(CinemaContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(Show sho)
        {
            var cinameContext = sho.ShowDate != null ? _context.Shows
                .Where(item => item.ShowDate == sho.ShowDate
                    && item.RoomId == sho.RoomId
                    && item.FilmId == sho.FilmId)
                .OrderByDescending(show => show.ShowDate)
                .Include(show => show.Film)
                .Include(show => show.Room) : _context.Shows
                .OrderByDescending(show => show.ShowDate)
                .Include(show => show.Film)
                .Include(show => show.Room);

            ViewData["shows"] = await cinameContext.ToListAsync();
            ViewData["Rooms"] = new SelectList(_context.Rooms, "RoomId", "Name");
            ViewData["Films"] = new SelectList(_context.Films, "FilmId", "Title");

            Show show = new Show();
            show.ShowDate = DateTime.Now;
            return View(show);
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var show = await _context.Shows
                .FirstOrDefaultAsync(m => m.ShowId == id);
            if (show == null)
            {
                return NotFound();
            }

            return View(show);
        }

        public IActionResult Create()
        {
            var user = HttpContext.Session.GetString("user");
            if (user == null || user.Equals(""))
            {
                return Redirect("~/Auth/Login");
            }

            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ShowId,RoomId,FilmId,ShowDate,Price,Status,Slot")] Show show)
        {
            var user = HttpContext.Session.GetString("user");
            if (user == null || user.Equals(""))
            {
                return Redirect("~/Auth/Login");
            }

            if (ModelState.IsValid)
            {
                _context.Add(show);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(show);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            var user = HttpContext.Session.GetString("user");
            if (user == null || user.Equals(""))
            {
                return Redirect("~/Auth/Login");
            }

            if (id == null)
            {
                return NotFound();
            }

            var show = await _context.Shows.FindAsync(id);
            if (show == null)
            {
                return NotFound();
            }
            return View(show);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ShowId,RoomId,FilmId,ShowDate,Price,Status,Slot")] Show show)
        {
            var user = HttpContext.Session.GetString("user");
            if (user == null || user.Equals(""))
            {
                return Redirect("~/Auth/Login");
            }
            if (id != show.ShowId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(show);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ShowExists(show.ShowId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(show);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            var user = HttpContext.Session.GetString("user");
            if (user == null || user.Equals(""))
            {
                return Redirect("~/Auth/Login");
            }
            if (id == null)
            {
                return NotFound();
            }

            var show = await _context.Shows
                .FirstOrDefaultAsync(m => m.ShowId == id);
            if (show == null)
            {
                return NotFound();
            }

            return View(show);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = HttpContext.Session.GetString("user");
            if (user == null || user.Equals(""))
            {
                return Redirect("~/Auth/Login");
            }
            var show = await _context.Shows.FindAsync(id);
            _context.Shows.Remove(show);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ShowExists(int id)
        {
            return _context.Shows.Any(e => e.ShowId == id);
        }
    }
}

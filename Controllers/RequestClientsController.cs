using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HalloDoc.DataContext;
using HalloDoc.DataModels;

namespace HalloDoc.Controllers
{
    public class RequestClientsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RequestClientsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: RequestClients
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.RequestClients.Include(r => r.Region).Include(r => r.Request);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: RequestClients/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.RequestClients == null)
            {
                return NotFound();
            }

            var requestClient = await _context.RequestClients
                .Include(r => r.Region)
                .Include(r => r.Request)
                .FirstOrDefaultAsync(m => m.RequestClientId == id);
            if (requestClient == null)
            {
                return NotFound();
            }

            return View(requestClient);
        }

        // GET: RequestClients/Create
        public IActionResult Create()
        {
            ViewData["RegionId"] = new SelectList(_context.Regions, "RegionId", "RegionId");
            ViewData["RequestId"] = new SelectList(_context.Requests, "RequestId", "RequestId");
            return View();
        }

        // POST: RequestClients/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RequestClientId,RequestId,FirstName,LastName,PhoneNumber,Location,Address,RegionId,NotiMobile,NotiEmail,Notes,Email,StrMonth,IntYear,IntDate,IsMobile,Street,City,State,ZipCode,CommunicationType,RemindReservationCount,RemindHouseCallCount,IsSetFollowupSent,Ip,IsReservationReminderSent,Latitude,Longitude")] RequestClient requestClient)
        {
            if (ModelState.IsValid)
            {
                _context.Add(requestClient);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["RegionId"] = new SelectList(_context.Regions, "RegionId", "RegionId", requestClient.RegionId);
            ViewData["RequestId"] = new SelectList(_context.Requests, "RequestId", "RequestId", requestClient.RequestId);
            return View(requestClient);
        }

        // GET: RequestClients/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.RequestClients == null)
            {
                return NotFound();
            }

            var requestClient = await _context.RequestClients.FindAsync(id);
            if (requestClient == null)
            {
                return NotFound();
            }
            ViewData["RegionId"] = new SelectList(_context.Regions, "RegionId", "RegionId", requestClient.RegionId);
            ViewData["RequestId"] = new SelectList(_context.Requests, "RequestId", "RequestId", requestClient.RequestId);
            return View(requestClient);
        }

        // POST: RequestClients/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RequestClientId,RequestId,FirstName,LastName,PhoneNumber,Location,Address,RegionId,NotiMobile,NotiEmail,Notes,Email,StrMonth,IntYear,IntDate,IsMobile,Street,City,State,ZipCode,CommunicationType,RemindReservationCount,RemindHouseCallCount,IsSetFollowupSent,Ip,IsReservationReminderSent,Latitude,Longitude")] RequestClient requestClient)
        {
            if (id != requestClient.RequestClientId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(requestClient);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RequestClientExists(requestClient.RequestClientId))
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
            ViewData["RegionId"] = new SelectList(_context.Regions, "RegionId", "RegionId", requestClient.RegionId);
            ViewData["RequestId"] = new SelectList(_context.Requests, "RequestId", "RequestId", requestClient.RequestId);
            return View(requestClient);
        }

        // GET: RequestClients/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.RequestClients == null)
            {
                return NotFound();
            }

            var requestClient = await _context.RequestClients
                .Include(r => r.Region)
                .Include(r => r.Request)
                .FirstOrDefaultAsync(m => m.RequestClientId == id);
            if (requestClient == null)
            {
                return NotFound();
            }

            return View(requestClient);
        }

        // POST: RequestClients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.RequestClients == null)
            {
                return Problem("Entity set 'ApplicationDbContext.RequestClients'  is null.");
            }
            var requestClient = await _context.RequestClients.FindAsync(id);
            if (requestClient != null)
            {
                _context.RequestClients.Remove(requestClient);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RequestClientExists(int id)
        {
            return (_context.RequestClients?.Any(e => e.RequestClientId == id)).GetValueOrDefault();
        }
    }
}

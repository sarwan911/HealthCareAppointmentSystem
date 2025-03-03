using Microsoft.AspNetCore.Mvc;
using Notification2.Models;
using Notification2.Services;

namespace Notification2.Controllers
{
    public class AppointmentController : Controller
    {
        private readonly NotificationService _notificationService;

        public AppointmentController(NotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpPost]
        public async Task<IActionResult> Create(AppointmentDetails model)
        {
            if (ModelState.IsValid)
            {
                // Save appointment to the database
                // ...

                // Send reminder notification
                await _notificationService.SendAppointmentReminder(model.AppointmentID);

                return RedirectToAction("Index");
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Cancel(int appointmentId)
        {
            // Cancel appointment in the database
            // ...

            // Notify cancellation
            await _notificationService.NotifyCancellation(appointmentId);

            return RedirectToAction("Index");
        }
    }
}
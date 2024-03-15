using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Quote.Interfaces.ServiceInterface;
using Quote.Modal;
using Quote.Models;
using Quote.Services;

namespace Quote.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;
        
        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }
        [HttpGet("GetNotification")]
        public async Task<IActionResult> GetNotification(int userId)
        {
            try
            {
                var consresp = await _notificationService.GetNotificationsAsync(userId);
                return Ok(consresp);

            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }


        public async Task<IActionResult> AddNotification(NotificationModal notificationModal)
        {
            try
            {

                var consresp = await _notificationService.AddNotification(notificationModal);
                return Ok(consresp);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        
    }

}

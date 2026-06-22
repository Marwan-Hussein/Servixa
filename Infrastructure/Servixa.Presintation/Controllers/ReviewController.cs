using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Servixa.Abstractions.Interfaces;
using Servixa.Shared.DTOs.Review;

namespace Servixa.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewService _reviewService;

        public ReviewController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateReview([FromBody] CreateReviewDto dto)
        {
            var result = await _reviewService.CreateReviewAsync(dto);
            return Ok(result);
        }

        [HttpGet("worker/{workerId}")]
        public async Task<IActionResult> GetReviewsByWorker(int workerId)
        {
            var result = await _reviewService.GetReviewsByWorkerAsync(workerId);
            return Ok(result);
        }

        [HttpGet("booking/{bookingId}")]
        public async Task<IActionResult> GetReviewsByBooking(int bookingId)
        {
            var result = await _reviewService.GetReviewsByBookingAsync(bookingId);
            return Ok(result);
        }
    }
}

using Announcement_Test_Task.IRepositories;
using Announcement_Test_Task.Models;
using Microsoft.AspNetCore.Mvc;

namespace Announcement_Test_Task.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AnnouncementsController : ControllerBase
    {
        private readonly IAnnouncementRepository _repository;

        public AnnouncementsController(IAnnouncementRepository repository)
        {
            _repository = repository;
        }

        [HttpPost]
        public async Task<IActionResult> AddAnnouncement(Announcement announcement)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            announcement.DateAdded = DateTime.UtcNow;
            await _repository.AddAsync(announcement);
            return CreatedAtAction(nameof(GetAnnouncement), new { id = announcement.Id }, announcement);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> EditAnnouncement(int id, Announcement announcement)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingAnnouncement = await _repository.GetAsync(id);
            if (existingAnnouncement == null)
                return NotFound();

            announcement.Id = id;
            announcement.DateAdded = existingAnnouncement.DateAdded;
            await _repository.UpdateAsync(announcement);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAnnouncement(int id)
        {
            var announcement = await _repository.GetAsync(id);
            if (announcement == null)
                return NotFound();

            await _repository.DeleteAsync(id);
            return NoContent();
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAnnouncements()
        {
            var announcements = await _repository.GetAllAsync();
            return Ok(announcements);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAnnouncement(int id)
        {
            var announcement = await _repository.GetAsync(id);
            if (announcement == null)
                return NotFound();

            var similarAnnouncements = await GetSimilarAnnouncementsAsync(announcement);
            return Ok(new { Announcement = announcement, Similar = similarAnnouncements });
        }

        private async Task<IEnumerable<Announcement>> GetSimilarAnnouncementsAsync(Announcement announcement)
        {
            var words = new HashSet<string>(
                (announcement.Title + " " + announcement.Description).Split(' ', StringSplitOptions.RemoveEmptyEntries),
                StringComparer.OrdinalIgnoreCase);

            var allAnnouncements = await _repository.GetAllAsync();
            return allAnnouncements
                .Where(a => a.Id != announcement.Id &&
                            (words.Overlaps(a.Title.Split(' ', StringSplitOptions.RemoveEmptyEntries)) ||
                             words.Overlaps(a.Description.Split(' ', StringSplitOptions.RemoveEmptyEntries))))
                .Take(3);
        }
    }
}

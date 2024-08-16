using Announcement_Test_Task.Models;

namespace Announcement_Test_Task.IRepositories
{
    public interface IAnnouncementRepository
    {
        Task AddAsync(Announcement announcement);
        Task UpdateAsync(Announcement announcement);
        Task DeleteAsync(int id);
        Task<Announcement> GetAsync(int id);
        Task<IEnumerable<Announcement>> GetAllAsync();
    }
}

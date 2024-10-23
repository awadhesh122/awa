using CandidateAPI.Models;

namespace CandidateAPI.Data
{
    public interface ICandidateRepository
    {
        Task<Candidate> GetByEmailAsync(string email);
        Task AddOrUpdateAsync(Candidate candidate);
        Task<List<Candidate>> GetAllAsync();
    }

}

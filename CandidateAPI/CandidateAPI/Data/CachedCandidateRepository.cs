namespace CandidateAPI.Data
{
    using CandidateAPI.Models;
    using Microsoft.Extensions.Caching.Memory;

    public class CachedCandidateRepository : ICandidateRepository
    {
        private readonly ICandidateRepository _innerRepository;
        private readonly IMemoryCache _cache;

        public CachedCandidateRepository(ICandidateRepository innerRepository, IMemoryCache cache)
        {
            _innerRepository = innerRepository;
            _cache = cache;
        }

        public async Task<Candidate> GetByEmailAsync(string email)
        {
            return await _cache.GetOrCreateAsync(email, async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5);
                return await _innerRepository.GetByEmailAsync(email);
            });
        }

        public async Task AddOrUpdateAsync(Candidate candidate)
        {
            await _innerRepository.AddOrUpdateAsync(candidate);
            _cache.Remove(candidate.Email);
        }

        public async Task<List<Candidate>> GetAllAsync()
        {
            return await _innerRepository.GetAllAsync();
        }
    }

}

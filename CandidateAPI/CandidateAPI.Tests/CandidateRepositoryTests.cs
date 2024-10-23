using CandidateAPI.Data;
using CandidateAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandidateAPI.Tests
{
    public class CandidateRepositoryTests
    {
        private readonly ApplicationDbContext _context;
        private readonly CandidateRepository _repository;

        public CandidateRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
            _context = new ApplicationDbContext(options);
            _repository = new CandidateRepository(_context);
        }

        [Fact]
        public async Task AddOrUpdateAsync_ShouldAddCandidate_WhenCandidateDoesNotExist()
        {
            // Arrange
            var candidate = new Candidate
            {
                Email = "test@example.com",
                FirstName = "John",
                LastName = "Doe",
                PhoneNumber = "1234567890",
                PreferredCallTime = "Morning",
                LinkedInProfile = "https://linkedin.com/in/johndoe",
                GitHubProfile = "https://github.com/johndoe",
                Comment = "Test comment"
            };

            // Act
            await _repository.AddOrUpdateAsync(candidate);
            var result = await _repository.GetByEmailAsync(candidate.Email);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(candidate.Email, result.Email);
        }

        [Fact]
        public async Task AddOrUpdateAsync_ShouldUpdateCandidate_WhenCandidateExists()
        {
            // Arrange
            var candidate = new Candidate
            {
                Email = "test@example.com",
                FirstName = "John",
                LastName = "Doe",
                PhoneNumber = "1234567890",
                PreferredCallTime = "Morning",
                LinkedInProfile = "https://linkedin.com/in/johndoe",
                GitHubProfile = "https://github.com/johndoe",
                Comment = "Test comment"
            };
            await _repository.AddOrUpdateAsync(candidate);
            candidate.FirstName = "Jane";

            // Act
            await _repository.AddOrUpdateAsync(candidate);
            var result = await _repository.GetByEmailAsync(candidate.Email);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Jane", result.FirstName);
        }

        [Fact]
        public async Task GetByEmailAsync_ShouldReturnNull_WhenCandidateDoesNotExist()
        {
            // Act
            var result = await _repository.GetByEmailAsync("nonexistent@example.com");

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllCandidates()
        {
            // Arrange
            var candidate1 = new Candidate
            {
                Email = "test1@example.com",
                FirstName = "John",
                LastName = "Doe",
                PhoneNumber = "1234567890",
                PreferredCallTime = "Morning",
                LinkedInProfile = "https://linkedin.com/in/johndoe",
                GitHubProfile = "https://github.com/johndoe",
                Comment = "Test comment"
            };
            var candidate2 = new Candidate
            {
                Email = "test2@example.com",
                FirstName = "Jane",
                LastName = "Doe",
                PhoneNumber = "0987654321",
                PreferredCallTime = "Afternoon",
                LinkedInProfile = "https://linkedin.com/in/janedoe",
                GitHubProfile = "https://github.com/janedoe",
                Comment = "Another test comment"
            };
            await _repository.AddOrUpdateAsync(candidate1);
            await _repository.AddOrUpdateAsync(candidate2);

            // Act
            var result = await _repository.GetAllAsync();

            // Assert
            Assert.Equal(2, result.Count);
        }
    }

}

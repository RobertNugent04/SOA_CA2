using Microsoft.EntityFrameworkCore;
using SOA_CA2.Infrastructure;
using SOA_CA2.Interfaces;
using SOA_CA2.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SOA_CA2.Repositories
{
    /// <summary>
    /// Repository for handling Call data operations.
    /// </summary>
    public class CallRepository : ICallRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<CallRepository> _logger;

        public CallRepository(AppDbContext context, ILogger<CallRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <inheritdoc />
        public async Task AddCallAsync(Call call)
        {
            try
            {
                _logger.LogInformation("Adding call to the database with ID: {CallId}.", call.CallId);
                await _context.Calls.AddAsync(call);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding call to the database with ID: {CallId}.", call.CallId);
                throw;
            }

        }

        /// <inheritdoc />
        public async Task<Call> GetCallByIdAsync(int callId)
        {
            try
            {
                _logger.LogInformation("Fetching call by ID: {CallId}.", callId);
                return await _context.Calls.FindAsync(callId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching call by ID: {CallId}.", callId);
                throw;
            }
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Call>> GetCallsForUserAsync(int userId)
        {
            try
            {
                _logger.LogInformation("Fetching calls for user ID: {UserId}.", userId);
                return await _context.Calls
               .Where(c => c.CallerId == userId || c.ReceiverId == userId)
               .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching calls for user ID: {UserId}.", userId);
                throw;
            }
        }

        /// <inheritdoc />
        public async Task UpdateCallAsync(Call call)
        {
            try 
            {
                _logger.LogInformation("Updating call with ID: {CallId}.", call.CallId);
                _context.Calls.Update(call);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating call with ID: {CallId}.", call.CallId);
                throw;
            }
        }

        /// <inheritdoc />
        public async Task SaveChangesAsync()
        {
            try
            {
                _logger.LogInformation("Saving changes to the database.");
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving changes to the database.");
                throw;
            }
        }
    }
}

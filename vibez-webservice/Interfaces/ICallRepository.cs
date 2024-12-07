using SOA_CA2.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SOA_CA2.Interfaces
{
    /// <summary>
    /// Interface for Call repository operations.
    /// </summary>
    public interface ICallRepository
    {
        /// <summary>
        /// Adds a new call to the repository.
        /// </summary>
        Task AddCallAsync(Call call);

        /// <summary>
        /// Retrieves a call by its ID.
        /// </summary>
        Task<Call> GetCallByIdAsync(int callId);

        /// <summary>
        /// Retrieves all calls for a specific user.
        /// </summary>
        Task<IEnumerable<Call>> GetCallsForUserAsync(int userId);

        /// <summary>
        /// Updates an existing call.
        /// </summary>
        Task UpdateCallAsync(Call call);

        /// <summary>
        /// Saves changes to the data store.
        /// </summary>
        Task SaveChangesAsync();
    }
}

using SOA_CA2.Models.DTOs.Call;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SOA_CA2.Interfaces
{
    /// <summary>
    /// Interface for Call service operations.
    /// </summary>
    public interface ICallService
    {
        /// <summary>
        /// Initiates a call from the caller to the receiver.
        /// </summary>
        Task<CallDto> InitiateCallAsync(int callerId, CallCreationDto dto);

        /// <summary>
        /// Updates the status of an existing call.
        /// </summary>
        Task UpdateCallStatusAsync(int callId, int userId, CallStatusUpdateDto dto);

        /// <summary>
        /// Retrieves all calls for a user.
        /// </summary>
        Task<IEnumerable<CallDto>> GetCallsForUserAsync(int userId);
    }
}

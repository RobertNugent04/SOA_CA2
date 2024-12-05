using AutoMapper;
using Microsoft.Extensions.Logging;
using SOA_CA2.Interfaces;
using SOA_CA2.Models;
using SOA_CA2.Models.DTOs.Call;
using SOA_CA2.Models.DTOs.Notification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SOA_CA2.Services
{
    /// <summary>
    /// Service for handling call-related operations.
    /// </summary>
    public class CallService : ICallService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CallService> _logger;
        private readonly INotificationService _notificationService;
        private readonly IMapper _mapper;

        public CallService(
            IUnitOfWork unitOfWork,
            ILogger<CallService> logger,
            INotificationService notificationService,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _notificationService = notificationService;
            _mapper = mapper;
        }

        /// <summary>
        /// Initiates a call from the caller to the receiver.
        /// </summary>
        public async Task<CallDto> InitiateCallAsync(int callerId, CallCreationDto dto)
        {
            try
            {
                _logger.LogInformation("User {CallerId} is initiating a {CallType} call to {ReceiverId}.", callerId, dto.CallType, dto.ReceiverId);

                // Create a new call record
                Call call = new Call
                {
                    CallerId = callerId,
                    ReceiverId = dto.ReceiverId,
                    CallType = dto.CallType,
                    CallStatus = "Initiated",
                    StartedAt = null,
                    EndedAt = null,
                };

                await _unitOfWork.Calls.AddCallAsync(call);
                await _unitOfWork.SaveChangesAsync();

                // Send notification to the receiver
                await _notificationService.SendNotificationAsync(callerId, new NotificationCreationDto
                {
                    UserId = dto.ReceiverId,
                    Type = "Call",
                    ReferenceId = call.CallId,
                    Message = $"is calling you ({dto.CallType})."
                });

                _logger.LogInformation("Call initiated successfully with CallId: {CallId}.", call.CallId);

                return _mapper.Map<CallDto>(call);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error initiating call from {CallerId} to {ReceiverId}.", callerId, dto.ReceiverId);
                throw;
            }
        }

        /// <summary>
        /// Updates the status of an existing call.
        /// </summary>
        public async Task UpdateCallStatusAsync(int callId, int userId, CallStatusUpdateDto dto)
        {
            try
            {
                _logger.LogInformation("User {UserId} is updating call status for CallId: {CallId} to {CallStatus}.", userId, callId, dto.CallStatus);

                Call call = await _unitOfWork.Calls.GetCallByIdAsync(callId);
                if (call == null)
                    throw new ArgumentException("Call not found.");

                if (call.CallerId != userId && call.ReceiverId != userId)
                    throw new UnauthorizedAccessException("User is not authorized to update this call.");

                call.CallStatus = dto.CallStatus;

                if (dto.CallStatus == "Accepted" && dto.StartedAt.HasValue)
                {
                    call.StartedAt = dto.StartedAt.Value;
                }

                if (dto.CallStatus == "Ended" && dto.EndedAt.HasValue)
                {
                    call.EndedAt = dto.EndedAt.Value;
                    if (call.StartedAt.HasValue)
                    {
                        call.Duration = (int)(call.EndedAt.Value - call.StartedAt.Value).TotalSeconds;
                    }
                }

                await _unitOfWork.Calls.UpdateCallAsync(call);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("Call status updated successfully for CallId: {CallId}.", callId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating call status for CallId: {CallId}.", callId);
                throw;
            }
        }

        /// <summary>
        /// Retrieves all calls for a user.
        /// </summary>
        public async Task<IEnumerable<CallDto>> GetCallsForUserAsync(int userId)
        {
            try
            {
                _logger.LogInformation("Retrieving calls for user {UserId}.", userId);

                IEnumerable<Call> calls = await _unitOfWork.Calls.GetCallsForUserAsync(userId);

                return calls.Select(c => _mapper.Map<CallDto>(c));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving calls for user {UserId}.", userId);
                throw;
            }
        }
    }
}

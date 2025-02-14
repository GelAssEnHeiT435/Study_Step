using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace Study_Step_Server.Services
{
    public class CustomUserIdProvider : IUserIdProvider
    {
        public virtual string? GetUserId(HubConnectionContext connection) {
            return connection.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }
    }
}

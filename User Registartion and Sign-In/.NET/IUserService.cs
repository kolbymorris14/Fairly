using Microsoft.AspNetCore.Mvc;
using Sabio.Models;
using Sabio.Models.Domain.Users;
using Sabio.Models.Requests.Users;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sabio.Services
{
    public interface IUserService
    {
        int Create(UserAddRequest model);

        Task<bool> LogInAsync(LoginRequest model);

        Task<bool> LogInTest(string email, string password, int id, string[] roles = null);

        User GetById(int id);

        void UpdateUserStatus(int userId, int statusId);

        void UpdateUserProfileVisability(int userId, bool viewable);

        void UpdateById(UserUpdateRequest model);

        void ConfirmUser(int userId);

        void AddUserToken(string token, int userId, int tokenType);

        void DeleteUserToken(string token);
    }
}
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Sabio.Data;
using Sabio.Data.Providers;
using Sabio.Models;
using Sabio.Models.AppSettings;
using Sabio.Models.Domain;
using Sabio.Models.Domain.Emails;
using Sabio.Models.Domain.Users;
using Sabio.Models.Requests.Users;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace Sabio.Services
{
    public class UserService : IUserService, IBaseUserMapper
    {
        private IAuthenticationService<int> _authenticationService;
        private IDataProvider _dataProvider;
        ILookUpService _lookUpService = null;    

        public UserService(IAuthenticationService<int> authSerice
            , IDataProvider dataProvider
            , ILookUpService lookUpService)
        {
            _authenticationService = authSerice;
            _dataProvider = dataProvider;
            _lookUpService = lookUpService;
          
        }

        public async Task<bool> LogInAsync(LoginRequest model)
        {
            bool isSuccessful = false;

            IUserAuthData response = Get(model);

            if (response != null)
            {
                await _authenticationService.LogInAsync(response);
                isSuccessful = true;
            }

            return isSuccessful;
        }

        public int Create(UserAddRequest model)
        {
            int userId = 0;
            string password = model.Password;
            
            //Password Hashed Here Removed for Security
            
            string procName = "[dbo].[Users_Insert]";

            _dataProvider.ExecuteNonQuery(
                procName,
                inputParamMapper: delegate (SqlParameterCollection paramCol)
                {
                    AddCommonParams(model, paramCol, hashedPassword);

                    SqlParameter idOut = new SqlParameter("@Id", SqlDbType.Int);
                    idOut.Direction = ParameterDirection.Output;
                    paramCol.Add(idOut);
                },
                returnParameters: delegate (SqlParameterCollection returnCol)
                {
                    object oId = returnCol["@Id"].Value;
                    int.TryParse(oId.ToString(), out userId);
                });
            return userId;
        }

        public void AddUserToken(string token, int userId, int tokenType)
        {
            string procName = "[dbo].[UserTokens_Insert]";

            _dataProvider.ExecuteNonQuery(
                procName,
                inputParamMapper: delegate (SqlParameterCollection paramCol)
                {
                    paramCol.AddWithValue("@Token", token);
                    paramCol.AddWithValue("@UserId", userId);
                    paramCol.AddWithValue("@TokenType", tokenType);
                });
        }

        public void DeleteUserToken(string token)
        {
            string procName = "[dbo].[UserTokens_Delete_ByToken]";

            _dataProvider.ExecuteNonQuery(
                procName,
                inputParamMapper: delegate (SqlParameterCollection col)
                {
                    col.AddWithValue("@Token", token);
                },
                returnParameters: null
           );
        }


        #region - GETS -
        private IUserAuthData Get(LoginRequest model)
        {
            string procName = "[dbo].[Users_Select_AuthData]";
            UserBase user = null;
            string hashedPassword = null;

            _dataProvider.ExecuteCmd(
                procName,
                inputParamMapper: delegate (SqlParameterCollection paramCol)
                {
                    paramCol.AddWithValue("@Email", model.Email);
                },
                singleRecordMapper: delegate (IDataReader reader, short set)
                {
                    int startingIndex = 0;
                    List<string> roles = null;
                    user = new UserBase();

                    user.Id = reader.GetSafeInt32(startingIndex++);
                    user.Name = reader.GetSafeString(startingIndex++);
                    hashedPassword = reader.GetSafeString(startingIndex++);
                    string rolesAsString = reader.GetSafeString(startingIndex++);
                    if (!string.IsNullOrEmpty(rolesAsString))
                    {
                        roles = GetRoles(JsonConvert.DeserializeObject<List<AuthRole>>(rolesAsString));
                    }
                    user.Roles = roles;
                    user.TenantId = "FairlyTenant";
                });

            bool isValidCredentials = BCrypt.BCryptHelper.CheckPassword(model.Password, hashedPassword);

            if (isValidCredentials)
            {
                return user;
            }
            else
            {
                return null;
            }
        }

        public User GetById(int id)
        {
            string procName = "[dbo].[Users_Select_ById]";
            User user = null;

            _dataProvider.ExecuteCmd(
                procName,
                inputParamMapper: delegate (SqlParameterCollection paramCol)
                {
                    paramCol.AddWithValue("@Id", id);
                },
                singleRecordMapper: delegate (IDataReader reader, short set)
                {
                    int startingIndex = 0;
                    user = MapFullUser(reader, ref startingIndex);
                }
            );
            return user;
        }

        #endregion

        #region - UPDATES -

        public void UpdateUserStatus(int userId, int statusId)
        {
            string procName = "[dbo].[Users_UpdateStatus]";

            _dataProvider.ExecuteNonQuery(
                procName,
                inputParamMapper: delegate (SqlParameterCollection paramCol)
                {
                    paramCol.AddWithValue("@UserId", userId);
                    paramCol.AddWithValue("@StatusId", statusId);
                });
        }

        public void UpdateUserProfileVisability(int userId, bool viewable)
        {
            string procName = "[dbo].[Users_Update_IsProfileViewable]";

            _dataProvider.ExecuteNonQuery(
                procName,
                inputParamMapper: delegate (SqlParameterCollection paramCol)
                {
                    paramCol.AddWithValue("@UserId", userId);
                    paramCol.AddWithValue("@IsProfileViewable", viewable);
                });
        }

        public void UpdateById(UserUpdateRequest model)
        {
            string procName = "[dbo].[Users_UpdateById]";

            _dataProvider.ExecuteNonQuery(
                procName,
                inputParamMapper: delegate (SqlParameterCollection paramCol)
                {
                    paramCol.AddWithValue("@Id", model.Id);
                    paramCol.AddWithValue("@Email", model.Email);
                    paramCol.AddWithValue("@FirstName", model.FirstName);
                    paramCol.AddWithValue("@LastName", model.LastName);
                });
        }
        #endregion

        public void ConfirmUser(int userId)
        {
            string procName = "[dbo].[Users_Confirm]";

            _dataProvider.ExecuteNonQuery(
                procName,
                inputParamMapper: delegate (SqlParameterCollection paramCol)
                {
                    paramCol.AddWithValue("@UserId", userId);
                });
        }

        #region Mappers

        public BaseUser MapUser(IDataReader reader, ref int startingIndex)
        {

            BaseUser user = new BaseUser();
            user.Id = reader.GetSafeInt32(startingIndex++);
            user.FirstName = reader.GetSafeString(startingIndex++);
            user.LastName = reader.GetSafeString(startingIndex++);
            user.Mi = reader.GetSafeString(startingIndex++);
            user.AvatarUrl = reader.GetSafeString(startingIndex++);
            return user;
        }

        private User MapFullUser(IDataReader reader, ref int startingIndex)
        {
            User user = new User();

            user.Id = reader.GetSafeInt32(startingIndex++);
            user.FirstName = reader.GetSafeString(startingIndex++);
            user.LastName = reader.GetSafeString(startingIndex++);
            user.Mi = reader.GetSafeString(startingIndex++);
            user.AvatarUrl = reader.GetSafeString(startingIndex++);
            user.Email = reader.GetSafeString(startingIndex++);
            user.IsConfirmed = reader.GetSafeBool(startingIndex++);
            user.Status = _lookUpService.MapSingleLookUp(reader, ref startingIndex);
            user.DateCreated = reader.GetSafeDateTime(startingIndex++);
            user.DateModified = reader.GetSafeDateTime(startingIndex++);
            user.IsProfileViewable = reader.GetSafeBool(startingIndex++);
            return user;
        }

        private static void AddCommonParams(UserAddRequest model, SqlParameterCollection paramCol, string hashedPassword)
        {
            paramCol.AddWithValue("@Email", model.Email);
            paramCol.AddWithValue("@FirstName", model.FirstName);
            paramCol.AddWithValue("@LastName", model.LastName);
            paramCol.AddWithValue("@Mi", model.Mi);
            paramCol.AddWithValue("@Password", hashedPassword);
            paramCol.AddWithValue("@AvatarUrl", model.AvatarUrl);
            paramCol.AddWithValue("@StatusId", model.StatusId);
            paramCol.AddWithValue("@IsProfileViewable", model.IsProfileViewable);
        }

        private static List<string> GetRoles(List<AuthRole> incomingRoles)
        {
            List<string> roles = new List<string>();

            foreach (AuthRole role in incomingRoles)
            {
                roles.Add(role.Name);
            }
            return roles;
        }

        #endregion
    }
}

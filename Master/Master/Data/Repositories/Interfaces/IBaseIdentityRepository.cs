using Master.Core.DTO;

namespace Master.Data.Repositories.Interfaces
{
    public interface IBaseIdentityRepository
    {
        Task<bool> CheckIfUserExistsByEmail(string email);
        Task<bool> CheckIfUserIsConfirmed(string email);
        Task<UserDto> GetUserByEmail(string email);
        Task<List<UserDto>> GetUserEmailsByIds(List<int> userIds);
        Task<AuthUserDto> GetUserRole(string email);
        public Task<List<AuthUserDto>> GetUserRoles(string email);
    }
}

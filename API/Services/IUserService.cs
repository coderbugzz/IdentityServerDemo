using API.Models;

namespace API.Services
{
    public interface IUserService
    {
        Task<ResponseModel<List<UserModel>>> GetUsers();  
    }
}

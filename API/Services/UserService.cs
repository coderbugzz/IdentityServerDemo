using API.Models;
using AutoMapper;
using DataAccess.Data;
using Microsoft.EntityFrameworkCore;

namespace API.Services
{
    public class UserService : IUserService
    {
        private readonly AppDBContext _dBContext;
        private readonly IMapper _mapper;

        public UserService(AppDBContext dBContext, IMapper mapper)
        {
            _dBContext = dBContext;
            _mapper = mapper;
        }

        public async Task<ResponseModel<List<UserModel>>> GetUsers()
        {
            ResponseModel<List<UserModel>> response = new ResponseModel<List<UserModel>>();
            response.Data = _mapper.Map<List<UserModel>>(await _dBContext.users.ToListAsync());
            response.Code = 200;
            return response;
        }
    }
}

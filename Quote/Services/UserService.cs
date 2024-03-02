using AutoMapper;
using Quote.Interfaces.ServiceInterface;
using Quote.Modal;
using Quote.Models;
using Quote.Repositorys;

namespace Quote.Services
{

    public class UserService : UserInterface
    {

        private readonly UserRepository _repo;
        private readonly IMapper _mapper;

        public UserService(UserRepository userRepository, IMapper mapper)
        {
            _repo = userRepository;
            _mapper = mapper;
        }

        public List<User> GetUsers()
        {
            return _repo.GetAll().ToList();
        }

        public User Register(RegisterModal user)
        {
             if(_repo.IsEmailExists(user.Email))
            {
                return null;
            }
            else
            {
                try
                {                  
                    User usermodel= new User();

                    var newuser = _mapper.Map(user, usermodel);
                    newuser.Role = "CUS";
                    
                    _repo.Add(newuser);
                    return newuser;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }
    }
}

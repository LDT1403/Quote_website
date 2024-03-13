using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Quote.Interfaces.ServiceInterface;
using Quote.Modal;
using Quote.Models;
using Quote.Repositorys;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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

        public async Task<List<User>> GetStaffById(int staffid)
        {
            try
            {
                var list = await _repo.GetAllAsync();

                if(list != null) {
                    list = list.Where(p => p.ManagerId == staffid).ToList();
                    return list;
                }
                return null;

            }catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        
        public async Task<List<User>> GetStaffByStatus(int staffid)
        {
            try
            {
                var list = await _repo.GetAllAsync();

                if(list != null)
                {
                    list = list.Where(p=> p.ManagerId == staffid && p.Status == "0").ToList();
                    return list;
                }
                else
                {
                    return null;
                }

            }catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<User>> GetUsersAsync()
        {
            try
            {
                return await _repo.GetAllAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error getting users: " + ex.Message);
            }
        }
        public async Task<User> GetUserIDAsync(int userId)
        {
            try
            {
                return await _repo.GetByIdAsync(userId);
            }
            catch (Exception ex)
            {
                throw new Exception("Error getting users: " + ex.Message);
            }
        }

        public async Task<User> RegisterAsync(RegisterModal user)
        {
            try
            {
                if (await _repo.IsEmailExists(user.Email))
                {
                    return null;
                }
                else
                {
                    User userModel = _mapper.Map<RegisterModal, User>(user);
                    userModel.Role = "CUS";

                    await _repo.AddAsync(userModel);
                    return userModel;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error registering user: " + ex.Message);
            }
        }

        public async Task<User> UserById(int userId)
        {
            try
            {
                var user = await _repo.GetByIdAsync(userId);
                return user;
            }catch (Exception ex)
            {
                throw new Exception("Error registering user: " + ex.Message);
            }
        }


        public async Task<User> UpdateStatusStaff(User user)
        {
            try
            {
                var item = await _repo.UpdateAsync(user);
                if (item == null)
                {
                    return null;
                }
                return item;

            }catch(Exception ex)
            {
                throw new Exception("Error registering user: " + ex.Message);
            }
        }
        public async Task<User> RegisterStaffAsync(User user)
        {
            try
            {
                if (await _repo.IsEmailExists(user.Email))
                {
                    return null;
                }
                else
                {
                    user.Role = "ST";

                    await _repo.AddAsync(user);
                    return user;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error registering user: " + ex.Message);
            }
        }
    }
}

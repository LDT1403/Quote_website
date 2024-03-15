using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Quote.Interfaces.RepositoryInterface;
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
        private readonly IRepoBase<User> _repo;
        private readonly IMapper _mapper;

        public UserService(IRepoBase<User> repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<List<User>> GetStaffById(int staffid)
        {
            try
            {
                var list = await _repo.GetAllAsync();

                if (list != null)
                {
                    list = list.Where(p => p.ManagerId == staffid).ToList();
                    return list;
                }
                return null;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public async Task<List<User>> GetStaffByStatus(int staffid)
        {
            try
            {
                var list = await _repo.GetAllAsync();

                
                    list = list.Where(p => p.ManagerId == staffid && p.Status == "0").ToList();
                    return list;
             

            }
            catch (Exception ex)
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

        public async Task<User> AddUserByGoogle(RegisterGoogle user)
        {
            try
            {
                User userModel = _mapper.Map<RegisterGoogle, User>(user);
                userModel.Role = "CUS";
                userModel.Password = "123456";
                userModel.Position = "Người Dùng Google";
                var useData = await _repo.AddAsync(userModel);
                return useData;
            }
            catch (Exception ex)
            {
                throw new Exception("Error registering user: " + ex.Message);
            }
        }

        public async Task<User> RegisterAsync(RegisterModal user)
        {
            try
            {
                var list = await _repo.GetAllAsync();
                foreach (var item in list)
                {
                    if (item.Email == user.Email)
                    {
                        return null;
                    }
                }
                User userModel = _mapper.Map<RegisterModal, User>(user);
                userModel.Role = "CUS";
                userModel.Position = "Người Dùng Hệ Thống";
                await _repo.AddAsync(userModel);
                return userModel;
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
            }
            catch (Exception ex)
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

            }
            catch (Exception ex)
            {
                throw new Exception("Error registering user: " + ex.Message);
            }
        }
        public async Task<User> RegisterStaffAsync(User user)
        {
            try
            {
                var list = await _repo.GetAllAsync();
                foreach (var item in list)
                {
                    if (item.Email == user.Email)
                    {
                        return null;
                    }
                }
                user.Role = "ST";
                user.Status = "0";
                await _repo.AddAsync(user);
                return user;
            }
            catch (Exception ex)
            {
                throw new Exception("Error registering user: " + ex.Message);
            }
        }
    }
}

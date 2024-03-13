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
    public class CartService : ICartService
    {
        private readonly IRepoBase<CartDetail> _repository;
        private readonly IRepoBase<Cart> _cartRepository;

        public CartService(IRepoBase<CartDetail> repository, IRepoBase<Cart> cartRepository)
        {
            _repository = repository;
            _cartRepository = cartRepository;
        }

        public async Task<CartDetail> AddCartAsync(CartDetail cartDetail)
        {
            try
            {
                await _repository.AddAsync(cartDetail);
                return cartDetail;
            }
            catch (Exception ex)
            {
                throw new Exception("Error adding cart detail: " + ex.Message);
            }
        }

        public async Task<Cart> CreateCartOfUser(Cart cart)
        {
            try
            {
                await _cartRepository.AddReturnAsync(cart);
                return cart;
            }
            catch (Exception ex)
            {
                throw new Exception("Error creating user's cart: " + ex.Message);
            }
        }

        public async Task<bool> DeleteCartDetail(int cartDetailId)
        {
            try
            {
                return await _repository.DeleteItemAsync(cartDetailId);
            }
            catch (Exception ex)
            {
                throw new Exception("Error getting cart IDs: " + ex.Message);
            }
        }

        public async Task<List<Cart>> GetCartAsync()
        {
            try
            {
                return await _cartRepository.GetAllAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error getting cart IDs: " + ex.Message);
            }
        }

        public async Task<List<CartDetail>> GetCartsAsync()
        {
            try
            {
                return await _repository.GetAllAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error getting carts: " + ex.Message);
            }
        }
    }
}

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
       
        private readonly IRepoBase<Cart> _cartRepository;

        public CartService(IRepoBase<Cart> cartRepository)
        {
          
            _cartRepository = cartRepository;
        }

        public async Task<Cart> AddCartAsync(Cart cartDetail)
        {
            try
            {
                await _cartRepository.AddAsync(cartDetail);
                return cartDetail;
            }
            catch (Exception ex)
            {
                throw new Exception("Error adding cart detail: " + ex.Message);
            }
        }



        public async Task<bool> DeleteCartDetail(int cartDetailId)
        {
            try
            {
                return await _cartRepository.DeleteItemAsync(cartDetailId);
            }
            catch (Exception ex)
            {
                throw new Exception("Error getting cart IDs: " + ex.Message);
            }
        }

        public async Task<List<Cart>> GetCartsAsync()
        {
            try
            {
                return await _cartRepository.GetAllAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error getting carts: " + ex.Message);
            }
        }
        public async Task<Cart> GetCartIdAsync(int cartId)
        {
            try
            {
                return await _cartRepository.GetByIdAsync(cartId);
            }
            catch (Exception ex)
            {
                throw new Exception("Error getting carts: " + ex.Message);
            }
        }
    }
}

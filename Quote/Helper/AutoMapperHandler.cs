using AutoMapper;
using Quote.Modal;
using Quote.Models;

namespace Quote.Helper
{
    public class AutoMapperHandler:Profile
    {
        public AutoMapperHandler() {
            CreateMap<User, RegisterModal>().ReverseMap();
            CreateMap<User, RegisterGoogle>().ReverseMap();
            CreateMap<Product, ProductModal>().ReverseMap();

            CreateMap<NotificationModal, Notification>().ReverseMap();
        }
    }
}

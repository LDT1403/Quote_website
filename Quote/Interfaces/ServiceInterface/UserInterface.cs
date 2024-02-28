using Quote.Modal;
using Quote.Models;

namespace Quote.Interfaces.ServiceInterface
{
    public interface UserInterface
    {
        List<User> GetUsers();
        User Register(RegisterModal user);

    }
}

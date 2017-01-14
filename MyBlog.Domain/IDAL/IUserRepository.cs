using MyBlog.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Domain.IDAL
{
   public interface IUserRepository
    {
        bool Save(User user);
        User Details(int? Id);
        IEnumerable<User> UserIEmum { get; }
        List<User> UserList { get; }
        User Delete(int? Id);

        bool UniqueEmail(string email);

    }
}

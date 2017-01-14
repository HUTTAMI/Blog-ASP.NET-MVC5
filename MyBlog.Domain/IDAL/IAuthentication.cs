using MyBlog.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Domain.IDAL
{
   public interface IAuthentication
    {
        User Authenticate(string username, string password);
        bool Logout();
    }
}

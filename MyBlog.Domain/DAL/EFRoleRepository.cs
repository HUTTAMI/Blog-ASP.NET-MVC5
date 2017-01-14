using MyBlog.Domain.IDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyBlog.Domain.Model;

namespace MyBlog.Domain.DAL
{
    public class EFRoleRepository : IRoleRepository
    {
        EFDbContext context = new EFDbContext();

        public IEnumerable<Role> RoleIEnum
        {
            get { return context.Roles; }
        }
    }
}

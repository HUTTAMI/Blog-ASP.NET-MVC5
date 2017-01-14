using MyBlog.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Domain.IDAL
{
  public  interface IImageRepository
    {
        bool Save(Image image);
        IEnumerable<Image> ImageIEnum { get; }
        List<Image> ImageList { get; }
        Image Delete(int? Id);
        Image Details(int? Id);

    }
}

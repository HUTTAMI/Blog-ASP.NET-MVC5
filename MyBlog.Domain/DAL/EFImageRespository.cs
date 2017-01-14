using MyBlog.Domain.IDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyBlog.Domain.Model;

namespace MyBlog.Domain.DAL
{
   public class EFImageRespository:IImageRepository
    {
        EFDbContext context = new EFDbContext();
        bool Succeeded;

        public IEnumerable<Image> ImageIEnum
        {
            get
            {
             return   context.Images;
            }

           
        }

        public List<Image> ImageList
        {
            get
            {
                return context.Images.ToList<Image>();
            }

          
        }

        public Image Details(int? Id)
        {
            Image dbEntry = context.Images.Find(Id);

            return dbEntry;
        }
        public Image Delete(int? Id)
        {
            Image dbEntry = context.Images.Find(Id);
            if (dbEntry != null)
            {
                context.Images.Remove(dbEntry);
                context.SaveChanges();
            }
            return dbEntry;
        }

        public bool Save(Image image)
        {


            if (image.Id == 0)
            {

                context.Images.Add(image);
                if (context.SaveChanges() > 0)
                {
                    Succeeded = true;
                }
                else
                {
                    Succeeded = false;
                }


            }
            else
            {
                Image dbEntry = context.Images.Find(image.Id);
                if (dbEntry != null)
                {
                    dbEntry.Id = image.Id;
                    dbEntry.Imagepath = image.Imagepath;
                    dbEntry.Size = image.Size;
                    dbEntry.Create_time = image.Create_time;
                    dbEntry.UserId = image.UserId;
                   
                 //   context.Images.Add(dbEntry);

                    if (context.SaveChanges() > 0)
                    {
                        Succeeded = true;
                    }
                    else
                    {
                        Succeeded = false;
                    }
                    image.Id = dbEntry.Id;
                }
            }


            return Succeeded;
        }
    }
}

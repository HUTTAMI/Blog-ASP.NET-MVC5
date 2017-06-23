using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Data
{
   
   public class Post
    {
       [Key]
        public int PostId { get; set; }

        [Required(ErrorMessage = "Title is required")]

        public string Title { get; set; }
        [Required(ErrorMessage = "Post is required")]
        public string Post_Content { get; set; }

        [DisplayName("Create Time:") ]
        
        public DateTime Create_time { get; set; }

        [DisplayName("Last Update:")]
        
        public DateTime Update_time { get; set; }
        public int UserId { get; set; }
        [Required(ErrorMessage = "Tag is required")]
        [DisplayName("Tags:")]
        public string Tages { get; set; }
        [Required(ErrorMessage = "Category is required")]
        [DisplayName("Category:")]
        public int CategoryId { get; set; }
        public int Frequence { get; set; }
        [Required(ErrorMessage = "Featured Image is required")]
        [DisplayName("Featured Image:")]
        public string FeaturedImage { get; set; }

        public virtual User UserDetails { get; set; }
        public virtual Category CategoryDetail { get; set; }
        public virtual IEnumerable<Category> CategoryDetails { get; set; }

    }
}

using MyBlog.Domain.IDAL;
using MyBlog.Domain.Model;
using MyBlog.UI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace MyBlog.UI.Controllers
{
    
    public class PostController : Controller
    {
        private readonly IPostRepository repositoryPost;
        private readonly ICategoryRepository repositoryCategory;
        private readonly ICommentRepository repositoryComment;
        private readonly ISettingRepository repositorySetting;
        // GET: Post
        public PostController(IPostRepository repoPost,ICategoryRepository repoCategory,
                                                       ICommentRepository repoComment,
                                                       ISettingRepository repoSetting)
        {
            repositoryPost = repoPost;
            repositoryCategory = repoCategory;
            repositoryComment = repoComment;
            repositorySetting = repoSetting;
        }
        [Authorize(Roles = "Admin")]
        //public ActionResult Index(int page = 1)
        //{
        //    int PageSize = 5;
        //    PostViewModel model = new PostViewModel
        //    {
        //        Posts = repositoryPost.PostIEnum
        //        .OrderBy(p => p.PostId)
        //        .OrderByDescending(p => p.Create_time)
        //        .Skip((page - 1) * PageSize)
        //        .Take(PageSize),
        //        PagingInfo = new PagingInfo
        //        {
        //            CurrentPage = page,
        //            ItemsPerPage = PageSize,
        //            TotalItems = repositoryPost.PostList.Count()
        //        },
        //    };
        //    return View(model);
        //}

       
        public ActionResult Index()
        {
            
           List<Post> model = repositoryPost.PostList;
            return View(model);
        }
        [Authorize(Roles = "SuperUser,Admin")]
        public ActionResult SuperUserIndex(int page = 1)
        {

            List<Post> model = repositoryPost.PostList;
            return View(model);
        }
        [AllowAnonymous]
        [ValidateInput(false)]
        public ActionResult PostByCategory(int category, int page = 1)
        {
            int PageSize = 5;
            PostViewModel model = new PostViewModel
            {
                Posts = repositoryPost.PostIEnum
                .Where(p =>p.CategoryId==category)
                .OrderBy(p => p.PostId)
                .OrderByDescending(p => p.Create_time)
                .Skip((page - 1) * PageSize)
                .Take(PageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    TotalItems = repositoryPost.PostList.Where(x => x.CategoryId==category).Count()
                },
                CurrentCategory =category
            };
            return View(model);
        }
        [Authorize(Roles = "SuperUser,Admin")]
        public ActionResult NewPost()
        {
            Post model = GetPostSession();
            model.PostId = 0;//Becouse HttpPost NewPost, Need Id to know Edit or AddNew .
            model.CategoryDetails = repositoryCategory.CategoryIEnum;
            return View(model);
           
        }
        [ValidateInput(false)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "SuperUser,Admin")]
        public ActionResult NewPost(Post data)
        {
            Post obj = GetPostSession();
           
            if (ModelState.IsValid)
            {

                var identity = (HttpContext.User as MyPrincipal).Identity as MyIdentity;
                int _CurrentUserId = Convert.ToInt32(identity.User.UserId);
                if (_CurrentUserId == 0)
                {
                    //becouse Sometime id = 0 ?????!!!! maybe session die???????
                    return View(data);
                }
              
                obj.PostId = data.PostId;
                obj.Title = data.Title;
                obj.Post_Content = data.Post_Content;

               
                obj.Update_time = DateTime.Now;//Need solution for this field no need any value.
                obj.UserId = _CurrentUserId;
                obj.Tages = data.Tages;
                obj.CategoryId = data.CategoryId;
                obj.Frequence = 0;//If not 0 will be Null on DB , we cant  do Null +1 .
                if (data.PostId == 0)
                {
                    obj.Create_time = DateTime.Now;
                    repositoryCategory.IncreaseFreqOne(data.CategoryId);//How many time we use this category
                }
                else
                {
                    obj.Create_time = data.Create_time;
                }
                repositoryPost.Save(obj);
                int? Newid = obj.PostId;
                if (obj != null)
                {
                    if(data.PostId == 0)
                    { 
                      TempData["message"] = string.Format("{0} was Added Successfully", obj.Title);
                     }
                    else
                    {
                      TempData["message"] = string.Format("{0} was Edited Successfully", obj.Title);
                    }
                }
                return RedirectToAction("Details", new { Id = Newid });
            }
            data.CategoryDetails = repositoryCategory.CategoryIEnum;
            return View(data);
        }
        [AllowAnonymous]
        public ActionResult Details(int? Id)
        {
            if (Id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post model = repositoryPost.Details(Id);

          // model.UserDetails.FName
            if (model == null)
            {
                return HttpNotFound();
            }
            return View(model);
        }


        [Authorize(Roles = "Admin,SuperUser")]
        public ActionResult Edit(int ?Id)
        {
            if (Id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post model = repositoryPost.Details(Id);
            model.CategoryDetails = repositoryCategory.CategoryIEnum;

            if (model == null)
            {
                return HttpNotFound();
            }
            //Send you to NewPost.chtml to save copy same page 
            return View("NewPost",model);
        }

        /// <summary>
        /// No Need This Method Becosue HttpPost NewPost do same  job . with same NewPost.chtml
        /// 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        //[ValidateInput(false)]
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit(Post data)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        Post obj = GetPostSession();
        //        obj.PostId = data.PostId;
        //        obj.Title = data.Title;
        //        obj.Post_Content = data.Post_Content;
        //        obj.Create_time = data.Create_time;
        //        obj.Update_time = DateTime.Now;//Need solution for this field no need any value
        //        obj.Tages = data.Tages;
        //        repositoryPost.SavePost(obj);
        //        int? Newid = obj.PostId;
        //        if (obj != null)
        //        {
        //            TempData["message"] = string.Format("{0} was Edited Successfully", obj.Title);
        //        }
        //        return RedirectToAction("Details", new { Id = Newid });
        //    }
        //    return View();
        //}
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? Id)
        {
            if (Id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadGateway);
            }
            Post post = repositoryPost.Details(Id);
           
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost,ActionName("Delete")]
        public ActionResult DeleteConfirm(int? Id)
        {
            Post post = repositoryPost.Details(Id);
            if (post.Frequence != 0)
            {
                string CommnetError = string.Format("You cannot delete this Post , Because its has {0} Comments", post.Frequence.ToString());
                //  ModelState.AddModelError("", CommnetError);
                TempData["message"] = CommnetError;
                return View(post);
            }
            Post _post = repositoryPost.Delete(Id);
            repositoryCategory.DecreaseFreqOne(post.CategoryId);//How many time we use this category
            if (_post != null)
            {
                TempData["message"] = string.Format("{0} was deleted", post.Title);
            }
            return RedirectToAction("Index","Post");
        }

        [AllowAnonymous]
        [ChildActionOnly]
        public ActionResult LastPost()
        {
            Setting _NumOfLastPost;
            _NumOfLastPost = repositorySetting.GetSetting;
            IEnumerable<Post> Model;
            Model = repositoryPost.PostIEnum.OrderByDescending(c => c.Create_time).Take(_NumOfLastPost.NumberOfLastPost);
            return PartialView("_LastPost",Model);
        }
        public ActionResult TopPost()
        {
            Setting _NumOfTopPost;
            _NumOfTopPost = repositorySetting.GetSetting;

            IEnumerable<Post> Model;
            Model = repositoryPost.PostIEnum.OrderByDescending(p =>p.Frequence).Take(_NumOfTopPost.NumberOfTopPost);
            return PartialView("_TopPost", Model);
        }
        
        public ActionResult Tag(string teg)
        {
            IEnumerable<Post> Model;
            Model = repositoryPost.PostIEnum.Where(p => p.Tages.Contains(teg));
            return View(Model);
        }

        [AllowAnonymous]
        [ValidateInput(false)]
        public ActionResult PostByTag(string tag, int page = 1)
        {
            int PageSize = 5;
            PostViewModel model = new PostViewModel
            {
                Posts = repositoryPost.PostIEnum
                .Where(p => p.Tages.StartsWith(tag) || p.Tages.EndsWith(tag))
                .OrderBy(p => p.PostId)
                .OrderByDescending(p => p.Create_time)
                .Skip((page - 1) * PageSize)
                .Take(PageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    //Start with || End with its same LIKE in SQL server
                    TotalItems = repositoryPost.PostList.Where(x => x.Tages.StartsWith(tag) || x.Tages.EndsWith(tag)).Count()
                },
                CurrentTag = tag
            };
            return View(model);
        }
        ///Sessions
        ///

        private Post GetPostSession()
        {
            if (Session["post"] == null)
            {
                Session["post"] = new Post();
            }
            return (Post)Session["post"];
        }

    }
}
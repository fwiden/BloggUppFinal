using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BlogUpp.Models;

namespace BlogUpp.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            using (BlogConnection conn = new BlogConnection())
            {

                var theid = TempData["id"];

                if (theid == null || (int)theid == 0)
                {
                    List<Post> allPost = conn.Posts.Select(x => x).OrderByDescending(x => x.BlogPostedOn).ToList();
                    ViewBag.AllPost = allPost;                    
                }
                else
                {
                    List<Post> allpost = conn.Posts.Where(x => x.CategoryID == (int)theid).OrderByDescending(x => x.BlogPostedOn).ToList();
                    ViewBag.AllPost = allpost;                    
                }                

                List<SelectListItem> Categorier = conn.Categories.Select(c => new SelectListItem { Value = c.CategoryID.ToString(), Text = c.CategoryDescription }).ToList();
                ViewBag.Drop = Categorier;

                List<Category> CatAll = conn.Categories.Select(x => x).ToList();
                ViewBag.AllCat = CatAll;
            }             
            return View();
        }

        [HttpPost]
        public ActionResult Index(Post post)
        {
                using (BlogConnection conn = new BlogConnection())
            {
                if (post.BlogTitle==null)
                {
                    List<Post> allpost = conn.Posts.Select(x => x).OrderByDescending(x => x.BlogPostedOn).ToList();
                    ViewBag.AllPost = allpost;
                }
                else
                {
                    List<Post> allpost = conn.Posts.Where(x => x.BlogTitle.Contains(post.BlogTitle)).OrderByDescending(y=>y.BlogPostedOn).ToList();
                    ViewBag.AllPost = allpost;
                }

                List<SelectListItem> Categorier = conn.Categories.Select(c => new SelectListItem { Value = c.CategoryID.ToString(), Text = c.CategoryDescription }).ToList();
                ViewBag.Drop = Categorier;

                List<Category> CatAll = conn.Categories.Select(x => x).ToList();
                ViewBag.AllCat = CatAll;                           
            }
            return View();          
        }

        public ActionResult Create()
        {
            using (BlogConnection conn = new BlogConnection())
            {
                List<SelectListItem> Categorier= conn.Categories.Select(c => new SelectListItem { Value = c.CategoryID.ToString(), Text = c.CategoryDescription }).ToList();
                ViewBag.Cats = Categorier;
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Create(Post newPost, FormCollection items)
        {
            newPost.CategoryID = int.Parse(items["Drop"]);
            newPost.BlogPostedOn = DateTime.Now;

            using (BlogConnection conn = new BlogConnection())
            {
                conn.Posts.Add(newPost);
                conn.SaveChanges();

                return RedirectToAction("Index");
            }
        }

        public ActionResult Sort(int id)
        {
            using (BlogConnection conn = new BlogConnection())
            {
                TempData["id"] = id;
                return RedirectToAction("Index");
            }
        }
    }
}
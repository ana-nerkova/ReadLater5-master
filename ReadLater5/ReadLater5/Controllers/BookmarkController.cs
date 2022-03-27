using Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using Microsoft.IdentityModel.Protocols;


namespace ReadLater5.Controllers
{
    public class BookmarkController : Controller
    {
        IBookmarkService _bookmarkService;
        ICategoryService _categoryService;
        int categoryId;

        
        public BookmarkController(IBookmarkService bookmarkService, ICategoryService categoryService)
        {
            _bookmarkService = bookmarkService;
            _categoryService = categoryService;
        }
        // GET: Bookmarks
        public IActionResult Index()
        {
            var UserEmail = User.Identity.Name;
            List<Bookmark> model = _bookmarkService.GetBookmarks().Where(x=> x.UserEmail == UserEmail).ToList();
            
            return View(model);
        }

        public void SetCategoryId(int id)
        {
            this.categoryId = id;
        }

        // GET: Bookmark/Details/1
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return new StatusCodeResult(Microsoft.AspNetCore.Http.StatusCodes.Status400BadRequest);
            }
            Bookmark bookmark = _bookmarkService.GetBookmark((int)id);
            if (bookmark == null)
            {
                return new StatusCodeResult(Microsoft.AspNetCore.Http.StatusCodes.Status404NotFound);
            }
            bookmark.Category = _categoryService.GetCategory((int)bookmark.CategoryId);
            return View(bookmark);

        }

        // GET: Bookmark/Create
        public IActionResult Create()
        {
            ViewBag.CList = _categoryService.GetCategories();

            return View();
        }

        // POST: Bookmarks/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Bookmark bookmark)
        {
            if (ModelState.IsValid)
            {
                List<Category> model = _categoryService.GetCategories();
                bookmark.UserEmail = User.Identity.Name;

                bookmark.CreateDate = DateTime.Now;
                _bookmarkService.CreateBookmark(bookmark);
                return RedirectToAction("Index");
            }

            return View(bookmark);
        }

        //// GET: Bookmarks/Edit/1
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new StatusCodeResult(Microsoft.AspNetCore.Http.StatusCodes.Status400BadRequest);
            }
            Bookmark Bookmark = _bookmarkService.GetBookmark((int)id);
            if (Bookmark == null)
            {
                return new StatusCodeResult(Microsoft.AspNetCore.Http.StatusCodes.Status404NotFound);
            }

            ViewBag.CList = _categoryService.GetCategories();
            return View(Bookmark);
        }

        // POST: Bookmark/Edit/1
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Bookmark bookmark)
        {
            bookmark.UserEmail = User.Identity.Name;
            if (ModelState.IsValid)
            {
                _bookmarkService.UpdateBookmark(bookmark);
                return RedirectToAction("Index");
            }
            return View(bookmark);
        }

        public ActionResult GoToLink(int id, String link)
        {
            Bookmark bookmark = _bookmarkService.GetBookmark((int)id);
            bookmark.counter++;
            _bookmarkService.UpdateBookmark(bookmark);
            return Redirect(link);
        }

        public IActionResult ViewStats()
        {

            List<Bookmark> model = _bookmarkService.GetBookmarks().ToList();
            //string connectionString = connectionString;
            //SqlConnection(ConfigurationManager.ConnectionStrings["sqlconnection"].ConnectionString);
            //ConnectionStringSettingsCollection connectionString = ConfigurationManager.ConnectionStrings;
            using (var conn = new SqlConnection())
            {
                using (var comm = conn.CreateCommand())
                {
                    conn.Open();


                    //--View most clicked links per user
                    comm.CommandText = "CREATE VIEW MostClicked as SELECT UserEmail, MAX(counter) as 'Clicked' FROM Bookmark GROUP BY UserEmail          select distinct m.UserEmail, Clicked, Url from Bookmark inner join MostClicked m on Bookmark.UserEmail = m.UserEmail and Bookmark.counter = m.Clicked";

                    //----Average clicks per User
                    comm.CommandText = "SELECT UserEmail, AVG(counter) as 'Clicked' FROM Bookmark GROUP BY UserEmail";

                    //Most clicked Category per User
                    comm.CommandText = "CREATE VIEW MostClickedCat as SELECT UserEmail, MAX(counter) as 'Clicked' FROM Bookmark GROUP BY UserEmail          select distinct m.UserEmail, c.Name from Bookmark b inner join MostClickedCat m on b.UserEmail = m.UserEmail inner join Categories c on b.CategoryId = c.ID and b.counter = m.Clicked";

                    comm.ExecuteNonQuery();
                    int value = (int)comm.ExecuteScalar();
                    SqlDataReader reader = comm.ExecuteReader();

                }
            }

            return View(model);
        }

        //GET: Bookmark/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new StatusCodeResult(Microsoft.AspNetCore.Http.StatusCodes.Status400BadRequest);
            }
            Bookmark bookmark = _bookmarkService.GetBookmark((int)id);
            if (bookmark == null)
            {
                return new StatusCodeResult(Microsoft.AspNetCore.Http.StatusCodes.Status404NotFound);
            }
            return View(bookmark);
        }

        // POST: Bookmark/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            Bookmark bookmark = _bookmarkService.GetBookmark(id);
            _bookmarkService.DeleteBookmark(bookmark);
            return RedirectToAction("Index");
        }
    }
}

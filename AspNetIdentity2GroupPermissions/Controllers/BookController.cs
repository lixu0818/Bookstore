using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IdentitySample.Models;

namespace TestBookstore.Web.Controllers
{
    
    public class BookController : Controller
    {
        public ActionResult BookList(int pageNumber, int bookPerPage = 6)
        {
            List<Book> activeBooks = null;

            using (BookstoreEntities dbContext = new BookstoreEntities())
            {
                ViewBag.totalCount = dbContext.Book.Where(book => book.status == true).Count();
                activeBooks = dbContext.Book.Where(book => book.status == true).
                    OrderBy(b => b.Id).Skip(bookPerPage * (pageNumber - 1)).
                    Take(bookPerPage).ToList();

            }

            return View(activeBooks);
        }


        public ActionResult HiddenBookList()
        {
            List<Book> allHiddenBook = null;
            using (BookstoreEntities dbContext = new BookstoreEntities())
            {
                allHiddenBook = dbContext.Book.Where(book => book.status == false).ToList();
            }

            return View("BookList", allHiddenBook);
        }


        public ActionResult ShowBookDetails(int id)
        {
            Book target = null;
            using (BookstoreEntities dbContext = new BookstoreEntities())
            {
                target = dbContext.Book.SingleOrDefault(t => t.Id == id);
            }
            return View(target);
        }


        public ActionResult ShowBookByPrice(int minPrice, int MaxPrice)
        {
            List<Book> selectedBooks = null;
            using (BookstoreEntities dbContext = new BookstoreEntities())
            {
                selectedBooks = dbContext.Book.Where(book => book.Price >= minPrice && book.Price < MaxPrice && book.status == true).ToList();
            }
            return View("BookList", selectedBooks);
        }

        [Authorize(Roles ="Admin")]
        public ActionResult AddBook()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult AddBook(Book book)
        {
            book.status = true;
            book.CoverImagePath = "~/Images/Covers/DefaultBookCover.jpg";
            using (BookstoreEntities dbContext = new BookstoreEntities())
            {
                if (this.Request.Files != null && this.Request.Files.Count > 0 && this.Request.Files[0].ContentLength > 0)
                {
                    string fileName = Path.GetFileName(this.Request.Files[0].FileName);
                    string filePathOfWebsite = "~/Images/Covers/" + fileName;
                    book.CoverImagePath = filePathOfWebsite;
                    this.Request.Files[0].SaveAs(this.Server.MapPath(filePathOfWebsite));
                }
                dbContext.Entry(book).State = System.Data.Entity.EntityState.Added;
                dbContext.SaveChanges();
            }
            return RedirectToAction("BookList", new { pageNumber = 1 });
        }


        [Authorize(Roles = "Admin")]
        public ActionResult DeleteBook(int id)
        {
            Book target = null;
            using (BookstoreEntities dbContext = new BookstoreEntities())
            {
                target = dbContext.Book.SingleOrDefault(t => t.Id == id);
                target.status = false;
                dbContext.Entry(target).State = System.Data.Entity.EntityState.Modified;
                dbContext.SaveChanges();
            }
            return RedirectToAction("BookList", new { pageNumber = 1 });
        }


        [Authorize(Roles = "Admin")]
        public ActionResult EditBook(int id)
        {
            Book target = null;
            using (BookstoreEntities dbContext = new BookstoreEntities())
            {
                target = dbContext.Book.SingleOrDefault(t => t.Id == id);
            }
            return View(target);
        }


        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult EditBook(Book book)
        {
            using (BookstoreEntities dbContext = new BookstoreEntities())
            {
                if (this.Request.Files != null && this.Request.Files.Count > 0 && this.Request.Files[0].ContentLength > 0)
                {
                    string fileName = Path.GetFileName(this.Request.Files[0].FileName);
                    string filePathOfWebsite = "~/Images/Covers/" + fileName;
                    book.CoverImagePath = filePathOfWebsite;
                    this.Request.Files[0].SaveAs(this.Server.MapPath(filePathOfWebsite));
                }
                dbContext.Entry(book).State = System.Data.Entity.EntityState.Modified;
                dbContext.SaveChanges();
            }
            return RedirectToAction("BookList", new { pageNumber = 1 });
        }
    }
}
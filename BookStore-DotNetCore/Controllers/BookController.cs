using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookStore_DotNetCore.Models;
using BookStore_DotNetCore.Models.Repositories;
using BookStore_DotNetCore.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
 

namespace BookStore_DotNetCore.Controllers
{
    public class BookController : Controller
    {
        private readonly IBookStoreRepository<Book> bookRepository;
        private readonly IBookStoreRepository<Author> authorRepository;

        public BookController(IBookStoreRepository<Book>bookRepository,IBookStoreRepository<Author> authorRepository)
        {
            this.bookRepository = bookRepository;
            this.authorRepository = authorRepository;
        }
        // GET: Book
        public ActionResult Index()
        {
            var books = bookRepository.List();
            return View(books);
        }

        // GET: Book/Details/5
        public ActionResult Details(int id)
        {
            var book = bookRepository.Find(id);
            return View(book);
        }

        // GET: Book/Create
        public ActionResult Create()
        {
            var model = new BookAuthorViewModel
            {
                Authors=FillSelectList()
            };
            return View(model);
        }

        // POST: Book/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(BookAuthorViewModel model)
        {
            try
            {
                if (model.AuthorId==-1)
                {
                    ViewBag.Message = "Please Select  an author from the list";

                    var vmodel = new BookAuthorViewModel { Authors = FillSelectList() };

                    return View(vmodel);
                }
                var author = authorRepository.Find(model.AuthorId);
                Book book = new Book { Id=model.BookId,Title=model.Title,Description=model.Description,Author=author };
                bookRepository.Add(book);
                return RedirectToAction(nameof(Index)); 
            }
            catch
            {
                return View();
            }
        }

        // GET: Book/Edit/5
        public ActionResult Edit(int id)
        {
            var book = bookRepository.Find(id);
            var authorId = book.Author == null ? book.Author.Id = 0 : book.Author.Id;

            var viewModel = new BookAuthorViewModel
            {
                BookId = book.Id,
                Title = book.Title,
                Description=book.Description,
                AuthorId= authorId,
                Authors=authorRepository.List().ToList()
            
            };
            return View(viewModel);
        }

        // POST: Book/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( BookAuthorViewModel viewModel)
        {
            try
            {
                // TODO: Add update logic here
                var author = authorRepository.Find(viewModel.AuthorId);
                Book book = new Book { Title = viewModel.Title, Description = viewModel.Description, Author = author };
                bookRepository.Update(viewModel.BookId,book);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Book/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Book/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        List<Author>FillSelectList()
        {
            var authors = authorRepository.List().ToList();
            authors.Insert(0, new Author { Id = -1, FullName = "----Please Select an Author----" });
            return authors;
        }
    }
}
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Drawing;
using WebApplication1.Data;
using WebApplication1.Models;
using WebApplication1.Repositories;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Text;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        private ILogger _logger;
        private IRepository _repository;
        public HomeController(IRepository repository, ILogger<HomeController> logger)
        {
            _repository = repository;
            _logger = logger;
        }
        public IActionResult Index()
        {
            _logger.LogInformation("User has entered Homepage Action");
            ViewBag.Rating = _repository.GetTwoMostRated()
                             .GroupJoin(
                                _repository.GetComments(),
                                animal => animal.AnimalID,
                                comment => comment.AnimalID,
                                (animal, comments) => new { Animal = animal, Comments = comments }
                            )
                            .Select(
                                x => new { Animal = x.Animal, CommentCount = x.Comments.Count() }
                            )
                            .ToList();

            return View(_repository.GetTwoMostRated());
        }

        [HttpGet]
        public IActionResult Catalog()
        {
            _logger.LogInformation("User has entered Catalog Action");
            ViewBag.Categories = _repository.GetCategories();
            ViewBag.Comments = _repository.GetComments();
            return View(_repository.GetAnimals());
        }

        [HttpGet]
        public IActionResult CatalogWithID(int categoryID)
        {
            _logger.LogInformation("User has entered CatalogWithID Action");
            ViewBag.Categories = _repository.GetCategories();
            ViewBag.Comments = _repository.GetComments();
            return View("Catalog", _repository.GetAnimals().Where(a => a.CategoryID == categoryID));
        }

        [HttpGet]
        public IActionResult ViewAnimal(string actionAnimalName)
        {
            _logger.LogInformation("User has entered ViewAnimal Action");
            var pet = _repository.GetAnimals().Where(o => o.AnimalName.Equals(actionAnimalName));
            ViewBag.Comments = _repository.GetComments().Where(o => o.AnimalID == pet.First().AnimalID);
            ViewBag.CommentsCount = _repository.GetComments().Where(o => o.AnimalID == pet.First().AnimalID).Count();
            return View(pet);
        }

        [HttpGet]
        public IActionResult LoginSignup()
        {
            _logger.LogInformation("User has entered LoginSignup Action");
            return View();
        }

        [Authorize(Roles = "Administrator")]
        [HttpGet]
        public IActionResult AdminPage()
        {
            _logger.LogInformation("User has entered AdminPage Action");
            ViewBag.Categories = _repository.GetCategories();
            return View();
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public IActionResult AddAnimalToDB(string animalName, int animalAge, string description, IFormFile image, string categoryID)
        {
            StringBuilder errorSb = new StringBuilder();
            _logger.LogInformation("User has entered AddAnimalToDB Action");
            if (ModelState.IsValid)
            {
                if(animalAge >= 0 && animalAge <=150)
                {
                    Animals newAnimal = new Animals
                    {
                        AnimalName = animalName,
                        AnimalAge = animalAge,
                        Description = description,
                        CategoryID = int.Parse(categoryID),
                        Category = _repository.GetCategories().Single(c => c.CategoryID == int.Parse(categoryID))
                    };

                    using (var ms = new MemoryStream())
                    {
                        image.CopyTo(ms);
                        var fileBytes = ms.ToArray();
                        newAnimal.Image = fileBytes;
                    }

                    _repository.InsertAnimal(newAnimal);
                    return RedirectToAction("AdminPage");
                }
                else
                {
                    errorSb.AppendLine("Animal age should be between 0-150");
                }
            }

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("You have a bunch of errors:");

            foreach (var modelState in ModelState.Values)
            {
                foreach (ModelError error in modelState.Errors)
                {
                    sb.AppendLine(error.ErrorMessage);
                }
            }
            if (int.Parse(categoryID) == 0)
                sb.AppendLine("The catagory field requires an input");

            if (errorSb.Length > 0)
                sb.Append(errorSb.ToString());

            TempData["Errors"] = sb.ToString();
            ViewBag.Categories = _repository.GetCategories();
            return View("~/Views/Home/AdminPage.cshtml");
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public IActionResult AddCategoryToDB(string CategoryName)
        {
            _logger.LogInformation("User has entered AddCategoryToDB Action");
            if (ModelState.IsValid)
            {
                Categories categories = new Categories
                {
                    CategoryName = CategoryName
                };
                _repository.InsertCategory(categories);
                return RedirectToAction("AdminPage");
            }

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("You have a bunch of errors:");

            foreach (var modelState in ModelState.Values)
            {
                foreach (ModelError error in modelState.Errors)
                {
                    sb.AppendLine(error.ErrorMessage);
                }
            }
            TempData["Errors"] = sb.ToString();
            ViewBag.Categories = _repository.GetCategories();
            return View("~/Views/Home/AdminPage.cshtml");
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public IActionResult UpdateAnimals(int animalId)
        {
            _logger.LogInformation("User has entered UpdateAnimals Action");
            Animals animal = _repository.GetAnimals().Single(a => a.AnimalID == animalId);
            return View(animal);
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public IActionResult EditAnimals(int updatedAnimalID, string animalName, int? animalAge, string animalDescription, IFormFile newImage)
        {
            _logger.LogInformation("User has entered EditAnimals Action");
            Animals updatedAnimal = _repository.GetAnimals().Single(a => a.AnimalID == updatedAnimalID);
            updatedAnimal.AnimalName = animalName.IsNullOrEmpty() ? updatedAnimal.AnimalName : animalName;
            updatedAnimal.AnimalAge = animalAge.HasValue ? animalAge.Value : updatedAnimal.AnimalAge;
            updatedAnimal.Description = animalDescription.IsNullOrEmpty() ? updatedAnimal.Description : animalDescription;

            if (newImage != null)
            { 
                using (var ms = new MemoryStream())
                {
                    newImage.CopyTo(ms);
                    var fileBytes = ms.ToArray();
                    updatedAnimal.Image = fileBytes;
                }
            }

            _repository.UpdateAnimal(updatedAnimalID, updatedAnimal);
            return RedirectToAction("Catalog");
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public IActionResult DeleteAnimal(int deleteAnimalId)
        {
            _logger.LogInformation("User has entered DeleteAnimal Action");
            _repository.DeleteAnimal(deleteAnimalId);
            return RedirectToAction("Catalog");
        }

        [HttpPost]
        public IActionResult AddComment(string commentText, string animalName)
        {
            _logger.LogInformation("User has entered AddComment Action");
            if (ModelState.IsValid)
            {
                int animalID = _repository.GetAnimals().Single(a => a.AnimalName == animalName).AnimalID;
                Comments newComment = new Comments
                {
                    AnimalID = animalID,
                    Comment = commentText,
                    UserNamePosted = User.FindFirstValue(ClaimTypes.Name)!
                };
                _repository.InsertComment(newComment);
                return RedirectToAction("ViewAnimal", new { actionAnimalName = animalName });
            }

            StringBuilder sb = new StringBuilder();
            sb.Append("You have a bunch of errors:");

            foreach (var modelState in ModelState.Values)
            {
                foreach (ModelError error in modelState.Errors)
                {
                    sb.AppendLine(error.ErrorMessage);
                }
            }
            TempData["Errors"] = sb.ToString();
            return RedirectToAction("ViewAnimal", new { actionAnimalName = animalName });
        }
    }
}

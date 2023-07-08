using Microsoft.AspNetCore.Http.HttpResults;
using NuGet.Protocol.Core.Types;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Repositories
{
    public class MyRepository : IRepository
    {
        private PetShopContext _context;

        public MyRepository(PetShopContext context)
        {
            _context = context;
        }

        public IEnumerable<Animals> GetAnimals()
        {
            return _context.Animal;
        }

        public IEnumerable<Categories> GetCategories()
        {
            return _context.Category;
        }

        public IEnumerable<Comments> GetComments()
        {
            return _context.Comment;
        }

        public IEnumerable<Animals> GetTwoMostRated()
        {
            return (_context.Animal
                            .Join(
                                _context.Comment,
                                animal => animal.AnimalID,
                                comment => comment.AnimalID,
                                (animal, comment) => new { Animal = animal, Comment = comment }
                            )
                            .GroupBy(
                                x => x.Animal,
                                x => x.Comment
                            )
                            .Select(
                                g => new { Animal = g.Key, CommentCount = g.Count() }
                            )
                            .OrderByDescending(
                                x => x.CommentCount
                            )
                            .Take(2)
                            .Select(
                                x => x.Animal
                            )
                            .ToList());
        }
        public void InsertAnimal(Animals animal)
        {
            _context.Animal.Add(animal);
            _context.SaveChanges();
        }
        public void InsertCategory(Categories category)
        {
            _context.Category.Add(category);
            _context.SaveChanges();
        }
        public void InsertComment(Comments comment)
        {
            _context.Comment.Add(comment);
            _context.SaveChanges();
        }

        public void UpdateAnimal(int id, Animals animal)
        {
            var animalToChange = _context.Animal.Single(a => a.AnimalID == id);
            if (animalToChange != null)
            {
                animalToChange.AnimalName = animal.AnimalName;
                animalToChange.AnimalAge = animal.AnimalAge;
                animalToChange.Image = animal.Image;
                animalToChange.Category = animal.Category;
                animalToChange.Description = animal.Description;
            }
            _context.SaveChanges();
        }

        public void UpdateComment(int id, Comments comment)
        {
            var commentToChange = _context.Comment.Single(a => a.CommentID == id);
            if (commentToChange != null)
            {
                commentToChange.Comment = comment.Comment;
            }
            _context.SaveChanges();
        }

        public void DeleteAnimal(int id)
        {
            var animal = _context.Animal.Single(a => a.AnimalID == id);
            foreach (var comment in _context.Comment)
            {
                if (comment.AnimalID == id)
                {
                    _context.Comment.Remove(comment);
                }
            }
            _context.Animal.Remove(animal);
            _context.SaveChanges();
        }

        //public void DeleteComment(int id)
        //{
        //    var comment = _context.Comment.Single(a => a.CommentID == id);
        //    _context.Comment.Remove(comment);

        //    _context.SaveChanges();
        //}
    }
}

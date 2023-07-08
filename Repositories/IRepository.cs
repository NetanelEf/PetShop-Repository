using WebApplication1.Models;

namespace WebApplication1.Repositories
{
    public interface IRepository
    {
        IEnumerable<Categories> GetCategories();
        IEnumerable<Animals> GetAnimals();
        IEnumerable<Comments> GetComments();
        IEnumerable<Animals> GetTwoMostRated();
        void InsertAnimal(Animals animal);
        void InsertComment(Comments comment);
        void InsertCategory(Categories category);
        void UpdateComment(int id, Comments comment);
        void UpdateAnimal(int id, Animals animal);
        //void DeleteComment(int id);
        void DeleteAnimal(int id);
    }
}

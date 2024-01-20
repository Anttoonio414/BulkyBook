using BulkyBook1.Models;

namespace BulkyBook.DataAccess.Repository.IRepository
{
    public interface IProductRepository : IRepository<Product>
    {

        void Update(Product obj);

    }
}

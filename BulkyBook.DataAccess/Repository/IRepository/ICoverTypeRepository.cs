using BulkyBook1.Models;

namespace BulkyBook.DataAccess.Repository.IRepository
{
    public interface ICoverTypeRepository : IRepository<CoverType>
    {

        void Update(CoverType obj);

    }
}

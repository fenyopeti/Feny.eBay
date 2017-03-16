using System.Collections.Generic;
using System.Linq;

namespace temalab.Services
{
    public interface IService<T>
    {

        T getOne(int? id);
        IEnumerable<T> getAll();
        void add(T record, string userName);
        void remove(int id);
        void Dispose();
    }
}
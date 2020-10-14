using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PersistenceLayer.Interfaces
{
    public interface ICrud<T> where T : class
    {
        Task Add(T Objeto);
        Task Update(T Objeto);
        Task Delete(T Objeto);
        Task<T> GetEntityById(long Id);
        Task<IEnumerable<T>> List();
    }
}

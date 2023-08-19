using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZadanieDomoweTydz3
{
    public interface ICRUD<T>
    {
        void Create(T item);
        T Read(string id);
        void Update(string id, T updatedItem);
        void Delete(string id);
        List<T> GetAll();
    }
}

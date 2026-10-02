using System;
using System.Collections.Generic;
using System.Text;

namespace BookTickets.Repository
{
    public interface ICRUD<T>
    {
        T Create(T entity);
        List<T> ReadAll();
        T ReadById(string id);
        T Update(T item, string id);
        T Delete(string id);
    }
}

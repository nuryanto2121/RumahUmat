using RumahUmat.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RumahUmat
{
    public interface IRepositoryController<T> where T : BaseEntity
    {
        OutPut Add(T item);
        OutPut Remove(int id);
        OutPut Update(T item);
        OutPut GetDataBy(int id);
        OutPut GetDataList();
    }
    public interface IRepository<T, K>
    {
        bool Save(T domain);
        bool Update(T domain);
        bool Delete(K key);
        List<T> GetList();
        List<T> GetList(int pageSize, int currentPage, string sortName, string sortOrder);
        T GetById(K key);

    }
}

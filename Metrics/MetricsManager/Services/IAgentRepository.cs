using FluentMigrator.Builders.Update;
using MetricsManager.Models;
using System.Data.SQLite;

namespace MetricsManager.Services
{
    public interface IAgentRepository<T> where T:class
    {
        public bool Add(T value);

        public T GetById(long id);

        public IList<T> GetAll();

        public bool Update(T value);

        public bool Delete(long id);

        public bool IsExist(long id);
    }
}

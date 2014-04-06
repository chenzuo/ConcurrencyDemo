using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace PetService.Repositories
{
    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class, new()
    {
        public abstract long Insert(T pet);

        public abstract void Update(T pet);

        public abstract T Get(long id);

        public abstract void Delete(long id);

        public RepositoryBase(IEntityRepository entities, IDbConnection connection = null)
        {
            this._entities = entities;
            this._connection = connection;
            this.IsValidConnection = (connection != null);
        }

        protected IEntityRepository _entities;
        protected IDbConnection _connection;
        protected bool IsValidConnection;
    }

    public interface IRepositoryBase<T> where T : class, new()
    {
        //T ToModel
    }
}
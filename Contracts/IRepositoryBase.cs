﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IRepositoryBase<T>
    {
        IQueryable<T> GetAll();
        IQueryable<T> GetByCondition(Expression<Func<T, bool>> expression);
        void Create(T entity);
        void Update(T entity);
        void Delete(T entity);

        //Task<IQueryable<T>> GetAllAsync();
        //Task<IQueryable<T>> GetByConditionAsync(Expression<Func<T, bool>> expression);
        //Task CreateAsync(T entity);
        //Task UpdateAsync(T entity);
        //Task DeletAsync(T entity);

    }
}

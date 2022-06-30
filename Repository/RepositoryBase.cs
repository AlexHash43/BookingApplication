using Contracts;
using Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        public AppointmentContext _appointmentContext { get; set; }
        public RepositoryBase(AppointmentContext appointmentContext)
        {
            _appointmentContext = appointmentContext;
        }

        public IQueryable<T> GetAll()
        {
            return _appointmentContext.Set<T>().AsNoTracking();
        }

        public IQueryable<T> GetByCondition(Expression<Func<T, bool>> expression)
        {
            return _appointmentContext.Set<T>().Where(expression).AsNoTracking();
        }

        public void Create(T entity)
        {
            _appointmentContext.Set<T>().Add(entity);
        }

        public void Update(T entity)
        {
            _appointmentContext.Set<T>().Update(entity);
        }

        public void Delete(T entity)
        {
            _appointmentContext.Set<T>().Remove(entity);
        }
    }
}

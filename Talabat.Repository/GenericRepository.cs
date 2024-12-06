﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Repositories;
using Talabat.Core.Specifications;
using Talabat.Repository.Data;

namespace Talabat.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly StoreContext _dbContext;

        public GenericRepository(StoreContext dbContext)
        {
            _dbContext = dbContext;
        }
        #region Without Specifications
        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            return await _dbContext.Set<T>().ToListAsync();
        }


        public async Task<T> GetByIdAsync(int id)
        {
            return await _dbContext.Set<T>().FindAsync(id);
            //return await _dbContext.Set<T>().Where(p => p.Id == id)
                                            //.Include(p => p.ProductBrand).Include(p => p.ProductType);
        }
        #endregion

        public async Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecifications<T> Spec)
        {
            return await ApplySpecification(Spec).ToListAsync();
        }
        public async Task<T> GetByIdWithSpecAsync(ISpecifications<T> Spec)
        {
            return await ApplySpecification(Spec).FirstOrDefaultAsync();
        }

        private IQueryable<T> ApplySpecification(ISpecifications<T> Spec)
        {
            return SpecificationEvaluator<T>.GetQuery(_dbContext.Set<T>(), Spec);
        }

        public async Task<int> GetCountWithSpecAsync(ISpecifications<T> Spec)
        {
            return await ApplySpecification(Spec).CountAsync();
        }
    }
}

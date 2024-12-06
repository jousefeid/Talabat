﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Specifications;

namespace Talabat.Core.BaseSpecifications
{
    public class BaseSpecifications<T> : ISpecifications<T> where T : BaseEntity
    {
        public Expression<Func<T, bool>> Criteria { get; set; }
        public List<Expression<Func<T, object>>> Includes { get; set; } = new List<Expression<Func<T, object>>>();
        public Expression<Func<T, object>> OrderBy { get; set; }
        public Expression<Func<T, object>> OrderByDescending { get; set; }
        public int Take { get; set; }
        public int Skip { get; set; }
        public bool IsPaginationEnabled { get; set; }

        //Get all

        public BaseSpecifications()
        {
            //Includes = new List<Expression<Func<T, object>>>();
        }

        //Get by id

        public BaseSpecifications(Expression<Func<T, bool>> criteriaExpression)
        {
            Criteria = criteriaExpression;
            //Includes = new List<Expression<Func<T, object>>>();
        }

        public void AddOrderBy(Expression<Func<T ,object>> OrderByExpression)
        {
            OrderBy = OrderByExpression;
        }
        public void AddOrderByDescending(Expression<Func<T ,object>> OrderByDescendingExpression)
        {
            OrderByDescending = OrderByDescendingExpression;
        }

        public void ApplyPagination(int skip ,int take)
        {
            IsPaginationEnabled = true;
            Skip = skip;
            Take = take;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Core.Specifications
{
    public interface ISpecifications<T> where T : BaseEntity
    {
        //_dbContext.Products.Where(p => p.Id == id).Include(p => p.ProductBrand).Include(p => p.ProductType)

        //Sign for property for where condition [where(p => p.Id == id)]
        public Expression<Func<T, bool>> Criteria { get; set; }

        //Sign for property for list of include [Include(p => p.ProductBrand).Include(p => p.ProductType)]
        public List<Expression<Func<T, object>>> Includes { get; set; }

        //prop for order by [orderby(P=>P.Name)]
        public Expression<Func<T,object>> OrderBy { get; set; }

        //prop for order by Descending [orderbyDescending(P=>P.Name)]
        public Expression<Func<T, object>> OrderByDescending { get; set; }

        //Take(20)
        public int Take { get; set; }

        //Skip(2)
        public int Skip { get; set; }

        public bool IsPaginationEnabled { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.BaseSpecifications;
using Talabat.Core.Entities;

namespace Talabat.Core.Specifications
{
    public class ProductWithBrandAndTypeSpecification : BaseSpecifications<Product>
    {
        //Ctor is used for getting all products
        public ProductWithBrandAndTypeSpecification(ProductSpecParams Params)
            :base(P =>
            (string.IsNullOrEmpty(Params.Search) || P.Name.ToLower().Contains(Params.Search))
            &&
            (!Params.BrandId.HasValue || P.ProductBrandId == Params.BrandId)//True
            &&
            (!Params.TypeId.HasValue || P.ProductTypeId == Params.TypeId)//True
            )

        {
            Includes.Add(p => p.ProductType);
            Includes.Add(p => p.ProductBrand);

            if (!string.IsNullOrEmpty(Params.Sort))
            {
                switch (Params.Sort)
                {
                    case "PriceAsc":
                        AddOrderBy(P => P.Price);
                        break;
                    case "PriceDesc":
                        AddOrderByDescending(P => P.Price);
                        break;
                    default:
                        AddOrderBy(P => P.Name);    
                        break;
                }
            }

            // products = 100
            // page size = 10
            // page index = 5

            //skip 40
            //take 10

            ApplyPagination(Params.PageSize * (Params.PageIndex - 1), Params.PageSize);
        }

        //Ctor is used for getting products by id
        public ProductWithBrandAndTypeSpecification(int id):base(p => p.Id == id)
        {
            Includes.Add(p => p.ProductType);
            Includes.Add(p => p.ProductBrand);
        }
    }
}

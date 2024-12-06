using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Talabat.Core.Entities;
using Talabat.Core.Repositories;
using Talabat.Core.Specifications;
using Talabat.Dtos;
using Talabat.Errors;
using Talabat.Helpers;

namespace Talabat.Controllers
{
    public class ProductsController : APIBaseController
    {
        private readonly IGenericRepository<Product> _productRepo;
        private readonly IMapper _mapper;
        private readonly IGenericRepository<ProductType> _productTypeRepo;
        private readonly IGenericRepository<ProductBrand> _productBrandRepo;

        public ProductsController(IGenericRepository<Product> ProductRepo 
                                 , IMapper mapper
                                 , IGenericRepository<ProductType> ProductTypeRepo
                                 , IGenericRepository<ProductBrand> ProductBrandRepo
                                                 )
        {
            _productRepo = ProductRepo;
            _mapper = mapper;
            _productTypeRepo = ProductTypeRepo;
            _productBrandRepo = ProductBrandRepo;
        }
        //Get all product 
        //BaseURL /api/products => Get
        [HttpGet]
        public async Task<ActionResult<Pagination<ProductToReturnDto>>> GetProducts([FromQuery]ProductSpecParams Params)
        {
            var Spec = new ProductWithBrandAndTypeSpecification(Params);
            var Products = await _productRepo.GetAllWithSpecAsync(Spec);
            var MappedProducts = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(Products);
            var CountSpec = new ProductWithFiltrationForCountAsync(Params);
            var Count = await _productRepo.GetCountWithSpecAsync(CountSpec);
            return Ok(new Pagination<ProductToReturnDto>(Params.PageIndex, Params.PageSize, MappedProducts, Count));
            //var ReturnedObject = new Pagination<ProductToReturnDto>()
            //{
            //    PageIndex = Params.PageIndex,
            //    PageSize = Params.PageSize,
            //    Data = MappedProducts
            //};
            //return OK(ReturnObject);
        }

        //Get product by id 
        //BaseURL /api/products/id => Get   
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ProductToReturnDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<ProductToReturnDto>>> GetProductById(int id)
        {
            var Spec = new ProductWithBrandAndTypeSpecification(id);
            var Product = await _productRepo.GetByIdWithSpecAsync(Spec);
            if (Product == null) return NotFound(new ApiResponse(404));
            var MappedProduct = _mapper.Map<Product, ProductToReturnDto>(Product);
            return Ok(MappedProduct);
        }

        //Get Types
        //BaseUrl/api/Products/Types
        [HttpGet("Types")]
        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetTypes()
        {
            var Types = await _productTypeRepo.GetAllAsync();
            return Ok(Types);
        }

        //Get Brands
        //BaseUrl/api/Products/Brands
        [HttpGet("Brands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetBrands()
        {
            var Brands = await _productBrandRepo.GetAllAsync();
            return Ok(Brands);
        }
    }
}

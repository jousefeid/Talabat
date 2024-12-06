using Microsoft.AspNetCore.Mvc;
using Talabat.Errors;
using Talabat.Repository.Data;

namespace Talabat.Controllers
{
    public class BuggyControlller : APIBaseController
    {
        private readonly StoreContext _dbContext;

        public BuggyControlller(StoreContext dbContext)
        {
            _dbContext = dbContext;
        }
        [HttpGet("NotFound")]
        public ActionResult GetNotFoundRequest()
        {
            var Product = _dbContext.Products.Find(100);
            if (Product is null)
                return NotFound(new ApiResponse(404));
            return Ok(Product);
        }
        [HttpGet("ServerError")]
        public ActionResult GetServerError()
        {
            var Product = _dbContext.Products.Find(100);
            var ProductToReturn = Product.ToString();
            //throw non reference exception
            return Ok(ProductToReturn);
        }
        [HttpGet("BadRequest")]
        public ActionResult GetBadRequest()
        {
            return BadRequest();
        }
        [HttpGet("BadRequest/{id}")]
        public ActionResult GetBadRequest(int id)
        {
            return Ok();
        }
    }
}

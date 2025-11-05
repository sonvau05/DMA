using Microsoft.AspNetCore.Mvc;
using ProductAPI.Models;
using ProductAPI.UnitOfWork;
using System.Threading.Tasks;

namespace ProductAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductCategoryController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductCategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddLink([FromBody] ProductCategory link)
        {
            await _unitOfWork.ProductCategories.AddAsync(link);
            await _unitOfWork.SaveAsync();
            return Ok("Link added");
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var links = await _unitOfWork.ProductCategories.GetAllAsync();
            return Ok(links);
        }
    }
}

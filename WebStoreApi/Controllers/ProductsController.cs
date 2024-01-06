using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebStoreApi.Models;
using WebStoreApi.Models.DTOS;
using WebStoreApi.Reposaitories.IReposaitories;

namespace WebStoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductReposaitory _products;
        private readonly IWebHostEnvironment _env;

        public ProductsController(IProductReposaitory products, IWebHostEnvironment env)
        {
            _products = products;
            _env = env;
        }

        [HttpGet("GetAllCategories")]
        public IActionResult GetAllCategories()
        {
            var Categories =  _products.GetAllCategories();

            return Ok(Categories);

        }


        [HttpGet(Name = "GetAllProducts")]
        public async Task<IActionResult> GetAll(string? search, string? category, int? MinPrice, int? MaxPrice, string? sort, string? order, int? page)
        {
            var products = await _products.GetAllProductsAsync(search,category,MinPrice,MaxPrice,sort,order,page);

            return Ok(products);

        }

        [HttpGet("{Id}", Name = "GetProduct")]
        public async Task<IActionResult> GetById(int Id)
        {
            if (Id == 0)
                return NotFound();
            var products = await _products.GetProductByIdAsync(Id);
            if (products == null)
                return NotFound();


            return Ok(products);

        }

        [HttpPost(Name = "AddProducts")]
        public async Task<IActionResult> AddContact([FromForm] ProductDto dto)
        {
            if(!_products.GetAllCategories().Contains(dto.Category))
            {
                ModelState.AddModelError("Category", "please select a valid category");
                return BadRequest(ModelState);
            }

            if(dto.Image == null)
            {
                ModelState.AddModelError("Image", "Product Image is required");
                return BadRequest(ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            string ImageFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            ImageFileName += Path.GetExtension(dto.Image.FileName);
            string imageFolder = _env.WebRootPath + "/Images/Products/";

            using ( var stream = System.IO.File.Create(imageFolder + ImageFileName))
            {
                dto.Image.CopyTo(stream);
            }
            var RecivedProduct = new Product()
            {
                Name = dto.Name,
                Image=ImageFileName,
                Brand = dto.Brand,
                Category = dto.Category,
                Description = dto.Description,
                Price = dto.Price,
            };

            var products = await _products.AddProductAsync(RecivedProduct);

            return Ok(products);

        }

        [HttpPut("{Id}", Name = "UpdateProducts")]
        public async Task<IActionResult> UpdateContact(int Id, [FromForm] ProductDto dto)
        {
            if (!_products.GetAllCategories().Contains(dto.Category))
            {
                ModelState.AddModelError("Category", "please select a valid category");
                return BadRequest(ModelState);
            }

            if (Id == 0)
                return BadRequest();

            
            if (await _products.GetProductByIdAsync(Id) == null)
            {
                ModelState.AddModelError("Id", "No Product with this Id");
                return BadRequest(ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var product = await _products.UpdateProductAsync(Id, dto);
            return Ok(product);

        }


        [HttpDelete("{Id}", Name = "deleteProduct")]
        public async Task<IActionResult> DeleteContact(int Id)
        {
            if (Id == 0)
                return BadRequest();
            
            if (await _products.DeleteProductAsync(Id) == false)
                {
                    ModelState.AddModelError("Id", "No Product with this Id");
                    return BadRequest(ModelState);
                }
            return Ok();
        }
    }
}

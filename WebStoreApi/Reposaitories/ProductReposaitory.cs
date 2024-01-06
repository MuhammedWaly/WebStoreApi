using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using WebStoreApi.Data;
using WebStoreApi.Models;
using WebStoreApi.Models.DTOS;
using WebStoreApi.Reposaitories.IReposaitories;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace WebStoreApi.Reposaitories
{
    public class ProductReposaitory : IProductReposaitory
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _env;
        private readonly List<string> Categories = new List<string>()
        {
            "Phones","Computers","Accessories","Printers","Cameras","Other"
        };

        public ProductReposaitory(ApplicationDbContext context, IMapper mapper, IWebHostEnvironment env)
        {
            _context = context;
            _mapper = mapper;
            _env = env;
        }

        
        public  List<string> GetAllCategories ()
        {
            return Categories;
        }


        public async Task<ProductsPaginationDto<Product>> GetAllProductsAsync(string? search, string? category,
            int? MinPrice, int? MaxPrice, string? sort, string? order, int? page)
        {
            IQueryable<Product> query =  _context.Products;

            if(search != null)
            {
                query = query.Where(x => x.Name.Contains(search)|| x.Description.Contains(search));
            }

            if (category != null)
            {
                query = query.Where(x => x.Category==category);
            }
            
            if (MinPrice != null)
            {
                query = query.Where(x => x.Price <= MinPrice);
            }
            
            if (MaxPrice != null)
            {
                query = query.Where(x => x.Price <= MaxPrice);
            }

            if (sort == null) sort = "Id";
            if (order == null|| order!="asc") order = "desc";
            
            if(sort.ToLower()== "name")
            {
                if(order == "asc")
                {
                    query = query.OrderBy(x => x.Name);
                }
                else
                {
                    query = query.OrderByDescending(x => x.Name);
                }
            }
            else if (sort.ToLower() == "brand")
            {
                if (order == "asc")
                {
                    query = query.OrderBy(x => x.Brand);
                }
                else
                {
                    query = query.OrderByDescending(x => x.Brand);
                }
            }

            else if (sort.ToLower() == "category")
            {
                if (order == "asc")
                {
                    query = query.OrderBy(x => x.Category);
                }
                else
                {
                    query = query.OrderByDescending(x => x.Category);
                }
            }

            else if (sort.ToLower() == "date")
            {
                if (order == "asc")
                {
                    query = query.OrderBy(x => x.CreatedAt);
                }
                else
                {
                    query = query.OrderByDescending(x => x.CreatedAt);
                }
            }
            else 
            {
                if (order == "asc")
                {
                    query = query.OrderBy(x => x.Id);
                }
                else
                {
                    query = query.OrderByDescending(x => x.Id);
                }
            }

            if (page == null || page < 1)
            {
                page = 1;
            }
            int pageSize = 5;
            int TotalPages = 0;
            decimal count = _context.Contacts.Count();
            TotalPages = (int)Math.Ceiling(count / pageSize);

            query = query
               .Skip((int)(page - 1) * pageSize)
               .Take(pageSize);

            var products = await query.ToListAsync();

            var response = new ProductsPaginationDto<Product>
            {
                Products = products,
                TotalPages = TotalPages,
                PageSize = pageSize,
                Page = page

            };
            return response;
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            var product = await _context.Products.FirstOrDefaultAsync(c => c.Id == id);
            if (product == null)
                throw new Exception("No product with this ID");
            
            return product;
        }


        public async Task<Product> AddProductAsync(Product Dto)
        {
            
            
            _context.Products.Add(Dto);
            await _context.SaveChangesAsync();
            return Dto;
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            var product = await _context.Products.FirstOrDefaultAsync(c => c.Id == id);
            if (product == null)
            {
                return false;
            }
            else
            {
                string imageFolder = _env.WebRootPath + "/Images/Products/";
                System.IO.File.Delete(imageFolder + product.Image);

                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
                return true;
            }
        }

      

        public async Task<Product> UpdateProductAsync(int id, ProductDto Dto)
        {
            var Product = await _context.Products.FindAsync(id);
            if (Product == null)
                throw new Exception("No Product with this Id");

            if(Dto.Image != null)
            {
                string ImageFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                ImageFileName += Path.GetExtension(Dto.Image.FileName);
                string imageFolder = _env.WebRootPath + "/Images/Products/";

                using (var stream = System.IO.File.Create(imageFolder + ImageFileName))
                {
                    Dto.Image.CopyTo(stream);
                }
                System.IO.File.Delete(imageFolder + Product.Image);

                Product.Image = ImageFileName;
            }

            Product.Name = Dto.Name ?? string.Empty;
            Product.Description = Dto.Description ?? string.Empty;
            Product.Category = Dto.Category ?? string.Empty;
            Product.Price = Dto.Price;
            Product.Brand = Dto.Brand;
           
            _context.Products.Update(Product);
            await _context.SaveChangesAsync();

            return Product;
        }
    }
}

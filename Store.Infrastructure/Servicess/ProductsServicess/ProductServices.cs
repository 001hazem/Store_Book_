using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Store.Core.Dtos.ProductDto;
using Store.Core.Exceptions;
using Store.Core.ViewModel;
using Store.Data.Models;
using Store.Infostructure;
using Store.Web.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace Store.Infrastructure.Servicess.ProductsServicess
{
    public class ProductServices : IProductServicess
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IFileService _fileService;


        public ProductServices( ApplicationDbContext context, IMapper mapper, IFileService fileService)
        {
            _context = context;
            _mapper = mapper;
            _fileService = fileService;

        }
        public async Task<List<ProductViewModel>> GetData()
        {
            var GetAll = await _context.Product.Include(x=>x.Category).Where(x => !x.IsDelete).ToListAsync();
            var products = _mapper.Map<List<ProductViewModel>>(GetAll);
            return products;

        }
        public async Task<PaginationViewModel> GetDataIndex(string searchKey, int page)
        {
            var pageSize = 10.0;
            var query = _context.Product               
                .Include(x => x.Category)
                .Where(x => !x.IsDelete && (x.Title.Contains(searchKey) || string.IsNullOrWhiteSpace(searchKey)))
                .OrderByDescending(x => x.CreatedAt).AsQueryable();

            var count = await query.CountAsync();
            var numberOfPages = Math.Ceiling(count / pageSize);

            if (page <= 1 || page > numberOfPages)
            {
                page = 1;
            }

            var skipValue = (int)((page - 1) * pageSize);

            var getAll = await query
                .Skip(skipValue)
                .Take((int)pageSize)
                .ToListAsync(); // Perform client-side evaluation

            var posts = _mapper.Map<List<ProductViewModel>>(getAll);
            var paginationViewModel = new PaginationViewModel();
            paginationViewModel.NumberOfPage = (int)numberOfPages;
            paginationViewModel.CurrentPage = page;
            paginationViewModel.data = posts;
            return paginationViewModel;
        }    
        
        public async Task<int> Create(CreateProductDto Input)
        {
            var CreateProduct = _mapper.Map<Product>(Input);
            if (Input.ImagesUrl != null)
            {
                CreateProduct.ImagesUrl = await _fileService.SaveFile(Input.ImagesUrl, "Images");
            }
            await _context.Product.AddAsync(CreateProduct);
            await _context.SaveChangesAsync();
            return CreateProduct.Id;
        }
       
        public async Task<int> Update(UpdateProductDto dto)
        {
            var product = await _context.Product.SingleOrDefaultAsync(x => x.Id == dto.Id && !x.IsDelete);
            if (product == null)
            {
                throw new EntityNotFoundException();
            }
            var productId = await _context.Product.FindAsync(dto.Id);
            var updatedPost = _mapper.Map(dto, product);
            updatedPost.UpdatedAt = DateTime.Now;
            if (dto.ImagesUrl != null)
            {
                product.ImagesUrl = await _fileService.SaveFile(dto.ImagesUrl,"Images");
            }
            _context.Product.Update(updatedPost);
            await _context.SaveChangesAsync();
            return product.Id;
        }

        public async Task<UpdateProductDto> Get(int id)
        {
            var GetItem = await _context.Product.SingleOrDefaultAsync(x => x.Id == id && !x.IsDelete); if (GetItem == null)
            {
                throw new IsTheUserNotExsit();
            }
            return _mapper.Map<UpdateProductDto>(GetItem);
        }

        public async Task<ProductDetailsViewModel> Detail(int productId)
        {
            var GetAll = await _context.Product.Include(x => x.Category).SingleOrDefaultAsync(x => x.Id == productId && !x.IsDelete);
            
            if (GetAll == null)
            {
                throw new EntityNotFoundException();

            }
            var products = _mapper.Map<ProductDetailsViewModel > (GetAll);
            return products;


        }

        public async Task<int> Delete(int id)
        {
            var deletePost = await _context.Product.SingleOrDefaultAsync(x => x.Id == id && !x.IsDelete);

            if (deletePost == null)
            {
                throw new EntityNotFoundException();

            }

            deletePost.IsDelete = true;
            _context.Product.Update(deletePost);
            await _context.SaveChangesAsync();
            return deletePost.Id;

        }



        //public async Task<int> UpdateStatus(int id, ContentStatus status)
        //{
        //    var updateStatus = await _context.Posts.Include(x => x.Author).SingleOrDefaultAsync(x => x.Id == id && !x.IsDelete);

        //    if (updateStatus == null)
        //    {
        //        throw new EntityNotFoundException();

        //    }
        //    var changeLog = new ContentChangeLog();
        //    changeLog.ContentId = updateStatus.Id;
        //    changeLog.Old = updateStatus.Status;
        //    changeLog.Type = ContentType.Post;
        //    changeLog.New = status;
        //    changeLog.ChangeAt = DateTime.Now;

        //    await _context.ContentChangeLogs.AddAsync(changeLog);
        //    await _context.SaveChangesAsync();


        //    updateStatus.Status = status;
        //    _context.Posts.Update(updateStatus);
        //    await _context.SaveChangesAsync();

        //    await _sendGrid.Execute("Hi !", updateStatus.Author.Email, "Your news status has been updated by the admin ", $"The Status {status}");
        //    return updateStatus.Id;

        //}

        //public async Task<List<ContentChangeLogViewModel>> ListChangeLog(int changeLog)
        //{

        //    var ListChangLog = await _context.ContentChangeLogs.Where(x => x.ContentId == changeLog && x.Type == ContentType.Post).ToListAsync();

        //    return _mapper.Map<List<ContentChangeLogViewModel>>(ListChangLog);

        //}

    }
}


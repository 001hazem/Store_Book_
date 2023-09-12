using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Store.Core.ViewModel;
using Store.Web.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Infrastructure.Servicess.OrderHeaderServicess
{
	public class OrderHeaderServiecs : IOrderHeaderServiecs
	{
		private readonly ApplicationDbContext _context;
		private readonly IMapper _mapper;

		public OrderHeaderServiecs(ApplicationDbContext context, IMapper mapper)
		{
			_context = context;
			_mapper = mapper;
		}

		public async Task<List<OrderHeaderViewModel>> GetData()
		{
			var GetAll = await _context.OrderHeader.Include(x=>x.user).Where(x => !x.IsDelete).ToListAsync();
			var categories = _mapper.Map<List<OrderHeaderViewModel>>(GetAll);
			return categories;

		}

	}
}

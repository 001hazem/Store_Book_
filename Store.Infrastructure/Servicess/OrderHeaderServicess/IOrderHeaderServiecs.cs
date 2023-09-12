using Store.Core.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Infrastructure.Servicess.OrderHeaderServicess
{
	public interface IOrderHeaderServiecs
	{
		Task<List<OrderHeaderViewModel>> GetData();
	}
}

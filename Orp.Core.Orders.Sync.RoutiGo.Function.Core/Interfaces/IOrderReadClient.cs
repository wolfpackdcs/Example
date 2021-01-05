using Orp.Core.Orders.DTO.Orders.V2;
using System.Threading.Tasks;

namespace Orp.Core.Orders.Sync.RoutiGo.Function.Core.Interfaces
{
    public interface IOrderReadClient
    {
        Task<OrderDTO> GetOrderAsync(string orderNumber);
    }
}

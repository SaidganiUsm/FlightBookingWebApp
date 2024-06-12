using FlightBookingApp.Core.Persistence.Paging;

namespace FlightBookingApp.Application.Common.DTOs
{
    public class GetListResponseDto<T> : BasePageableModel
    {
        public IList<T> Items
        {
            get => _items ??= new List<T>();
            set => _items = value;
        }

        private IList<T>? _items;
    }
}

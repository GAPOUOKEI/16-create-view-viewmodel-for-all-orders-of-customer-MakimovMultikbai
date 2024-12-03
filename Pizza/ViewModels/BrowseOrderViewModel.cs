using Pizza.Models;
using Pizza.Services;
using System.Collections.ObjectModel;

namespace Pizza.ViewModels
{
    internal class BrowseOrderViewModel : BindableBase
    {
        private long _id;
        public long Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }

        private IOrderRepository _repository;
        private Order _order;
        public Order Order 
        { 
            get => _order; 
            set => SetProperty(ref _order, value);
        }
        private ObservableCollection<OrderItem> _orderItems;
        public ObservableCollection<OrderItem> OrderItems 
        { 
            get => _orderItems; 
            set => SetProperty(ref _orderItems, value);
        }
        public BrowseOrderViewModel(IOrderRepository repository)
        {
            _repository = repository;
        }
        private List<OrderItem> _itemsList;
        public async void LoadData(long Id)
        {
            _order = await _repository.GetOrderById(Id);
            _itemsList = await _repository.GetOrderItemsByOrderId(Id);
            OrderItems = new ObservableCollection<OrderItem>(_itemsList);
        }
    }
}

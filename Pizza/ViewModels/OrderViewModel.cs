using Pizza.Models;
using Pizza.Services;
using System.Collections.ObjectModel;

namespace Pizza.ViewModels
{
    class OrderViewModel : BindableBase
    {
        private Guid _id;
        public Guid Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }

        private IOrderRepository _orderRepository;
        public OrderViewModel(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
            _orders = new ObservableCollection<Order>();
            BrowsingOrderCommand = new RelayCommand<Order>(OnBrowsingOrder);

        }
        private ObservableCollection<Order>? _orders;
        public ObservableCollection<Order>? Orders
        {
            get => _orders;
            set => SetProperty(ref _orders, value);
        }

        private List<Order>? _orderList;
        public async void LoadData(Guid Id)
        {
            this.Id = Id;

            _orderList = await _orderRepository.GetOrdersByCustomerAsync(Id);
            Orders = new ObservableCollection<Order>(_orderList);
        }
        public RelayCommand<Order> BrowsingOrderCommand { get; private set; }

        public event Action<Order> BrowsingOrderRequested = delegate { };

        private void OnBrowsingOrder(Order order) 
        {
            BrowsingOrderRequested(order);
        }
    }
}

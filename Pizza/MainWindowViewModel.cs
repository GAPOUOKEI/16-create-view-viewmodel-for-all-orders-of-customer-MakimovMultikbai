﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Pizza.Models;
using Pizza.Services;
using Pizza.ViewModels;
using Unity;

namespace Pizza
{
    class MainWindowViewModel : BindableBase
    {
        private AddEditCustomerViewModel    _addEditCustomerVewModel;
        private CustomerListViewModel       _customerListViewModel;
        private OrderPerpViewModel          _orderPrepViewModel;
        private OrderViewModel              _orderViewModel;
        private BrowseOrderViewModel        _browseOrderViewModel;

        private ICustomerRepository _customerRepository = new CustomerRepository();

        public MainWindowViewModel()
        {
            NavigationCommand = new RelayCommand<string>(OnNavigation);

            _customerListViewModel = RepoContainer.Container.Resolve<CustomerListViewModel>();  
            _addEditCustomerVewModel = RepoContainer.Container.Resolve<AddEditCustomerViewModel>();
            _orderPrepViewModel = RepoContainer.Container.Resolve<OrderPerpViewModel>();
            _orderViewModel = RepoContainer.Container.Resolve<OrderViewModel>();
            _browseOrderViewModel = RepoContainer.Container. Resolve<BrowseOrderViewModel>();

            _customerListViewModel.AddCustomerRequested +=NavigationToAddCustomer;
            _customerListViewModel.EditCustomerRequested += NavigationToEditCustomer;

            _customerListViewModel.PlaceOrderRequested += NavigateToOrder;
            _customerListViewModel.CustomerOrdersRequested += NavigateToCustomerOrders;
            _orderViewModel.BrowsingOrderRequested += NavigateToBrowseOrder;

            _addEditCustomerVewModel.Done += () => CurrentViewModel = _customerListViewModel;
            _orderPrepViewModel.Done += () => CurrentViewModel = _customerListViewModel;
        }
        private BindableBase _currentViewModel;
        public BindableBase CurrentViewModel
        {
            get => _currentViewModel;
            set => SetProperty(ref _currentViewModel, value);
        }

       public RelayCommand<string> NavigationCommand { get; private set; }

        //открывать список клиентов
        private void OnNavigation(string dest)
        {
            switch (dest)
            {
                case "orderPrep":
                    CurrentViewModel = _orderPrepViewModel; break;
                case "customers":
                default:
                       CurrentViewModel = _customerListViewModel; break;
            }
        }
        
        //открывать окно для редактирования клиента
        private void NavigationToEditCustomer(Customer customer)
        {
            _addEditCustomerVewModel.IsEditeMode = true; 
            _addEditCustomerVewModel.SetCustomer(customer);
            CurrentViewModel = _addEditCustomerVewModel;

        }

        //открывать окно для добавления клиента
        //private void NavigationToAddCustomer(Customer cust)----------------------------
        private void NavigationToAddCustomer()
        {
            _addEditCustomerVewModel.IsEditeMode = false;
            _addEditCustomerVewModel.SetCustomer(new Customer
            {
                Id = Guid.NewGuid(),
            });
            CurrentViewModel = _addEditCustomerVewModel;
            
        }

        //окно для оформления заказа
        private async void NavigateToOrder(Customer customer)
        {
            _orderPrepViewModel.Id = customer.Id;
            CurrentViewModel = _orderPrepViewModel;
        }

        private async void NavigateToCustomerOrders(Customer customer)
        {
            _orderViewModel.LoadData(customer.Id);
            CurrentViewModel = _orderViewModel;
        }
        private async void NavigateToBrowseOrder(Order order)
        {
            _browseOrderViewModel.LoadData(order.Id); 
            CurrentViewModel = _browseOrderViewModel;
        }
    }
}

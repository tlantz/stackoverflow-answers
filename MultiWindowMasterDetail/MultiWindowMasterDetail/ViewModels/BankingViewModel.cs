using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace MultiWindowMasterDetail.ViewModels
{
    public sealed class BankingViewModel : INotifyPropertyChanged
    {
        private readonly Dictionary<long, BankingInfo> _bankingDatabase = new Dictionary<long, BankingInfo>();
        private readonly ObservableCollection<Customer> _customers = new ObservableCollection<Customer>();

        private BankingInfo _selectedBankingInfo;
        private Customer _selectedCustomer;

        public BankingViewModel()
        {
            this.AddCustomer("Joyce", 3.50);
            this.AddCustomer("Tim", -100.0);
            this.AddCustomer("Momo", 1000000.0);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public ReadOnlyObservableCollection<Customer> Customers { get { return new ReadOnlyObservableCollection<Customer>(this._customers); } }

        public BankingInfo SelectedBankingInfo { get { return this._selectedBankingInfo; } }

        public Customer SelectedCustomer
        {
            get { return this._selectedCustomer; }
            set
            {
                this._selectedCustomer = value;
                this.OnSelectedCustomerChanged(value);
            }
        }

        private void AddCustomer(string name, double cash)
        {
            var customer = new Customer(this._customers.Count + 1, name);
            var banking = new BankingInfo(customer, cash);
            this._customers.Add(customer);
            this._bankingDatabase.Add(customer.Id, banking);
        }

        private BankingInfo LookupBankingInfo(Customer customer)
        {
            // could be looking up from SQL-server, for example
            // but here - just as a toy app, we generated the banking data
            return this._bankingDatabase[customer.Id];
        }

        private void OnPropertyChanged(string name)
        {
            if (null != this.PropertyChanged)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        private void OnSelectedCustomerChanged(Customer newCustomer)
        {
            this.OnPropertyChanged("SelectedCustomer");
            if (null == newCustomer)
            {
                this._selectedBankingInfo = null;
            }
            else
            {
                this._selectedBankingInfo = this.LookupBankingInfo(newCustomer);
            }
            this.OnPropertyChanged("SelectedBankingInfo");
        }

        public sealed class BankingInfo : INotifyPropertyChanged
        {
            private readonly double _cash;
            private readonly long _id;

            public BankingInfo(Customer customer, double cash)
            {
                this._id = customer.Id;
                this._cash = cash;
            }

            public event PropertyChangedEventHandler PropertyChanged;

            public double Cash { get { return this._cash; } }

            public long Id { get { return this._id; } }

            private void OnPropertyChanged(string name)
            {
                if (null != this.PropertyChanged)
                {
                    this.PropertyChanged(this, new PropertyChangedEventArgs(name));
                }
            }
        }

        public sealed class Customer : INotifyPropertyChanged
        {
            private readonly long _id;
            private readonly string _name;

            public Customer(long id, string name)
            {
                this._id = id;
                this._name = name;
            }

            public event PropertyChangedEventHandler PropertyChanged;

            public long Id { get { return this._id; } }

            public string Name { get { return this._name; } }

            private void OnPropertyChanged(string name)
            {
                if (null != this.PropertyChanged)
                {
                    this.PropertyChanged(this, new PropertyChangedEventArgs(name));
                }
            }
        }
    }
}

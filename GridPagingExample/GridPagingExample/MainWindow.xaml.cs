using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;

namespace GridPagingExample
{
    public sealed partial class MainWindow 
    {
        private readonly PagingCollectionView _cview;

        public MainWindow()
        {
            InitializeComponent();
            this._cview = new PagingCollectionView(
                new List<object>
                {
                    new { Animal = "Lion", Eats = "Tiger" },
                    new { Animal = "Tiger", Eats =  "Bear" },
                    new { Animal = "Bear", Eats = "Oh my" },
                    new { Animal = "Wait", Eats = "Oh my isn't an animal" },
                    new { Animal = "Oh well", Eats = "Who is counting anyway" },
                    new { Animal = "Need better content", Eats = "For posting on stackoverflow" },
                    new { Animal = "Turtle", Eats = "Pizza" }
                },
                2
            );
            this.DataContext = this._cview;
        }

        private void OnNextClicked(object sender, RoutedEventArgs e)
        {
            this._cview.MoveToNextPage();
        }

        private void OnPreviousClicked(object sender, RoutedEventArgs e)
        {
            this._cview.MoveToPreviousPage();
        }
    }

    public sealed class PagingCollectionView : CollectionView
    {
        private readonly IList _innerList;
        private readonly int _itemsPerPage;

        private int _currentPage = 1;

        public PagingCollectionView(IList innerList, int itemsPerPage)
            : base(innerList)
        {
            this._innerList = innerList;
            this._itemsPerPage = itemsPerPage;
        }

        public override int Count
        {
            get 
            {
                // originally we were always returning the itemsPerPage as pointed out
                // here: http://stackoverflow.com/questions/25300154/wpf-datagrid-pagination
                // this is wrong on the last page
                // of course, using Microsoft's implementation is recommended! this was
                // meant to be an example on how to get started with paging data in WPF
                // http://msdn.microsoft.com/en-us/library/system.windows.data.pagedcollectionview(v=vs.95).aspx
                // credit to http://stackoverflow.com/users/1398211/novitchi-s for 
                // pointing out the bug
                if (this._currentPage < this.PageCount) // page 1..n-1
                {
                    return this._itemsPerPage;
                }
                else // page n
                {
                    var itemsLeft = this._innerList.Count % this._itemsPerPage;
                    if (0 == itemsLeft)
                    {
                        return this._itemsPerPage; // exactly itemsPerPage left
                    }
                    else
                    {
                        // return the remaining items
                        return itemsLeft;
                    }
                }
            }
        }

        public int CurrentPage
        {
            get { return this._currentPage; }
            set
            {
                this._currentPage = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs("CurrentPage"));
            }
        }

        public int ItemsPerPage { get { return this._itemsPerPage; } }

        public int PageCount
        {
            get 
            { 
                return (this._innerList.Count + this._itemsPerPage - 1) 
                    / this._itemsPerPage; 
            }
        }

        private int EndIndex
        {
            get
            {
                var end = this._currentPage * this._itemsPerPage - 1;
                return (end > this._innerList.Count) ? this._innerList.Count : end;
            }
        }

        private int StartIndex
        {
            get { return (this._currentPage - 1) * this._itemsPerPage; }
        }

        public override object GetItemAt(int index)
        {
            var offset = index % (this._itemsPerPage); // maybe not be the logic you want
            return this._innerList[this.StartIndex + offset];
        }

        public void MoveToNextPage()
        {
            if (this._currentPage < this.PageCount)
            {
                this.CurrentPage += 1;
            }
            this.Refresh();
        }

        public void MoveToPreviousPage()
        {
            if (this._currentPage > 1)
            {
                this.CurrentPage -= 1;
            }
            this.Refresh();
        }
    }
}

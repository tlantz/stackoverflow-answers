using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using Visifire.Charts;

namespace VisifireDatabinding.ViewModels
{
    public sealed class ExampleViewModel : INotifyPropertyChanged
    {
        private readonly ObservableCollection<My3DLineChartObject>
            _markerChartsCollections = new ObservableCollection<My3DLineChartObject>();
        private readonly Random _rand = new Random();

        public ExampleViewModel()
        {
            foreach (var i in Enumerable.Range(0, 10))
            {
                this._markerChartsCollections.Add(new My3DLineChartObject(this._rand));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged { add { } remove { } }

        public IList MarkerChartsCollections 
        { 
            get { return this._markerChartsCollections; } 
        }

        public sealed class My3DLineChartObject
        {
            private readonly DataPointCollection _xAxisCollection = new DataPointCollection();
            private readonly DataPointCollection _yAxisCollection = new DataPointCollection();
            private readonly DataPointCollection _zAxisCollection = new DataPointCollection();

            public My3DLineChartObject(Random rand)
            {
                this.PopulatePoints(this._xAxisCollection, rand);
                this.PopulatePoints(this._yAxisCollection, rand);
                this.PopulatePoints(this._zAxisCollection, rand);
            }

            public event PropertyChangedEventHandler PropertyChanged { add { } remove { } }

            public DataPointCollection XAxisCollection { get { return this._xAxisCollection; } }

            public DataPointCollection YAxisCollection { get { return this._yAxisCollection; } }

            public DataPointCollection ZAxisCollection { get { return this._zAxisCollection; } }

            private static LightDataPoint GetRandomPoint(double x, Random rand)
            {
                var r = rand.NextDouble();
                return new LightDataPoint
                {
                    XValue = x,
                    YValue = r - 0.5,
                    ZValue = r - 0.5
                };
            }

            private void PopulatePoints(DataPointCollection points, Random rand)
            {
                foreach (var i in Enumerable.Range(0, 100))
                {
                    points.Add(GetRandomPoint(i, rand));
                }
            }
        }
    }
}

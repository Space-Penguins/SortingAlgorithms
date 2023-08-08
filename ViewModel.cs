using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WpfSample
{
    public class ViewModel : INotifyPropertyChanged
    {
        private ISeries[] _series = Array.Empty<ISeries>();

        public ISeries[] Series
        {
            get => _series;
            set
            {
                if (_series != value)
                {
                    _series = value;
                    OnPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void UpdateSeries(int[] newValues)
        {
            Series = new ISeries[]
            {
                new ColumnSeries<int>
                {
                    Values = newValues
                }
            };
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DetectionLangue.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private BaseViewModel _viewModelActuel;
        private DetectionLangueViewModel _detectionLangueViewModel;

        public MainViewModel()
        {
            _detectionLangueViewModel = new DetectionLangueViewModel();
            _viewModelActuel = _detectionLangueViewModel;
        }

        public BaseViewModel ViewModelActuel
        {
            get { return _viewModelActuel; }
            set
            {
                _viewModelActuel = value;
                OnPropertyChanged();
            }
        }
    }
}

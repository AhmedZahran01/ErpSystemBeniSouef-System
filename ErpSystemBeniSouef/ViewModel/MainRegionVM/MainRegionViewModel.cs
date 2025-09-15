using AutoMapper;
using ErpSystemBeniSouef.Core.Contract;
using ErpSystemBeniSouef.Core.DTOs.MainAreaDtos;
using ErpSystemBeniSouef.Dtos.MainAreaDto;
using ErpSystemBeniSouef.ViewModel.Commands.MainRegionCommands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ErpSystemBeniSouef.ViewModel.MainRegionVM
{
    public class MainRegionViewModel : INotifyPropertyChanged
    {
        private readonly IMainAreaService _mainAreaService;
        private readonly IMapper _mapper;

        public ObservableCollection<MainAreaDto> MainRegions { get; set; } = new();
        public string RegionName { get; set; }
        public string StartNumber { get; set; }
        public string SearchText { get; set; }
        private MainAreaDto _selectedRegion;
        public MainAreaDto SelectedRegion
        {
            get => _selectedRegion;
            set { _selectedRegion = value; OnPropertyChanged(); }
        }

        public ICommand LoadCommand { get; }
        public ICommand AddCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand ResetCommand { get; }
        public ICommand SearchCommand { get; }

        public MainRegionViewModel(IMainAreaService mainAreaService, IMapper mapper)
        {
            _mainAreaService = mainAreaService;
            _mapper = mapper;

            LoadCommand = new RelayCommand(async _ => await LoadRegions());
            AddCommand = new RelayCommand(async _ => await AddRegion());
            DeleteCommand = new RelayCommand(async _ => await DeleteRegion());
            ResetCommand = new RelayCommand(_ => Reset());
            SearchCommand = new RelayCommand(_ => Search());

            Task.Run(() => LoadRegions());
        }

        private async Task LoadRegions()
        {
            var regions = await _mainAreaService.GetAllAsync();
            MainRegions.Clear();
            foreach (var r in regions)
                MainRegions.Add(r);
        }

        private async Task AddRegion()
        {
            if (string.IsNullOrWhiteSpace(RegionName) || !int.TryParse(StartNumber, out int startNum))
                return;

            var dto = new CreateMainAreaDto { Name = RegionName.Trim(), StartNumbering = startNum };
            var success = _mainAreaService.Create(dto);
            if (success == 1)
            {
                await LoadRegions();
                RegionName = string.Empty;
                StartNumber = string.Empty;
                OnPropertyChanged(nameof(RegionName));
                OnPropertyChanged(nameof(StartNumber));
            }
        }

        private async Task DeleteRegion()
        {
            if (SelectedRegion == null) return;

            bool success = _mainAreaService.SoftDelete(SelectedRegion.Id);
            if (success)
                MainRegions.Remove(SelectedRegion);
        }

        private void Reset()
        {
            SearchText = string.Empty;
            OnPropertyChanged(nameof(SearchText));
            Task.Run(() => LoadRegions());
        }

        private void Search()
        {
            if (string.IsNullOrEmpty(SearchText))
                return;

            var filtered = MainRegions.Where(r => r.Name != null && r.Name.ToLower().Contains(SearchText.ToLower())).ToList();

            MainRegions.Clear();
            foreach (var item in filtered)
                MainRegions.Add(item);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }


}

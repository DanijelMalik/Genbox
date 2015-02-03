using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using Generator4Developers.Presentation.Windows.Common;
using Generator4Developers.Presentation.Windows.Models;
using Generator4Developers.Presentation.Windows.Services;

namespace Generator4Developers.Presentation.Windows.ViewModels
{
    public sealed class MainViewModel : ViewModel
    {
        private IGeneratorService _selectedService;
        private string _result;
        private ICommand _generateAndCopyCommand;
        private ICommand _generateCommand;
        private ICommand _copyCommand;
        private ObservableCollection<HistoryRecord> _history;
        private HistoryRecord _selectedHistoryRecord;
        private bool _stayOnTop;
        private ObservableCollection<IGeneratorService> _services;
        private bool _isShowSettings;
        private ICommand _toggleSettingCommand;
        private ICommand _clearHistoryCommand;
        private ICommand _copyAllCommand;
        private string _errorMessage;
        private bool _isError;

        public MainViewModel()
        {
            IsInitializing = true;
            Services = new ObservableCollection<IGeneratorService>();
            RegisterServices();
            InitializeDefaults();
            IsInitializing = false;
        }

        public ObservableCollection<IGeneratorService> Services
        {
            get { return _services; }
            set
            {
                _services = value;
                OnPropertyChanged();
            }
        }

        private void RegisterServices()
        {
            var services = Assembly.GetExecutingAssembly().GetTypes().Where(x => x.CustomAttributes.Any(a => a.AttributeType == typeof(GeneratorServiceAttribute)));

            foreach (var generatorService in services.Select(Activator.CreateInstance).OfType<IGeneratorService>())
            {
                Services.Add(generatorService);
                generatorService.ModeChanged += (sender, args) =>
                {
                    if (IsInitializing)
                    {
                        return;
                    }

                    var formattedText = args.FormattedText;

                    if (!String.IsNullOrEmpty(formattedText) && History != null && History.Any())
                    {
                        Result = formattedText;
                        History.First().Text = formattedText;
                    }
                };
            }
        }

        private bool IsInitializing { get; set; }

        private void InitializeDefaults()
        {
            SelectedService = Services.FirstOrDefault();
            History = new ObservableCollection<HistoryRecord>();
        }

        public IGeneratorService SelectedService
        {
            get { return _selectedService; }
            set
            {
                _selectedService = value;
                OnPropertyChanged();

                if (_selectedService != null)
                {
                    _selectedService.Activate();
                }
            }
        }

        public string Result
        {
            get { return _result; }
            set
            {
                _result = value;
                OnPropertyChanged();
            }
        }

        public ICommand GenerateAndCopyCommand
        {
            get { return _generateAndCopyCommand ?? (_generateAndCopyCommand = new RelayCommand(GenerateAndCopy)); }
        }

        public ICommand GenerateCommand
        {
            get { return _generateCommand ?? (_generateCommand = new RelayCommand(Generate)); }
        }

        public ICommand CopyCommand
        {
            get { return _copyCommand ?? (_copyCommand = new RelayCommand(Copy)); }
        }

        public ObservableCollection<HistoryRecord> History
        {
            get { return _history; }
            private set
            {
                _history = value;
                OnPropertyChanged();
            }
        }

        public HistoryRecord SelectedHistoryRecord
        {
            get { return _selectedHistoryRecord; }
            set
            {
                _selectedHistoryRecord = value;
                OnPropertyChanged();
            }
        }

        public bool StayOnTop
        {
            get { return _stayOnTop; }
            set
            {
                _stayOnTop = value;
                OnPropertyChanged();
            }
        }

        public bool IsShowSettings
        {
            get { return _isShowSettings; }
            set
            {
                _isShowSettings = value;
                OnPropertyChanged();
            }
        }

        public ICommand ToggleSettingCommand
        {
            get { return _toggleSettingCommand ?? (_toggleSettingCommand = new RelayCommand(ToggleSettings)); }
        }

        public ICommand ClearHistoryCommand
        {
            get { return _clearHistoryCommand ?? (_clearHistoryCommand = new RelayCommand(ClearHistory)); }
        }

        public ICommand CopyAllCommand
        {
            get { return _copyAllCommand ?? (_copyAllCommand = new RelayCommand(CopyAll)); }
        }

        private void CopyAll()
        {
            if (!History.Any())
            {
                return;
            }

            var text = String.Join(Environment.NewLine, History.Select(x => x.Text));
            Clipboard.SetText(text);
        }

        private void ClearHistory()
        {
            History.Clear();
        }

        private void ToggleSettings()
        {
            IsShowSettings = !IsShowSettings;
        }

        private void Copy()
        {
            if (SelectedHistoryRecord != null && !String.IsNullOrEmpty(SelectedHistoryRecord.Text))
            {
                Clipboard.SetText(SelectedHistoryRecord.Text);
            }
        }

        public void GenerateAndCopy()
        {
            Generate();
            Copy();
        }

        public void Generate()
        {
            try
            {
                IsError = false;
                Result = SelectedService.Generate();

                if (!String.IsNullOrEmpty(Result))
                {
                    History.Insert(0, new HistoryRecord { Text = Result, Timestamp = DateTime.Now });
                    SelectedHistoryRecord = History.FirstOrDefault();
                }
            }
            catch (InvalidOperationException ex)
            {
                IsError = true;
                ErrorMessage = ex.Message;
            }
        }

        public string ErrorMessage
        {
            get { return _errorMessage; }
            set
            {
                _errorMessage = value;
                OnPropertyChanged();
            }
        }

        public bool IsError
        {
            get { return _isError; }
            set
            {
                _isError = value;
                OnPropertyChanged();
            }
        }
    }
}
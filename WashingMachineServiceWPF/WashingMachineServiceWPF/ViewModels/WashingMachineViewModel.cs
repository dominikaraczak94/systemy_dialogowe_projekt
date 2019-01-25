using Caliburn.Micro;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using WashingMachineService.Services;
using WashingMachineServiceWPF.Models;
using WashingMachineServiceWPF.Services;

namespace WashingMachineServiceWPF.ViewModels
{
    public class WashingMachineViewModel : PropertyChangedBase, IHandle<RecognitionResultEvent>
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly RecognitionService _recognitionService;
        private readonly SynthesizerService _synthesizerService;

        private readonly WashingProgramModel _washingProgramModel;

        private Visibility _mainGridVisibility;

        private string _result;

        private string _washingMachineInfo;

        private string _washingMachineSummary;

        private Visibility _washingProgramGridVisibility;

        private bool _washingProgramView;

        private Visibility _washingSummaryGridVisibility;

        private bool _washingSummaryView;

        private Visibility _washingTemperatureGridVisibility;

        private bool _washingTemperatureView;

        private Visibility _washingTimeGridVisibility;
        private Visibility _orderHistoryGridVisibility;

        private bool _washingTimeView;
        private string _choice;
        private List<string> _listOfTimeTypes;
        private List<string> _listOfTemperatures;
        private List<Order> _orderHistoryList;
        private Order _order;
        private DatabaseService _databaseService;

        public WashingMachineViewModel()

        {
            _washingProgramModel = new WashingProgramModel();
            MainGridVisibility = Visibility.Visible;
            WashingProgramGridVisibility = Visibility.Hidden;
            WashingTimeGridVisibility = Visibility.Hidden;
            WashingTemperatureGridVisibility = Visibility.Hidden;
            WashingSummaryGridVisibility = Visibility.Hidden;
            OrderHistoryGridVisibility = Visibility.Hidden;
            _eventAggregator = new EventAggregator();
            _eventAggregator.Subscribe(this);
            _recognitionService = new RecognitionService(_eventAggregator);
            _synthesizerService = new SynthesizerService();
            _databaseService = new DatabaseService();
        }

        public Visibility MainGridVisibility
        {
            get => _mainGridVisibility;
            set
            {
                _mainGridVisibility = value;
                NotifyOfPropertyChange(() => MainGridVisibility);
            }
        }

        public string WashingMachineProgramInfo
        {
            get => _washingMachineInfo;
            set
            {
                _washingMachineInfo = value;
                NotifyOfPropertyChange(() => WashingMachineProgramInfo);
            }
        }

        public string WashingMachineProgramSummary
        {
            get => _washingMachineSummary;
            set
            {
                _washingMachineSummary = value;
                NotifyOfPropertyChange(() => WashingMachineProgramSummary);
            }
        }

        public Visibility WashingProgramGridVisibility
        {
            get => _washingProgramGridVisibility;
            set
            {
                _washingProgramGridVisibility = value;
                NotifyOfPropertyChange(() => WashingProgramGridVisibility);
            }
        }

        public Visibility WashingSummaryGridVisibility
        {
            get => _washingSummaryGridVisibility;
            set
            {
                _washingSummaryGridVisibility = value;
                NotifyOfPropertyChange(() => WashingSummaryGridVisibility);
            }
        }

        public Visibility WashingTemperatureGridVisibility
        {
            get => _washingTemperatureGridVisibility;
            set
            {
                _washingTemperatureGridVisibility = value;
                NotifyOfPropertyChange(() => WashingTemperatureGridVisibility);
            }
        }

        public Visibility OrderHistoryGridVisibility
        {
            get => _orderHistoryGridVisibility;
            set
            {
                _orderHistoryGridVisibility = value;
                NotifyOfPropertyChange(() => OrderHistoryGridVisibility);
            }
        }

        public Visibility WashingTimeGridVisibility
        {
            get => _washingTimeGridVisibility;
            set
            {
                _washingTimeGridVisibility = value;
                NotifyOfPropertyChange(() => WashingTimeGridVisibility);
            }
        }

        public List<string> ListOfTimeTypes
        {
            get => _listOfTimeTypes;
            set
            {
                _listOfTimeTypes = value;
                NotifyOfPropertyChange(() => ListOfTimeTypes);
            }
        }

        public List<string> ListOfTemperatures
        {
            get => _listOfTemperatures;
            set
            {
                _listOfTemperatures = value;
                NotifyOfPropertyChange(() => ListOfTemperatures);
            }
        }

        public List<Order> OrderHistoryList
        {
            get => _orderHistoryList;
            set
            {
                _orderHistoryList = value;
                NotifyOfPropertyChange(() => OrderHistoryList);
            }
        }

        public void Handle(RecognitionResultEvent message)
        {
            _result = message.Message;
            if (_washingProgramView)
            {
                _washingProgramView = ProceedWashingProgram(_washingProgramModel.Programs);
                if (_washingProgramView)
                {
                    _order.WashingProgram = _result;
                }

                _washingTimeView = !_washingProgramView;
                if (!_washingProgramView)
                {
                    WashingProgramGridVisibility = Visibility.Hidden;
                    WashingTimeGridVisibility = Visibility.Visible;
                    _synthesizerService.Synthesize("Wybierz długość prania");
                    WashingMachineProgramInfo = string.Empty;
                    return;
                }
            }

            if (_washingTimeView)
            {
                var washingTimeModel = _databaseService.SelectProgramDefaultTime(_order.WashingProgram);
                ListOfTimeTypes = washingTimeModel.CreateWashingTimesList();
                _washingTimeView = ProceedWashingProgram(washingTimeModel.Times);
                if (_washingTimeView)
                {
                    _order.WashingTimeType = _result;
                    _order.CalculatedWashingTime = washingTimeModel.RecalculateTime(_result);
                }

                _washingTemperatureView = !_washingTimeView;
                if (!_washingTimeView)
                {
                    WashingTimeGridVisibility = Visibility.Hidden;
                    WashingTemperatureGridVisibility = Visibility.Visible;
                    _synthesizerService.Synthesize("Wybierz temperaturę prania");
                    WashingMachineProgramInfo = string.Empty;
                    return;
                }
            }

            if (_washingTemperatureView)
            {
                var washingTemperatureModel = _databaseService.SelectProgramTemperatures(_order.WashingProgram);
                ListOfTemperatures = washingTemperatureModel.Select(t => $"{t.WashingTemperature}°C").OrderBy(t => t).ToList();
                _washingTemperatureView = ProceedWashingProgram(washingTemperatureModel.Select(t => $"{t.WashingTemperature}").ToList());
                if (_washingTemperatureView)
                {
                    int.TryParse(_result, out int temp);
                    _order.WashingTemperature = temp;
                }

                _washingSummaryView = !_washingTemperatureView;
                if (!_washingTemperatureView)
                {
                    WashingTemperatureGridVisibility = Visibility.Hidden;
                    WashingSummaryGridVisibility = Visibility.Visible;
                    WashingMachineProgramInfo = string.Empty;
                    return;
                }
            }

            if (_washingSummaryView)
            {
                var summary =
                    $"Wybrano \ntryb prania: {_order.WashingProgram} \nczas prania: {_order.WashingTimeType} \ntemperaturę: {_order.WashingTemperature}";
                WashingMachineProgramSummary = summary + "°C\n" + _order.WashingTimeToString();
                WashingMachineProgramInfo = "Powiedz OK";
                if (_result == "okej")
                {
                    _databaseService.InsertOrderToDatabase(_order);
                    _synthesizerService.Synthesize(
                        $"Wybrano\n tryb prania: {_order.WashingProgram},\n czas prania: {_order.WashingTimeType}\ntemperaturę: {_order.WashingTemperature}stopni Celsjusza\n" + _order.WashingTimeToString());
                    Application.Current.Shutdown();
                }
            }
        }

        public bool ProceedWashingProgram(List<string> elements)
        {
            if (elements.Contains(_result))
            {
                WashingMachineProgramInfo = "Twój wybór to: " + _result + ". Jeśli się zgadzasz powiedz OK";
                _choice = _result;
            }
            else if (_result == "okej")
            {
                _synthesizerService.Synthesize($"Wybrano {_choice}");
                return false;
            }
            else
            {
                WashingMachineProgramInfo = "Nie można rozpoznać, powiedz ponownie.";
            }

            return true;
        }

        public void StartRecognition()
        {
            MainGridVisibility = Visibility.Hidden;
            WashingProgramGridVisibility = Visibility.Visible;
            _recognitionService.RecognizeSpeechContinuously();
            _washingProgramView = true;
            _order = new Order();
            _synthesizerService.Synthesize("Wybierz program");
        }

        public void GetFromHistory()
        {
            MainGridVisibility = Visibility.Hidden;
            OrderHistoryGridVisibility = Visibility.Visible;
            OrderHistoryList = _databaseService.ReadOrdersFromDatabase();
        }

        public void BackToMainMenu()
        {
            MainGridVisibility = Visibility.Visible;
            OrderHistoryGridVisibility = Visibility.Hidden;
        }
    }
}
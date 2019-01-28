using System.Linq;
using WashingMachineServiceWPF.Models;

namespace WashingMachineServiceWPF.Services
{
    public class RecognitionFromSentenceService
    {
        public string RecognitionResult { get; set; }
        private WashingProgramModel _washingProgramModel;
        private Order _order;

        public RecognitionFromSentenceService()
        {
            _order = new Order();
            RecognitionResult = "";
        }

        public void Recognize(string result)
        {
            if (RecognizeProgram(result))
            {
                if (RecognizeTime(result))
                { }
            }
        }

        private bool RecognizeProgram(string result)
        {
            _washingProgramModel = new WashingProgramModel();
            if (_washingProgramModel.Programs.Any(p => result.Contains(p)))
            {
                _order.WashingProgram = _washingProgramModel.Programs.FirstOrDefault(p => result.Contains(p));
                RecognitionResult += "Wybrany program: " + _order.WashingProgram;
                return true;
            }
            RecognitionResult = "Nie rozpoznano programu.";
            return false;
        }

        private bool RecognizeTime(string result)
        {
            _washingProgramModel = new WashingProgramModel();
            if (_washingProgramModel.Programs.Any(p => result.Contains(p)))
            {
                _order.WashingProgram = _washingProgramModel.Programs.FirstOrDefault(p => result.Contains(p));
                RecognitionResult += $"Wybrany czas: " + _order.WashingProgram;
                return true;
            }
            RecognitionResult = "Nie rozpoznano programu.";
            return false;
        }
    }
}
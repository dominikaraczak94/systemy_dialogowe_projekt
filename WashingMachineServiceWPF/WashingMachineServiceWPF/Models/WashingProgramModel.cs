using System.Collections.Generic;

namespace WashingMachineServiceWPF.Models
{
    public class WashingProgramModel
    {
        public List<string> Programs => new List<string>()
            {"delikatne", "codzienne", "dziecięce", "sportowe", "mocno zabrudzone", "buty"};
        
    }
}

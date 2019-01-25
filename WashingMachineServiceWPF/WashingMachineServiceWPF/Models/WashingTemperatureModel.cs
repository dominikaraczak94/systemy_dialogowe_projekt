using System.Collections.Generic;

namespace WashingMachineServiceWPF.Models
{
    public class WashingTemperatureModel
    {
        public List<string> Temperatures => new List<string>()
            {"30", "40", "60"};

        public int WashingTemperature { get; set; }
        public int WashingProgramId { get; set; }
        public int Id { get; set; }
    }
}
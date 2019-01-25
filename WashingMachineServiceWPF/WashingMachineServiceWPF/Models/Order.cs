namespace WashingMachineServiceWPF.Models
{
    public class Order
    {
        public string WashingProgram { get; set; }
        public int WashingTemperature { get; set; }
        public string CalculatedWashingTime { get; set; }
        public int Id { get; set; }
        public string WashingTimeType { get; set; }

        public override string ToString()
        {
            return $"PROGRAM: {WashingProgram}  CZAS: {CalculatedWashingTime}  TEMPERATURA:{WashingTemperature}°C";
        }

        public string WashingTimeToString()
        {
            var stringVal = "Czas trwania prania: ";
            var hours = CalculatedWashingTime.Split(':')[0];
            var minutes = CalculatedWashingTime.Split(':')[1];

            if (hours == "1")
            {
                stringVal += hours + " godzina ";
            }
            else if (hours != "0")
            {
                stringVal += hours + " godziny ";
            }

            if (minutes.EndsWith("2") || minutes.EndsWith("3") || minutes.EndsWith("4"))
            {
                stringVal += minutes + " minuty.";
            }
            else
            {
                stringVal += minutes + " minut.";
            }

            return stringVal;
        }
    }
}
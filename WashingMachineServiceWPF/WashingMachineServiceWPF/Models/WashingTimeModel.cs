using System;
using System.Collections.Generic;
using System.Linq;

namespace WashingMachineServiceWPF.Models
{
    public class WashingTimeModel
    {
        public List<string> Times => new List<string>()
            {"krótkie", "standardowe", "długie"};

        public List<string> TimesToWrite => new List<string>()
            {"krotkie", "standardowe", "dlugie"};

        public int Id { get; set; }
        public string WashingTime { get; set; }
        public int WashingProgramId { get; set; }

        public string RecalculateTime(string timeType)
        {
            var hours = WashingTime.Split(':')[0];
            var minutes = WashingTime.Split(':')[1];

            var time = int.Parse(hours) * 60;
            if (!string.IsNullOrEmpty(minutes))
            {
                time += int.Parse(minutes);
            }

            if (timeType == "krotkie" || timeType == "krótkie")
            {
                time = time * 4 / 10;
            }
            else if (timeType == "dlugie" || timeType == "długie")
            {
                time = time * 13 / 10;
            }
            TimeSpan span = TimeSpan.FromMinutes(time);
            return span.ToString(@"h\:mm");
        }

        public List<string> CreateWashingTimesList()
        {
            return TimesToWrite.Select(t => $"{t} ({RecalculateTime(t)})").ToList();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AquaCulture.Application.Dto.FishFarm
{
    public class SearchFishFarmDto
    {
        public string? SearchTerm { get; set; }
        public bool? HasBarge { get; set; }
        public int? MinAvailableCages { get; set; }
        public int? MaxAvailableCages { get; set; }
        public string? SortBy { get; set; }
    }
}

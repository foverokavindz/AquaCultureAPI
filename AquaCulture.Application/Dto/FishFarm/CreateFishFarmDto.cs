using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AquaCulture.Application.Dto.FishFarm
{
    public class CreateFishFarmDto
    {
        public string Name { get; set; } = string.Empty;
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public int NoOfCages { get; set; }
        public bool HasBarge { get; set; }
        public string PictureUrl { get; set; }
    }
}

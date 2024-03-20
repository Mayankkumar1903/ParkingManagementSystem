using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingManagement.Model
{
    public class ParkingSpaceModel
    {
        public int ParkingSpaceId {  get; set; }
        public string ParkingSpaceTitle { get; set; }
        public string Availability { get; set; }
        public string RegistrationNumber { get; set; }
    }
}
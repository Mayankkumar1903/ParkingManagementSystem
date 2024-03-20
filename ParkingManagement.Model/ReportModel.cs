using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingManagement.Model
{
    public class ReportModel
    {
        public string ParkingZone { get; set; }
        public string ParkingSpace { get; set; }
        public int NumberOfBookings { get; set; }
        public int NumberOfVehiclesParked { get; set; }

    }
}

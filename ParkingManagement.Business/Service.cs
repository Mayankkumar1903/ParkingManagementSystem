using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ParkingManagement.DAL;
using ParkingManagement.Model;

namespace ParkingManagement.Business
{
    public class Service
    {
        public SessionModel Login(LoginModel model)
        {
            return new DataAccess().Login(model);
        }
        public List<ParkingSpaceModel> AllParkingSpace()
        {
            return new DataAccess().AllParkingSpace();
        }
        public bool BookSpace(string vehicleRegistrationNumber)
        {
            return new DataAccess().BookSpace(vehicleRegistrationNumber);
        }
        public bool FreeSpace(string vehicleRegistrationNumber)
        {
            return new DataAccess().FreeSpace(vehicleRegistrationNumber);
        }
        public List<ReportModel> GenerateParkingReport(DateTime startDate, DateTime endDate)
        {
            return new DataAccess().GenerateParkingReport(startDate, endDate);
        }
        public bool Signup(SignupModel userdata)
        {
            return new DataAccess().Signup(userdata);
        }
        public bool AddParkingSpace(ParkingModel model)
        {
            return new DataAccess().AddParkingSpace(model);
        }
        public List<ParkingZoneModel> AllParkingZone()
        {
            return new DataAccess().AllParkingZone();
        }
        public bool FindEmail(string email)
        {
            return new DataAccess().FindEmail(email);
        }
        public bool DeleteAllTransaction()
        {
            return new DataAccess().DeleteAllTransaction();
        }
        public bool BookSpaceById(ParkingSpaceModel parkingmodel)
        {
            return new DataAccess().BookSpaceById(parkingmodel);
        }
        public bool FreeSpaceById(int id)
        {
            return new DataAccess().FreeSpaceById(id);
        }
    }
}

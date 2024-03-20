using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using ParkingManagement.Model;
using ParkingManagement.Utils;
using ParkingManagement.Logger;

namespace ParkingManagement.DAL
{
    public class DataAccess
    {
        /// <summary>
        /// Check is valid credential or not if valid then user login
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public SessionModel Login(LoginModel model)
        {
            SessionModel session = null;
            try
            {
                using (ParkingManagementEntities1 context = new ParkingManagementEntities1())
                {
                    User user = context.Users.FirstOrDefault(u => u.Email == model.Email);
                    if (user != null && BCryptConvertion.Verify(user.Password, model.Password))
                    {
                        session = new SessionModel();
                        session.Email = model.Email;
                        session.Name = user.Name;
                        session.Type = user.Type;
                        SessionUtil.SetSession(session);
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerClass.AddLog(ex);
            }
            return session;
        }

        /// <summary>
        /// Return AllParkingSpace
        /// </summary>
        /// <returns></returns>
        public List<ParkingSpaceModel> AllParkingSpace()
        {
            List<ParkingSpaceModel> allparkingspace = null;
            try
            {
                using (ParkingManagementEntities1 context = new ParkingManagementEntities1())
                {
                    var parkingSpacesInfo = context.ParkingSpaces
                                                    .Select(p => new
                                                    {
                                                        ParkingSpaceId=p.ParkingSpaceID,
                                                        ParkingSpaceTitle = p.ParkingSpaceTitle,
                                                        ParkingStatus = p.VehicleParkings.OrderByDescending(vp => vp.VehicleParkingID).Select(vp => vp.ReleaseDateTime).FirstOrDefault(),
                                                        RegistrationNumber = p.VehicleParkings.OrderByDescending(vp => vp.VehicleParkingID).Where(vp => vp.BookingDateTime != null).Count(vp => vp.ReleaseDateTime == null) != 0 ? p.VehicleParkings.OrderByDescending(Vp => Vp.VehicleParkingID).Select(vp => vp.RegistrationNumber).FirstOrDefault() : null
                                                    })
                                                    .OrderBy(p=>p.ParkingSpaceTitle).ToList();


                    allparkingspace = new List<ParkingSpaceModel>();

                    foreach (var info in parkingSpacesInfo)
                    {
                        ParkingSpaceModel parkingspace = new ParkingSpaceModel();
                        parkingspace.ParkingSpaceId = info.ParkingSpaceId;
                        parkingspace.ParkingSpaceTitle = info.ParkingSpaceTitle;
                        parkingspace.Availability = info.ParkingStatus == null && info.RegistrationNumber != null ? "occupied" : "vacant";
                        parkingspace.RegistrationNumber = info.RegistrationNumber;
                        allparkingspace.Add(parkingspace);
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerClass.AddLog(ex);
            }
            return allparkingspace;
        }

        public bool BookSpaceById(ParkingSpaceModel parkingmodel)
        {
            bool flag = false;
            try
            {
                using(ParkingManagementEntities1 context = new ParkingManagementEntities1())
                {
                    ParkingSpace parkingspace= context.ParkingSpaces.FirstOrDefault(u => u.ParkingSpaceID == parkingmodel.ParkingSpaceId);
                    if (parkingspace != null)
                    {
                        var newVehicleParking = new VehicleParking
                        {
                            ParkingZoneID = parkingspace.ParkingZoneID,
                            ParkingSpaceID = parkingspace.ParkingSpaceID,
                            RegistrationNumber = parkingmodel.RegistrationNumber,
                            BookingDateTime = DateTime.Now
                        };
                        context.VehicleParkings.Add(newVehicleParking);
                        context.SaveChanges();
                        flag = true;
                    }
                }
            }
            catch(Exception ex)
            {
                LoggerClass.AddLog(ex);
            }
            return flag;
        }

        public bool FreeSpaceById(int id)
        {
            bool flag = false;
            try
            {
                using (ParkingManagementEntities1 context = new ParkingManagementEntities1())
                { 
                    VehicleParking vehicleparking = context.VehicleParkings.OrderByDescending(v=>v.VehicleParkingID).FirstOrDefault(v=>v.ParkingSpaceID == id);
                    if (vehicleparking != null)
                    {
                        vehicleparking.ReleaseDateTime = DateTime.Now;
                        context.SaveChanges();
                    }
                }
                flag = true;
            }
            catch (Exception ex)
            {
                LoggerClass.AddLog(ex);
            }
            return flag;
        }

        /// <summary>
        /// Book A parking Space for a valid car number
        /// </summary>
        /// <param name="vehicleRegistrationNumber"></param>
        /// <returns></returns>
        public bool BookSpace(string vehicleRegistrationNumber)
        {
            bool flag = true;
            try
            {
                using (ParkingManagementEntities1 context = new ParkingManagementEntities1())
                {

                    var vacantParkingSpace = context.ParkingSpaces
                .Where(ps => !context.VehicleParkings.Any(vp =>
                    vp.ParkingSpaceID == ps.ParkingSpaceID &&
                    (vp.ReleaseDateTime == null || vp.BookingDateTime == null)))
                .Select(ps => new
                {
                    ParkingSpaceID = ps.ParkingSpaceID,
                    ParkingZoneID = ps.ParkingZoneID
                })
                .FirstOrDefault();

                    if (vacantParkingSpace != null)
                    {
                        var newVehicleParking = new VehicleParking
                        {
                            ParkingZoneID = vacantParkingSpace.ParkingZoneID,
                            ParkingSpaceID = vacantParkingSpace.ParkingSpaceID,
                            RegistrationNumber = vehicleRegistrationNumber,
                            BookingDateTime = DateTime.Now
                        };

                        context.VehicleParkings.Add(newVehicleParking);
                        context.SaveChanges();
                        flag = true;
                    }
                    else
                    {
                        flag = false;
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerClass.AddLog(ex);
            }

            return flag;
        }

        /// <summary>
        /// Free A parking Space for a car 
        /// </summary>
        /// <param name="vehicleRegistrationNumber"></param>
        /// <returns></returns>
        public bool FreeSpace(string vehicleRegistrationNumber)
        {
            bool flag = false;
            try
            {
                using (ParkingManagementEntities1 context = new ParkingManagementEntities1())
                {
                    VehicleParking vehicleparking = context.VehicleParkings.Where(vp => vp.RegistrationNumber == vehicleRegistrationNumber && vp.ReleaseDateTime == null).OrderByDescending(vp => vp.VehicleParkingID).FirstOrDefault();
                    if (vehicleparking != null)
                    {
                        vehicleparking.ReleaseDateTime = DateTime.Now;
                        context.SaveChanges();
                        flag = true;
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerClass.AddLog(ex);
            }

            return flag;
        }

        /// <summary>
        /// Return Parking Report for car 
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public List<ReportModel> GenerateParkingReport(DateTime startDate, DateTime endDate)
        {
            List<ReportModel> report = null;
            endDate = endDate.AddDays(1);
            try
            {
                using (ParkingManagementEntities1 context = new ParkingManagementEntities1())
                {
                    report = (from pz in context.ParkingZones
                              from ps in context.ParkingSpaces.Where(x => x.ParkingZoneID == pz.ParkingZoneID).DefaultIfEmpty()
                              from vp in context.VehicleParkings.Where(x => x.ParkingSpaceID == ps.ParkingSpaceID && x.BookingDateTime >= startDate && x.BookingDateTime <= endDate).OrderByDescending(x => x.VehicleParkingID).Take(1).DefaultIfEmpty()
                              select new ReportModel
                              {
                                  ParkingZone = pz.ParkingZoneTitle,
                                  ParkingSpace = ps.ParkingSpaceTitle,
                                  NumberOfBookings = context.VehicleParkings.Count(x => x.ParkingSpaceID == ps.ParkingSpaceID && x.BookingDateTime >= startDate && x.BookingDateTime <= endDate),
                                  NumberOfVehiclesParked = ((vp != null && vp.ReleaseDateTime.HasValue) || vp == null) ? 0 : 1
                              }).Distinct().ToList();
                }
            }
            catch (Exception ex)
            {
                LoggerClass.AddLog(ex);
            }
            return report;
        }

        /// <summary>
        /// SignUp for new user
        /// </summary>
        /// <param name="userdata"></param>
        /// <returns></returns>
        public bool Signup(SignupModel userdata)
        {
            bool flag = false;
            try
            {
                using (ParkingManagementEntities1 context = new ParkingManagementEntities1())
                {
                    User user = new User();
                    user.Name = userdata.Name;
                    user.Email = userdata.Email;
                    user.Password = BCryptConvertion.Encrypt(userdata.Password);
                    user.Type = Convert.ToInt32(userdata.Type);
                    context.Users.Add(user);
                    context.SaveChanges();
                    flag = true;
                }
            }
            catch (Exception ex)
            {
                LoggerClass.AddLog(ex);
            }
            return flag;
        }

        /// <summary>
        /// Add Parking Space for zones
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool AddParkingSpace(ParkingModel model)
        {
            bool flag = false;
            try
            {

                using (ParkingManagementEntities1 context = new ParkingManagementEntities1())
                {
                    ParkingZone existingZone = context.ParkingZones.FirstOrDefault(z => z.ParkingZoneTitle == model.ParkingZoneTitle);

                    if (existingZone == null)
                    {
                        ParkingZone parkingzone = new ParkingZone();
                        parkingzone.ParkingZoneTitle = model.ParkingZoneTitle;
                        context.ParkingZones.Add(parkingzone);

                        context.SaveChanges();

                        for (int i = 1; i <= model.NumberOfSpaces; i++)
                        {
                            string spaceName = $"{model.ParkingZoneTitle}{i:D2}";

                            if (context.ParkingSpaces.Any(s => s.ParkingSpaceTitle == spaceName))
                            {
                                int counter = 1;
                                do
                                {
                                    counter++;
                                    spaceName = $"{model.ParkingZoneTitle}{counter:D2}";
                                } while (context.ParkingSpaces.Any(s => s.ParkingSpaceTitle == spaceName));
                            }

                            ParkingSpace space = new ParkingSpace { ParkingSpaceTitle = spaceName, ParkingZoneID = parkingzone.ParkingZoneID };
                            context.ParkingSpaces.Add(space);
                        }

                        context.SaveChanges();
                        flag = true;
                    }
                    else
                    {
                        var lastSpace = context.ParkingSpaces
                            .Where(s => s.ParkingZoneID == existingZone.ParkingZoneID)
                            .OrderByDescending(s => s.ParkingSpaceTitle)
                            .FirstOrDefault();

                        int startCounter = (lastSpace != null) ? int.Parse(lastSpace.ParkingSpaceTitle.Substring(existingZone.ParkingZoneTitle.Length)) + 1 : 1;

                        for (int i = startCounter; i < startCounter + model.NumberOfSpaces; i++)
                        {
                            string spaceName = $"{model.ParkingZoneTitle}{i:D2}";
                            ParkingSpace space = new ParkingSpace { ParkingSpaceTitle = spaceName, ParkingZoneID = existingZone.ParkingZoneID };
                            context.ParkingSpaces.Add(space);
                        }

                        context.SaveChanges();
                        flag = true;
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerClass.AddLog(ex);
            }
            return flag;
        }

        /// <summary>
        /// Return All parking zone
        /// </summary>
        /// <returns></returns>
        public List<ParkingZoneModel> AllParkingZone()
        {
            List<ParkingZoneModel> parkingzones = null;
            try
            {
                using (ParkingManagementEntities1 context = new ParkingManagementEntities1())
                {
                    parkingzones = context.ParkingZones.Select(p => new ParkingZoneModel
                    {
                        ParkingZoneID = p.ParkingZoneID,
                        ParkingZoneTitle = p.ParkingZoneTitle,
                    })
                                                             .ToList();
                }
            }
            catch (Exception ex)
            {
                LoggerClass.AddLog(ex);
            }

            return parkingzones;
        }

        /// <summary>
        /// Find out a email already exists or not
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public bool FindEmail(string email)
        {
            bool flag = false;
            try
            {
                using (ParkingManagementEntities1 context = new ParkingManagementEntities1())
                {
                    int count = context.Users.Where(u => u.Email == email).Count();
                    if (count == 0)
                    {
                        flag = true;
                    }
                }
            }
            catch(Exception ex)
            {
                LoggerClass.AddLog(ex);
            }
            return flag;
        }

        /// <summary>
        /// Delete All transaction
        /// </summary>
        /// <returns></returns>
        public bool DeleteAllTransaction()
        {
            bool flag = false;
            try
            {
                using (ParkingManagementEntities1 context = new ParkingManagementEntities1())
                {
                    context.Database.ExecuteSqlCommand("truncate table vehicleparking");
                    context.SaveChanges();
                    flag = true;
                }
            }
            catch (Exception ex)
            {
                LoggerClass.AddLog(ex);
            }
            return flag;
        }
    }
}

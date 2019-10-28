using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prague_Parking_2._0
{
    class Vehicle
    {
        private string licensePlate;
        private DateTime parkingTime;
        private VehicleTypeEnum vehicleTypes;

        public Vehicle()
        {

        }

        public Vehicle(VehicleTypeEnum vehicleTypes, string licensePlate, DateTime parkingTime)
        {
            this.vehicleTypes = vehicleTypes;
            this.licensePlate = licensePlate;
            this.parkingTime = parkingTime;
        }

        public VehicleTypeEnum VehicleTypes
        {
            get { return this.vehicleTypes; }
        }

        public string LicensePlate
        {
            get { return this.licensePlate; }
        }

        public DateTime ParkingTime
        {
            get { return this.parkingTime; }
        }

    }

    public enum VehicleTypeEnum
    {
        Car = 2, MC = 1
    }
}

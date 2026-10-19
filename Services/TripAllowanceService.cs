using Microsoft.EntityFrameworkCore;
using NPOI.OpenXml4Net.OPC;
using TMSWeb.Models;
using TMSWeb.Models.Entities.LOG;
using TMSWeb.Repositories;

namespace TMSWeb.Services
{
    public class ServiceAllowance
    {
        public decimal truckValue;
        public decimal driverValue;
        public decimal oldTruck;
        public decimal newTruck;
        public decimal oldDriver;
        public decimal newDriver;
    }
    public class TripAllowanceService
    {
        private readonly DbTMSContext db;
        private readonly IConfiguration _configuration;

        public TripAllowanceService(
            DbTMSContext _db,
            IConfiguration configuration)
        {
            db = _db;
            _configuration = configuration;
        }

        public async Task<ServiceAllowance> GetDriverCostAsync(TblSupplier oldSupplier, TblSupplier newSupplier, string placeCode)
        {
            var deductDriver = _configuration.GetValue<decimal>("Deduction:driver");
            var driverContainer = _configuration.GetValue<decimal>("Allowance:driver_container_pickup");

            var driverAllowance = await db.TblDriverRates
                .AsNoTracking()
                .Where(w => w.PlaceCode == placeCode)
                .Select(s => s.RateValue)
                .FirstOrDefaultAsync();

            ServiceAllowance allowance = new ServiceAllowance();
            allowance.driverValue = driverAllowance;
            if (newSupplier.FleetTypeCode == "F1")
            {
                allowance.newDriver = driverAllowance - deductDriver;
                allowance.oldDriver = driverContainer;
            }
            else
            {
                allowance.newDriver = 0;
                allowance.oldDriver = driverContainer;
            }

            return allowance;
        }

        public async Task<ServiceAllowance> GetTruckCostAsync(TblSupplier oldSupplier, TblSupplier newSupplier, TblTruckDetail newTruck, string placeCode)
        {
            var deductTruck = _configuration.GetValue<decimal>("Deduction:supplier");
            var truckContainer = _configuration.GetValue<decimal>("Allowance:supplier_container_pickup");

            decimal cost = 0m;
            ServiceAllowance allowance = new ServiceAllowance();
            //รถใหม่เป็นภายใน
            if (newSupplier.FleetTypeCode == "F1")
            {
                allowance.truckValue = await this.GetFleetcostAsync(newTruck.TruckCode, newTruck.TruckTypeCode, placeCode);
                allowance.newTruck =  allowance.truckValue - deductTruck;

                //รถเก่าเป็นภายใน
                if (oldSupplier.FleetTypeCode == "F1")
                {
                    allowance.oldTruck = truckContainer;
                }
                //รถเก่าเป็นภายนอก
                else
                {
                    allowance.oldTruck = 0;
                }
            }
            //รถใหม่เป็นภายนอก
            else
            {
                allowance.newTruck = await GetSupplierCostAsync(newSupplier.SupplierCode, placeCode);
                //รถเก่าเป็นภายใน
                if (oldSupplier.FleetTypeCode == "F1")
                {
                    allowance.oldTruck = truckContainer;
                }
            }

            return allowance;
        }

        private async Task<decimal> GetFleetcostAsync(string newTruck, string newTruckType, string placeCode)
        {
            var fleetRate = await (from fleetrate in db.TblFleetRates.AsNoTracking()
                                   join fleetsuppliertype in db.TblFleetSupplierTypes.AsNoTracking()
                                      on fleetrate.FleetSupplierTypeCode equals fleetsuppliertype.FleetSupplierTypeCode
                                   join fleetsupplier in db.TblFleetSuppliers.AsNoTracking()
                                      on fleetsuppliertype.FleetSupplierTypeCode equals fleetsupplier.FleetSuppplierTypeCode
                                   join truck in db.TblTruckDetails.AsNoTracking()
                                      on fleetsupplier.FleetSupplierCode equals truck.FleetSupplierCode
                                   where truck.TruckCode == newTruck && fleetrate.PlaceCode == placeCode
                                   select fleetrate).FirstOrDefaultAsync();

            decimal rateValue = 0.00m;
            switch (newTruckType)
            {
                case "01":
                    rateValue = (decimal?)fleetRate.Truck4w ?? 0;
                    break;
                case "02":
                    rateValue = (decimal?)fleetRate.Truck6w ?? 0;
                    break;
                case "03":
                    rateValue = (decimal?)fleetRate.Truck10w ?? 0;
                    break;
            }

            return rateValue;
        }

        private async Task<decimal> GetSupplierCostAsync(string supplierCode, string placeCode)
        {
            return await db.TblSupplierRates
                .AsNoTracking()
                .Where(w => w.SupplierCode == supplierCode &&
                            w.PlaceCode == placeCode)
                .Select(s => (decimal?)s.CostRate)
                .FirstOrDefaultAsync() ?? 0;
        }
    }
}

using BrazosApp.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrazosApp.DataAccess.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        //public DbSet<ApplyFor> ApplyFor { get; set; }

        //public DbSet<ApplicationForLicense> ApplicationForLicenses { get; set; }

        public DbSet<Role> Roles { get; set; }

        public DbSet<Users> Users { get; set; }

        public DbSet<Territory> Territories { get; set; }

        public DbSet<TerritoryWiseInspectors> TerritoryWiseInspectors { get; set; }

        public DbSet<TerritoryAssignedType> TerritoryAssignedTypes { get; set; }

        //public DbSet<Category> Category { get; set; }

        //public DbSet<SubCategory> SubCategory { get; set; }

        //public DbSet<PermitType> PermitType { get; set; }

        public DbSet<PermitStatus> PermitStatus { get; set; }

        public DbSet<RiskCategory> RiskCategory { get; set; }

        public DbSet<LanguageType> LanguageType { get; set; }

        public DbSet<ApplicationType> ApplicationType { get; set; }

        public DbSet<ApplicationFor> ApplicationFor { get; set; }
        public DbSet<Application> Applications { get; set; }

        public DbSet<BusinessType> BusinessTypes { get; set; }

        public DbSet<OperationType> OperationTypes { get; set; }

        public DbSet<Establishment> Establishments { get; set; }

        public DbSet<EstablishmentOwner> EstablishmentOwners { get; set; }

        public DbSet<RFOperationDetails> RFOperationDetails { get; set; }

        public DbSet<Document> Documents { get; set; }

        public DbSet<MFVehicleInformation> MFVehicleInformation { get; set; }

        public DbSet<MFOperationDetails> MFOperationDetails { get; set; }

        public DbSet<FeePrograms> Programs { get; set; }

        public DbSet<JurisdictionAccounts> Jurisdictions { get; set; }

        public DbSet<EstablishmentSize> EstablishmentSizes { get; set; }

        public DbSet<EstablishmentTypes> EstablishmentTypes { get; set; }

        public DbSet<Notes> Notes { get; set; }

        public DbSet<AgencyStaffReqFields> AgencyStaffReqFields { get; set; }

        public DbSet<Fees> Fees { get; set; }

        public DbSet<Payment> Payments { get; set; }
        public DbSet<PaymentSplit> PaymentSplits { get; set; }

        public DbSet<ScheduleStatus> ScheduleStatus { get; set; }

        public DbSet<InspectionPurposes> InspectionPurposes { get; set; }

        public DbSet<Schedule> Schedules { get; set; }

        public DbSet<InspectionItemCode> InspectionItemCodes { get; set; }

        public DbSet<Section> Sections { get; set; }

        public DbSet<SubSection> SubSections { get; set; }

        public DbSet<Item>  Items { get; set; }

        public DbSet<Inspection> Inspections { get; set; }

        public DbSet<InspectionItemAdjuncTable> InspectionItemAdjuncTable { get; set; }

        public DbSet<InspectionItemDetails> InspectionItemDetails { get; set; }

        public DbSet<OpeningInspectionData> OpeningInspectionData { get; set; }

        public DbSet<TemperatureObservation> TemperatureObservation { get; set; }

        public DbSet<RFMFInspectionData> RFMFInspectionData { get; set; }

        public DbSet<WaterSource> WaterSources { get; set; }
        public DbSet<PublicSewage> PublicSewage { get; set; }
        public DbSet<PrivateSeptic> PrivateSeptic { get; set; }

        public DbSet<Area> Areas { get; set; }

        public DbSet<AreaWiseInspectors> AreaWiseInspectors { get; set; }

        public DbSet<FeesDetailsTable> FeesDetailsTables { get; set; }

        public DbSet<TFOperationDetails> TFOperationDetails { get; set; }   

        public DbSet<MFOperationLocations> MFOperationLocations { get; set; }
        public DbSet<FoodRenewalHistory> FoodRenewalHistory { get; set; }

        public DbSet<RenewalTempHistory> RenewalTempHistories { get; set; }
        public DbSet<PermitReferenceCount> PermitReferenceCount { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Seed();
            modelBuilder.EntityDecorate();
        }
    }
}

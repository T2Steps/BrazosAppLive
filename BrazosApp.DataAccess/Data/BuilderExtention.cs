using BrazosApp.Models;
using BrazosApp.Utility;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrazosApp.DataAccess.Data
{
    public static class BuilderExtention
    {
        public static void Seed(this ModelBuilder builder)
        {
            builder.Entity<Role>().HasData(
                new List<Role>()
                {
                    new Role(){ Id = 1, Name = SD.SuperAdmin, IsActive = true },
                    new Role(){ Id = 2, Name = SD.AdminInspector, IsActive = true },
                    new Role(){ Id = 3, Name = SD.Admin, IsActive = true },
                    new Role(){ Id = 4, Name = SD.Inspector, IsActive = true },
                    new Role(){ Id = 5, Name = SD.Clerk, IsActive = true },
                    new Role(){ Id = 6, Name = SD.ViewOnly, IsActive = true }
                });

            builder.Entity<TerritoryAssignedType>().HasData(
                new List<TerritoryAssignedType>()
                {
                    new(){ Id = 1, Name = SD.SystemAssigned},
                    new(){ Id = 2, Name = SD.DefaultAssigned},
                    new(){ Id = 3, Name = SD.NormalAssigned},
                });

            builder.Entity<LanguageType>().HasData(
                new List<LanguageType>()
                {
                    new(){ Id = 1, Name = "English", Code = "E", IsActive=true},
                    new(){ Id = 2, Name = "Spanish", Code = "SP", IsActive=true},
                });

            builder.Entity<ApplicationType>().HasData(
                new List<ApplicationType>()
                {
                    new(){Id = 1, Name = "Food", IsActive = true},
                    new(){Id = 2, Name = "Foster Home", IsActive = true},
                    new(){Id = 3, Name = "Daycare Sanitation", IsActive = true},
                    new(){Id = 4, Name = "Pools", IsActive = true},
                    new(){Id = 5, Name = "Complaints", IsActive = true},

                });


            builder.Entity<PermitStatus>().HasData(
                new List<PermitStatus>()
                {
                    new(){Id = 1, Name = "Incomplete", IsActive=true},
                    new(){Id = 2, Name = "Pending Admin Review", IsActive=true},
                    new(){Id = 3, Name = "Admin Review", IsActive=true},
                    new(){Id = 4, Name = "Pending Plan Review", IsActive=true},
                    new(){Id = 5, Name = "Plan Review", IsActive=true},
                    new(){Id = 6, Name = "Pending Build-Out", IsActive=true},
                    new(){Id = 7, Name = "Pending Payment", IsActive=true},
                    new(){Id = 8, Name = "Opening Inspection", IsActive=true},
                    new(){Id = 9, Name = "Active", IsActive=true},
                    new(){Id = 10, Name = "Renewal", IsActive=true},
                    new(){Id = 11, Name = "Expired", IsActive=true},
                    new(){Id = 12, Name = "Area 51", IsActive=true},
                    new(){Id = 13, Name = "Inactive", IsActive=true},
                    new(){Id = 14, Name = "Closed", IsActive=true},
                    new(){Id = 15, Name = "Inspection", IsActive=true},
                    new(){Id = 16, Name = "Completed", IsActive=true},
                });

            builder.Entity<ApplicationFor>().HasData(
                new List<ApplicationFor>()
                {
                    new(){ Id=1, Name ="Retail Food New", LanguageTypeId = 1, ApplicationTypeId=1, Code="RF", Purpose="NewPermit", IsActive=true},
                    new(){ Id=2, Name ="Retail Food New (Spanish)", LanguageTypeId = 2,ApplicationTypeId=1,Code="RF",Purpose="NewPermit", IsActive=true},
                    new(){ Id=3, Name ="Mobile Food New", LanguageTypeId = 1,ApplicationTypeId=1,Code="MF",Purpose="NewPermit", IsActive=true},
                    new(){ Id=4, Name ="Mobile Food New (Spanish)", LanguageTypeId = 2,ApplicationTypeId=1,Code="MF",Purpose="NewPermit", IsActive=true},

                    new(){ Id=5, Name ="Retail Food Change of Owner", LanguageTypeId = 1,ApplicationTypeId=1,Code="RF",Purpose="OwnerChange", IsActive=true},
                    new(){ Id=6, Name ="Retail Food Change of Owner (Spanish)", LanguageTypeId = 2,ApplicationTypeId=1,Code="RF",Purpose="OwnerChange", IsActive=true},
                    new(){ Id=7, Name ="Mobile Food Change of Owner", LanguageTypeId = 1,ApplicationTypeId=1,Code="MF",Purpose="OwnerChange", IsActive=true},
                    new(){ Id=8, Name ="Mobile Food Change of Owner (Spanish)", LanguageTypeId = 2,ApplicationTypeId=1,Code="MF",Purpose="OwnerChange", IsActive=true},

                    new(){ Id=9, Name ="Temporary Food", LanguageTypeId = 1,ApplicationTypeId=1,Code="TF",Purpose="NewPermit", IsActive=true},
                    new(){ Id=10, Name ="Temporary Food (Spanish)", LanguageTypeId = 2,ApplicationTypeId=1,Code="TF",Purpose="NewPermit", IsActive=true},
                    new(){ Id=11, Name ="Food Handler Enrollment Application", LanguageTypeId = 1,ApplicationTypeId=1,Code="FHEA",Purpose="NewPermit", IsActive=true},
                    new(){ Id=12, Name ="Food Handler Enrollment Application (Spanish)", LanguageTypeId = 2,ApplicationTypeId=1,Code="FHEA",Purpose="NewPermit", IsActive=true},
                    new(){ Id=13, Name ="Foster Home", LanguageTypeId = 1,ApplicationTypeId=2,Code="FH",Purpose="NewPermit", IsActive=true},
                    new(){ Id=14, Name ="Daycare Sanitation", LanguageTypeId = 1,ApplicationTypeId=3,Code="DS",Purpose="NewPermit", IsActive=true},
                    new(){ Id=15, Name ="Pools", LanguageTypeId = 1,ApplicationTypeId=4,Code="PL",Purpose="NewPermit", IsActive=true},
                    new(){ Id=16, Name ="Complaints", LanguageTypeId = 1,ApplicationTypeId=5,Code="C",Purpose="Complaint", IsActive=true},
                });

                  builder.Entity<OperationType>().HasData(
                        new List<OperationType>()
                        {
                              new(){ Id = 1, Name = "Annually (12 months)", SpName = "Anual (12 meses)", Code="RF", IsActive= true},
                              new(){ Id = 2, Name = "Seasonal (4 months)", SpName = "Temporal (4 meses)", Code="RF", IsActive= true}
                        });

                  builder.Entity<BusinessType>().HasData(
                        new List<BusinessType>()
                        {
                              new(){ Id = 1, Name = "Restaurant/food production", SpName = "Restaurante/producción de comida", Code = "RF", IsActive = true},
                              new(){ Id = 2, Name = "Establishment with TCS food production", SpName = "Establecimiento con preparación de alimentos o bebidas TCS", Code = "RF", IsActive = true},
                              new(){ Id = 3, Name = "Establishment without TCS food production", SpName = "Establecimiento sin preparación de alimentos o bebidas TCS", Code = "RF", IsActive = true},
                              new(){ Id = 4, Name = "Bar without TCS ingredients (beverages only)", SpName = "Bar con ingredientes TCS o alimentos", Code = "RF", IsActive = true},
                              new(){ Id = 5, Name = "Bar with TCS ingredients and/or food", SpName = "Bar sin ingredientes TCS", Code = "RF", IsActive = true},
                              new(){ Id = 6, Name = "Retail with TCS pre-packaged products (no preparation)", SpName = "Venta con productos TCS preenvasados (sin preparación)", Code = "RF", IsActive = true},
                              new(){ Id = 7, Name = "Grocery Store", SpName = "Tienda de comida", Code = "RF", IsActive = true},
                              new(){ Id = 8, Name = "Long term care facility", SpName = "Centro de cuidados a largo plazo", Code = "RF", IsActive = true},
                              new(){ Id = 9, Name = "Hospital", SpName = "Hospital", Code = "RF", IsActive = true},
                              new(){ Id = 10, Name = "Farmer’s Market", SpName = "Mercado de agricultores", Code = "RF", IsActive = true},
                              new(){ Id = 11, Name = "School", SpName = "Escuela", Code = "RF", IsActive = true},
                              new(){ Id = 12, Name = "Daycare", SpName = "Guardería de niños", Code = "RF", IsActive = true},
                              new(){ Id = 13, Name = "Central Preparation Facility", SpName = "Instalación central de preparación", Code = "RF", IsActive = true},
                              new(){ Id = 14, Name = "Non-profit 501C (must include proof)", SpName = "Sin fines de lucro 501C (debe incluir prueba)", Code = "RF", IsActive = true},
                              new(){ Id = 15, Name = "Food Truck with TCS foods", SpName="Camión de comida con alimentos TCS", Code = "MF", IsActive = true},
                              new(){ Id = 16, Name = "Food Truck without TCS foods", SpName="Camión de comida sin alimentos TCS", Code = "MF", IsActive = true},
                              new(){ Id = 17, Name = "Food Pushcart with TCS foods", SpName="Carrito de comida con alimentos TCS", Code = "MF", IsActive = true},
                              new(){ Id = 18, Name = "Food Pushcart without TCS foods", SpName="Carrito de comida sin alimentos TCS", Code = "MF", IsActive = true},
                              new(){ Id = 19, Name = "Roadside Vendor with TCS foods", SpName="Vendedor ambulante con alimentos TCS", Code = "MF", IsActive = true},
                              new(){ Id = 20, Name = "Roadside Vendor without TCS foods", SpName="Vendedor ambúlate sin alimentos TCS", Code = "MF", IsActive = true},
                              new(){ Id = 21, Name = "Migrate", SpName="Emigrar", Code = "RF", IsActive = true},
                        });

                //builder.Entity<JurisdictionAccounts>().HasData( 
                //    new List<JurisdictionAccounts>()
                //    {
                //        new(){ Id = 1, Name = "City of Bryan", IsActive=true},
                //        new(){ Id = 2, Name = "City of College Station", IsActive=true},
                //        new(){ Id = 3, Name = "Brazos County", IsActive=true},
                //    });

                builder.Entity<EstablishmentSize>().HasData(
                    new List<EstablishmentSize>()
                    {
                            new(){ Id = 1, Name = "Small", IsActive=true},
                            new(){ Id = 2, Name = "Medium", IsActive=true},
                            new(){ Id = 3, Name = "Large", IsActive=true},
                    });

            builder.Entity<FeePrograms>().HasData(
                    new List<FeePrograms>()
                    {
                            new(){ Id = 1, Name = "Retail Food", IsActive=true},
                            new(){ Id = 2, Name = "Mobile Food", IsActive=true},
                            new(){ Id = 3, Name = "Temporary Food", IsActive=true},
                            new(){ Id = 4, Name = "Pools", IsActive=true},
                            new(){ Id = 5, Name = "Foster Care", IsActive=true},
                            new(){ Id = 6, Name = "Daycare", IsActive=true},
                            new(){ Id = 7, Name = "OSSF", IsActive=true},
                            new(){ Id = 8, Name = "FHC & L", IsActive=true},
                            new(){ Id = 9, Name = "Clinic - C4", IsActive=true},
                            new(){ Id = 10, Name = "Clinic - CHS", IsActive=true},
                            new(){ Id = 11, Name = "General", IsActive=true},
                            new(){ Id = 12, Name = "Other Service", IsActive=true},
                    });

            builder.Entity<RiskCategory>().HasData(
                new List<RiskCategory>()
                {
                    new(){ Id=1, Name="Low", Description="Low (1 Routine Inspection in 12 Months From Activation Date", Code="L" },
                    new(){ Id=2, Name="Medium", Description="Low (1 Routine Inspection in 6 Months From Activation Date", Code="M" },
                    new(){ Id=3, Name="High", Description="Low (1 Routine Inspection in 4 Months From Activation Date", Code="H" }
                });

            builder.Entity<InspectionPurposes>().HasData(new List<InspectionPurposes>()
            {
                new(){ Id=1, Name="Opening Inspection", Code = "COM", IsActive=true},
                new(){ Id=2, Name="Compliance", Code = "RF",IsActive=true},
                new(){ Id=3, Name="Routine", Code = "RF",IsActive=true},
                new(){ Id=4, Name="Field Investigation", Code = "RF", IsActive=true},
                new(){ Id=5, Name="Visit",Code = "RF", IsActive=true},
                new(){ Id=6, Name="Other",Code = "RF", IsActive=true},
                new(){ Id=7, Name="Walk Through",Code = "COM", IsActive=true},
            });


            builder.Entity<ScheduleStatus>().HasData(new List<ScheduleStatus>()
            {
                new(){ Id=1, Name="Unassigned", IsActive=true},
                new(){ Id=2, Name="Assigned", IsActive=true},
                new(){ Id=3, Name="Skipped", IsActive=true},
                new(){ Id=4, Name="Inspected", IsActive=true},
                new(){ Id=5, Name="Deleted", IsActive=true},
                new(){ Id=6, Name="Inactive", IsActive=true},
            });

            builder.Entity<InspectionItemCode>().HasData(new List<InspectionItemCode>()
            {
                new(){ Id=1, Name="RFO"},
                new(){ Id=2, Name="MFO"},
            });

            builder.Entity<WaterSource>().HasData(
                    new List<WaterSource>()
                    {
                            new(){ Id = 1, Name = "City of Bryan", IsActive=true},
                            new(){ Id = 2, Name = "City of College Station", IsActive=true},
                            new(){ Id = 3, Name = "Wellborn SUD", IsActive=true},
                            new(){ Id = 4, Name = "Wickson Creek SUD", IsActive=true},
                            new(){ Id = 5, Name = "Undine Texas LLC", IsActive=true},
                            new(){ Id = 6, Name = "Texas A&M University Main Campus", IsActive=true},
                            new(){ Id = 7, Name = "Benchley Oaks Subdivision", IsActive=true},
                            new(){ Id = 8, Name = "Sanderson Farms Bryan Facility", IsActive=true},
                            new(){ Id = 9, Name = "Ramblewood Mobile Home Park", IsActive=true},
                            new(){ Id = 10, Name = "Wheelock Express", IsActive=true},
                            new(){ Id = 11, Name = "Carousel Mobile Home Park", IsActive=true},
                            new(){ Id = 12, Name = "Al Leonard Ranch", IsActive=true},
                            new(){ Id = 13, Name = "Lakewood Estates", IsActive=true},
                            new(){ Id = 14, Name = "Smetana Forest", IsActive=true},
                            new(){ Id = 15, Name = "Migrate", IsActive=true},
                    });

            builder.Entity<PublicSewage>().HasData(
                    new List<PublicSewage>()
                    {
                            new(){ Id = 1, Name = "City of Bryan", IsActive=true},
                            new(){ Id = 2, Name = "City of College Station", IsActive=true},
                            new(){ Id = 3, Name = "Meadow Creek Sewer Company", IsActive=true},
                            new(){ Id = 4, Name = "NI America Texas Development", IsActive=true},
                            new(){ Id = 5, Name = "Migrate", IsActive=true},
                    });

            builder.Entity<PrivateSeptic>().HasData(
                    new List<PrivateSeptic>()
                    {
                            new(){ Id = 1, Name = "OSSF", IsActive=true},
                    });

            builder.Entity<Area>().HasData(
                  new List<Area>()
                  {
                        new(){Id = 1, AreaNumber = 0, Description = "Unassigned - new establishments", IsActive= true},
                        new(){Id = 2, AreaNumber = 1, Description = "Assigned to inspector", IsActive= true},
                        new(){Id = 3, AreaNumber = 2, Description = "Assigned to inspector", IsActive= true},
                        new(){Id = 4, AreaNumber = 3, Description = "Assigned to inspector", IsActive= true},
                        new(){Id = 5, AreaNumber = 4, Description = "Assigned to inspector", IsActive= true},
                        new(){Id = 6, AreaNumber = 5, Description = "Assigned to inspector", IsActive= true},
                        new(){Id = 7, AreaNumber = 6, Description = "Assigned to inspector", IsActive= true},
                        new(){Id = 8, AreaNumber = 7, Description = "Assigned to inspector", IsActive= true},
                        new(){Id = 9, AreaNumber = 8, Description = "Currently food establishments on OSSF - will be fed into inspector assigned areas prior to migration", IsActive= true},
                        new(){Id = 10, AreaNumber = 12, Description = "Texas A&M owned permits", IsActive= true},
                        new(){Id = 11, AreaNumber = 51, Description = "Establishments not in current operation", IsActive= true},
                        
                  });

            //builder.Entity<ApplyFor>().HasData(
            //    new List<ApplyFor>()
            //    {
            //        new ApplyFor(){ Id = 1, Name = "Retail Food", IsActive = true, CreatedOn = DateTime.Now },
            //        new ApplyFor(){ Id = 2, Name = "Mobile Food", IsActive = true, CreatedOn = DateTime.Now },
            //        new ApplyFor(){ Id = 3, Name = "Temporary Food", IsActive = true, CreatedOn = DateTime.Now },
            //        new ApplyFor(){ Id = 4, Name = "Events", IsActive = true, CreatedOn = DateTime.Now },
            //        new ApplyFor(){ Id = 5, Name = "Farmers Market", IsActive = true, CreatedOn = DateTime.Now },
            //        new ApplyFor(){ Id = 6, Name = "Childcare", IsActive = true, CreatedOn = DateTime.Now }
            //    });
        }
    }
}

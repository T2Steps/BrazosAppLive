using BrazosApp.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace BrazosApp.DataAccess.Data
{
    public static class EntityDecoration
    {
        public static void EntityDecorate(this ModelBuilder builder)
        {
            //var Employee = builder.Entity<Employee>();

            //Employee.ToTable("Test"); //Rename The Database Name
            //Employee.Property(b => b.Name).IsRequired(false); //Is Required or Not Required
            //Employee.HasKey(b => b.Id); //Setting Unique Key
            ////Employee.HasKey(b=> new {b.Id, b.Name}); //Setting Multiple Unique Key
            //Employee.Ignore(b => b.EncryptedId); //Setting Not Mapped Property
            ////Employee.HasOne(b=>b.Establishment).WithMany(a=>a.Employee).HasForeignKey("EstablishmentId").IsRequired(); //Foreign Key
            //Employee.Property(b => b.JoiningDate).HasColumnName("DateOfJoining");//Change Column Name
            //Employee.Property(b => b.Name).HasColumnType("varchar(20)"); //Sets Length
            //Employee.Property(b => b.Name).HasMaxLength(30);
            //Employee.Property(b => b.Salary).HasColumnType("decimal(18,2)");

            builder.Entity<Events>(e =>
            {
                e.HasKey(b => b.Id);
                e.Property(b => b.Name).IsRequired(false).HasMaxLength(50);
                e.Property(b => b.Location).IsRequired(false).HasMaxLength(150);
                e.Property(b => b.IsActive).HasDefaultValue(true);
                e.Property(b => b.StartDate)/*.IsRequired(false)*/.HasColumnType("datetime2");
                e.Property(b => b.EndDate)/*.IsRequired(false)*/.HasColumnType("datetime2");
                e.Property(b => b.EncryptedId).IsRequired(false);
                e.Ignore(b => b.EncryptedId);
                //e.Property(b => b.Details).IsRequired(false);
                //e.Ignore(b => b.Details);
            });


            //builder.Entity<TFOperationDetails>(e =>
            //{
            //    e.HasKey(b => b.Id);
            //    e.HasOne(b => b.Establishment).WithOne(a => a.Details).HasForeignKey("EstablishmentId").IsRequired();
            //    e.HasOne(b => b.Event).WithOne(a => a.Details).HasForeignKey("EventId").IsRequired();
            //    e.Property(b => b.ListOfFoodToBePrepared).IsRequired(false);
            //    e.Property(b => b.PreparingTime).IsRequired(false);
            //    e.Property(b => b.ServingTime).IsRequired(false);
            //    e.Property(b => b.IsActive).HasDefaultValue(true);
            //    e.Property(b => b.CreatedOn).IsRequired(false).HasDefaultValue(DateTime.Now);
            //    e.Property(b => b.UpdatedOn).IsRequired(false);
            //});

            //var events = builder.Entity<Events>();
            //events.HasKey(b => b.Id);
            //events.Property(b=>b.Name).IsRequired();
            //events.Property(b => b.Name).HasMaxLength(50);
            //events.Property(b=>b.IsActive).HasDefaultValue(true);
            //events.Property(b => b.StartDate).HasColumnType("date");
            //events.Property(b => b.EndDate).HasColumnType("date");
            //events.Ignore(b => b.EncryptedId);

            builder.Entity<TerritoryWiseInspectors>()
            .HasOne<Territory>(i => i.Territory)
            .WithMany(t => t.Inspectors)
            .HasForeignKey(i => i.TerritoryId)
            .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<AreaWiseInspectors>()
            .HasOne<Area>(i => i.Area)
            .WithMany(t => t.Inspectors)
            .HasForeignKey(i => i.AreaId)
            .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Application>()
                .Property(p => p.Status).HasComment("1='Not Verified', 2='Verified', 3='Expired'");

            builder.Entity<Notes>()
                .Property(p => p.Status).HasComment("0='Not Edited', 1='Edited', 2='Deleted'");

            builder.Entity<Fees>()
                .Property(p => p.Status).HasComment("1 = Pending, 2 = Paid, 3 = Cancelled, 4 = Renewal Pending, 5= Late Renewal Pending, 6 = 30 days, 7 = 60 days, 8 = Delinquent, 9 = Voided, 10 = Refund Requested, 11 = Refunded");

            builder.Entity<FeesDetailsTable>()
                .Property(p => p.Status).HasComment("1 = Pending, 2 = Paid, 3 = Cancelled, 4 = Renewal Pending, 5= Late Renewal Pending, 6 = 30 days, 7 = 60 days, 8 = Delinquent, 9 = Voided, 10 = Refund Requested, 11 = Refunded");

            builder.Entity<PaymentOnlineInfo>()
                .Property(p => p.RedirectApiCallStatus).HasComment("0 = Cancelled, 1 = Declined, 2 = Approved");

            builder.Entity<Payment>()
                .Property(p => p.PaymentStatus).HasComment("1 = Pending, 2 = Paid, 3 = Cancelled, 4 = Failure, 5 = Refunded, 6 = Voided, 7 = Renewal");

            builder.Entity<PaymentDetailsTable>()
                .Property(p => p.PaymentStatus).HasComment("1 = Pending, 2 = Paid, 3 = Cancelled, 4 = Failure, 5 = Refunded, 6 = Voided, 7 = Renewal");

            builder.Entity<Payment>()
                .Property(p => p.PaymentType).HasComment("1 = Online, 2 = Offline");

            builder.Entity<Payment>()
                .Property(p => p.PaymentMethod).HasComment("0 = CreditCard, 1 = ECheck, 2 = Cash, 3 = Cheque, 4 = Card, 5 = Money Order");

            builder.Entity<PaymentSplit>()
                .Property(p => p.PaymentMethod).HasComment("0 = CreditCard, 1 = ECheck, 2 = Cash, 3 = Cheque, 4 = Card, 5 = Money Order");


            
        }
    }
}

using BrazosApp.DataAccess.Data;
using BrazosApp.Models;
using BrazosApp.Utility;
using BrazosApp.Utility.Helpers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BrazosApp.DataAccess.Initializer
{
    public class DbInitializer : IDbInitializer
    {
        private readonly ApplicationDbContext _db;
        //private readonly PoolDbContext _Pdb;
        private readonly IPasswordHasher _passwordHasher;

        public DbInitializer(ApplicationDbContext db, IPasswordHasher passwordHasher)
        {
            _db = db;
            //_Pdb = Pdb;
            _passwordHasher = passwordHasher;
        }

        public async Task Initialize()
        {
            try
            {
                if (_db.Database.GetPendingMigrations().Count() > 0)
                {
                    _db.Database.Migrate();
                }
                //if (_Pdb.Database.GetPendingMigrations().Count() > 0)
                //{
                //    _Pdb.Database.Migrate();
                //}
            }
            catch (Exception ex) { }

            var Role = _db.Roles.FirstOrDefault(x=>x.Name==SD.SuperAdmin);
            if (Role == null) 
            {

            }
            else
            {
                var user = _db.Users.FirstOrDefault(x => x.BHCD == "11213"); ;
                if (user == null)
                {
                    var password = _passwordHasher.HashPassword("superadmin1@123*");

                    user = new Users();
                    user.FirstName = "Holly";
                    user.LastName = "Ulbrich";
                    //user.EmailId = "hulbrich@brazoscountytx.gov";
                    user.EmailId = "superAdmin1@inspect2go44.com";
                    user.BHCD = "11213";
                    user.RegisteredSanitarian = "4384";
                    user.DesignatedRepresentative = "OS0029344";
                    //user.CertifiedPoolOperator = "";
                    user.Password = password.Hash;
                    user.Salt = password.Salt;
                    user.IsLoggedIn = false;
                    user.IsActive = true;
                    user.IsDelete = false;
                    user.CreatedBy = 1;
                    //user.UpdatedBy = 1;
                    //user.UpdatedOn = DateTime.Now;
                    user.CreatedOn = DateTime.Now;
                    user.RoleId = Role.Id;
                    user.LastSeenTime = DateTime.Now;
                    await _db.Users.AddAsync(user);
                    _db.SaveChanges();
                }

                user = null;

                user = _db.Users.FirstOrDefault(x => x.BHCD == "12420");
                if (user == null)
                {
                    var password = _passwordHasher.HashPassword("superadmin2@123*");

                    user = new Users();
                    user.FirstName = "Mayra";
                    user.LastName = "Orocio";
                    //user.EmailId = "morocio@brazoscountytx.gov";
                    user.EmailId = "superAdmin2@inspect2go44.com";
                    user.BHCD = "12420";
                    user.RegisteredSanitarian = "5021";
                    user.DesignatedRepresentative = "OS0033961";
                    user.CertifiedPoolOperator = "652596";
                    user.Password = password.Hash;
                    user.Salt = password.Salt;
                    user.IsLoggedIn = false;
                    user.IsActive = true;
                    user.IsDelete = false;
                    user.CreatedBy = 1;
                    //user.UpdatedBy = 1;
                    //user.UpdatedOn = DateTime.Now;
                    user.CreatedOn = DateTime.Now;
                    user.RoleId = Role.Id;
                    user.LastSeenTime = DateTime.Now;
                    await _db.Users.AddAsync(user);
                    _db.SaveChanges();
                }
            }


            

            //var ExistingRole = _db.Roles.FirstOrDefault(x => x.Name == SD.SuperAdmin);
            //if (ExistingRole == null)
            //{
            //    Role role = new Role();
            //    role.Name = SD.SuperAdmin;
            //    role.IsActive = true;
            //    _db.Roles.AddAsync(role);
            //    _db.SaveChanges();
            //}

            //if (_db.Users.FirstOrDefault(u => u.Email == "superadmin@brazos.com") == null)
            //{
            //    var hashsalt = _passwordHasher.HashPassword("Admin123*");

            //    User user = new User()
            //    {
            //        FirstName = "Super",
            //        LastName = "Admin",
            //        Email = "superadmin@brazos.com",
            //        Password = hashsalt.Hash,
            //        StoredSalt = hashsalt.Salt,
            //        RoleId = role.Id,
            //        IsActive = true,
            //        CreatedBy = 0,
            //        UpdatedBy = 0,
            //        CreatedOn = DateTime.UtcNow
            //    };

            //    _db.Users.Add(user);
            //    _db.SaveChanges();
            //}
        }
    }
}

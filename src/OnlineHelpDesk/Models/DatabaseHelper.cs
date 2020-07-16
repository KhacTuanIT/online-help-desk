using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using OnlineHelpDesk.Services;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineHelpDesk.Models
{
    public class DatabaseHelper
    {
        public static void InitializeRequiredData()
        {
            using (var context = ApplicationDbContext.Create())
            {
                var dbSeeder = new DatabaseSeeder(context);

                dbSeeder.SeedRolesAndAdmin();
                dbSeeder.SeedRequestType();
                dbSeeder.SeedStatusType();
            }
        }

        public static void SeedData()
        {
            using (var db = ApplicationDbContext.Create())
            {
                var dbSeeder = new DatabaseSeeder(db);

                dbSeeder.SeedUsers();
                dbSeeder.SeedFacility();
                dbSeeder.SeedEquipmentType();
                dbSeeder.SeedEquipment();
            }
        }
    }

    public class DatabaseSeeder
    {
        private ApplicationDbContext db;

        public DatabaseSeeder(ApplicationDbContext db) => this.db = db;

        public void SeedUsers()
        {
            using (var userService = new UserService(db))
            {
                userService.CreateUser(new ApplicationUser()
                {
                    UserName = "ST000001",
                    UserIdentityCode = "ST000001",
                    FullName = "Sample Student",
                    Email = "student@ohd.com"
                }, role: "Student");

                userService.CreateUser(new ApplicationUser()
                {
                    UserName = "AS000001",
                    UserIdentityCode = "AS000001",
                    FullName = "Sample Assignor",
                    Email = "assignor@ohd.com"
                }, role: "Assignor");

                userService.CreateUser(new ApplicationUser()
                {
                    UserName = "FH000001",
                    UserIdentityCode = "FH000001",
                    FullName = "Sample Facility Head",
                    Email = "f.head@ohd.com"
                }, role: "FacilityHead");
            }
        }

        public void SeedFacility()
        {
            if (db.Facilities.Any()) return;
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    db.Facilities.AddOrUpdate(x => x.Name,
                        new Facility { Name = "Classroom", CreatedAt = DateTime.Now },
                        new Facility { Name = "Lab-Assistants", CreatedAt = DateTime.Now },
                        new Facility { Name = "Hostels", CreatedAt = DateTime.Now },
                        new Facility { Name = "Canteen", CreatedAt = DateTime.Now },
                        new Facility { Name = "Gymnasium", CreatedAt = DateTime.Now },
                        new Facility { Name = "Computer Centre", CreatedAt = DateTime.Now },
                        new Facility { Name = "Faculty Club", CreatedAt = DateTime.Now },
                        new Facility { Name = "Others", CreatedAt = DateTime.Now }
                        );

                    db.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                }
            }
        }

        public void SeedEquipmentType()
        {
            if (db.EquipmentTypes.Any()) return;
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    List<string> equipmentTypes = new List<string>()
                        {
                            "Treadmill",
                            "Dumbbells",
                            "Exercise Bike",
                            "Elliptical",
                            "Kettelbell",
                            "Medicine Ball",
                            "Rowing Machine",
                            "Punching Bag",
                            "Bar",
                            "Trampoline",
                            "Jump Rope",
                            "Blackboard",
                            "Computer",
                            "Clock",
                            "Globe",
                            "File holder",
                            "Map",
                            "Chair",
                            "Desk",
                            "Laptop",
                            "Cellings lights",
                            "Projector",
                            "Wireless",
                            "Air-conditioner",
                            "Cellings fans",
                            "Lamp",
                            "Shower",
                            "Bed",
                            "Mirror",
                            "Door",
                            "Window",
                            "Toiletries",
                            "Monitor",
                            "Case",
                            "Motherboard",
                            "Memory",
                            "Mouse",
                            "Speaker",
                            "Keyboard",
                            "Cable",
                            "Microphone",
                            "Modem",
                            "Webcam",
                            "CD-ROM drive",
                            "CPU",
                            "Hard drive",
                            "USB drive",
                            "Water cooler",
                            "Refrigerator",
                            "Vacuum cleaner",
                            "Appliance plug",
                            "Evaporative cooler",
                            "Washing machine",
                            "Remote",
                            "Pipette",
                            "Test tube rack",
                            "Test tube",
                            "Erlenmeyer flask",
                            "Beaker",
                            "Bunsen burner",
                            "Barometer",
                            "Indicator",
                            "Stopwatch",
                            "Speedometer",
                            "Protractor",
                            "Alcohol burner",
                            "Syringe",
                            "Graduated cylinder",
                            "Dropper",
                            "Tongs",
                            "Turing fork",
                            "Stergoscope",
                            "Magnet",
                            "Magnifiers",
                            "Telescope",
                            "Microscope",
                            "Thermometer",
                            "Friability tester",
                            "Pulleys",
                            "Tape",
                            "Dry-cell batery",
                            "Level",
                            "Balance scale",
                            "Chart",
                            "Dissecting set",
                            "Earth science",
                            "Printer",
                            "Scanner",
                            "Fax machine"
                        };

                    equipmentTypes.ForEach(typename =>
                    {
                        db.EquipmentTypes.AddOrUpdate(x => x.TypeName,
                            new EquipmentType { TypeName = typename });
                    });

                    db.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                }
            }
        }

        public void SeedEquipment()
        {
            if (db.Equipments.Any()) return;
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    // Add all ET to each Facility
                    for (int i = 0; i < db.Facilities.Count(); i++)
                    {
                        for (int j = 0; j < db.EquipmentTypes.Count(); j++)
                        {
                            //context.Equipments.AddOrUpdate(e => new { e.FacilityId, e.ArtifactId },
                            db.Equipments.Add(
                                new Equipment
                                {
                                    FacilityId = i + 1,
                                    ArtifactId = j + 1
                                });
                        }
                    }

                    db.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                }
            }
        }


        #region Important data, called in InitializeRequiredData
        public void SeedRolesAndAdmin()
        {
            if (db.Roles.Any()) return;
            var roleStore = new RoleStore<IdentityRole>(db);
            var roleManager = new RoleManager<IdentityRole>(roleStore);
            var userStore = new UserStore<ApplicationUser>(db);
            var userManager = new UserManager<ApplicationUser>(userStore);

            // Add missing roles
            if (!roleManager.RoleExists("SuperAdmin"))
            {
                // Create new role: Super Admin
                roleManager.Create(new IdentityRole("SuperAdmin"));

                // Create default admin
                var newUser = new ApplicationUser()
                {
                    UserName = "admin",
                    FullName = "Super Admin",
                    Email = "admin@ohd.com",
                    CreatedAt = DateTime.UtcNow,
                    MustChangePassword = true
                };
                userManager.Create(newUser, "admin@123");
                userManager.SetLockoutEnabled(newUser.Id, false);
                userManager.AddToRole(newUser.Id, "SuperAdmin");
                // Note:
                // - Create new user with role SuperAdmin
                // - Default password ADM@123a
                // - He must to change password at first login
            }

            if (!roleManager.RoleExists("Student"))
            {
                roleManager.Create(new IdentityRole("Student"));
            }

            if (!roleManager.RoleExists("Assignor"))
            {
                roleManager.Create(new IdentityRole("Assignor"));
            }

            if (!roleManager.RoleExists("FacilityHead"))
            {
                roleManager.Create(new IdentityRole("FacilityHead"));
            }
        }

        public void SeedRequestType()
        {
            if (db.RequestTypes.Any()) return;
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    db.RequestTypes.AddOrUpdate(x => x.TypeName,
                        new RequestType { TypeName = "Q&A" },
                        new RequestType { TypeName = "Report broken equipment" },
                        new RequestType { TypeName = "Additional equipment required" }
                        );

                    db.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                }
            }
        }

        public void SeedStatusType()
        {
            if (db.StatusTypes.Any()) return;
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    db.StatusTypes.AddOrUpdate(x => x.TypeName,
                        new StatusType { TypeName = "Created" },
                        new StatusType { TypeName = "Assigned" },
                        new StatusType { TypeName = "Processing" },
                        new StatusType { TypeName = "Completed" },
                        new StatusType { TypeName = "Closed" }
                        );

                    db.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                }
            }
        }
        #endregion

    }

}
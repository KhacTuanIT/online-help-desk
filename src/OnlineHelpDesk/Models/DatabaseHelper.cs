using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace OnlineHelpDesk.Models
{
    public class DatabaseHelper
    {
        public static void InitializeIdentity()
        {
            using (var context = ApplicationDbContext.Create())
            {
                if (context.Users.Any()) return;

                var roleStore = new RoleStore<IdentityRole>(context);
                var roleManager = new RoleManager<IdentityRole>(roleStore);
                var userStore = new UserStore<ApplicationUser>(context);
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
        }

        public static void InitializeFacility()
        {
            using (var context = ApplicationDbContext.Create())
            {
                if (context.Facilities.Any()) return;
                var facility = context.Facilities;
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        List<string> facilities = new List<string>()
                        {
                            "Classroom",
                            "Lab-Assistants",
                            "Hostels",
                            "Canteen",
                            "Gymnasium",
                            "Computer Centre",
                            "Faculty Club",
                            "Others",
                        };
                        foreach (var fa in facilities)
                        {
                            facility.Add(new Facility()
                            {
                                Name = fa,
                                CreatedAt = DateTime.Now
                            });
                        }
                        
                        context.SaveChanges();
                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                    }
                }
            }
        }

        public static void InitializeStatusType()
        {
            using (var context = ApplicationDbContext.Create())
            {
                if (context.StatusTypes.Any()) return;
                var statusType = context.StatusTypes;
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        List<string> statusTypes = new List<string>()
                        {
                            "Created",
                            "Assigned",
                            "Processing",
                            "Completed",
                            "Closed"
                        };
                        foreach (var st in statusTypes)
                        {
                            statusType.Add(new StatusType()
                            {
                                TypeName = st
                            });
                        }
                        context.SaveChanges();
                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                    }
                }
            }
        }
        
        public static void InitializeEquipmentType()
        {
            using (var context = ApplicationDbContext.Create())
            {
                if (context.EquipmentTypes.Any()) return;
                var equipmentType = context.EquipmentTypes;
                using (var transaction = context.Database.BeginTransaction())
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
                        foreach (var et in equipmentTypes)
                        {
                            equipmentType.Add(new EquipmentType()
                            {
                                artifact_name = et
                            });
                        }
                        context.SaveChanges();
                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                    }
                }
            }
        }

        public static void InitializeRequestType()
        {
            using (var context = ApplicationDbContext.Create())
            {
                if (context.RequestTypes.Any()) return;
                var requestType = context.RequestTypes;
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        List<string> requestTypes = new List<string>()
                        {
                            "Q&A",
                            "Report broken equipment",
                            "Additional equipment required"
                        };
                        foreach (var rt in requestTypes)
                        {
                            requestType.Add(new RequestType()
                            {
                                TypeName = rt
                            });
                        }
                        context.SaveChanges();
                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                    }
                }
            }
        }

        public static void InitializeEquipment()
        {
            using (var context = ApplicationDbContext.Create())
            {
                if (context.Equipments.Any()) return;
                var equipment = context.Equipments;
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        #region equipments datasample
                        List<MediateEquipmentViewModel> equipments = new List<MediateEquipmentViewModel>()
                        {
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 1,
                                EquipmentTypeId = 12
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 1,
                                EquipmentTypeId = 13
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 1,
                                EquipmentTypeId = 14
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 1,
                                EquipmentTypeId = 15
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 1,
                                EquipmentTypeId = 16
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 1,
                                EquipmentTypeId = 17
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 1,
                                EquipmentTypeId = 18
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 1,
                                EquipmentTypeId = 19
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 1,
                                EquipmentTypeId = 20
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 1,
                                EquipmentTypeId = 21
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 1,
                                EquipmentTypeId = 22
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 1,
                                EquipmentTypeId = 23
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 1,
                                EquipmentTypeId = 24
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 1,
                                EquipmentTypeId = 25
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 1,
                                EquipmentTypeId = 54
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 2,
                                EquipmentTypeId = 12
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 2,
                                EquipmentTypeId = 13
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 2,
                                EquipmentTypeId = 14
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 2,
                                EquipmentTypeId = 17
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 2,
                                EquipmentTypeId = 18
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 2,
                                EquipmentTypeId = 19
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 2,
                                EquipmentTypeId = 21
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 2,
                                EquipmentTypeId = 22
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 2,
                                EquipmentTypeId = 23
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 2,
                                EquipmentTypeId = 24
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 2,
                                EquipmentTypeId = 25
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 2,
                                EquipmentTypeId = 55
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 2,
                                EquipmentTypeId = 56
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 2,
                                EquipmentTypeId = 57
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 2,
                                EquipmentTypeId = 58
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 2,
                                EquipmentTypeId = 59
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 2,
                                EquipmentTypeId = 60
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 2,
                                EquipmentTypeId = 61
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 2,
                                EquipmentTypeId = 62
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 2,
                                EquipmentTypeId = 63
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 2,
                                EquipmentTypeId = 64
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 2,
                                EquipmentTypeId = 65
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 2,
                                EquipmentTypeId = 66
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 2,
                                EquipmentTypeId = 67
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 2,
                                EquipmentTypeId = 68
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 2,
                                EquipmentTypeId = 69
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 2,
                                EquipmentTypeId = 70
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 2,
                                EquipmentTypeId = 71
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 2,
                                EquipmentTypeId = 72
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 2,
                                EquipmentTypeId = 73
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 2,
                                EquipmentTypeId = 74
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 2,
                                EquipmentTypeId = 75
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 2,
                                EquipmentTypeId = 76
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 2,
                                EquipmentTypeId = 77
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 2,
                                EquipmentTypeId = 78
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 2,
                                EquipmentTypeId = 79
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 2,
                                EquipmentTypeId = 80
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 2,
                                EquipmentTypeId = 81
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 2,
                                EquipmentTypeId = 82
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 2,
                                EquipmentTypeId = 83
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 2,
                                EquipmentTypeId = 84
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 2,
                                EquipmentTypeId = 85
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 2,
                                EquipmentTypeId = 86
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 3,
                                EquipmentTypeId = 21
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 3,
                                EquipmentTypeId = 23
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 3,
                                EquipmentTypeId = 24
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 3,
                                EquipmentTypeId = 25
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 3,
                                EquipmentTypeId = 26
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 3,
                                EquipmentTypeId = 27
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 3,
                                EquipmentTypeId = 28
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 3,
                                EquipmentTypeId = 29
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 3,
                                EquipmentTypeId = 30
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 3,
                                EquipmentTypeId = 31
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 3,
                                EquipmentTypeId = 32
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 4,
                                EquipmentTypeId = 18
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 4,
                                EquipmentTypeId = 19
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 4,
                                EquipmentTypeId = 21
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 4,
                                EquipmentTypeId = 23
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 4,
                                EquipmentTypeId = 24
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 4,
                                EquipmentTypeId = 25
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 4,
                                EquipmentTypeId = 48
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 4,
                                EquipmentTypeId = 49
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 4,
                                EquipmentTypeId = 50
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 4,
                                EquipmentTypeId = 51
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 4,
                                EquipmentTypeId = 52
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 4,
                                EquipmentTypeId = 53
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 5,
                                EquipmentTypeId = 1
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 5,
                                EquipmentTypeId = 2
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 5,
                                EquipmentTypeId = 3
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 5,
                                EquipmentTypeId = 4
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 5,
                                EquipmentTypeId = 5
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 5,
                                EquipmentTypeId = 6
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 5,
                                EquipmentTypeId = 7
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 5,
                                EquipmentTypeId = 8
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 5,
                                EquipmentTypeId = 9
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 5,
                                EquipmentTypeId = 10
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 5,
                                EquipmentTypeId = 11
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 5,
                                EquipmentTypeId = 21
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 5,
                                EquipmentTypeId = 23
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 5,
                                EquipmentTypeId = 24
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 5,
                                EquipmentTypeId = 25
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 6,
                                EquipmentTypeId = 18
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 6,
                                EquipmentTypeId = 19
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 6,
                                EquipmentTypeId = 20
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 6,
                                EquipmentTypeId = 21
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 6,
                                EquipmentTypeId = 22
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 6,
                                EquipmentTypeId = 23
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 6,
                                EquipmentTypeId = 24
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 6,
                                EquipmentTypeId = 25
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 6,
                                EquipmentTypeId = 33
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 6,
                                EquipmentTypeId = 34
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 6,
                                EquipmentTypeId = 35
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 6,
                                EquipmentTypeId = 36
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 6,
                                EquipmentTypeId = 37
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 6,
                                EquipmentTypeId = 38
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 6,
                                EquipmentTypeId = 39
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 6,
                                EquipmentTypeId = 40
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 6,
                                EquipmentTypeId = 41
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 6,
                                EquipmentTypeId = 42
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 6,
                                EquipmentTypeId = 43
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 6,
                                EquipmentTypeId = 44
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 6,
                                EquipmentTypeId = 45
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 6,
                                EquipmentTypeId = 46
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 6,
                                EquipmentTypeId = 47
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 7,
                                EquipmentTypeId = 13
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 7,
                                EquipmentTypeId = 14
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 7,
                                EquipmentTypeId = 16
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 7,
                                EquipmentTypeId = 18
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 7,
                                EquipmentTypeId = 19
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 7,
                                EquipmentTypeId = 20
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 7,
                                EquipmentTypeId = 21
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 7,
                                EquipmentTypeId = 22
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 7,
                                EquipmentTypeId = 23
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 7,
                                EquipmentTypeId = 24
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 7,
                                EquipmentTypeId = 25
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 8,
                                EquipmentTypeId = 1
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 8,
                                EquipmentTypeId = 2
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 8,
                                EquipmentTypeId = 3
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 8,
                                EquipmentTypeId = 4
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 8,
                                EquipmentTypeId = 5
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 8,
                                EquipmentTypeId = 6
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 8,
                                EquipmentTypeId = 7
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 8,
                                EquipmentTypeId = 8
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 8,
                                EquipmentTypeId = 9
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 8,
                                EquipmentTypeId = 10
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 8,
                                EquipmentTypeId = 11
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 8,
                                EquipmentTypeId = 12
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 8,
                                EquipmentTypeId = 13
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 8,
                                EquipmentTypeId = 14
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 8,
                                EquipmentTypeId = 15
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 8,
                                EquipmentTypeId = 16
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 8,
                                EquipmentTypeId = 17
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 8,
                                EquipmentTypeId = 18
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 8,
                                EquipmentTypeId = 19
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 8,
                                EquipmentTypeId = 20
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 8,
                                EquipmentTypeId = 21
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 8,
                                EquipmentTypeId = 22
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 8,
                                EquipmentTypeId = 23
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 8,
                                EquipmentTypeId = 24
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 8,
                                EquipmentTypeId = 25
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 8,
                                EquipmentTypeId = 26
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 8,
                                EquipmentTypeId = 27
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 8,
                                EquipmentTypeId = 28
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 8,
                                EquipmentTypeId = 29
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 8,
                                EquipmentTypeId = 30
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 8,
                                EquipmentTypeId = 31
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 8,
                                EquipmentTypeId = 32
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 8,
                                EquipmentTypeId = 33
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 8,
                                EquipmentTypeId = 34
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 8,
                                EquipmentTypeId = 35
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 8,
                                EquipmentTypeId = 36
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 8,
                                EquipmentTypeId = 37
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 8,
                                EquipmentTypeId = 38
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 8,
                                EquipmentTypeId = 39
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 8,
                                EquipmentTypeId = 40
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 8,
                                EquipmentTypeId = 41
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 8,
                                EquipmentTypeId = 42
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 8,
                                EquipmentTypeId = 43
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 8,
                                EquipmentTypeId = 44
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 8,
                                EquipmentTypeId = 45
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 8,
                                EquipmentTypeId = 46
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 8,
                                EquipmentTypeId = 47
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 8,
                                EquipmentTypeId = 48
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 8,
                                EquipmentTypeId = 49
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 8,
                                EquipmentTypeId = 50
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 8,
                                EquipmentTypeId = 51
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 8,
                                EquipmentTypeId = 52
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 8,
                                EquipmentTypeId = 53
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 8,
                                EquipmentTypeId = 54
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 8,
                                EquipmentTypeId = 55
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 8,
                                EquipmentTypeId = 56
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 8,
                                EquipmentTypeId = 57
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 8,
                                EquipmentTypeId = 58
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 8,
                                EquipmentTypeId = 59
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 8,
                                EquipmentTypeId = 60
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 8,
                                EquipmentTypeId = 61
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 8,
                                EquipmentTypeId = 62
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 8,
                                EquipmentTypeId = 63
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 8,
                                EquipmentTypeId = 64
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 8,
                                EquipmentTypeId = 65
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 8,
                                EquipmentTypeId = 66
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 8,
                                EquipmentTypeId = 67
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 8,
                                EquipmentTypeId = 68
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 8,
                                EquipmentTypeId = 69
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 8,
                                EquipmentTypeId = 70
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 8,
                                EquipmentTypeId = 71
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 8,
                                EquipmentTypeId = 72
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 8,
                                EquipmentTypeId = 73
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 8,
                                EquipmentTypeId = 74
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 8,
                                EquipmentTypeId = 75
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 8,
                                EquipmentTypeId = 76
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 8,
                                EquipmentTypeId = 77
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 8,
                                EquipmentTypeId = 78
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 8,
                                EquipmentTypeId = 79
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 8,
                                EquipmentTypeId = 80
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 8,
                                EquipmentTypeId = 81
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 8,
                                EquipmentTypeId = 82
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 8,
                                EquipmentTypeId = 83
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 8,
                                EquipmentTypeId = 84
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 8,
                                EquipmentTypeId = 85
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 8,
                                EquipmentTypeId = 86
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 8,
                                EquipmentTypeId = 87
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 8,
                                EquipmentTypeId = 88
                            },
                            new MediateEquipmentViewModel()
                            {
                                FacilityId = 8,
                                EquipmentTypeId = 89
                            }
                        };
                        #endregion
                        foreach (var eq in equipments)
                        {
                            equipment.Add(new Equipment()
                            {
                                FacilityId = eq.FacilityId,
                                ArtifactId = eq.EquipmentTypeId
                            });
                        }
                        context.SaveChanges();
                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                    }
                }
            }
        }
    }
}
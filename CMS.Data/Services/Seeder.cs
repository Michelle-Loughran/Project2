
using System.Data;
using System.Diagnostics.Metrics;
using CMS.Data.Entities;
using CMS.Data.Security;

namespace CMS.Data.Services
{
    public static class Seeder
    {
        // use this class to seed the database with dummy test data using an IUserService 
        public static void Seed( IPatientService svc)
        {
            // seeder destroys and recreates the database - NOT to be called in production!!!
            svc.Initialise();

            // add admin users
            var admin = svc.AddUser( 
                new User {
                    Firstname = "The",
                    Surname = "Admin",                 
                    Password = "password",
                    Email = "admin@mail.com",
                    Role = Role.admin
                }
            );

            var manager = svc.AddUser( 
                new User {
                    Firstname = "The",
                    Surname = "Manager",
                    Email = "manager@mail.com",
                    Password = "password",                   
                    Role = Role.manager
                }
            );

    
            // ===================  add carers ==================

            var c1 = svc.AddCarer(new User
            {
                Title = "Mr",
                Firstname = "Carer",
                Surname = "One",
                DOB = new DateTime(1977, 05, 28),
                NationalInsuranceNo = "NR456123D",
                DBSCheck = true,
                Email = "carer1@mail.com",
                Password = "password",
                Street = "34 Warren Hill",
                Town = "Newry",
                County = "Down",
                Postcode = "BT34 2PH",
                MobileNumber = "01234567891",
                HomeNumber = "02830303030",
                Qualifications = "8 GCSE's, Maths, A, English B, Social Care A, Accounts C,Economics C Art B, RE C, PSE C",
                PhotoUrl = "/images/Carer1.jpg"
                 
            });

            var c2 = svc.AddCarer(new User
            {
                Title = "Mrs",
                Firstname = "Mary",
                Surname = "Hegarty",
                DOB = new DateTime(1977, 06, 28),
                NationalInsuranceNo = "NR561234D",
                DBSCheck = true,
                Email = "carer2@mail.com",
                Password = "password",
                Street = "34 Warren Hill",
                Town = "Newry",
                County = "Down",
                Postcode = "BT34 2PH",
                MobileNumber = "01234567891",
                HomeNumber = "02830303030",
                Qualifications = "8 GCSE's, Maths, A, English B, Social Care A, Accounts C,Economics C Art B, RE C, PSE C",
                PhotoUrl = "/images/Carer2.jpg",
                Role= Role.carer
               
            });

            var c3 = svc.AddCarer(new User
            {
                Title = "Mrs",
                Firstname = "Mary",
                Surname = "Hasselhoff",
                DOB = new DateTime(1965, 06, 30),
                NationalInsuranceNo = "NR612345D",
                DBSCheck = true,
                Email = "carer3H@mail.com",
                Password = "password",
                Street = "35 Warren Hill",
                Town = "Newry",
                County = "Down",
                Postcode = "BT34 2PH",
                MobileNumber = "01234567891",
                HomeNumber = "02830303030",
                Qualifications = "8 GCSE's, Maths, A, English B, Social Care A, Accounts C,Economics C Art B, RE C, PSE C",
                PhotoUrl = "/images/Carer2.jpg",             
            });



            // =====    add patient data  ==============
            var p1 = svc.AddPatient(new Patient
            {
                Title = "Mr",
                Firstname = "Joe",
                Surname = "Bloggs",
                NationalInsuranceNo = "NR12345C",
                DOB = new DateTime(1945, 03, 03),
                Street = "24 Warren Hill",
                Town = "Newry",
                County = "Down",
                Postcode = "BT34 2PH",
                PhotoUrl = "/images/1.jpg",
                MobileNumber = "01234567891",
                HomeNumber = "02830303030",
                Email = "joe@mail.com",
                GP = "Dr A Blagg",
                SocialWorker = " Dr Minnie Mouse",
                CarePlan = "See File for details"

                //  ....
            });

            var p2 = svc.AddPatient(new Patient
            {
                Title = "Mrs",
                Firstname = "Mary",
                Surname = "Bloggs",
                NationalInsuranceNo = "NR23456C",
                DOB = new DateTime(1946, 02, 04),
                Street = "25 Warren Hill",
                Town = "Newry",
                County = "Down",
                Postcode = "BT34 2PH",
                PhotoUrl = "/images/2.jpg",
                MobileNumber = "01234567892",
                HomeNumber = "02830303031",
                Email = "Mary@mail.com",
                GP = "Dr A Blagg",
                SocialWorker = " Dr Minnie Mouse",
                CarePlan = "See File for details"
                // ....
            });


            var p3 = svc.AddPatient(new Patient
            {
                Title = "Mr",
                Firstname = "Tom",
                Surname = "Bloggs",
                NationalInsuranceNo = "NR34567C",
                DOB = new DateTime(1947, 05, 28),
                Street = "26 Warren Hill",
                Town = "Newry",
                County = "Down",
                Postcode = "BT34 2PH",
                PhotoUrl = "/images/3.jpg",
                MobileNumber = "01234567892",
                HomeNumber = "02830303031",
                Email = "Tom@mail.com",
                GP = "Dr A Blagg",
                SocialWorker = " Dr Minnie Mouse",
                CarePlan = "See File for details"
                // ....
            });


            var p4 = svc.AddPatient(new Patient
            {
                Title = "Mr",
                Firstname = "Donald",
                Surname = "Bloggs",
                NationalInsuranceNo = "NR45678C",
                DOB = new DateTime(1948, 05, 28),
                Street = "27 Warren Hill",
                Town = "Newry",
                County = "Down",
                Postcode = "BT34 2PH",
                PhotoUrl = "/images/4.jpg",
                MobileNumber = "01234567891",
                HomeNumber = "02830303030",
                Email = "Donald@mail.com",
                GP = "Dr A Blagg",
                SocialWorker = " Dr Minnie Mouse",
                CarePlan = "See File for details"
                // ....
            });


            var p5 = svc.AddPatient(new Patient
            {
                Title = "Mr",
                Firstname = "Mickey",
                Surname = "Bloggs",
                NationalInsuranceNo = "NR56789C",
                DOB = new DateTime(1947, 05, 28),
                Street = "28 Warren Hill",
                Town = "Newry",
                County = "Down",
                Postcode = "BT34 2PH",
                PhotoUrl = "/images/5.jpg",
                MobileNumber = "01234567891",
                HomeNumber = "02830303030",
                Email = "Mickey@mail.com",
                GP = "Dr A Blagg",
                SocialWorker = " Dr Minnie Mouse",
                CarePlan = "See File for details"
                // ....
            });

            var p6 = svc.AddPatient(new Patient
            {
                Title = "Miss",
                Firstname = "Nancy",
                Surname = "Bloggs",
                NationalInsuranceNo = "NR67891C",
                DOB = new DateTime(1953, 05, 28),
                Street = "29 Warren Hill",
                Town = "Newry",
                County = "Down",
                Postcode = "BT34 2PH",
                PhotoUrl = "/images/6.jpg",
                MobileNumber = "01234567891",
                HomeNumber = "02830303030",
                Email = "Nancy@mail.com",
                GP = "Dr A Blagg",
                SocialWorker = " Dr Minnie Mouse",
                CarePlan = "See File for details"
                // ....
            });


            var p7 = svc.AddPatient(new Patient
            {
                Title = "Mr",
                Firstname = "Brian",
                Surname = "Bloggs",
                NationalInsuranceNo = "NR789123C",
                DOB = new DateTime(1953, 05, 28),
                Street = "30 Warren Hill",
                Town = "Newry",
                County = "Down",
                Postcode = "BT34 2PH",
                PhotoUrl = "/images/7.jpg",
                MobileNumber = "01234567891",
                HomeNumber = "02830303030",
                Email = "Brian@mail.com",
                GP = "Dr A Blagg",
                SocialWorker = " Dr Minnie Mouse",
                CarePlan = "See File for details"
                // ....
            });


            var p8 = svc.AddPatient(new Patient
            {
                Title = "Mr",
                Firstname = "Niall",
                Surname = "Bloggs",
                NationalInsuranceNo = "NR891012C",
                DOB = new DateTime(1950, 05, 28),
                Street = "31 Warren Hill",
                Town = "Newry",
                County = "Down",
                Postcode = "BT34 2PH",
                PhotoUrl = "/images/8.jpg",
                MobileNumber = "01234567891",
                HomeNumber = "02830303030",
                Email = "Niall@mail.com",
                GP = "Dr A Blagg",
                SocialWorker = " Dr Minnie Mouse",
                CarePlan = "See File for details"
                // ....
            });


            var p9 = svc.AddPatient(new Patient
            {
                Title = "Mr",
                Firstname = "Michael",
                Surname = "Bloggs",
                NationalInsuranceNo = "NR910234C",
                DOB = new DateTime(1953, 05, 28),
                Street = "32 Warren Hill",
                Town = "Newry",
                County = "Down",
                Postcode = "BT34 2PH",
                PhotoUrl = "/images/9.jpg",
                MobileNumber = "01234567891",
                HomeNumber = "02830303030",
                Email = "Michael@mail.com",
                GP = "Dr A Blagg",
                SocialWorker = " Dr Minnie Mouse",
                CarePlan = "See File for details"
                // ....
            });

            var p10 = svc.AddPatient(new Patient
            {
                Title = "Mr",
                Firstname = "John",
                Surname = "Bloggs",
                NationalInsuranceNo = "NR101234C",
                DOB = new DateTime(1929, 05, 28),
                Street = "33 Warren Hill",
                Town = "Newry",
                County = "Down",
                Postcode = "BT34 2PH",
                PhotoUrl = "/images/10.jpg",
                MobileNumber = "01234567891",
                HomeNumber = "02830303030",
                Email = "Mary@mail.com",
                GP = "Dr A Blagg",
                SocialWorker = " Dr Minnie Mouse",
                CarePlan = "See File for details"
                // ....
            });

           // ================== Conditions =======================
            var con1 = svc.AddCondition(new Condition
            {
                Name = "Diabetes",
                Description = "Diabetes is a condition that causes a person's blood sugar level to become too high. There are 2 main types of diabetes: type 1 diabetes - a lifelong condition where the body's immune system attacks and destroys the cells that produce insulin. type 2 diabetes - where the body does not produce enough insulin, or the body's cells do not react to insulin properly Type 2 diabetes is far more common than type 1. In the UK, over 90% of all adults with diabetes have type 2.High blood sugar that develops during pregnancy is known as gestational diabetes. It usually goes away after giving birth."
            });
            //https://www.nhs.uk/conditions/diabetes/


            var con2 = svc.AddCondition(new Condition
            {

                Name = "Arthritis",
                Description = " Arthritis is a common condition that causes pain and inflammation in a joint or joints. Arthritis affects people of all ages, including children. Osteoarthritis and rheumatoid arthritis are the 2 most common types of arthritis."
            });
            // https://www.nhs.uk/conditions/arthritis/     


            var con3 = svc.AddCondition(new Condition
            {
                Name = "Heart Disease",
                Description = " Coronary heart disease is the term that describes what happens when your heart's blood supply is blocked or interrupted by a build-up of fatty substances in the coronary arteries.."
            });

            svc.AddPatientCondition(p1.Id, con1.Id, "Severe", DateTime.Now);
            svc.AddPatientCondition(p1.Id, con2.Id, "Moderate", DateTime.Now);



            // ===========  add family ==============
            var m1 = svc.AddMember(new User
            {
                Title = "Mr",
                Firstname = "Member",
                Surname = "One",              
                Email = "member1@mail.com",
                Password = "password",
                Street = "34 Big Hill",
                Town = "Rostrevor",
                County = "Down",
                Postcode = "BT34 2PH",
                MobileNumber = "01234567891",
                HomeNumber = "02830303030",
                           
            });

            var m2 = svc.AddMember(new User
            {
                Title = "Mrs",
                Firstname = "Member",
                Surname = "Two",              
                Email = "member2@mail.com",
                Password = "password",
                Street = "34 Big Hill",
                Town = "Rostrevor",
                County = "Down",
                Postcode = "BT34 2PH",
                MobileNumber = "01234567891",
                HomeNumber = "02830303030",
                           
            });

            // ============== Schedule CareEvents APPOINTMENTS =================

            // Appointments patient 1 carer 1
            var ap1 = svc.SchedulePatientCareEvent(new PatientCareEvent
            {
                CarePlan = p1.CarePlan,
                DateTimeOfEvent = new DateTime(2023, 05, 28, 12,0,0),
                PatientId = p1.Id,               
                UserId = c1.Id,
            });
            var ap3 = svc.SchedulePatientCareEvent(new PatientCareEvent
            {
                CarePlan = p1.CarePlan,
                DateTimeOfEvent = new DateTime(2023, 05, 28, 20,30,0),
                PatientId = p1.Id,              
                UserId = c1.Id,
            });

            // // Appointments patient 2 carer 2
            var ap4 = svc.SchedulePatientCareEvent(new PatientCareEvent
            {
                CarePlan = p1.CarePlan,  
                DateTimeOfEvent = new DateTime(2023, 05, 28, 10,30,0),
                PatientId = p2.Id,
                UserId = c2.Id,
            });
            var ap5 = svc.SchedulePatientCareEvent(new PatientCareEvent
            {
                CarePlan = p2.CarePlan,
                DateTimeOfEvent = new DateTime(2023, 05, 28, 12,45,0),
                PatientId = p2.Id,
                UserId = c2.Id,
            });           
            var ap6 = svc.SchedulePatientCareEvent(new PatientCareEvent
            {
                CarePlan = p2.CarePlan,    
                DateTimeOfEvent = new DateTime(2023, 05, 28, 18,0,0),
                PatientId = p2.Id,
                UserId = c2.Id,
            });
            
            // // Appointments patient 3 carer 2
            var ap7 = svc.SchedulePatientCareEvent(new PatientCareEvent
            {
                CarePlan = p3.CarePlan, 
                DateTimeOfEvent = new DateTime(2023, 05, 28, 8,0,0),
                PatientId = p3.Id,
                UserId = c2.Id,
            });
            
            var ap8 = svc.SchedulePatientCareEvent(new PatientCareEvent
            {
                CarePlan = p3.CarePlan,  
                DateTimeOfEvent = new DateTime(2023, 05, 28, 15,30,0),
                PatientId = p3.Id,
                UserId = c2.Id,
            });
            
            // Appointments patient 4 carer 3
            var ap9 = svc.SchedulePatientCareEvent(new PatientCareEvent        
            {
                CarePlan = p4.CarePlan,   
                DateTimeOfEvent = new DateTime(2023, 05, 28, 7,30,0),
                PatientId = p4.Id,
                UserId = c3.Id,
            });            
            var ap10 = svc.SchedulePatientCareEvent(new PatientCareEvent        
            {
                CarePlan = p4.CarePlan,  
                DateTimeOfEvent = new DateTime(2023, 05, 28, 12,45,0),
                PatientId = p4.Id,
                UserId = c3.Id,
            });            
            var ap11 = svc.SchedulePatientCareEvent(new PatientCareEvent        
            {
                CarePlan = p4.CarePlan,   
                DateTimeOfEvent = new DateTime(2023, 05, 28, 17,50,0),
                PatientId = p4.Id,
                UserId = c3.Id,
            });
            
            // Appointments patient 5 carer 3
            var ap12 = svc.SchedulePatientCareEvent(new PatientCareEvent        
            {
                CarePlan = p5.CarePlan, 
                DateTimeOfEvent = new DateTime(2023, 05, 28, 6,45,0),
                PatientId = p5.Id,
                UserId = c2.Id,
            });
            var ap13 = svc.SchedulePatientCareEvent(new PatientCareEvent        
            {
                CarePlan = p5.CarePlan,   
                DateTimeOfEvent = new DateTime(2023, 05, 28, 11,0,0),
                PatientId = p5.Id,
                UserId = c3.Id,
            });
            
            var ap14= svc.SchedulePatientCareEvent(new PatientCareEvent        
            {
                CarePlan = p5.CarePlan,   
                DateTimeOfEvent = new DateTime(2023, 05, 28, 22,0,0),
                PatientId = p5.Id,
                UserId = c3.Id,
            });
            
            // Appointments patient 6 carer 1
            var ap15 = svc.SchedulePatientCareEvent(new PatientCareEvent        
            {
                CarePlan = p6.CarePlan,  
                DateTimeOfEvent = new DateTime(2023, 05, 28, 7,50,0),
                PatientId = p6.Id,
                UserId = c1.Id,
            });           
            var ap16 = svc.SchedulePatientCareEvent(new PatientCareEvent        
            {
                CarePlan = p6.CarePlan,      
                DateTimeOfEvent = new DateTime(2023, 05, 28, 11,0,0),
                PatientId = p6.Id,
                UserId = c1.Id,
            });            
            var ap17 = svc.SchedulePatientCareEvent(new PatientCareEvent        
            {
                CarePlan = p6.CarePlan,
                DateTimeOfEvent = new DateTime(2023, 05, 28, 14,45,0),
                PatientId = p6.Id,
                UserId = c1.Id,
            });              
            var ap18 = svc.SchedulePatientCareEvent(new PatientCareEvent        
            {
                CarePlan = p6.CarePlan,     
                DateTimeOfEvent = new DateTime(2023, 05, 28,20,45,0),
                PatientId = p6.Id,
                UserId = c1.Id,
            });

            // Appointments patient 7 manager
            var ap19 = svc.SchedulePatientCareEvent(new PatientCareEvent        
            {
                CarePlan = p7.CarePlan,  
                DateTimeOfEvent = new DateTime(2023, 05, 28, 7,50,0),
                PatientId = p7.Id,
                UserId = manager.Id,
            });   

            // add member 1 to patient 1 family
            svc.AddPatientFamilyMember(p1.Id, m1.Id, true);
            svc.AddPatientFamilyMember(p1.Id, m2.Id, false);

        }
    }

}

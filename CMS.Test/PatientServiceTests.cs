using Xunit;
using CMS.Data.Services;
using CMS.Data.Security;
using CMS.Data.Entities;
using System;

namespace CMS.Test;

[Collection("Sequential")]
public class PatientServiceTests
{

    private readonly IPatientService svc;
    public PatientServiceTests()
    {
        svc = new PatientServiceDb();
        svc.Initialise();
    }
     [Fact]
        public void GetUsers_WhenNoneExist_ShouldReturnNone()
        {
            // act
            var users = svc.GetUsers();

            // assert
            Assert.Equal(0, users.Count);
        }
        
        [Fact]
        public void AddUser_When2ValidUsersAdded_ShouldCreate2Users()
        {
            // arrange
            svc.AddUser("an","admin", "admin@mail.com", "admin", Role.admin );
            svc.AddUser("a", "guest", "guest@mail.com", "guest", Role.guest);

            // act
            var users = svc.GetUsers();

            // assert
            Assert.Equal(2, users.Count);
        }

        [Fact]
        public void GetPage1WithpageSize2_When3UsersExist_ShouldReturn2Pages()
        {
            // act
            svc.AddUser("an", "admin", "admin@mail.com", "admin", Role.admin );
            svc.AddUser("a", "manager", "manager@mail.com", "manager", Role.manager);
            svc.AddUser("a", "guest", "guest@mail.com", "guest", Role.guest);

            // return first page with 2 users per page
            var pagedUsers = svc.GetUsers(1,2);

            // assert
            Assert.Equal(2, pagedUsers.TotalPages);
        }
 [Fact]
        public void GetPage1WithPageSize2_When3UsersExist_ShouldReturnPageWith2Users()
        {
            // act
            svc.AddUser("an", "admin", "admin@mail.com", "admin", Role.admin );
            svc.AddUser("a", "manager", "manager@mail.com", "manager", Role.manager);
            svc.AddUser("a", "guest", "guest@mail.com", "guest", Role.guest);

            var pagedUsers = svc.GetUsers(1,2);

            // assert
            Assert.Equal(2, pagedUsers.Data.Count);
        }

        [Fact]
        public void GetPage1_When0UsersExist_ShouldReturn0Pages()
        {
            // act
            var pagedUsers = svc.GetUsers(1,2);

            // assert
            Assert.Equal(0, pagedUsers.TotalPages);
            Assert.Equal(0, pagedUsers.TotalRows);
            Assert.Empty(pagedUsers.Data);
        }
          [Fact]
        public void UpdateUser_WhenUserExists_ShouldWork()
        {
            // arrange
            var user = svc.AddUser("an", "admin", "admin@mail.com", "admin", Role.admin );
            
            // act
            user.Firstname = "the";
            user.Surname = "administrator";
            user.Email = "admin@mail.com";            
            var updatedUser = svc.UpdateUser(user);

            // assert
            Assert.Equal("the", updatedUser.Firstname);
            Assert.Equal("administrator", updatedUser.Surname);
            Assert.Equal("admin@mail.com", updatedUser.Email);
        }

        [Fact]
        public void Login_WithValidCredentials_ShouldWork()
        {
            // arrange
            svc.AddUser("an", "admin", "admin@mail.com", "admin", Role.admin );
            
            // act            
            var user = svc.Authenticate("admin@mail.com","admin");

            // assert
            Assert.NotNull(user);
           
        }

        [Fact]
        public void Login_WithInvalidCredentials_ShouldNotWork()
        {
            // arrange
            svc.AddUser("an", "admin", "admin@mail.com", "admin", Role.admin );

            // act      
            var user = svc.Authenticate("admin@mail.com","xxx");

            // assert
            Assert.Null(user);
           
        }
         [Fact]
        public void ForgotPasswordRequest_ForValidUser_ShouldGenerateToken()
        {
            // arrange
            svc.AddUser("an", "admin", "admin@mail.com", "admin", Role.admin );

            // act      
            var token = svc.ForgotPassword("admin@mail.com");

            // assert
            Assert.NotNull(token);
           
        }
         [Fact]
        public void ForgotPasswordRequest_ForInValidUser_ShouldReturnNull()
        {
            // arrange
          
            // act      
            var token = svc.ForgotPassword("admin@mail.com");

            // assert
            Assert.Null(token);
           
        }
         [Fact]
        public void ResetPasswordRequest_WithValidUserAndToken_ShouldReturnUser()
        {
            // arrange
            svc.AddUser("an", "admin", "admin@mail.com", "admin", Role.admin );
            var token = svc.ForgotPassword("admin@mail.com");
            
            // act      
            var user = svc.ResetPassword("admin@mail.com", token, "password");
        
            // assert
            Assert.NotNull(user);
            Assert.True(Hasher.ValidateHash(user.Password, "password"));          
        }
          [Fact]
        public void ResetPasswordRequest_WithValidUserAndExpiredToken_ShouldReturnNull()
        {
            // arrange
            svc.AddUser("an", "admin", "admin@mail.com", "admin", Role.admin );
            var expiredToken = svc.ForgotPassword("admin@mail.com");
            svc.ForgotPassword("admin@mail.com");
            
            // act      
            var user = svc.ResetPassword("admin@mail.com", expiredToken, "password");
        
            // assert
            Assert.Null(user);  
        }

        [Fact]
        public void ResetPasswordRequest_WithInValidUserAndValidToken_ShouldReturnNull()
        {
            // arrange
            svc.AddUser("an", "admin", "admin@mail.com", "admin", Role.admin );          
            var token = svc.ForgotPassword("admin@mail.com");
            
            // act      
            var user = svc.ResetPassword("unknown@mail.com", token, "password");
        
            // assert
            Assert.Null(user);  
        }
         [Fact]
        public void ResetPasswordRequests_WhenAllCompleted_ShouldExpireAllTokens()
        {
            // arrange
            svc.AddUser("an", "admin", "admin@mail.com", "admin", Role.admin );       
            svc.AddUser("a", "guest", "guest@mail.com", "guest", Role.guest );          

            // create token and reset password - token then invalidated
            var token1 = svc.ForgotPassword("admin@mail.com");
            svc.ResetPassword("admin@mail.com", token1, "password");

            // create token and reset password - token then invalidated
            var token2 = svc.ForgotPassword("guest@mail.com");
            svc.ResetPassword("guest@mail.com", token2, "password");
         
            // act  
            // retrieve valid tokens 
            var tokens = svc.GetValidPasswordResetTokens();   

            // assert
            Assert.Empty(tokens);
        }
    [Fact]
    public void GetAllPatients_WhenNone_ShouldReturnZero()
    {
        // arrange

        // act
        var patients = svc.GetAllPatients();

        // assert
        Assert.Equal(0, patients.Count);
    }

    [Fact]
    public void AddPatient_WhenValid_ShouldReturnPatient()
    {
        // arrange

        // act add patient to database
        var patient = svc.AddPatient(new Patient
        {
            Title = " Mr ",
            Firstname = "Joe",
            Surname = "Bloggs",
            NationalInsuranceNo = "BR128734B",
            DOB = new DateTime(1947, 03, 03),
            Email = "joe@mail.com",
            Street = " 27 Warren Hill ",
            Town = "Newry",
            County = " Down",
            Postcode = " BT34 2PH",
            MobileNumber = "07892891702",
            HomeNumber = "028302515",
            GP = " Dr R Campbell",
            SocialWorker = "Cathy Burke ",
            CarePlan = "Get dressed and washed"
            // ....
        });

        // assert
        Assert.NotNull(patient);
        Assert.Equal("Joe", patient.Firstname);
        Assert.Equal("Bloggs", patient.Surname);
        Assert.Equal("joe@mail.com", patient.Email);
        Assert.Equal("BR128734B", patient.NationalInsuranceNo);
    }


    [Fact]
    public void GetPatient_WhenExists_ShouldReturnPatient()
    {
        // arrange
        var ap = svc.AddPatient(new Patient
        {
            Firstname = "John",
            Surname = "White",
            NationalInsuranceNo = " BR234567",
            DOB = new DateTime(1947, 09, 24),
            Email = "john@mail.com",
            Street = " 28 Warren Hill ",
            Town = "Newry",
            County = " Down",
            Postcode = " BT34 2PH",
            MobileNumber = "01234567891",
            HomeNumber = "028301234",
            GP = " Dr R Campbell",
            SocialWorker = "Cathy Burke ",
            CarePlan = "Get dressed and washed"
            // ....
        });

        // act
        var patient = svc.GetPatientById(ap.Id);

        // assert -- TODO Check for other properties
        Assert.NotNull(patient);
        Assert.Equal(ap.Firstname, patient.Firstname);
        Assert.Equal(ap.Surname, patient.Surname);
        Assert.Equal(ap.DOB, patient.DOB);
        Assert.Equal(ap.NationalInsuranceNo, patient.NationalInsuranceNo);
        Assert.Equal(ap.Email, patient.Email);
        Assert.Equal(ap.MobileNumber, patient.MobileNumber);
    }

    [Fact]
    public void UpdatePatient_WhenExistsAndIsValid_ShouldReturnUpdatedPatient()
    {
        // arrange
        var p = svc.AddPatient(Factory.MakePatient());

        // act == TODO - need to check for all properties except Id
        var updated = svc.UpdatePatient(new Patient
        {
            Id = p.Id,
            Firstname = p.Firstname,
            Surname = "Changed",
            NationalInsuranceNo = p.NationalInsuranceNo,
            DOB = p.DOB,
            Email = p.Email,
            Street = p.Street,
            Town = p.Town,
            County = p.County,
            Postcode = p.Postcode,
            MobileNumber = p.MobileNumber,
            HomeNumber = p.HomeNumber,
            GP = p.GP,
            SocialWorker = p.SocialWorker,
            CarePlan = p.CarePlan
            // ....
        });

        // assert
        Assert.NotNull(updated);
        Assert.Equal("Changed", updated.Surname);
    }

    [Fact]
    public void UpdatePatient_WhenDuplicateNationalInsuranceNo_ShouldReturnNull()
    {
        // arrange
        var p1 = svc.AddPatient(new Patient
        {
            Firstname = "Joe",
            Surname = "Bloggs",
            Email = "joe@mail.com",
            NationalInsuranceNo = "1234"
            // ...
        });
        var p2 = svc.AddPatient(new Patient
        {
            Firstname = "Fred",
            Surname = "Bloggs",
            Email = "Fred@mail.com",
            NationalInsuranceNo = "34565"
            // ...
        });

        // act
        var updated = svc.UpdatePatient(new Patient
        {
            Id = p1.Id,
            Firstname = p1.Firstname,
            Surname = p1.Surname,
            Email = p1.Email,
            NationalInsuranceNo = p2.NationalInsuranceNo
            // ....
        });

        // assert
        Assert.Null(updated);
    }

    [Fact]
    public void DeletePatient_WhenNotExists_ShouldReturnFalse()
    {
        // arrange     

        // act
        var success = svc.DeletePatient(1);

        // assert
        Assert.False(success);
    }

    [Fact]
    public void DeletePatient_WhenExists_ShouldReturnTrue()
    {
        // arrange
        var dummy = svc.AddPatient(new Patient
        {
            Firstname = "Joe",
            Surname = "Bloggs",
            // ....
        });

        // act
        var success = svc.DeletePatient(dummy.Id);

        // assert
        Assert.True(success);
    }

    //========================Carer Tests========================================
    [Fact]
    public void GetAllCarers_WhenNone_ShouldReturnZero()
    {
        // arrange

        // act
        var carers = svc.GetAllCarers();
        // assert
        Assert.Equal(0, carers.Count);
    }

    [Fact]
    public void AddCarer_WhenValid_ShouldReturnCarer()
    {
        // arrange
        // act
        var carer = svc.AddCarer(new User
        {
            Title = " Mr ",
            Firstname = "Donald",
            Surname = "Duck",
            NationalInsuranceNo = "CD123456",
            DOB = new DateTime(1968, 03, 04),
            Email = "Donald@mail.com",
            Street = "28 Warren Hill",
            Password = "password",
            Town = "Newry",
            County = " Down",
            Postcode = " BT34 2PH",
            MobileNumber = "01234567898",
            HomeNumber = "028302515",
            Qualifications = "BTEC Level III Social Healthcare"
            // ....
        });
        // assert -- TODO Check for all properties (except Id)
        Assert.NotNull(carer);
        Assert.Equal("Donald", carer.Firstname);
        Assert.Equal("Duck", carer.Surname);
        Assert.Equal("Donald@mail.com", carer.Email);
        Assert.Equal("CD123456", carer.NationalInsuranceNo);
        Assert.Equal("28 Warren Hill", carer.Street);
        Assert.Equal("Newry", carer.Town);
        Assert.Equal(" Down", carer.County);
        Assert.Equal("01234567898", carer.MobileNumber);
    }
  
    [Fact]
    public void GetCarer_WhenExists_ShouldReturnCarer()
    {
        // arrange
       
        var dummy = svc.AddCarer(new User
        {
            Role = Role.carer,
            Firstname = "John",
            Surname = "White",
            NationalInsuranceNo = "BR234567",
            DOB = new DateTime(1947, 09, 24),
            Email = "john@mail.com",
            Street = " 28 Warren Hill ",
            Town = "Newry",
            County = " Down",
            Postcode = " BT34 2PH",
            MobileNumber = "01234567891",
            HomeNumber = "028301234",
            // ....
        });

        // act
        var carer = svc.GetCarerById(dummy.Id);

        // assert
        Assert.NotNull(carer);
        Assert.Equal("BR234567", carer.NationalInsuranceNo);

    }

    [Fact]
    public void GetCarerByEmail_WhenValid_ShouldReturnCarer()
    {
        // arrange
   
        var c = svc.AddCarer(new User
        {
            Role = Role.carer,
            Title = " Mr ",
            Firstname = "Minnie",
            Surname = "Mouse",
            NationalInsuranceNo = "CD987654321",
            DOB = new DateTime(1983, 03, 04),
            Email = "Minnie@mail.com",
            Street = " 30 Warren Hill ",
            Town = "Newry",
            County = " Down",
            Postcode = " BT34 2PH",
            MobileNumber = "01234567898",
            HomeNumber = "028302515",
            Qualifications = "BTEC Level II Social Healthcare",
          
            // ....
        });

        // act
        var carer = svc.GetCarerByEmail(c.Email);

        // assert
        Assert.NotNull(carer);
        Assert.Equal("Minnie", carer.Firstname);
        Assert.Equal("Mouse", carer.Surname);
        Assert.Equal("Minnie@mail.com", carer.Email);
        Assert.Equal("CD987654321", carer.NationalInsuranceNo);
    }

    [Fact]
    public void UpdateCarer_WhenExistsAndIsValid_ShouldReturnUpdatedCarer()
    {
        // arrange

        var c = svc.AddCarer(new User
        {
            Role = Role.carer,
            Firstname = "Daffy",
            Surname = "Duck",
            NationalInsuranceNo = " BR4567123",
            DOB = new DateTime(1964, 09, 24),
            Email = "Daffy@mail.com",
            Street = " 32 Warren Hill ",
            Town = "Newry",
            County = " Down",
            Postcode = " BT34 2PH",
            MobileNumber = "07778899007",
            HomeNumber = "028304321",
            Qualifications = "BTEC Level II Social Healthcare"
          
            // ....
        });


        // act
        var updated = svc.UpdateCarer(new User
        {
            Id = c.Id,
            Role = c.Role,
            Firstname = c.Firstname,
            Surname = "Changed",
            NationalInsuranceNo = c.NationalInsuranceNo,
            DOB = c.DOB,
            Email = c.Email,
            Street = c.Street,
            Town = c.Town,
            County = c.County,
            Postcode = c.Postcode,
            MobileNumber = c.MobileNumber,
            HomeNumber = c.HomeNumber,
            Qualifications = "BTEC Level III Social Healthcare"

            // ....
        });


        // assert
        Assert.NotNull(updated);
        Assert.Equal("Changed", updated.Surname);
        Assert.Equal("BTEC Level III Social Healthcare", updated.Qualifications);
    }

    [Fact]
    public void DeleteCarer_WhenNotExist_ShouldReturnFalse()
    {
        // arrange     

        // act
        var success = svc.DeleteCarer(1);

        // assert
        Assert.False(success);
    }

    [Fact]
    public void DeleteCarer_WhenExists_ShouldReturnTrue()
    {
        // arrange
       
        var dummy = svc.AddCarer(new User
        {
            Firstname = "Minnie",
            Surname = "Mouse",
           
        });

        // act
        var success = svc.DeleteCarer(dummy.Id);

        // assert
        Assert.True(success);
    }

    //======================CareEvent Management Tests================================== 
    [Fact]
    public void AddPatientCareEvent_WhenValid_ShouldReturnCareEvent()
    {
        // arrange
        var p = svc.AddPatient(Factory.MakePatient());
        
      
        var c = svc.AddCarer(new User
        {
            Title = " Mr ",
            Firstname = "Minnie",
            Surname = "Mouse",
            NationalInsuranceNo = "CD987654321",
            DOB = new DateTime(1983, 03, 04),
            Email = "Minnie@mail.com",
            Street = " 30 Warren Hill ",
            Town = "Newry",
            County = " Down",
            Postcode = " BT34 2PH",
            MobileNumber = "01234567898",
            HomeNumber = "028302515",
            Qualifications = "BTEC Level II Social Healthcare",
          
            // ....
        });

        // act
        var pce = svc.SchedulePatientCareEvent( new PatientCareEvent {
            DateTimeOfEvent =  new DateTime(2023, 06, 1), CarePlan = "CarePlan", Issues = "Nothing to report",
             PatientId = p.Id, UserId= c.Id
        });

        Assert.NotNull(p);
        Assert.NotNull(c);
        Assert.NotNull(pce);
        Assert.Equal("CarePlan", pce.CarePlan);
        Assert.Equal("Nothing to report", pce.Issues);

    }

    [Fact]
    public void AddPatientCareEvent_WhenInPast_ShouldNotAddCareEvent()
    {
        // arrange
        var p = svc.AddPatient(Factory.MakePatient());
    
        var c = svc.AddCarer(new User
        {
            Title = " Mr ",
            Firstname = "Minnie",
            Surname = "Mouse",
            NationalInsuranceNo = "CD987654321",
            DOB = new DateTime(1983, 03, 04),
            Email = "Minnie@mail.com",
            Street = " 30 Warren Hill ",
            Town = "Newry",
            County = " Down",
            Postcode = " BT34 2PH",
            MobileNumber = "01234567898",
            HomeNumber = "028302515",
            Qualifications = "BTEC Level II Social Healthcare",
          
            // ....
        });
        
        var pce1 = svc.SchedulePatientCareEvent( new PatientCareEvent {
            DateTimeOfEvent =  new DateTime(2023, 06, 1), CarePlan = "CarePlan", Issues = "Nothing to report",
             PatientId = p.Id, UserId= c.Id
        });

        // act - create care event in past (before last care event)
        var pce2 = svc.SchedulePatientCareEvent( new PatientCareEvent {
            DateTimeOfEvent =  new DateTime(2023, 05, 1), CarePlan = "CarePlan", Issues = "Nothing to report",
             PatientId = p.Id, UserId= c.Id
        });
        Assert.NotNull(p);
        Assert.NotNull(c);
        Assert.NotNull(pce1);
        Assert.Null(pce2);
    }

    [Fact]
    public void GetPatientCareEvents_WhenExists_ShouldReturnCareEvent()
    {
        // arrange
      
        var c = svc.AddCarer(new User
        {
            Title = " Mr ",
            Firstname = "Minnie",
            Surname = "Mouse",
            NationalInsuranceNo = "CD987654321",
            DOB = new DateTime(1983, 03, 04),
            Email = "Minnie@mail.com",
            Street = " 30 Warren Hill ",
            Town = "Newry",
            County = " Down",
            Postcode = " BT34 2PH",
            MobileNumber = "01234567898",
            HomeNumber = "028302515",
            Qualifications = "BTEC Level II Social Healthcare",
          
            // ....
        });

        var p = svc.AddPatient(new Patient
        {
            Firstname = "John",
            Surname = "White",
            NationalInsuranceNo = " BR234567",
            DOB = new DateTime(1947, 09, 24),
            Email = "john@mail.com",
            Street = " 28 Warren Hill ",
            Town = "Newry",
            County = " Down",
            Postcode = " BT34 2PH",
            MobileNumber = "01234567891",
            HomeNumber = "028301234",
            GP = " Dr R Campbell",
            SocialWorker = "Cathy Burke ",
            CarePlan = "Get dressed and washed",
            PhotoUrl =  "/images/carer.jpg"
            // ....
        });

        var pce = svc.SchedulePatientCareEvent( new PatientCareEvent {
            DateTimeOfEvent =  new DateTime(2023, 06, 1), CarePlan = "CarePlan", Issues = "Nothing to report",
             PatientId = p.Id, UserId= c.Id
        });
        var gpce = svc.GetPatientCareEventById(pce.Id);
        // assert
        Assert.NotNull(c);
        Assert.NotNull(p);
        Assert.Equal(pce.Id, gpce.Id);
        Assert.NotNull(pce);
        Assert.NotNull(gpce);

    }


    [Fact]
    public void TestDeletePatientCareEvent__ShouldReturntrue()
    {
        //Arrange
        var p = svc.AddPatient(Factory.MakePatient());
       
        var c = svc.AddCarer(new User
        {
            Title = " Mr ",
            Firstname = "Minnie",
            Surname = "Mouse",
            NationalInsuranceNo = "CD987654321",
            DOB = new DateTime(1983, 03, 04),
            Email = "Minnie@mail.com",
            Street = " 30 Warren Hill ",
            Town = "Newry",
            County = " Down",
            Postcode = " BT34 2PH",
            MobileNumber = "01234567898",
            HomeNumber = "028302515",
            Qualifications = "BTEC Level II Social Healthcare",
          
            // ....
        });

        var pce = svc.SchedulePatientCareEvent( new PatientCareEvent {
            DateTimeOfEvent =  new DateTime(2023, 06, 1), CarePlan = "CarePlan", Issues = "Nothing to report",
             PatientId = p.Id, UserId= c.Id
        });

        // act
        var deleted = svc.DeletePatientCareEvent(pce.Id);

        // assert
        Assert.True(deleted);
        Assert.NotNull(p);
        Assert.NotNull(c);
    }


    [Fact]
    public void TestUpdatePatientCareEvent_WhenAdded_ShouldReturntrue()
    {
        // arrange
        var p = svc.AddPatient(new Patient
        {
            Title = " Mr ",
            Firstname = "Mary",
            Surname = "Brown",
            NationalInsuranceNo = "ZY1098765432B",
            DOB = new DateTime(1941, 03, 04),
            Email = "Mary@mail.com",
            Street = " 45 Warren Hill ",
            Town = "Newry",
            County = " Down",
            Postcode = " BT34 2PH",
            MobileNumber = "07892891702",
            HomeNumber = "028302515",
            GP = " Dr D Campbell",
            SocialWorker = "John Burke ",
            CarePlan = "Get lunch, clear dishes to dishwasher"
            // ....
        });
      
        var c = svc.AddCarer(new User
        {
            Title = " Mr ",
            Firstname = "Minnie",
            Surname = "Mouse",
            NationalInsuranceNo = "CD987654321",
            DOB = new DateTime(1983, 03, 04),
            Email = "Minnie@mail.com",
            Street = " 30 Warren Hill ",
            Town = "Newry",
            County = " Down",
            Postcode = " BT34 2PH",
            MobileNumber = "01234567898",
            HomeNumber = "028302515",
            Qualifications = "BTEC Level II Social Healthcare",
           
            // ....
        });
        
        var pce = svc.SchedulePatientCareEvent( new PatientCareEvent {
            DateTimeOfEvent =  new DateTime(2023, 06, 1), 
            CarePlan = "CarePlan", Issues = "Nothing to report",
            PatientId = p.Id, 
            UserId= c.Id
        });

        // act 
        pce.Issues += " Updated";
        var updatedpce = svc.UpdatePatientCareEvent(pce);

        Assert.NotNull(p);
        Assert.NotNull(c);
        Assert.NotNull(updatedpce);
        Assert.Equal(p.Id, p.Id);
        Assert.Equal(c.Id, c.Id);
        Assert.Equal("Nothing to report Updated", updatedpce.Issues);

    }


    // TODO - Needs tests to Complete ScheduledPatientCareEvents


    
    //  =====================Test Patient Condition Management================================== 

    [Fact]
    public void AddConditionToPatient_WhenInPast_ShouldNotAddPatientCondition()
    {
        // arrange
        var p = svc.AddPatient(Factory.MakePatient());
        var c = svc.AddCondition(Factory.MakeConditions());

        // act
        var pc = svc.AddPatientCondition(p.Id, c.Id, "PatientNote", new DateTime(2023, 3, 2, 13, 5, 11, 123));
        var pc2 = svc.AddPatientCondition(p.Id, c.Id, "PatientNote", new DateTime(2023, 3, 1, 13, 5, 11, 123));
        // assert
        Assert.NotNull(p);
        Assert.NotNull(c);
        Assert.NotNull(pc);
        Assert.Equal("PatientNote", pc.Note);
        Assert.Null(pc2);

    }
    [Fact]
    public void AddPatientCondition_WhenValid_ShouldReturnPatientCondition()
    {
        // arrange
        var p = svc.AddPatient(Factory.MakePatient());
        var c = svc.AddCondition(Factory.MakeConditions());

        // act
        var pc = svc.AddPatientCondition(p.Id, c.Id, "PatientCondition and notes", new DateTime(2023, 3, 2, 13, 5, 11, 123));

        Assert.NotNull(p);
        Assert.NotNull(c);
        Assert.NotNull(pc);
        Assert.Equal("PatientCondition and notes", pc.Note);

    }

   [Fact]
    public void GetPatientCondition_WhenExists_ShouldReturnPatientConditions()
    {
        // arrange
        var p = svc.AddPatient(Factory.MakePatient());
        var c = svc.AddCondition(Factory.MakeConditions());
        // act

        var pc = svc.AddPatientCondition(p.Id, c.Id, "PatientCondition and notes", new DateTime(2023, 3, 2, 13, 5, 11, 123));

        var gpc = svc.GetPatientConditionById(pc.Id);
        // assert
        Assert.NotNull(p);
        Assert.NotNull(c);
        Assert.NotNull(pc);
        Assert.NotNull(gpc);
        Assert.Equal("Arthritis", c.Name );
        Assert.Equal( "Arthritis is a condition that causes pain and inflammation in a joint", c.Description );

    }


  [Fact]
    public void TestDeletePatientCondition__ShouldReturntrue()
    {
        //Arrange
        var p = svc.AddPatient(Factory.MakePatient());
        var c = svc.AddCondition(Factory.MakeConditions());

        var pc = svc.AddPatientCondition(p.Id, c.Id, "Condition note",  new DateTime(2023, 3, 2, 13, 5, 11, 123));

        // act
        var deleted = svc.DeleteCondition(pc.Id);

        // assert
        Assert.True(deleted);
    }

     [Fact]
    public void TestUpdatePatientCondition_WhenAdded_ShouldReturnTrue()
    {
        //Arrange
        var ap = svc.AddPatient(Factory.MakePatient());
        var ac = svc.AddCondition(Factory.MakeConditions());
        var pc = svc.AddPatientCondition(ap.Id, ac.Id, "Note Condition",  new DateTime(2023, 3, 2, 13, 5, 11, 123));

        // act -- what is purpose of ap.Id parameter
        pc.Note = pc.Note + " Updated";
        var updatedpc = svc.UpdatePatientCondition(pc);

        Assert.NotNull(ap);
        Assert.NotNull(ac);
        Assert.NotNull(updatedpc);
        Assert.Equal(ap.Id, ap.Id);
        Assert.Equal(ac.Id, ac.Id);

        Assert.Equal("Note Condition Updated", updatedpc.Note);

    }
    //======================Family Management Testss=================================
    
    //======================Patient Family Management Tests==================================

    [Fact]
    public void AddFamilyMember_WhenValid_ShouldReturnFamilyMember()
    {
        // arrange
        var p = svc.AddPatient(Factory.MakePatient());
        var m = svc.AddMember(Factory.MakeMember());

        // act
        var fm = svc.AddPatientFamilyMember(p.Id, m.Id, true);

        // assert
        Assert.NotNull(fm);
        Assert.Equal(p.Id, fm.PatientId);
        Assert.Equal(m.Id, fm.MemberId);
        Assert.True(fm.Primary);       
    }

    [Fact]
    public void GetFamily_WhenExists_ShouldReturnFamily()
    {
        // arrange
        var fm = svc.AddMember(Factory.MakeMember());

        // act
        var gfm = svc.GetMemberById(fm.Id);

        // assert 
        Assert.NotNull(gfm);
        Assert.Equal(fm.Firstname, fm.Firstname);
        Assert.Equal(fm.Surname, fm.Surname);
        Assert.Equal(fm.Email, fm.Email);
        Assert.Equal(fm.MobileNumber, fm.MobileNumber);

    }
    [Fact]
    public void UpdateFamily_WhenExistsAndIsValid_ShouldReturnUpdatedFamily()
    {
        // arrange
        var fm = svc.AddMember(Factory.MakeMember());

        // act == TODO - need to check for all properties except Id
        var updatedfm = svc.UpdateMember(new User
        {
            Id = fm.Id,
            Firstname = fm.Firstname,
            Surname = "Changed Surname",
            Email = fm.Email,
            MobileNumber = "0000111122223333",
            // ....
        });
        Assert.NotNull(fm);
        Assert.NotNull(updatedfm);
        Assert.Equal(fm.Id, fm.Id);
      
    }

} 


static class Factory
{

    public static Patient MakePatient()
    {
        return new Patient
        {
            Title = "Mr",
            Firstname = "Mark",
            Surname = "Brown",
            NationalInsuranceNo = "ZY1098765432B",
            DOB = new DateTime(1941, 03, 04),
            Email = "Mark@mail.com",
            Street = " 45 Warren Hill ",
            Town = "Newry",
            County = " Down",
            Postcode = " BT34 2PH",
            MobileNumber = "07892891702",
            HomeNumber = "028302515",
            GP = " Dr D Campbell",
            SocialWorker = "John Burke ",
            CarePlan = "Get lunch, clear dishes to dishwasher"
            // ....
        };
    }
     public static Patient MakePatient2()
    {
        return new Patient
        {
            Title = "Mrs",
            Firstname = "Francis",
            Surname = "Deery",
            NationalInsuranceNo = "DY1058765432B",
            DOB = new DateTime(1971, 5, 04),
            Email = "francis@mail.com",
            Street = "Main St",
            Town = "Belfast",
            County = "Antrin",
            Postcode = " BT3 1ER",
            MobileNumber = "0749788702",
            HomeNumber = "02890123456",
            GP = " Dr F Kelly",
            SocialWorker = "John Burke",
            CarePlan = "Get lunch, clear dishes to dishwasher"
            // ....
        };
    }

    public static User MakeCarer()
    {
        
        return new User
        {
            Role = Role.carer,
            Title = "Mr",
            Firstname = "Donald",
            Surname = "Duck",
            NationalInsuranceNo = "CD123456",
            DOB = new DateTime(1968, 03, 04),
            Email = "donald@mail.com",
            Password = "password",
            Street = " 28 Warren Hill ",
            Town = "Newry",
            County = " Down",
            Postcode = " BT34 2PH",
            MobileNumber = "01234567898",
            HomeNumber = "028302515",
            Qualifications = "BTEC Level III Social Healthcare"
            // ....
        };
    }
    public static User MakeCarer2()
    {
        
        return new User
        {
            Role = Role.carer,
            Title = "Mrs",
            Firstname = "Minnie",
            Surname = "Mouse",
            NationalInsuranceNo = "CD146456",
            DOB = new DateTime(1988, 03, 04),
            Email = "minnie@mail.com",
            Password = "password",
            Street = " 28 Warren Hill ",
            Town = "Newry",
            County = " Down",
            Postcode = " BT34 2PH",
            MobileNumber = "01234567898",
            HomeNumber = "028302515",
            Qualifications = "BTEC Level III Social Healthcare"
            // ....
        };
    }

    public static Condition MakeConditions()
    {
        return new Condition {
        Name = "Arthritis", 
        Description = "Arthritis is a condition that causes pain and inflammation in a joint"

        };
    }
    public static User MakeMember()
    {
        return new User
        {   
            Role = Role.family,
            Firstname = "Minnie",
            Surname = "Mouse",
            MobileNumber = "07778899032",
            Email = "family@mail.com",

            // ....
        };

    }
}

         
    



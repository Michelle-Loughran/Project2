using Microsoft.EntityFrameworkCore;
using CMS.Data.Repositories;
using CMS.Data.Entities;
using CMS.Data.Security;
using System.Runtime.Intrinsics.X86;
using System.IO;
using Microsoft.VisualBasic;

namespace CMS.Data.Services;

public class PatientServiceDb : IPatientService
{
    private readonly PatientDbContext db;


    public PatientServiceDb()
    {
        db = new PatientDbContext();
    }

    public void Initialise()
    {
        db.Initialise();
    }

    // ------------------ User Related Operations ------------------------

    // retrieve list of Users
    public IList<User> GetUsers()
    {
        return db.Users.ToList();
    }

    // retrieve paged list of users
    public Paged<User> GetUsers(int page = 1, int size = 10, string orderBy = "id", string direction = "asc")
    {
        var query = (orderBy.ToLower(), direction.ToLower()) switch
        {
            ("id", "asc") => db.Users.OrderBy(r => r.Id),
            ("id", "desc") => db.Users.OrderByDescending(r => r.Id),
            ("name", "asc") => db.Users.OrderBy(r => r.Name),
            ("name", "desc") => db.Users.OrderByDescending(r => r.Name),
            ("email", "asc") => db.Users.OrderBy(r => r.Email),
            ("email", "desc") => db.Users.OrderByDescending(r => r.Email),
            _ => db.Users.OrderBy(r => r.Id)
        };

        return query.ToPaged(page, size, orderBy, direction);
    }

    // Retrive User by Id 
    public User GetUser(int id)
    {
        return db.Users.FirstOrDefault(s => s.Id == id);
    }
    // Find a user with specified email address
    public User GetUserByEmail(string email)
    {
        return db.Users.FirstOrDefault(u => u.Email == email);
    }

    // Verify if email is available or registered to specified user
    public bool IsEmailAvailable(string email, int userId)
    {
        return db.Users.FirstOrDefault(u => u.Email == email && u.Id != userId) == null;
    }

    public User Authenticate(string email, string password)
    {
        // retrieve the user based on the EmailAddress (assumes EmailAddress is unique)
        var user = GetUserByEmail(email);

        // Verify the user exists and Hashed User password matches the password provided
        return (user != null && Hasher.ValidateHash(user.Password, password)) ? user : null;
        //return (user != null && user.Password == password ) ? user: null;
    }
    public IList<User> GetUsersByRole(Role role)
    {
        return db.Users
                   .Where(u => u.Role == role)
                   .ToList();

    }

    public User Register(User u)
    {
        // check user doesn't already exist
        var exists = GetUserByEmail(u.Email);
        if (exists != null)
        {
            return null;
        }

        // create new user
        var user = new User
        {
            Firstname = u.Firstname,
            Surname = u.Surname,
            Email = u.Email,
            // hash password
            Password = Hasher.CalculateHash(u.Password),
            Role = u.Role,
        };

        db.Users.Add(user);
        db.SaveChanges();

        // return new user
        return user;
    }

    public User AddUser(string firstname, string surname, string email, string password, Role role)
    {
        // return new user
        return AddUser(new User
        {
            Firstname = firstname,
            Surname = surname,
            Email = email,
            Password = password,
            Role = role
        });
    }

    public User AddUser(User u)
    {
        var exists = GetUserByEmail(u.Email);
        if (exists != null)
        {
            return null;
        }

        var user = new User
        {
            Firstname = u.Firstname,
            Surname = u.Surname,
            Email = u.Email,
            Password = Hasher.CalculateHash(u.Password),  // hash password
            Role = u.Role,

            Street = u.Street,
            Town = u.Town,
            County = u.County,
            Postcode = u.Postcode,
            MobileNumber = u.MobileNumber,
            HomeNumber = u.HomeNumber,
            NationalInsuranceNo = u.NationalInsuranceNo,
            DBSCheck = u.DBSCheck,
            Qualifications = u.Qualifications,
            PhotoUrl = u.PhotoUrl
        };

        db.Users.Add(user);
        db.SaveChanges();
        // return new user
        return user;
    }

    public User UpdateUser(User updated)
    {
        // verify the user exists
        var user = GetUser(updated.Id);
        if (user == null)
        {
            return null;
        }

        // check for another user with the same Email
        var found = db.Users
                      .FirstOrDefault(c => c.Email == updated.Email &&
                                           c.Id != updated.Id);
        if (found != null)
        {
            return null;
        }

        // update the information for the carer and save         
        user.Title = updated.Title;
        user.Firstname = updated.Firstname;
        user.Surname = updated.Surname;
        user.DOB = updated.DOB;
        user.Email = updated.Email;
        user.Password = Hasher.CalculateHash(updated.Password);
        user.Street = updated.Street;
        user.Town = updated.Town;
        user.County = updated.County;
        user.Postcode = updated.Postcode;
        user.MobileNumber = updated.MobileNumber;
        user.HomeNumber = updated.HomeNumber;
        user.NationalInsuranceNo = updated.NationalInsuranceNo;
        user.DBSCheck = updated.DBSCheck;
        user.Qualifications = updated.Qualifications;
        user.PhotoUrl = updated.PhotoUrl;
        user.Role = updated.Role;

        db.SaveChanges();
        return user;
    }

    public string ForgotPassword(string email)
    {
        var user = db.Users.FirstOrDefault(u => u.Email == email);
        if (user != null)
        {
            // invalidate any previous tokens
            db.ForgotPasswords
                .Where(t => t.Email == email && t.ExpiresAt > DateTime.Now).ToList()
                .ForEach(t => t.ExpiresAt = DateTime.Now);
            var f = new ForgotPassword { Email = email };
            db.ForgotPasswords.Add(f);
            db.SaveChanges();
            return f.Token;
        }
        return null;
    }

    public User ResetPassword(string email, string token, string password)
    {
        // find user by email
        var user = db.Users.FirstOrDefault(u => u.Email == email);
        if (user == null)
        {
            return null; // user not found
        }
        // find valid reset token for user
        var reset = db.ForgotPasswords
                       .FirstOrDefault(t => t.Email == email && t.Token == token && t.ExpiresAt > DateTime.Now);
        if (reset == null)
        {
            return null; // reset token invalid
        }

        // valid token and user so update password, invalidate the token and return the user           
        reset.ExpiresAt = DateTime.Now;
        user.Password = Hasher.CalculateHash(password);
        db.SaveChanges();
        return user;
    }

    public IList<string> GetValidPasswordResetTokens()
    {
        // return non expired tokens
        return db.ForgotPasswords.Where(t => t.ExpiresAt > DateTime.Now)
                                  .Select(t => t.Token)
                                  .ToList();
    }

    //==========================Patient Functions===========================================
    // Return a list of Patients 
    public IList<Patient> GetAllPatients(string order = null)
    {
        return db.Patients.ToList();
    }
    public IList<Patient> GetPatientsForMember(int memberId)
    {
        return db.FamilyMembers.Where(fm => fm.MemberId == memberId)
                 .Select(fm => fm.Patient)
                 .ToList();
    }

    //View specific patient by Id displaying information
    public Patient GetPatientById(int id)
    {
        return db.Patients
                    .Include(p => p.PatientConditions)
                    .ThenInclude(pc => pc.Condition)
                    .Include(p => p.CareEvents)
                    .ThenInclude(ce => ce.User)
                    .FirstOrDefault(p => p.Id == id);
    }

    public IList<Patient> SearchPatients(string query)
    {
        // TODO - query to search patient name, national ins etc containing query
        return new List<Patient>();
    }

    //Create a new patient and store to the Database
    public Patient AddPatient(Patient p)
    {
        //check patient being passed does not exist
        var exists = GetPatientById(p.Id);

        if (exists != null)
        {
            return null; // Patient cannot be added as they already exist
        }

        // update the information for the patient and save
        var patient = new Patient
        {
            Title = p.Title,
            Firstname = p.Firstname,
            Surname = p.Surname,
            NationalInsuranceNo = p.NationalInsuranceNo,
            DOB = p.DOB,
            Street = p.Street,
            Town = p.Town,
            County = p.County,
            Postcode = p.Postcode,
            HomeNumber = p.HomeNumber,
            MobileNumber = p.MobileNumber,
            Email = p.Email,
            GP = p.GP,
            SocialWorker = p.SocialWorker,
            CarePlan = p.CarePlan,
            PhotoUrl = p.PhotoUrl,

            // check for missing attributes - all here
        };

        //add patient to database
        db.Patients.Add(patient);
        db.SaveChanges();
        return patient;// return added patient to database
    }

    //Edit an existing patient (selected by name and dob) and update the database
    public Patient UpdatePatient(Patient updated)
    {
        // verify the patient exists
        var patient = GetPatientById(updated.Id);
        if (patient == null)
        {
            return null;
        }

        // check for another patient with the NationalInsuranceNo
        var found = db.Patients
                      .FirstOrDefault(p => p.NationalInsuranceNo == updated.NationalInsuranceNo &&
                                           p.Id != updated.Id);
        if (found != null)
        {
            return null;
        }

        // update the information for the patient and save
        patient.Title = updated.Title;
        patient.Firstname = updated.Firstname;
        patient.Surname = updated.Surname;
        patient.NationalInsuranceNo = updated.NationalInsuranceNo;
        patient.DOB = updated.DOB;
        patient.Street = updated.Street;
        patient.Town = updated.Town;
        patient.County = updated.County;
        patient.Postcode = updated.Postcode;
        patient.HomeNumber = updated.HomeNumber;
        patient.MobileNumber = updated.MobileNumber;
        patient.Email = updated.Email;
        patient.GP = updated.GP;
        patient.SocialWorker = updated.SocialWorker;
        patient.CarePlan = updated.CarePlan;
        patient.CareEvents = updated.CareEvents;
        // check for missing attributes

        db.SaveChanges();
        return patient;
    }

    // Delete the patient identified by Id returning true if deleted and false if not found
    public bool DeletePatient(int id)
    {
        var p = GetPatientById(id);
        if (p == null)
        {
            return false;
        }
        db.Patients.Remove(p);
        db.SaveChanges();
        return true;
    }

    //==========================Carer Functions===========================================
    // retrieve list of Carers with their main details
    public IList<User> GetAllCarers(string order = null)
    {
        return db.Users.Where(u => u.Role == Role.carer || u.Role == Role.manager)
                 .ToList();
    }

    // retrieve specific Carer with their main details
    public User GetCarerById(int id)
    {
        return db.Users
                 .Where(u => u.Role == Role.carer || u.Role == Role.manager)
                 .FirstOrDefault(c => c.Id == id);
    }
    public User GetCarerByUserId(int id)
    {
        return db.Users
                 .Where(u => u.Role == Role.carer || u.Role == Role.manager)
                 .FirstOrDefault(u => u.Id == id);
    }


    // retrieve specific Carer with their email
    public User GetCarerByEmail(string email)
    {
        return db.Users
                 .Where(u => u.Role == Role.carer)
                 .FirstOrDefault(c => c.Email == c.Email);

    }

    public User AddCarer(User c)
    {
        //check carer being passed does not exist
        var exists = GetCarerById(c.Id);
        if (exists != null)
        {
            return null; // the Carer already exists
        }

        // create the carer and save
        var carer = new User
        {
            Title = c.Title,
            Firstname = c.Firstname,
            Surname = c.Surname,
            NationalInsuranceNo = c.NationalInsuranceNo,
            DOB = c.DOB,
            DBSCheck = c.DBSCheck,
            Street = c.Street,
            Town = c.Town,
            County = c.County,
            Postcode = c.Postcode,
            MobileNumber = c.MobileNumber,
            HomeNumber = c.HomeNumber,
            Qualifications = c.Qualifications,
            Email = c.Email,
            PhotoUrl = c.PhotoUrl,
            Password = Hasher.CalculateHash(c.Password),
            Role = Role.carer
        };

        //add Carer to database
        db.Users.Add(carer);
        db.SaveChanges();
        return carer;
    }

    // Delete the carer identified by Id returning true if deleted and false if not found
    public bool DeleteCarer(int carerId)
    {
        var c = GetCarerById(carerId);
        if (c == null)
        {
            return false;
        }
        db.Users.Remove(c);
        db.SaveChanges();
        return true;
    }

    public User UpdateCarer(User updated)
    {
        // verify the carer exists
        var carer = GetCarerById(updated.Id);
        if (carer == null)
        {
            return null;
        }

        // check for another Carer with the NationalInsuranceNo
        var found = db.Users
                      .FirstOrDefault(c => c.NationalInsuranceNo == updated.NationalInsuranceNo &&
                                           c.Id != updated.Id);
        if (found != null)
        {
            return null;
        }

        // update the information for the carer and save
        carer.Title = updated.Title;
        carer.Firstname = updated.Firstname;
        carer.Surname = updated.Surname;
        carer.NationalInsuranceNo = updated.NationalInsuranceNo;
        carer.DOB = updated.DOB;
        carer.DBSCheck = updated.DBSCheck;
        carer.Street = updated.Street;
        carer.Town = updated.Town;
        carer.County = updated.County;
        carer.Postcode = updated.Postcode;
        carer.MobileNumber = updated.MobileNumber;
        carer.HomeNumber = updated.HomeNumber;
        carer.Qualifications = updated.Qualifications;
        carer.Email = updated.Email;
        carer.Password = Hasher.IsHashed(updated.Password) ? updated.Password : Hasher.CalculateHash(updated.Password);
        carer.Role = Role.carer;
        db.SaveChanges();
        return carer;
    }

    // ------------------ Member Management ----------------
    // retrieve list of Carers with their main details
    public IList<User> GetAllMembers(string order = null)
    {
        return db.Users.Where(u => u.Role == Role.family)
                 .ToList();
    }

    // retrieve specific Carer with their main details
    public User GetMemberById(int id)
    {
        return db.Users
                 .Where(u => u.Role == Role.family)
                 .FirstOrDefault(c => c.Id == id);
    }

    // retrieve specific Carer with their email
    public User GetMemberByEmail(string email)
    {
        return db.Users
                 .Where(u => u.Role == Role.family)
                 .FirstOrDefault(c => c.Email == email);

    }

    public User AddMember(User c)
    {
        //check member being passed does not exist
        var exists = GetMemberByEmail(c.Email);
        if (exists != null)
        {
            return null; // the member already exists
        }

        // create the member and save
        var member = new User
        {
            Firstname = c.Firstname,
            Surname = c.Surname,
            Email = c.Email,
            Street = c.Street,
            Town = c.Town,
            County = c.County,
            Postcode = c.Postcode,
            MobileNumber = c.MobileNumber,
            HomeNumber = c.HomeNumber,
            Role = Role.family,
            Password = Hasher.CalculateHash(c.Password)
        };

        //add Carer to database
        db.Users.Add(member);
        db.SaveChanges();
        return member;
    }

    // Delete the carer identified by Id returning true if deleted and false if not found
    public bool DeleteMember(int memberId)
    {
        var m = GetMemberById(memberId);
        if (m == null)
        {
            return false;
        }
        db.Users.Remove(m);
        db.SaveChanges();
        return true;
    }

    public User UpdateMember(User updated)
    {
        // verify the carer exists
        var member = GetMemberById(updated.Id);
        if (member == null)
        {
            return null;
        }

        // check for another member with the Email
        var found = db.Users
                      .FirstOrDefault(c => c.Email == updated.Email && c.Id != updated.Id);
        if (found != null)
        {
            return null;
        }

        // update the information for the member and save         
        member.Firstname = updated.Firstname;
        member.Surname = updated.Surname;
        member.DOB = updated.DOB;
        member.Email = updated.Email;
        member.Street = updated.Street;
        member.Town = updated.Town;
        member.County = updated.County;
        member.Postcode = updated.Postcode;
        member.MobileNumber = updated.MobileNumber;
        member.HomeNumber = updated.HomeNumber;
        member.Password = Hasher.IsHashed(updated.Password) ? updated.Password : Hasher.CalculateHash(updated.Password);

        db.SaveChanges();
        return member;
    }

    //  //======================CareEvent Management==================================

    public IList<PatientCareEvent> GetAllPatientCareEvents(string order = null)
    {
        return db.PatientCareEvents.ToList();

    }
    public PatientCareEvent GetPatientCareEventById(int id)
    {
        return db.PatientCareEvents
                 .Include(e => e.User)
                 .Include(e => e.Patient)
                 .FirstOrDefault(pce => pce.Id == id);
    }

    public IList<PatientCareEvent> GetScheduledPatientCareEventsForUser(int id)
    {
        return db.PatientCareEvents
                 .Include(e => e.User)
                 .Include(e => e.Patient)
                 .Where(pce => pce.UserId == id && pce.DateTimeCompleted == DateTime.MaxValue)
                 .ToList();
    }

    public PatientCareEvent SchedulePatientCareEvent(PatientCareEvent ce)
    {
        var last = db.PatientCareEvents.Where(pce => pce.PatientId == ce.PatientId)
                                       .OrderByDescending(ce => ce.DateTimeOfEvent)
                                       .FirstOrDefault();

        if (last is not null && last.DateTimeOfEvent >= ce.DateTimeOfEvent)
        {
            return null; // Careevent  cannot be added as it already exists
        }

        var patient = GetPatientById(ce.PatientId);
        var user = GetCarerById(ce.UserId);

        if (patient is null || user is null)
        {
            return null; // Careevent  cannot be added as as no such patient or user (carer)
        }

        var pce = new PatientCareEvent
        {
            DateTimeOfEvent = ce.DateTimeOfEvent,
            CarePlan = ce.CarePlan,
            PatientId = ce.PatientId,
            UserId = ce.UserId
            // Issues and DateTimeCompleted are not set at this time
        };

        //add pce to database
        db.PatientCareEvents.Add(pce);
        db.SaveChanges();
        return pce;
    }

    public PatientCareEvent CompletePatientCareEvent(PatientCareEvent ce)
    {
        var careevent = GetPatientCareEventById(ce.Id);
        if (careevent is null)
        {
            return null; // Careevent  does not exist
        }

        careevent.Issues = ce.Issues;
        careevent.DateTimeCompleted = ce.DateTimeCompleted;

        db.SaveChanges();
        return careevent;
    }


    public bool DeletePatientCareEvent(int careEventId)
    {
        var pce = db.PatientCareEvents.FirstOrDefault(e => e.Id == careEventId);
        if (pce == null)
        {
            return false;
        }
        db.PatientCareEvents.Remove(pce);
        db.SaveChanges();
        return true;
    }


    public PatientCareEvent UpdatePatientCareEvent(PatientCareEvent updated)
    {
        var patientCareEvent = GetPatientCareEventById(updated.Id);
        if (patientCareEvent == null)
        {
            return null;
        }
        // update patient care event      
        patientCareEvent.PatientId = updated.PatientId;
        patientCareEvent.UserId = updated.UserId;
        patientCareEvent.CarePlan = updated.CarePlan;
        patientCareEvent.Issues = updated.Issues;
        patientCareEvent.DateTimeOfEvent = updated.DateTimeOfEvent;

        db.SaveChanges();
        return patientCareEvent;
    }

    //  ====================== Condition Management==================================
    public IList<Condition> GetAllConditions(string order = null)
    {
        return db.Conditions.ToList();

    }

    // Condition GetConditionById(int id);
    public Condition GetConditionById(int id)
    {
        return db.Conditions.FirstOrDefault(c => c.Id == id);

        //  .Include(d => d.Description)
        // .FirstOrDefault(co => co.Id == id);

    }
    public Condition AddCondition(Condition con)
    {
        var exists = db.Conditions.Where(c => c.Name == con.Name).Any();

        if (exists)
        {
            return null; // Condition already exists
        }

        var condition = new Condition
        {
            Name = con.Name,
            Description = con.Description

        };
        //add condition to database
        db.Conditions.Add(condition);
        db.SaveChanges();
        return condition;
    }



    //Condition DeleteCondition(int id);
    public bool DeleteCondition(int id)
    {
        var dc = GetConditionById(id);
        if (dc == null)
        {
            return false;
        }
        db.Conditions.Remove(dc);
        db.SaveChanges();
        return true;
    }

    // Condition UpdateCondition(int id, Condition updated);
    public Condition UpdateCondition(Condition updated)
    {
        var condition = GetConditionById(updated.Id);
        if (condition == null)
        {
            return null;
        }

        condition.Name = updated.Name;
        condition.Description = updated.Description;
        condition.Id = updated.Id;

        db.SaveChanges();
        return condition;
    }

    //  ======================Patient Condition Management==================================

    public IList<PatientCondition> GetAllPatientConditions(int patientId)
    {
        return db.PatientConditions.Where(pc => pc.PatientId == patientId).ToList();
    }
    public PatientCondition GetPatientConditionById(int id)
    {
        return db.PatientConditions
                .Include(ce => ce.Patient)
                .FirstOrDefault(pc => pc.Id == id);

    }
    public PatientCondition AddPatientCondition(int patientId, int conditionId, string note, DateTime on)
    {
        var pc = db.PatientConditions.FirstOrDefault(pc => pc.PatientId == patientId && pc.ConditionId == conditionId);

        //check if patient condition already exists
        if (pc is not null)
        {
            return null;
        }

        // check patient and condition exist
        var patient = GetPatientById(patientId);
        var condition = GetConditionById(conditionId);
        if (patient is null || condition is null)
        {
            return null;
        }

        var patientCondition = new PatientCondition
        {
            PatientId = patientId,
            ConditionId = conditionId,
            Note = note,
            DateTimeConditionAdded = on
        };


        db.PatientConditions.Add(patientCondition);
        db.SaveChanges();
        return patientCondition;
    }

    public bool RemovePatientCondition(int conditionId)
    {
        var rpc = db.PatientConditions.FirstOrDefault(e => e.Id == conditionId);
        if (rpc == null)
        {
            return false;
        }

        db.PatientConditions.Remove(rpc);
        db.SaveChanges();
        return true;
    }

    public PatientCondition UpdatePatientCondition(PatientCondition updated)
    {
        var pc = GetPatientConditionById(updated.Id);
        if (pc == null)
        {
            return null;
        }
        // update note
        pc.Id = updated.Id;
        pc.Note = updated.Note;
        db.PatientConditions.Update(pc);
        db.SaveChanges();
        return pc;
    }

    //======================Patient Family Management==================================

    //  FamilyMember UpdatePatientFamilyMember(FamilyMember updated);
    //  bool RemovePatientFamilyMember(int memberId);
    public IList<FamilyMember> GetPatientFamilyMembers(int patientId)
    {
        return db.FamilyMembers
            .Include(fm => fm.Patient)
            .Include(fm => fm.Member)
            .Where(fm => fm.PatientId == patientId)
            .ToList();
    }
    public FamilyMember GetPatientFamilyMemberById(int patientId, int memberId)
    {
        return db.FamilyMembers
                 .Include(fm => fm.Patient)
                 .Include(fm => fm.Member)
                 .FirstOrDefault(fm => fm.MemberId == memberId && fm.PatientId == patientId);
    }

    public FamilyMember AddPatientFamilyMember(int patientId, int memberId, bool primary = false)
    {

        var patient = GetPatientById(patientId);
        var member = GetMemberById(memberId);

        if (patient == null || member == null)
        {
            return null; // Patient or Condition does not exist
        }

        var exists = GetPatientFamilyMemberById(patientId, memberId);
        if (exists is not null)
        {
            return null;
        }

        var fm = new FamilyMember
        {
            PatientId = patientId,
            MemberId = member.Id,
            Primary = primary,
        };
        db.FamilyMembers.Add(fm);
        db.SaveChanges();
        return fm;
    }

    public FamilyMember UpdatePatientFamilyMember(FamilyMember updated)
    {
        var fm = GetPatientFamilyMemberById(updated.PatientId, updated.MemberId);
        if (fm is null)
        {
            return null;
        }
        // update primary carer     
        fm.Primary = updated.Primary;
        db.FamilyMembers.Update(fm);
        db.SaveChanges();
        return fm;
    }

    public bool RemovePatientFamilyMember(FamilyMember fm)
    {
        var familyMember = GetPatientFamilyMemberById(fm.PatientId, fm.MemberId);
        if (familyMember is null)
        {
            return false;
        }
        db.FamilyMembers.Remove(familyMember);
        db.SaveChanges();
        return true;
    }
}


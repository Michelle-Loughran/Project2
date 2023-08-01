using CMS.Data.Entities;

namespace CMS.Data.Services
{
    public interface IPatientService
    {
        void Initialise();

        // user management
        IList<User> GetUsers();
        Paged<User> GetUsers(int page=1, int size=20, string orderBy="id", string direction="asc");
        User GetUser(int id);
        IList<User> GetUsersByRole(Role role);
        User GetUserByEmail(string email);
        bool IsEmailAvailable(string email, int userId);
        User AddUser(string firstname, string surname, string email, string password, Role role);
        User AddUser(User u);
        User Register(User u);
        User UpdateUser(User user);
        User Authenticate(string email, string password);
        string ForgotPassword(string email);
        User ResetPassword(string email, string token, string password);
        IList<string> GetValidPasswordResetTokens();

        //======================Patient Management==================================
        IList<Patient> GetAllPatients(string order = null);

        IList<Patient> GetPatientsForMember(int memberId);
        
        Patient GetPatientById(int id);
        IList<Patient> SearchPatients(string query);
        Patient AddPatient(Patient p);
        bool DeletePatient(int id);
        Patient UpdatePatient(Patient updated);
      
        //======================Carer Management==================================
        IList<User> GetAllCarers(string order=null);
        User GetCarerById(int id);
        User GetCarerByUserId(int id);
        User GetCarerByEmail(string email);
        User AddCarer(User c );
        bool DeleteCarer(int id);
        User UpdateCarer(User updated);

        //======================CareEvent Management==================================
        
        IList<PatientCareEvent> GetAllPatientCareEvents(string order=null);

        IList<PatientCareEvent> GetScheduledPatientCareEventsForUser(int userId);

        PatientCareEvent GetPatientCareEventById(int id);
        PatientCareEvent SchedulePatientCareEvent(PatientCareEvent ce);
        PatientCareEvent CompletePatientCareEvent(PatientCareEvent ce);
        
        bool DeletePatientCareEvent(int careEventId);
        PatientCareEvent UpdatePatientCareEvent(PatientCareEvent updated);

        //=============================Condition Management===================================
        IList<Condition> GetAllConditions(string order=null);
        Condition AddCondition(Condition condition);
        bool DeleteCondition(int id);
        Condition UpdateCondition(Condition updated);
        Condition GetConditionById(int id);
    

        //======================Patient Condition Management==================================
        IList<PatientCondition> GetAllPatientConditions(int patientId);
        PatientCondition GetPatientConditionById(int id);
        PatientCondition AddPatientCondition(int patientId, int conditionId, string note, DateTime on);
        bool RemovePatientCondition(int conditionId);
        PatientCondition UpdatePatientCondition(PatientCondition updated);


        //======================Family Member Management =================================
        IList<User> GetAllMembers(string order=null);
        User GetMemberById(int id);
        User GetMemberByEmail(string email);
        User AddMember(User c );
        bool DeleteMember(int id);
        User UpdateMember(User updated);

        //====================== Patient Family Management==================================

        IList<FamilyMember> GetPatientFamilyMembers(int patientId);
        FamilyMember GetPatientFamilyMemberById(int patientId, int memberId);
        FamilyMember AddPatientFamilyMember(int patientId, int memberId, bool primary = false);
        FamilyMember UpdatePatientFamilyMember(FamilyMember updated);
        bool RemovePatientFamilyMember(FamilyMember fm);

    }

}

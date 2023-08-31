
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

using CMS.Data.Entities;
using CMS.Data.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using CMS.Data.Security;
using CMS.Web.Models;

/**
 *  Appointment Management Controller
 */
namespace CMS.Web.Controllers
{
    [Authorize]

    public class AppointmentController : BaseController
    {
        private readonly IPatientService svc;

    public AppointmentController()
    {
        svc = new PatientServiceDb();
    }

        [Authorize(Roles="admin, carer, manager")]
        public IActionResult Index()
        {
            // load appointments using service and pass to view
            var appointments = svc.GetAllAppointments();
        
            return View(appointments);

        }    

        // HTTP POST - Add appointment 
        [Authorize(Roles="admin, manager")]
        public IActionResult AddAppointment(int id)
        { 
            var userId = User.GetSignedInUserId();
           //retrieve patient from service
           var patient = svc.GetPatientById(id);
           // check if the returned patient is  null and if so alert and return to index
            if (patient == null)
            {      
            Alert("Patient does not exist", AlertType.warning);  
              //patient = svc.GetPatientById(id);
              return RedirectToAction(nameof(Index),"Appointment");
            } 

            var avm = new AppointmentViewModel
            {
                Patient = patient,
                UserId = userId,
                PatientId = patient.Id,
                //retrieve Patient Name to display in form 
                PatientFirstname = patient.Firstname,
                PatientSurname = patient.Surname,
                // CarerFirstname = userId.
                // CarerSurname = User.CarerSurname,
                DateTimeCompleted = DateTime.Now,
                Role = Role.carer
            };
            ViewBag.Carers = new SelectList(svc.GetAllCarers(),"Id","Name");
            ViewBag.Patients = new SelectList(svc.GetAllPatients(),"Id","Name");
            ViewBag.Users = new SelectList(svc.GetUsers(),"Id","Name");

            return View("Create", avm);
        }
        //POST/Appointment/AddAppointment
        [ValidateAntiForgeryToken]
        [HttpPost]

        [Authorize(Roles = "admin,manager")]
        public IActionResult AddAppointment (int patientId,[Bind("PatientId,PatientFirstName, PatientSurname, CarerFirstname, CarerSurname, Date, Time, DateTimeCompleted, UserId ")] Appointment app)

        { 
            if (app == null)
            {
                Alert($"Appointment Event Does not exist {app.Id}", AlertType.warning);
                return RedirectToAction(nameof(Index));
            }
    
         if (ModelState.IsValid)
            {
                // call service AddAppointment method using data in app
                svc.AddAppointment(app);

                Alert("Appointment scheduled successfully!", AlertType.warning);

                return RedirectToAction(nameof(Index), "Appointment", new { Id = app.PatientId });
            }
                        // redisplay the form for editing as there are validation errors
            return View(app);
            }        
              // GET /patient/details/{id}
    public IActionResult PatientDetails(int id)
    { 
        // retrieve the patient with specified id from the service
        var patient = svc.GetPatientById(id);
      
        // check if patient is null and alert/redirect 
        if (patient == null) {
            Alert("Patient Does not Exist", AlertType.warning);
            return RedirectToAction(nameof(Index));
        }
        return View(patient);
    }
            //Patient exists so create new appointment using view model properties to populate the new appointment properties
           // complete POST action to add patient care event to database       
                
             

        //     [Authorize(Roles="admin, carer, manager")]
        //     public IActionResult Scheduled()
        // {
        //     // user will be a manager or a carer
        //     var userId = User.GetSignedInUserId();

        //     var scheduled = svc.GetAppointmentsBySignedInUser(userId);
            
        //     return View(scheduled);
        // }
        // [Authorize(Roles="admin, carer, manager")]
        //     public IActionResult Index()
        // {
        //     // user will be a manager or a carer
        //     var userId = User.GetSignedInUserId();

        //     var scheduled = svc.GetScheduledPatientCareEventsForUser(userId);
            
        //     return View(scheduled);
        // }
    }
}


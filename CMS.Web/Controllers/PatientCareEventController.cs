using Microsoft.AspNetCore.Mvc;

using CMS.Data.Entities;
using CMS.Data.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CMS.Web.Controllers
{
    [Authorize]
    public class PatientCareEventController : BaseController
    {
        private readonly IPatientService svc;

        public PatientCareEventController()
        {
            svc = new PatientServiceDb();
        }


        [Authorize(Roles="admin, carer, manager")]
        public IActionResult Index()
        {
            // load patientcare-events using service and pass to view
            
            var pce = svc.GetAllPatientCareEvents();
            if (pce is null)
            {
                Alert("Patient Care Event Does not Exist", AlertType.warning);
                return RedirectToAction(nameof(Index));
            }
            return View(pce);
        }

        [Authorize(Roles="carer, manager")]

        public IActionResult Scheduled()
        {
            // user will be a manager or a carer
            var userId = User.GetSignedInUserId();

            var scheduled = svc.GetScheduledPatientCareEventsForUser(userId);
            
            return View(scheduled);
        }

        // GET /Patient Care Event/details/{id}
        public IActionResult Details(int id)
        {
            var pce = svc.GetPatientCareEventById(id);

            // check if patientcare-events is null and alert/redirect 
            if (pce is null)
            {
                Alert("Patient Care Event Does not Exist", AlertType.warning);
                return RedirectToAction(nameof(Index));
            }

            return View(pce);
        }

        // GET: /patientcareevent/schedule/id   
        [Authorize(Roles="manager")]
        public IActionResult Schedule(int id)
        {
            var patient = svc.GetPatientById(id);
            if (patient is null)
            {
                Alert("Patient does not exist", AlertType.warning);
                return RedirectToAction(nameof(Index), "Patient");
            }     

            // display blank form to create a carer
            var pce = new PatientCareEvent
            {
                Patient = patient,
                PatientId = patient.Id,                              
                CarePlan = patient.CarePlan,                
            };

            //return the new Patient care-event to the view
            ViewBag.Carers = new SelectList(svc.GetAllCarers(),"Id","Name");
            return View(pce);
        }

        // POST: /patientcareevent/schedule/patientId   
        [ValidateAntiForgeryToken]
        [HttpPost]
        [Authorize(Roles="manager")]
        public IActionResult Schedule(int patientId, [Bind("DateTimeOfEvent, CarePlan, PatientId, UserId")] PatientCareEvent pce)
        {    // Check patient care event being passed in has Id preset before adding properties
            if (pce == null)
            {
                Alert($"Patient Care Event Does not exist {pce.Id}", AlertType.warning);
                return RedirectToAction(nameof(Index));
            }

            // complete POST action to add patient care event to database
            if (ModelState.IsValid)
            {
                // call service Addpatientcareevent method using data in pce
                svc.SchedulePatientCareEvent(pce);

                Alert("Patient-care-event scheduled successfully!", AlertType.warning);

                return RedirectToAction(nameof(Details), "Patient", new { Id = pce.PatientId });
            }

            // redisplay the form for editing as there are validation errors
            return View(pce);
        }


        // GET: /patientcareevent/complete/id   
        [Authorize(Roles="carer, manager")]
        public IActionResult Complete(int id)
        {
            var pce = svc.GetPatientCareEventById(id);
            if (pce is null)
            {
                Alert("Scheduled Care Event does not exist", AlertType.warning);
                return RedirectToAction(nameof(Index), "Patient");
            }            

            // set default completion to current date/time - will be updated in form
            pce.DateTimeCompleted = DateTime.Now;

            //return the new Patient care-event to the view
            return View(pce);
        }


        // POST /patientcareevent/complete/
        [ValidateAntiForgeryToken]
        [HttpPost]
        [Authorize(Roles="carer, manager")]
        public IActionResult Complete(int id, [Bind("Id, DateTimeOfEvent, DateTimeCompleted, CarePlan, Issues, PatientId, UserId")] PatientCareEvent pce)
        {   
            // complete POST action to add patient care event to database
            if (ModelState.IsValid)
            {
                // call service Addpatientcareevent method using data in pce
                svc.CompletePatientCareEvent(pce);

                Alert("Patient-care-event completed successfully!", AlertType.info);

                return RedirectToAction(nameof(Details), "Patient", new { Id = pce.PatientId });
            }
            
            // reload patient and user into model being sent back for validation
            var ce = svc.GetPatientCareEventById(id);
            pce.Patient = ce.Patient;
            pce.User = ce.User;

            // redisplay the form for editing as there are validation errors
            return View(pce);
        }


        // GET /PatientCareEvent/Delete/{id}
        public IActionResult Delete(int id)
        {
            var pce = svc.GetPatientCareEventById(id);

            // check if patientcare-events is null and alert/redirect 
            if (pce is null)
            {
                Alert("Patient Care Event Does not Exist", AlertType.warning);
                return RedirectToAction(nameof(Index));
            }

            return View(pce);
        }

        // POST /PatientCareEvent/Delete/{id}
        [HttpPost]
        public IActionResult DeleteConfirm(int id)
        {
            var ce = svc.GetPatientCareEventById(id);
            if (ce is null) 
            {
                Alert("Patient Care Event Could not be deleted", AlertType.warning); 
            }
           svc.DeletePatientCareEvent(id);           

           return RedirectToAction("Details", "Patient", new { id = ce.PatientId });
        }

    }
}

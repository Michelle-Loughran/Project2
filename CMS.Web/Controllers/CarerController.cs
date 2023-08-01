using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CMS.Data.Services;
using CMS.Data.Entities;

namespace CMS.Web.Controllers
{
    public class CarerController : BaseController
    {
        private readonly IPatientService svc;

    public CarerController()
    {
        svc = new PatientServiceDb();
    }

    // GET /Carers
    public IActionResult Index()
    {
        // load carers using service and pass to view
        var carers = svc.GetAllCarers();
        
        return View(carers);
    }

    // GET /carers/details/{id}
    public IActionResult Details(int id)
    {
        var carer = svc.GetCarerById(id);
      
        // check if carers is null and alert/redirect 
        if (carer is null) {
                Alert("Carer Does not Exist", AlertType.warning);
            return RedirectToAction(nameof(Index));
        }

        return View(carer);
    }

    // GET: /carers/create   
    public IActionResult Create()
    {
       
        //return the new carer to the view
        var carer = new User {
            Role=Role.carer
        };

        return View(carer);
    }

    // POST /carers/create
    //[ValidateAntiForgeryToken]
    [HttpPost]
    public IActionResult Create(User c)
    {   
      
        // complete POST action to add patient
        if (ModelState.IsValid)
        {
            // call service Addpatient method using data in s
            var carer = svc.AddCarer(c);
            if (carer is null) 
            {
                Alert("Issue creating the carer", AlertType.warning);
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Details), new { Id = carer.Id});   
        }
        
        // redisplay the form for editing as there are validation errors
        return View(c);
    }

    // GET /Carer/edit/{id}
    public IActionResult Edit(int id)
    {
        // load the Carer using the service
        var carer = svc.GetCarerById(id);

        // check if Carer is null and Alert/Redirect
        if (carer is null)
        {
            Alert("Carer not found", AlertType.warning);
            return RedirectToAction(nameof(Index));
        }  

        // pass patient to view for editing
        return View(carer);
    }

    // POST /patient/edit/{id}
    [ValidateAntiForgeryToken]
    [HttpPost]
    public IActionResult Edit(int id, User c)
    {       
        // complete POST action to save family member changes
        if (ModelState.IsValid)
        {            
            var carer = svc.GetCarerById(c.Id);
            if (carer is null) 
            {
                Alert("Issue editing the carer", AlertType.warning);
            }

            // redirect back to view the patient details
            return RedirectToAction(nameof(Details), new { Id = c.Id });
        }

        // redisplay the form for editing as validation errors
        return View(c);
    }

    // GET / carer/delete/{id}   
    public IActionResult Delete(int id)
    {
        // load the carer using the service
        var carer = svc.GetCarerById(id);
        // check the returned carer is not null and if so return NotFound()
        if (carer == null)
        {
            Alert("Carer not found", AlertType.warning);
            return RedirectToAction(nameof(Index));
        }     
        // pass carer to view for deletion confirmation
        return View(carer);
    }

    // POST /carer/delete/{id}
    [HttpPost]
    [ValidateAntiForgeryToken]   
    public IActionResult DeleteConfirm(int id)
    {
        // delete patient via service
        var deleted = svc.DeleteCarer(id);
        if (deleted)
        {
            Alert("Carer deleted", AlertType.success);            
        }
        else
        {
            Alert("Carer could not  be deleted", AlertType.warning);           
        }
        
        // redirect to the index view
        return RedirectToAction(nameof(Index));
    }

}
}

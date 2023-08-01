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
    [Route("[controller]")]
    public class FamilyController : BaseController
    {
        private readonly IPatientService svc;

    public FamilyController()
    {
        svc = new PatientServiceDb();
    }

    // GET /family
    public IActionResult Index()
    {
        // load patients using service and pass to view
        var families = svc.GetAllMembers();
        
        return View(families);
    }

    // GET /family/details/{id}
    public IActionResult Details(int id)
    {
        var family = svc.GetMemberById(id);
      
        // check if family is null and alert/redirect 
        if (family is null) {
            Alert("Family Member Does not Exist", AlertType.warning);
            return RedirectToAction(nameof(Index));
        }

        return View(family);
    }

    // GET: /family/create   
    public IActionResult Create()
    {
        // display blank form to create a family member
        var fm = new FamilyMember();
        //return the new family member to the view
        return View(fm);
    }

    // POST /family/create
    //[ValidateAntiForgeryToken]
    [HttpPost]
    public IActionResult Create(User fm)
    {   
      
        // complete POST action to add family member
        if (ModelState.IsValid)
        {
            // call service Addfamily method using data in svc
            var familymember = svc.AddMember(fm);
            if (familymember is null) 
            {
                Alert("Issue creating the family member", AlertType.warning);
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Details), new { Id = familymember.Id});   
        }
        
        // redisplay the form for editing as there are validation errors
        return View(fm);
    }

    // GET /family/edit/{id}
    public IActionResult Edit(int id)
    {
        // load the family member using the service
        var family = svc.GetMemberById(id);

        // check if family is null and Alert/Redirect
        if (family is null)
        {
            Alert("Family member not found", AlertType.warning);
            return RedirectToAction(nameof(Index));
        }  

        // pass patient to view for editing
        return View(family);
    }

    // POST /patient/edit/{id}
    [ValidateAntiForgeryToken]
    [HttpPost]
    public IActionResult Edit(int id, FamilyMember fm)
    {       
        // complete POST action to save family member changes
        if (ModelState.IsValid)
        {            
            var family = svc.GetMemberById(fm.Id);
            if (family is null) 
            {
                Alert("Issue editing the family member", AlertType.warning);
            }

            // redirect back to view the patient details
            return RedirectToAction(nameof(Details), new { Id = fm.Id });
        }

        // redisplay the form for editing as validation errors
        return View(fm);
    }

    // GET / family/delete/{id}   
    public IActionResult Delete(int id)
    {
        // load the family member using the service
        var family = svc.GetMemberById(id);
        // check the returned patient is not null and if so return NotFound()
        if (family == null)
        {
            Alert("Family Member not found", AlertType.warning);
            return RedirectToAction(nameof(Index));
        }     
        
        // pass patient to view for deletion confirmation
        return View(family);
    }

    // POST /family/delete/{id}
    [HttpPost]
    [ValidateAntiForgeryToken]   
    public IActionResult DeleteConfirm(int id)
    {
        // delete family member via service
        var deleted = svc.DeleteMember(id);
        if (deleted)
        {
            Alert("family member deleted", AlertType.success);            
        }
        else
        {
            Alert("family member could not  be deleted", AlertType.warning);           
        }
        
        // redirect to the index view
        return RedirectToAction(nameof(Index));
    }

}
}

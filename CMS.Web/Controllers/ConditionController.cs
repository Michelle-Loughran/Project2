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
    public class ConditionController : BaseController
    {
       private readonly IPatientService svc;

    public ConditionController()
    {
        svc = new PatientServiceDb();
    }

    // GET /Conditions
    public IActionResult Index()
    {
        // load Conditions using service and pass to view
        var conditions = svc.GetAllConditions();
        
        return View(conditions);
    }

    // GET /conditions/details/{id}
    public IActionResult Details(int id)
    {
        var condition = svc.GetConditionById(id);
      
        // check if condition is null and alert/redirect 
        if (condition is null) {
            Alert("Condition Does not Exist", AlertType.warning);
            return RedirectToAction(nameof(Index));
        }
        return View(condition);
    }

    // GET: /condition/create   
    public IActionResult Create()
    {
        var con  = new Condition();
        // display blank form to create a condition
        return View(con);
    }

    // POST /condition/create
    //[ValidateAntiForgeryToken]
    [HttpPost]
    public IActionResult Create(Condition con)
    {   
        // complete POST action to add condition
        if (ModelState.IsValid)
        {
            // call service Addcondition method using data in s
            var condition = svc.AddCondition(con);
            if (con is null) 
            {
                Alert("Issue creating the condition", AlertType.warning);
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Details), new { Id = condition.Id});   
        }
        
        // redisplay the form for editing as there are validation errors
        return View(con);
    }

    // GET /family/edit/{id}
    public IActionResult Edit(int id)
    {
        // load the condition using the service
        var condition = svc.GetConditionById(id);

        // check if condition is null and Alert/Redirect
        if (condition is null)
        {
            Alert("Condition not found", AlertType.warning);
            return RedirectToAction(nameof(Index));
        }  

        // pass patient to view for editing
        return View(condition);
    }

    // POST /patient/edit/{id}
    [ValidateAntiForgeryToken]
    [HttpPost]
    public IActionResult Edit(int id, Condition con)
    {       
        // complete POST action to save condition changes
        if (ModelState.IsValid)
        {            
            var condition = svc.UpdateCondition(con);
            if (condition is null) 
            {
                Alert("Issue editing the condition", AlertType.warning);
            }

            // redirect back to view the condition details
            return RedirectToAction(nameof(Details), new { Id = con.Id });
        }

        // redisplay the form for editing as validation errors
        return View(con);
    }

    // GET / condition/delete/{id}   
    public IActionResult Delete(int id)
    {
        // load the condition using the service
        var condition = svc.GetConditionById(id);
        // check the returned condition is not null and if so return NotFound()
        if (condition == null)
        {
            Alert("Condition not found", AlertType.warning);
            return RedirectToAction(nameof(Index));
        }     
        // pass condition to view for deletion confirmation
        return View(condition);
    }
    // POST /condition/delete/{id}
    [HttpPost]
    [ValidateAntiForgeryToken]   
    public IActionResult DeleteConfirm(int id)
    {
        // delete patient via service
        var deleted = svc.DeleteCondition(id);
        if (deleted)
        {
            Alert("Condition deleted", AlertType.success);            
        }
        else
        {
            Alert("Condition could not  be deleted", AlertType.warning);           
        }
        
        // redirect to the index view
        return RedirectToAction(nameof(Index));
    }

}
}

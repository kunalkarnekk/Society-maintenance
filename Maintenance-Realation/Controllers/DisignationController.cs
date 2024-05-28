using Microsoft.AspNetCore.Mvc;
using Society_DataAccess.Repository.IRepository;
using Society_Models;

namespace Maintenance_Realation.Controllers
{

    
    public class DisignationController : Controller
    {
        private readonly IUnitOfWorkRepository _unitOfWork;

        public DisignationController(IUnitOfWorkRepository unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            List<Disignation> obj = _unitOfWork.Disignation.GetAll().ToList();
            return View(obj);

           
        }

        public IActionResult Upsert(int? id)
        {
            if (id == 0 || id == null)
            {
                return View(new Disignation());
            }

            Disignation obj = new();

            obj = _unitOfWork.Disignation.Get(u => u.Id == id);

            if(obj == null)
            {
                return NotFound();
            }
            return View(obj);
        }

        [HttpPost]
        public IActionResult Upsert(Disignation disignation)
        {

           
            if (ModelState.IsValid)
            {
                if(disignation.Id == 0 )
                {
                    _unitOfWork.Disignation.Add(disignation);
                    TempData["success"] = "Disignation Added successfully";
                }
                
                else
                {
                        _unitOfWork.Disignation.Update(disignation);
                        TempData["success"] = "Disignation Updated Successfully";
                    
                }
                _unitOfWork.Save();
            }
            
            

            return RedirectToAction(nameof(Index));
        }


        public IActionResult Delete(int? id)
        {
            if(id == 0 || id == null)
            {
                return NotFound();
            }

            Disignation obj = _unitOfWork.Disignation.Get(u => u.Id == id);

            if(obj == null)
            {
                return NotFound();
            }

            _unitOfWork.Disignation.Remove(obj);
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

    }
}

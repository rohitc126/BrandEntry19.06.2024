using BusinessLayer;
using BusinessLayer.DAL;
using BusinessLayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace eSGBIZ.Controllers
{
    public class BrandController : BaseController
    {
        #region
        [Authorize]
        public ActionResult BrandEntry()
        {
            ViewBag.Header = "Brand Entry";
            Brand_Entry brEntry = new Brand_Entry();
            try
            {
                brEntry.BRAND_ID = 0;
                brEntry.oldBrandId = 0;
                List<MaterialMaster> materialList = new DAL_Common().GetMaterialList();
                brEntry.MaterialList = new SelectList(materialList, "Material_Id", "Material_Name");


                List<Unit_Master> UList = new DAL_Common().GetUnit();
                brEntry.UnitList = new SelectList(UList, "UOM_ID", "UOM");
            }
            catch (Exception ex)
            {
                Danger("<b>Exception occurred.</b>", true);
            }
            return View(brEntry);
        }

      #endregion

        public ActionResult _Brand_List()
        {
            return PartialView("_Brand_List");
        }

        #region
        [HttpPost]
        public ActionResult _Brand_ListDATA()
         {
             // Server Side Processing  
             int start = Convert.ToInt32(Request["start"]);
             int length = Convert.ToInt32(Request["length"]);
             string searchValue = Request["search[value]"];
             string sortColumnName = Request["columns[" + Request["order[0][column]"] + "][name]"];
             string sortDirection = Request["order[0][dir]"];
             int totalRow = 0;

             Brand_Entry obj = new Brand_Entry();

             List<Brand_List> _obj = new List<Brand_List>();
             try
             {

                 _obj = new DAL_BRAND().Get_Brand_Dtls();
                 totalRow = _obj.Count();
             }
             catch (Exception ex)
             {
                 Danger(string.Format("<b>Exception occured.</b>"), true);
             }

             if (!string.IsNullOrEmpty(searchValue)) // Filter Operation
             {
                 _obj = _obj.
                     Where(x => x.Material_Name.ToLower().Contains(searchValue.ToLower()) || x.Brand_Code.ToLower().Contains(searchValue.ToLower())
                         || x.Brand.ToLower().Contains(searchValue.ToLower()) || x.Rate.ToString().Contains(searchValue.ToLower())
                         || x.UOM.ToLower().Contains(searchValue.ToLower()) || x.Effective_Dt.ToLower().Contains(searchValue.ToLower()) || x.Remark.ToLower().Contains(searchValue.ToLower())
                          ).ToList<Brand_List>();

             }
             int totalRowFilter = _obj.Count();
            
             if (length == -1)
             {
                 length = totalRow;
             }
             _obj = _obj.Skip(start).Take(length).ToList<Brand_List>();

             var jsonResult = Json(new { data = _obj, draw = Request["draw"], recordsTotal = totalRow, recordsFiltered = totalRowFilter }, JsonRequestBehavior.AllowGet);
             jsonResult.MaxJsonLength = int.MaxValue;
             return jsonResult;
         }
        #endregion


        #region
        [HttpPost]
        [SubmitButtonSelector(Name = "Save")]
        [ActionName("BrandEntry")]
        public ActionResult BrandEntry_Save(Brand_Entry brEntry)
        {
            var errors = ModelState.Where(x => x.Value.Errors.Count > 0).Select(x => new { x.Key, x.Value.Errors }).ToArray();

            if (ModelState.IsValid)
            {
                try
                {
                    ResultMessage objMst;
                    string result = new DAL_BRAND().INSERT_BRAND(emp.Employee_Code, brEntry, out objMst);

                    if (result == "")
                    {
                        Success(string.Format("<b> Brand Inserted Successfully. Brand.: </b><b style ='color:red'>" + objMst.CODE + "</b>"), true);
                        return RedirectToAction("BrandEntry", "Brand");
                    }
                    else
                    {
                        Danger(string.Format("<b> Error:</b>" + result), true);
                        return RedirectToAction("BrandEntry", "Brand");
                    }
                }
                catch (Exception ex)
                {
                    Danger(string.Format("<b>Error:</b>" + ex.Message), true);
                    return RedirectToAction("BrandEntry", "Brand");
                }
            }
            else
            {
                Danger(string.Format("<b>Error:102:</b>" + string.Join(";", ModelState.Values.SelectMany(z => z.Errors).Select(z => z.ErrorMessage))), true);
                return RedirectToAction("BrandEntry", "Brand");
            }

            List<MaterialMaster> MaterialList = new DAL_Common().GetMaterialList();
            brEntry.MaterialList = new SelectList(MaterialList, "Material_Id, Material_Name");

            List<Unit_Master> UList = new DAL_Common().GetUnit();
            brEntry.UnitList = new SelectList(UList, "UOM_ID", "UOM");

            return View(brEntry);
        }
        #endregion

        #region
        [HttpPost]
        [SubmitButtonSelector(Name = "Update")]
        [ActionName("BrandEntry")]
        public ActionResult BrandEntry_Update(Brand_Entry brEntry)
        {
            var errors = ModelState.Where(x => x.Value.Errors.Count > 0).Select(x => new { x.Key, x.Value.Errors }).ToArray();

            if (ModelState.IsValid)
            {
                try
                {
                    ResultMessage objMst;
                    string result = new DAL_BRAND().Update_Brand(emp.Employee_Code, brEntry, out objMst);

                    if (result == "")
                    {
                        Success(string.Format("<b> Brand Entry successfully Updated. Brand.: </b><b style ='color:red'>" +objMst.CODE + "</b>"), true);
                        return RedirectToAction("BrandEntry", "Brand");
                    }
                    else
                    {
                        Danger(string.Format("<b>Error:</b>" + result), true);
                        return RedirectToAction("BrandEntry", "Brand");
                    }
                }
                catch (Exception ex)
                {
                    Danger(string.Format("<b>Error:</b>" + ex.Message), true);
                    return RedirectToAction("BrandEntry", "Brand");
                }
            }
            else
            {
                Danger(string.Format("<b>Error:102 :</b>" + string.Join("; ", ModelState.Values.SelectMany(z => z.Errors).Select(z => z.ErrorMessage))), true);
                return RedirectToAction("BrandEntry", "Brand");
            }

            List<MaterialMaster> MaterialList = new DAL_Common().GetMaterialList();
            brEntry.MaterialList = new SelectList(MaterialList, "Material_Id", "Material_Name");

            List<Unit_Master> UList = new DAL_Common().GetUnit();
            brEntry.UnitList = new SelectList(UList, "UOM_ID", "UOM");

            return View(brEntry);
        }
        #endregion


        #region
        [HttpPost]
[ActionName("DeleteBrand")]
public ActionResult DeleteBrand_Post(int brandID)
{
    try
    {
        ResultMessage objMst;
        Brand_Entry brEntry = new Brand_Entry
        {
            BRAND_ID = brandID
        };

        string result = new DAL_BRAND().Delete_Brand(emp.Employee_Code, brEntry, out objMst);

        if (string.IsNullOrEmpty(result))
        {
            var successMessage = string.Format("Brand deleted successfully. Brand ID: <b style='color:red'>{0}</b>", objMst.ID);
            return Json(new { success = true, message = successMessage });
        }
        else
        {
            var errorMessage = string.Format("Failed to delete the brand: <b>{0}</b>", result);
            return Json(new { success = false, message = errorMessage });
        }
    }
    catch (Exception ex)
    {
        var errorMessage = string.Format("An error occurred while deleting the brand: <b>{0}</b>", ex.Message);
        return Json(new { success = false, message = errorMessage });
    }
}
        #endregion
    }

}
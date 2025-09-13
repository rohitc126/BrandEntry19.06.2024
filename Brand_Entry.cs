using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace BusinessLayer.Entity
{
    public class Brand_Entry
    {
        public int? BRAND_ID { get; set; }
        [Required(ErrorMessage = "Select Product")]
        public int? Material_Id { get; set; }
        public SelectList MaterialList { get; set; }
        [Required(ErrorMessage ="Enter Brand")]
        public string Brand { get; set; }
        [Required(ErrorMessage ="Enter Rate")]
        public int Rate { get; set; }
        [Required(ErrorMessage = "Select Unit")]
        public int UOM_ID { get; set; }
        public SelectList UnitList { get; set; }

        [Required(ErrorMessage = "Select Effective Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> EffDate { get; set; }

        [Required(ErrorMessage = "Enter Remark")]
        public string Remark { get; set; }
        public string Brand_Code { get; set; }
        public List<Brand_List> brandlist { get; set; }



        public int? oldBrandId { get; set; }
    }

    public class Brand_List
    {
        public int BRAND_ID { get; set; }
        public int? Material_Id { get; set; }
        public string Material_Name { get; set; }
        public string Brand { get; set; }
        public int? Rate { get; set; }
        public int UOM_ID { get; set; }
        public string UOM { get; set; }
        public string Remark { get; set; }
        public string Effective_Dt { get; set; }
        public string Brand_Code { get; set; }
        public string Addby { get; set; }
        public DateTime Addon { get; set; }
      
    }
}

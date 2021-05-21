using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace STI_Online_Monitoring.Models
{
    public class VisitModel
    {

        [Key]
        [Display(Name = "VisitLogID")]
        public int VisitLogID { get; set; }
        //
        [Display(Name = "GuestID")]
        public int GuestID { get; set; }
        //
        [Required(ErrorMessage = "Date is required.")]
        public string DateOfVisit { get; set; }
        //
        [Display(Name = "TimeIn")]
        public string TimeIn { get; set; }
        //
        [Display(Name = "TimeOut")]
        public string TimeOut { get; set; }
        //

        [Required(ErrorMessage = "Department is required.")]
        public string Department { get; set; }
        //

        [StringLength(100, MinimumLength = 8, ErrorMessage = "Transaction must contain atleast 8 characters")]
        public string Transactions { get; set; }
        public List<VisitModel> Visitinfo { get; set; }
        //
        [Display(Name = "Status")]
        public string Status { get; set; }
    }
    public class GuestModel
    {
        [Key]
        [Display(Name = "GuestID")]
        public int GuestID { get; set; }
        //
        [Required]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Required. Must contain atleast 2 characters")]
        [Display(Name = "Last Name")]
        public string LasttName { get; set; }
        //
        [Required]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Required. Must contain atleast 2 characters")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        //
        [StringLength(50, MinimumLength = 0, ErrorMessage = "Muust not exceed to 30 characters")]
        [Display(Name = "Middle Name")]
        public string MiddleName { get; set; }
        //
        [StringLength(3, MinimumLength = 0, ErrorMessage = "Must contain atleast 3 characters")]
        [Display(Name = "Suffix")]
        public string Suffix { get; set; }
        //
        [Required(ErrorMessage = "Gender is required.")]
        [Display(Name = "Gender")]
        public string Gender { get; set; }
        //
        [Required]
        [StringLength(15, MinimumLength = 7, ErrorMessage = "Required. Must contain atleast 7-15 characters")]
        [Display(Name = "Contact Number")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Must be numeric")]
        public string ContactNumber { get; set; }
        //
        [Required]
        [StringLength(50, MinimumLength = 8, ErrorMessage = "Required. Must contain atleast 8-50 characters")]
        [Display(Name = "Address")]
        public string Address { get; set; }
        //

        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [StringLength(50, MinimumLength = 8, ErrorMessage = "Required. Must contain atleast 8-50 characters")]
        [Display(Name = "Email Address")]
        public string Email { get; set; }
        //
        [Key]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        //
        [Display(Name = "Type")]
        public string Type { get; set; }
        public string FullName()
        {
            return this.LasttName+ ", " + this.FirstName + " " + this.MiddleName +" "+this.Suffix;
        }

        //*************************************************************
        [Key]
        [Display(Name = "VisitLogID")]
        public int VisitLogID { get; set; }

        //
        
        [Required(ErrorMessage = "Date is required.")]
        public string DateOfVisit { get; set; }
        //
        
        [Display(Name = "TimeIn")]
        public string TimeIn { get; set; }
        //
        
        [Display(Name = "TimeOut")]
        public string TimeOut { get; set; }
        //

        [Required(ErrorMessage = "Department is required.")]
        public string Department { get; set; }
        //

        [StringLength(100, MinimumLength = 8, ErrorMessage = "Transaction must contain atleast 8 characters")]
        public string Transactions { get; set; }
        public List<GuestModel> Guestinfo { get; set; }
        //
       
       
    }

}
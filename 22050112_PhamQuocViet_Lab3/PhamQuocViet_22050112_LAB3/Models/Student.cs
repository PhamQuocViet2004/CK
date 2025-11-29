using System;
using System.ComponentModel.DataAnnotations;

namespace YourNamespace.Models
{
    public class Student
    {
        [Display(Name = "Mã học viên")]
        public string StudentId { get; set; }

        [Required]
        [Display(Name = "Họ và tên")]
        public string FullName { get; set; }

        [Required]
        [Display(Name = "Giới tính")]
        public string Gender { get; set; } // "Male" / "Female" / "Other"

        [DataType(DataType.Date)]
        [Display(Name = "Ngày sinh")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? DateOfBirth { get; set; }

        [Display(Name = "Học phí")]
        public decimal TuitionFee { get; set; }

        [Display(Name = "Ảnh")]
        public string ImageFileName { get; set; } // chỉ lưu tên file

        [Display(Name = "Ghi chú")]
        public string Notes { get; set; }
    }
}

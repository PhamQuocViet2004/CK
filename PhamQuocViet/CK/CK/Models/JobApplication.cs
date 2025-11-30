using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace CK.Models
{
    public class JobApplication
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Công việc ứng tuyển")]
        public int JobId { get; set; }

        [ForeignKey("JobId")]
        public virtual Job? Job { get; set; }

        [Display(Name = "Mã ứng viên")] // Hoặc "Tài khoản người nộp"
        public string? UserId { get; set; }

        [Display(Name = "File CV đính kèm")]
        public string? CVPatch { get; set; }

        [Display(Name = "Thư giới thiệu")]
        [StringLength(1000, ErrorMessage = "Thư giới thiệu không được dài quá 1000 ký tự")]
        public string? CoverLetter { get; set; }

        [Display(Name = "Trạng thái hồ sơ")]
        public string Status { get; set; } = "Pending";

        [Display(Name = "Ngày nộp hồ sơ")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}")] // Định dạng ngày giờ đẹp
        public DateTime AppliedDate { get; set; } = DateTime.Now;
    }
}
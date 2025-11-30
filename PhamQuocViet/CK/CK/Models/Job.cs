using System.ComponentModel.DataAnnotations;

namespace CK.Models
{
    public class Job
    {
        public int Id { get; set; }

        [Display(Name = "Tiêu đề công việc")]
        [Required(ErrorMessage = "Vui lòng nhập tiêu đề công việc")] // Bắt buộc nhập
        [StringLength(200, ErrorMessage = "Tiêu đề không được quá 200 ký tự")] // Giới hạn độ dài
        public string Title { get; set; } = string.Empty;

        [Display(Name = "Mô tả công việc")]
        [Required(ErrorMessage = "Vui lòng nhập mô tả công việc")] // Thêm bắt buộc
        public string? Description { get; set; }

        [Display(Name = "Yêu cầu ứng viên")]
        [Required(ErrorMessage = "Vui lòng nhập yêu cầu công việc")] // Thêm bắt buộc
        public string? Requirements { get; set; }

        [Display(Name = "Mức lương")]
        [Required(ErrorMessage = "Vui lòng nhập mức lương")] // Thêm bắt buộc
        [Range(0, 9999999999, ErrorMessage = "Mức lương phải lớn hơn 0")] // Chặn nhập số âm
        public decimal Salary { get; set; }

        [Display(Name = "Địa điểm làm việc")]
        [Required(ErrorMessage = "Vui lòng nhập địa điểm")] // Thêm bắt buộc
        public string? Location { get; set; }

        [Display(Name = "Ngày đăng tin")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [Display(Name = "Trạng thái hiển thị")]
        public bool IsActive { get; set; } = true;

        // Quan hệ: Một tin tuyển dụng có nhiều hồ sơ nộp vào
        public virtual ICollection<JobApplication>? JobApplications { get; set; }
    }
}
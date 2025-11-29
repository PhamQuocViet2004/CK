using System.ComponentModel.DataAnnotations;

namespace Lab4.Models
{
    public class Member2
    {
        [Required(ErrorMessage = "Vui lòng nhập tên đơn vị tuyển!")]
        public string CompanyName { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn loại hình doanh nghiệp!")]
        public string CompanyType { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập số lượng nhân viên!")]
        public int EmployeeCount { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập địa chỉ!")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập người liên hệ!")]
        public string ContactPerson { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập số điện thoại!")]
        public string Phone { get; set; }

        public string Fax { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập Email!")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ!")]
        public string Email { get; set; }

        public string Website { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên đăng nhập!")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập mật khẩu!")]
        [MinLength(5, ErrorMessage = "Mật khẩu phải lớn hơn 5 ký tự!")]
        public string Password { get; set; }

        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "Xác nhận mật khẩu không trùng khớp!")]
        public string ConfirmPassword { get; set; }

        public bool ReceiveEmail { get; set; }
    }
}

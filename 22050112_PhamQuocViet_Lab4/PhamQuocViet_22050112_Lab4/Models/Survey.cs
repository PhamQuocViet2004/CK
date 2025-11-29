using System.ComponentModel.DataAnnotations;

namespace Lab4.Models
{
    public class Survey
    {
        [Required(ErrorMessage = "Vui lòng nhập họ tên!")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập email!")]
        [EmailAddress(ErrorMessage = "Địa chỉ email không hợp lệ!")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tuổi!")]
        [Range(10, 100, ErrorMessage = "Tuổi phải từ 10 đến 100!")]
        public int Age { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn nghề nghiệp!")]
        public string Occupation { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn câu trả lời!")]
        public string Recommend { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn ít nhất một ngôn ngữ!")]
        public string[] KnownLanguages { get; set; }

        public string Comment { get; set; }
    }
}

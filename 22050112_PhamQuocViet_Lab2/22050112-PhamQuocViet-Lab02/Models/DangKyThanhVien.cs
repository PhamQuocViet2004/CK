using System.ComponentModel.DataAnnotations;

namespace LAB2.Models
{
    public class DangKyThanhVien
    {
        [Required(ErrorMessage = "Vui lòng nhập tên đơn vị")]
        public string TenDonVi { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn loại hình doanh nghiệp")]
        public string LoaiHinhDoanhNghiep { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Số lượng nhân viên phải > 0")]
        public int SoLuongNhanVien { get; set; }

        public string DiaChi { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập người liên hệ")]
        public string NguoiLienHe { get; set; }

        [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
        public string DienThoai { get; set; }

        public string SoFax { get; set; }

        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public string Email { get; set; }

        [Url(ErrorMessage = "Website không hợp lệ")]
        public string Website { get; set; }

        [Required(ErrorMessage = "Tên đăng nhập không được để trống")]
        public string TenDangNhap { get; set; }

        [Required(ErrorMessage = "Mật khẩu không được để trống")]
        [DataType(DataType.Password)]
        public string MatKhau { get; set; }

        [Compare("MatKhau", ErrorMessage = "Xác nhận mật khẩu không khớp")]
        [DataType(DataType.Password)]
        public string XacNhanMatKhau { get; set; }

        public bool NhanThuDienTu { get; set; }
    }
}

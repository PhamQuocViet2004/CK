namespace CK.Models
{
    public class DashboardViewModel
    {
        public int JobCount { get; set; } // Tổng số việc làm
        public int AppCount { get; set; } // Tổng số hồ sơ
        public int PendingCount { get; set; } // Hồ sơ chờ duyệt
        public int ApprovedCount { get; set; } // Hồ sơ đã nhận
    }
}
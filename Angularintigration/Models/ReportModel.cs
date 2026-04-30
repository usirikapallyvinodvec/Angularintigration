namespace Angularintigration.Models
{
    public class ReportModel
    {
        public int TotalUsers { get; set; }

        public int TotalModerators { get; set; }

        public int TotalAdmins { get; set; }

        public int TotalPosts { get; set; }

        public int ApprovedPosts { get; set; }

        public int PendingPosts { get; set; }

        public int RejectedPosts { get; set; }

        public int AnonymousPosts { get; set; }

        public int ActiveUsers { get; set; }

        public int BlockedUsers { get; set; }
    }
}

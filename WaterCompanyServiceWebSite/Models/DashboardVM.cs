namespace WaterCompanyServiceWebSite.Models
{
    public class DashboardVM
    {
        public SubscriptionForecast subscriptionForecast { get; set; }
        public float completedPer { get; set; }
        public float rejectedPer { get; set; }
        public float onprogressPer { get; set; }        
        public int completedCount { get; set; }
        public int rejectedCount { get; set; }
        public int onprogressCount { get; set; }
        public int totalRequests { get; set; }
    }
}

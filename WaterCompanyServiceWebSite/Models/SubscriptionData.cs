using Microsoft.ML.Data;

namespace WaterCompanyServiceWebSite.Models
{
    public class SubscriptionData
    {
        [LoadColumn(0)]
        public DateTime date { get; set; }

        [LoadColumn(1)]
        public float requests { get; set; }

        public SubscriptionData(DateTime date, int requests)
        {
            this.date = date;
            this.requests = requests;
        }
    }
}

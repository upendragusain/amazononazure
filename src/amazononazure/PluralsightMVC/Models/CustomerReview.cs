using System;

namespace PluralsightMVC.Models
{
    public class CustomerReview
    {
        public int Id { get; set; }

        public string Review { get; set; }

        public string CustomerName { get; set; }

        public Uri CustomerPicture { get; set; }

        public DateTime ReviewDate { get; set; }
    }
}

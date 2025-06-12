namespace OnlineCatering.Models.ViewModels
{
    public class BookingViewModel
    {
        public int BookingId { get; set; }
        public string CatererName { get; set; }
        public DateTime BookingDate { get; set; }
        public string Venue { get; set; }
        public decimal? BillAmount { get; set; }
        public string PaymentMode { get; set; }
        public string BookingStatus { get; set; }
        public List<string> MenuItems { get; set; }
    }

}

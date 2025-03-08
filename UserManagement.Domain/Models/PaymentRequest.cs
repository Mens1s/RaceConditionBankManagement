namespace UserManagement.Domain.Models
{
    public class PaymentRequest
    {
        public int TakerId { get; set; }
        public int GiverId { get; set; }
        public decimal Amount { get; set; }
    }
}

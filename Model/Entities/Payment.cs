namespace BookStoreApp.Model.Entities
{
    public class Payment
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int OrderId { get; set; }
        public decimal Amount { get; set; } // ödenecek tutar
        public PaymentMethod PaymentMethod { get; set; }
        public DateTime PaymentDate { get; set; }
        public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.Pending;

    }

    public enum PaymentMethod
    {
        CreditCard,
        Cash,
        BankTransfer,
        Paypal
    }

    public enum PaymentStatus
    {
        Pending,
        Completed,
        Failed
    }
}

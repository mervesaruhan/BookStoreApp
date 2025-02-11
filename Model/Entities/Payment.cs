namespace BookStoreApp.Model.Entities
{
    public class Payment
    {
        public int Id { get; set; }


        // User ile ilişki: bir kullanıcının birden çok ödemesi olabilir: bire - çok ilişki
        public int UserId { get; set; }
        public User User { get; set; }

        // Order ile ilişki: bir ödemenin bir siparişi olabilir: bire - bir ilişki
        public int OrderId { get; set; } 
        public Order Order { get; set; }


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

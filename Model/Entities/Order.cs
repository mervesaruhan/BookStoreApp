namespace BookStoreApp.Model.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime OrderDate { get; set; }
        public PaymentType PaymentType { get; set; }
        public PaymentStatus Status { get; set; }
    }

    public enum PaymentStatus
    {
        Pending,
        Completed,
        Failed

    }

    public enum PaymentType
    {
        CreditCard,
        EFT

    }
}

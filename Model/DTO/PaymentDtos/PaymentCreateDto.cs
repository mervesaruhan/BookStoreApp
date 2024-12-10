using BookStoreApp.Model.Entities;
using System.ComponentModel.DataAnnotations;

namespace BookStoreApp.Model.DTO.PaymentDtos
{
    public class PaymentCreateDto
    {
        [Required(ErrorMessage = "Kullanıcı ID gereklidir.")]
        [Range(1, int.MaxValue, ErrorMessage = "Geçerli bir kullanıcı ID'si giriniz.")]
        public int UsertId { get; set; }
        [Required(ErrorMessage = "Sipariş ID gereklidir.")]
        [Range(1, int.MaxValue, ErrorMessage = "Geçerli bir sipariş ID'si giriniz.")]
        public int OrderId { get; set; }
        [Required(ErrorMessage = "Ödeme tutarı gereklidir.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Ödeme tutarı sıfırdan büyük olmalıdır.")]
        public decimal Amount { get; set; }
        [Required(ErrorMessage = "Ödeme yöntemi gereklidir.")]
        public PaymentMethod PaymentMethod { get; set; }

    }
}

using BookStoreApp.Model.Entities;
using System.ComponentModel.DataAnnotations;

namespace BookStoreApp.Model.DTO.PaymentDtos
{
    public class PaymentCreateDto
    {

        [Required(ErrorMessage = "Ödeme yöntemi gereklidir.")]
        [EnumDataType(typeof(PaymentMethod), ErrorMessage = "Geçersiz ödeme yöntemi.")]
        public PaymentMethod PaymentMethod { get; set; }

    }
}

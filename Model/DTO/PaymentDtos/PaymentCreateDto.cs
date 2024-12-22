using BookStoreApp.Model.Entities;
using System.ComponentModel.DataAnnotations;

namespace BookStoreApp.Model.DTO.PaymentDtos
{
    public class PaymentCreateDto
    {
        //[Required(ErrorMessage = "Kullanıcı ID gereklidir.")]
        //[Range(1, int.MaxValue, ErrorMessage = "Geçerli bir kullanıcı ID'si giriniz.")]
        //public int UsertId { get; set; }


        [Required(ErrorMessage = "Ödeme yöntemi gereklidir.")]
        [EnumDataType(typeof(PaymentMethod), ErrorMessage = "Geçersiz ödeme yöntemi.")]
        public PaymentMethod PaymentMethod { get; set; }

    }
}

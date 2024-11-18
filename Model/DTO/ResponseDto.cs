namespace BookStoreApp.Model.DTO
{
    public class ResponseDto<T>
    {
        public T? Data { get; set; }//başarılı durum
        public List<string>? Errors { get; set; } //başarısız durumlar
    }
}

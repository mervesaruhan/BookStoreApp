namespace BookStoreApp.Model.DTO
{
    public class ResponseDto<T>
    {
        public T? Data { get; set; }//başarılı durum
        public List<string>? Errors { get; set; } //başarısız durumlar

        //static Factory method design pattern
        public static ResponseDto<T> Succes(T data)
        {
            return new ResponseDto<T>
            {
                Data = data
            };
        }

        //birden fazla hata mesajı döneceksem
        public static ResponseDto<T> Fail(List<string> errors)
        {
            return new ResponseDto<T>
            {
                Errors = errors
            };
        }
        //tek bir hata mesajı varsa (method overloading)
        public static ResponseDto<T> Fail(string message)
        {
            return new ResponseDto<T>
            {
                Errors = new List<string> { message }
            };

        }

    }
}

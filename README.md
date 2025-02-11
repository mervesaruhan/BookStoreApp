Bookstore API, kitap satışına odaklanan bir e-ticaret platformunun backend altyapısını oluşturan ASP.NET Core Web API uygulamasıdır.
Proje kapsamında MVC mimarisi kullanılmış olup, Entity Framework Core ile veritabanı bağlantısı tamamlanmıştır.

📌 Proje kapsamında uygulanan bazı teknikler:

✅ Model Binding ve DTO kullanımı ile veri akışı yönetimi

✅ Response Model oluşturma ile tutarlı API yanıtları sağlama

✅ Asenkron yapı (async/await) ile performansı artırma

✅ Extension Methods ile kod tekrarını azaltma ve yönetilebilirlik sağlama

✅ AutoMapper ile DTO ve Entity dönüşümlerini kolaylaştırma

✅ Fluent Validation ile model validasyonu

✅ MVC mimarisi ve Katmanlı Mimari prensipleri


🚀 Özellikler
📖 Kitap Yönetimi: Kitap ekleme, güncelleme, silme ve listeleme işlemleri

👤 Kullanıcı Yönetimi: Kullanıcı kaydı, giriş ve kimlik doğrulama işlemleri

🛒 Sipariş Yönetimi: Kullanıcıların kitap siparişi oluşturması ve sipariş geçmişinin görüntülenmesi

💳 Ödeme Entegrasyonu (Gelecek Planları): Siparişlere bağlı ödeme işlemlerinin entegre edilmesi

🔑 JWT ile Kimlik Doğrulama (Planlanan): Kullanıcıların güvenli bir şekilde giriş yapabilmesi için JWT entegrasyonu



📂 Veri Modeli
Proje, temel olarak aşağıdaki entityler üzerine kuruludur:


📖 Book (Kitap)

Kitap bilgilerini saklar.

 İlişki: Category ile Many-to-One ilişkisi vardır (Her kitap bir kategoriye ait olabilir).

 

 📂 Category (Kategori)
 
Kitapların kategorilendirilmesini sağlar.

🔗 İlişki: Book entity’si ile One-to-Many ilişkisi vardır (Bir kategori, birden fazla kitaba sahip olabilir).




👤 User (Kullanıcı)

Kullanıcı bilgilerini saklar.

🔗 İlişki: Order entity’si ile One-to-Many ilişkisi vardır (Bir kullanıcı birden fazla sipariş verebilir).




🛒 Order (Sipariş)

Kullanıcıların sipariş bilgilerini tutar.

 İlişki:
 

User ile Many-to-One ilişkisi vardır (Bir kullanıcı birden fazla sipariş verebilir).

OrderItem ile One-to-Many ilişkisi vardır (Bir sipariş, birden fazla kitap içerebilir).



📦 OrderItem (Sipariş Kalemi)

Bir siparişin içindeki kitapları saklar.

🔗 İlişki:


Order ile Many-to-One ilişkisi vardır (Bir sipariş birden fazla kitap içerebilir).

Book ile Many-to-One ilişkisi vardır (Bir kitap farklı siparişlerde yer alabilir).








🛠 KULLANILAN TEKNOLOJİLER

ASP.NET Core Web API – API geliştirme

Entity Framework Core – ORM (Veritabanı yönetimi)

MSSQL – Kalıcı veritabanı desteği

AutoMapper – DTO ve entity dönüşümleri

Fluent Validation – Model doğrulama

Swagger – API dokümantasyonu

JWT (Planlanan) – Kimlik doğrulama

Asenkron Programlama – Performans artırımı için

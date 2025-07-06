-- KuYuMe Appointment Systems
KuYuMe Appointment Systems, bireylerin ve kurumların çevrim içi ortamda güvenli, hızlı ve esnek bir şekilde randevu almasını sağlayan modern bir web tabanlı uygulamadır.
Proje, kullanıcı deneyimini artırmayı, hizmet süreçlerini dijitalleştirerek zaman ve kaynak kullanımını optimize etmeyi hedefler.

-- Özellikler
- ASP.NET Core 9.0 (MVC + Web API)
- Entity Framework Core (Code First + Migration)
- JWT ile güvenli kimlik doğrulama
- Rol tabanlı erişim kontrolü (Admin, Çalışan, Müşteri)
- Redis + In-Memory Cache ile yüksek performans
- E-posta bildirim sistemi (MailKit)
- Swagger UI ile uç nokta dokümantasyonu
- Docker konteyner desteği
- CI/CD için GitHub Actions entegrasyonu
- Serilog ile gelişmiş loglama altyapısı

-- Kullanıcı Rollerine Göre İşlevler
Müşteri: Hizmet seçme, çalışan filtreleme, randevu oluşturma/görüntüleme/iptal etme.

Çalışan: Kendisine atanmış randevuları yönetme.

Yönetici: Kullanıcı, çalışan ve hizmet yönetimi, istatistik takibi.

-- Güvenlik
ASP.NET Core Identity altyapısı

JWT tabanlı API kimlik doğrulama

XSS’e karşı input validasyonu ve encoding

SQL Injection’a karşı parametreli sorgular

⚙️ Teknolojiler
ASP.NET Core 9.0, C#

Entity Framework Core

SQL Server

Redis + In-Memory Cache

Docker

Swagger

Serilog

GitHub Actions

Bu proje eğitim amaçlıdır ve açık kaynak lisansı ile paylaşılmıştır.

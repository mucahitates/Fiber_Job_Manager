# FiberJobManager

FiberJobManager, Almanya’daki yer altı internet kablo döşeme projelerinde  
**iş takibini, revizyon kayıtlarını ve ekip yönetimini** kolaylaştırmak için geliştirilmiş bir sistemdir.

🎯 Amaç:  
DXF / CSV tabanlı saha işlerinde:

- ✔️ İşlerin kime atandığını görmek  
- ✔️ Revizyon tarihçesini izlemek  
- ✔️ Hangi kullanıcı neyi değiştirdi kaydetmek  
- ✔️ Merkezi, çok kullanıcılı bir yapı sağlamak  

---

## 🚀 Özellikler

### 👤 Kullanıcı Yönetimi
- Admin & Worker rolleri
- Kullanıcı ekleme / güncelleme / silme
- Silinen kullanıcıya bağlı işlerin devredilmesi

### 📌 İş Yönetimi
- İş oluşturma
- Kullanıcıya atama
- Durum değiştirme (Pending / InProgress / Done)
- Tüm kullanıcılar tüm işleri görebilir
- Sadece **atanmış kullanıcı** güncelleme yapabilir  
  (Admin her şeyi yönetebilir)

### 📝 Revizyon & Geçmiş

- Manuel revizyon notları ekleme
- **Otomatik revizyon kaydı**
  - İş güncellendiğinde
- İş bazlı history görüntüleme

---

## 🛠️ Kullanılan Teknolojiler

| Teknoloji | Amaç |
|----------|------|
| **ASP.NET Core Web API** | Backend API |
| **Entity Framework Core** | ORM (MySQL ile iletişim) |
| **MySQL** | Veritabanı |
| **Swagger (OpenAPI)** | API test & dokümantasyon |
| **JWT (planlı)** | Kimlik doğrulama |
| **WPF / Blazor Hybrid (planlı)** | Masaüstü arayüz |

---

## 📂 Proje Yapısı
```
FiberJobManager
├─FiberJobManager.Api
│  │
│  ├── Controllers # API uç noktaları
│  │ ├── AuthController.cs
│  │ ├── JobsController.cs
│  │ ├── RevisionsController.cs
│  │ ├── UsersController.cs
│  │
│  ├── Data
│  │ └── ApplicationDbContext.cs # Veritabanı erişimi
│  │
│  ├── Models
│  │ ├── Job.cs
│  │ ├── User.cs
│  │ ├── Revision.cs
│  │ ├── LoginRequest.cs
│  │ ├── LoginResponse.cs
│  │
│  ├── Migrations # EF Core migration dosyaları
│  │
│  └── Program.cs # Uygulama başlangıç noktası
│
├── FiberJobManager.Desktop
│  │
│  ├── Controllers # API uç noktaları
│  │ ├── MainWindow.xaml
│  │ ├── MainWindow.xaml.cs
│  │ ├── DashboardWindows.xaml
│  │ ├── DashboardWindows.xaml.cs
│  │ 
│
├──.gitignore
│
├──README.md


```
⚙️ Kurulum

### 1️⃣ Bağımlılıkları yükle
bash
dotnet restore

"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=fiber_db;User=root;Password=1234;"
}

dotnet ef database update
dotnet run
http://localhost:5210/swagger


🔌 API Özet

| Method | Endpoint          | Açıklama                           |
| ------ | ----------------- | ---------------------------------- |
| POST   | `/api/auth/login` | Kullanıcı giriş yapar, token döner |


👤 USERS

| Method | Endpoint               | Açıklama                         |
| ------ | ---------------------- | -------------------------------- |
| GET    | `/api/users`           | Tüm kullanıcılar                 |
| GET    | `/api/users/{id}`      | Kullanıcı getir                  |
| POST   | `/api/users`           | Yeni kullanıcı ekle              |
| PUT    | `/api/users/{id}`      | Kullanıcı güncelle               |
| DELETE | `/api/users/{id}`      | Kullanıcı sil (işler boşa düşer) |
| GET    | `/api/users/{id}/jobs` | Kullanıcıya atanmış işler        |

📌 JOBS

| Method | Endpoint                            | Açıklama                              |
| ------ | ----------------------------------- | ------------------------------------- |
| GET    | `/api/jobs`                         | Tüm işleri listele                    |
| POST   | `/api/jobs`                         | Yeni iş oluştur                       |
| GET    | `/api/jobs/{id}`                    | İşi getir                             |
| PUT    | `/api/jobs/{id}`                    | İşi güncelle                          |
| DELETE | `/api/jobs/{id}`                    | İşi sil                               |
| POST   | `/api/jobs/{jobId}/assign/{userId}` | İşi kullanıcıya ata                   |
| PUT    | `/api/jobs/{jobId}/update/{userId}` | Kullanıcının kendi işini güncellemesi |
| GET    | `/api/jobs/{jobId}/revisions`       | İş revizyon geçmişi                   |



📝 REVISIONS

| Method | Endpoint         | Açıklama             |
| ------ | ---------------- | -------------------- |
| POST   | `/api/revisions` | Manuel revizyon ekle |



Son geliştirmeler:

* Kullanıcı girişi

* JWT Token

* Role-based authorization

* Masaüstü (WPF / Blazor Hybrid) arayüz



🔐 Planlanan Geliştirmeler

* DXF / CSV dosya yönetimi



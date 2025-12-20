# 🎮 Happy Caro – Online Gomoku Game
# _NT106.Q14_NHOM4_

## Danh sách thành viên
1.	24520656	Đinh Võ Gia Huy
2.	24520514	Trần Lê Văn Hiếu
3.	24520661	Hoàng Gia Huy
4.	24520653	Cao Phan Đức Huy
5.	24521358	Nguyễn Hoàng Phú


## 📌 Giới thiệu đề tài

**Happy Caro** là ứng dụng game Caro (Gomoku) online được xây dựng theo mô hình **Client – Server**, cho phép người chơi đăng nhập, kết bạn, chat và thi đấu Caro trực tuyến theo thời gian thực.

Hệ thống sử dụng **Socket TCP/IP** để truyền và đồng bộ dữ liệu bàn cờ giữa các client, đảm bảo tính chính xác, ổn định và nhất quán trong suốt quá trình chơi.

Dự án được thực hiện trong khuôn khổ môn **Lập trình mạng căn bản (NT106)**.

---

## 🧩 Công nghệ sử dụng

* **Ngôn ngữ**: C# (.NET, WinForms)
* **Giao thức mạng**: TCP/IP
* **Mô hình**: Client – Server
* **Cơ sở dữ liệu**: SQL Server
* **Định dạng dữ liệu**: JSON

---

## ⚙️ Hướng dẫn cài đặt & cấu hình

### 1️⃣ Yêu cầu môi trường

* Windows 10 trở lên
* Visual Studio 2019 hoặc mới hơn
* SQL Server (Express / Developer)
* .NET Framework phù hợp với project

---

### 2️⃣ Tạo cơ sở dữ liệu

#### Bước 1: Tạo database

Mở **SQL Server Management Studio (SSMS)** và tạo database mới:

```sql
CREATE DATABASE HappyCaro;
```

#### Bước 2: Chạy file `setup.sql`

* Chọn database **HappyCaro**
* Mở file `setup.sql` trong thư mục project
* Execute file để tạo toàn bộ bảng dữ liệu cần thiết

> File `setup.sql` đã bao gồm đầy đủ các bảng và ràng buộc dữ liệu cho hệ thống.

---

### 3️⃣ Cấu hình Connection String

Do mỗi máy có cấu hình SQL Server khác nhau, **mỗi người cần tự chỉnh connection string cho phù hợp**.

#### Ví dụ sử dụng SQL Server local:

```csharp
Data Source=localhost;
Initial Catalog=HappyCaro;
Integrated Security=True;
```

#### Ví dụ sử dụng SQL Server Express:

```csharp
Data Source=.\\SQLEXPRESS;
Initial Catalog=HappyCaro;
Integrated Security=True;
```

📍 Connection string được cấu hình trong file:

* `App.config` (Client)
* `App.config` hoặc file cấu hình tương ứng (Server)

⚠️ **Lưu ý**:

* `Data Source` phụ thuộc vào instance SQL Server trên từng máy
* Không commit connection string chứa thông tin nhạy cảm (username/password) lên repository

---

### 4️⃣ Chạy chương trình

1. **Chạy Server trước**

   * Build & Run project Server
   * Kiểm tra server lắng nghe cổng TCP thành công

2. **Chạy Client**

   * Build & Run project Client
   * Đăng nhập để bắt đầu sử dụng hệ thống

---

## 🚀 Chức năng chính

* Đăng ký / đăng nhập tài khoản
* Kết bạn và chat realtime
* Tạo phòng / ghép cặp chơi Caro online
* Đồng bộ bàn cờ theo thời gian thực
* Xử lý ngắt kết nối và thoát trận

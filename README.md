# EddieShop
## Dự án trang Web bán hàng 
Bắt đầu thi công: 27/10/2021  
# 1. Công nghệ sử dụng
* Front-end: Vuejs
* Back-end: Asp.net Core Web API
* Database: MySQL  
# 2. Cách sử dụng 
* Clone project hoặc download file zip  
### 2.1. Database
* Cơ sở dữ liệu được config trong source code backend được chạy online trên một máy chủ nên không cần config gì  
* Nếu muốn chạy trên local hãy vào ('/SourceCode/Database) để lấy file backup vào hệ quản trị cơ sở dữ liệu của bạn và đổi cấu hình trong Backend trong file **appsettings.json** giống với cấu hình cơ sở dữ liệu của bạn  
### 2.2. Backend  
* Chạy project back-end và qua dịch vụ iis để chạy Swagger API (Khuyến khích sử dụng Visual Studio: Ctrl + F5, hãy kiểm tra bạn đã cài đặt các thư việc C# hỗ trợ bật project lên chưa)  
### 2.3. Frontent  
* Cài đặt môi trường nodejs cho máy của bạn (Lưu ý chọn những phiên bản phù hợp để chạy được phiên bản node-sass trong chương trình - Khuyến cáo version 12.13.0)   
* Cài packages cho chương trình (vào đúng thư mục cùng cấp với tệp package.json trong thư mục Frontent):  npm install
* Chạy project front-end: npm start || npm run dev || v.v (Tuỳ vào cài đặt npm)  
# 3. Một số tài khoản của ứng dụng:  
#### Admin: admin001 Pass: 12345678@Admin  
#### User: username0001 Pass: 12345678@User  
#### User: username0002 Pass: 12345678@User 

HƯỚNG DẪN CHẠY (Visual Studio 2022 hoặc CLI)

1. Giải nén file AspNetLoginAdminDemo.zip
2. Mở Visual Studio 2022 -> Open project -> chọn file AspNetLoginAdminDemo.csproj
3. Nhấn F5 để chạy project
4. Mở trình duyệt: http://localhost:5000/login.html để đăng nhập
   - Tài khoản: alice/pass1, bob/pass2, carol/pass3
5. Mở http://localhost:5000/admin.html để vào trang quản trị
   - Nhập admin key: adminkey123
   - Xem danh sách users, click “Clear token” để vô hiệu hóa đăng nhập user đó.
6. Có thể reset user bằng API:
   POST /api/admin/reset/{username}?key=adminkey123

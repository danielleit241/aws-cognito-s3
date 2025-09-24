# AWS Cognito S3 Image Manager

Ứng dụng mẫu sử dụng **AWS Cognito** để xác thực người dùng và **AWS S3** để lưu trữ/quản lý ảnh thông qua các API CRUD bảo mật. Người dùng đăng nhập qua giao diện Cognito, sau đó có thể upload, lấy, xoá, và lấy pre-signed URL ảnh qua các endpoint bảo vệ bởi token.

---

## Đăng nhập & Xác thực

- Ứng dụng sử dụng AWS Cognito để xác thực người dùng.
- Người dùng bấm vào link sau để đăng nhập:

  👉 [Đăng nhập với Cognito](https://ap-southeast-1nthbbp8qn.auth.ap-southeast-1.amazoncognito.com/login?client_id=514pnolv292n7poibd412lq2qk&response_type=code&scope=email+openid+phone&redirect_uri=https%3A%2F%2Fd84l1y8p4kdic.cloudfront.net)

- Sau khi đăng nhập thành công, mã code sẽ được chuyển về frontend (`https://d84l1y8p4kdic.cloudfront.net`) để lấy token và sử dụng các API.

---

## Các API chính

Các API đều yêu cầu xác thực (Bearer token từ Cognito).

- **Lấy thông tin claims người dùng**
  - Xem các thông tin claims của user hiện tại để kiểm tra quyền và thông tin đăng nhập.

- **Upload ảnh lên S3**
  - Cho phép upload file ảnh (tối đa 5MB) vào bucket S3, trả về đường dẫn truy cập ảnh.

- **Lấy ảnh từ S3**
  - Cho phép lấy nội dung ảnh từ S3 theo key.

- **Xoá ảnh khỏi S3**
  - Xoá file ảnh khỏi bucket theo key.

- **Lấy pre-signed URL**
  - Cung cấp pre-signed URL để truy cập ảnh từ S3 bằng link tạm thời, bảo mật.

---

## Ví dụ endpoint

| Method   | Endpoint                        | Mô tả                                  |
|----------|---------------------------------|----------------------------------------|
| GET      | `/api/claims`                   | Lấy thông tin claims người dùng        |
| POST     | `/api/images`                   | Upload ảnh mới lên S3                  |
| GET      | `/api/images/{key}`             | Lấy ảnh từ S3 theo key                 |
| DELETE   | `/api/images/{key}`             | Xoá ảnh khỏi S3                        |
| GET      | `/api/images/{key}/presigned`   | Lấy pre-signed URL cho ảnh             |

---

## Cấu trúc dự án

```plaintext
.
├── src/
│   ├── auth/           # Xử lý xác thực Cognito
│   ├── s3/             # Tương tác với AWS S3
│   ├── routes/         # Định nghĩa API CRUD ảnh
│   └── ...
├── README.md
├── ...
```

---

## Cài đặt & chạy thử

1. **Cài đặt dependencies**

```bash
dotnet restore
```
hoặc (nếu dùng NodeJS)
```bash
npm install
```

2. **Cấu hình biến môi trường**

Tạo file `.env` hoặc cập nhật `appsettings.json` với các giá trị:

```env
COGNITO_CLIENT_ID=514pnolv292n7poibd412lq2qk
COGNITO_DOMAIN=https://ap-southeast-1nthbbp8qn.auth.ap-southeast-1.amazoncognito.com
COGNITO_REDIRECT_URI=https://d84l1y8p4kdic.cloudfront.net
AWS_REGION=ap-southeast-1
S3_BUCKET_NAME=<your-s3-bucket>
```

3. **Chạy ứng dụng**

```bash
dotnet run
```
hoặc
```bash
npm start
```

---

## Công nghệ sử dụng

- [AWS Cognito](https://aws.amazon.com/cognito/) - Xác thực người dùng
- [AWS S3](https://aws.amazon.com/s3/) - Lưu trữ file ảnh

---

## Đóng góp

- Fork repo, tạo branch mới, commit & tạo pull request.
- Báo lỗi hoặc đề xuất: tạo issue mới trên repo.

---

## Liên hệ

- Tác giả: [danielleit241](https://github.com/danielleit241)
- Mọi đóng góp và phản hồi đều được hoan nghênh!

---

**Lưu ý bảo mật:** Không commit thông tin nhạy cảm (AWS Key, secret, v.v.) lên public repo!

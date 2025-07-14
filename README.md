# Fashion Store API

## Running the Project

### 1. Normal Run (localhost only):
```bash
dotnet run --project Fashion.Api
```

### 2. Run with Public URL (to access from other devices):
```bash
dotnet run --project Fashion.Api --launch-profile Public
```

Or:
```bash
dotnet run --project Fashion.Api --urls "http://0.0.0.0:5125;https://0.0.0.0:7225"
```

### 3. Run with specific profile:
```bash
dotnet run --project Fashion.Api --launch-profile https
```

## Available URLs:

- **HTTP:** `http://localhost:5125` or `http://0.0.0.0:5125`
- **HTTPS:** `https://localhost:7225` or `https://0.0.0.0:7225`
- **Swagger:** `https://localhost:7225/swagger`

## To access from other devices:

If you want to access from another device on the same network:

1. **Find your device IP:**
   ```bash
   ipconfig
   ```

2. **Use your device IP:**
   ```
   http://YOUR_IP_ADDRESS:5125
   https://YOUR_IP_ADDRESS:7225
   ```

## Usage Example:

### 1. Create Admin:
```bash
curl -X POST "http://YOUR_IP:5125/api/auth/admin/register" \
-H "Content-Type: application/json" \
-d '{
  "name": "Admin User",
  "phoneNumber": "+1234567890",
  "password": "123456",
  "confirmPassword": "123456",
  "adminSecretKey": "SuperSecretAdminKey123"
}'
```

### 2. Admin Login:
```bash
curl -X POST "http://YOUR_IP:5125/api/auth/admin/login" \
-H "Content-Type: application/json" \
-d '{
  "phoneNumber": "+1234567890",
  "password": "123456"
}'
```

### 3. Register Product (with token):
```bash
curl -X POST "http://YOUR_IP:5125/api/admin/dashboard/products" \
-H "Content-Type: application/json" \
-H "Authorization: Bearer YOUR_JWT_TOKEN" \
-d '{
  "name": "Product Name",
  "description": "Product Description",
  "price": 99.99,
  "category": 1,
  "style": 1,
  "productType": 1,
  "storeActivity": 1
}'
```

## Important Notes:

1. **CORS:** CORS is configured to allow access from any origin
2. **Authentication:** All protected endpoints require JWT token
3. **HTTPS:** In production, always use HTTPS
4. **Firewall:** Make sure to open ports 5125 and 7225 in firewall 
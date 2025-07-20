# Postman Testing Guide 🚀

## 📋 **خطوات الاستخدام:**

### 1. **استيراد Collection:**
1. افتح Postman
2. اضغط على `Import`
3. اختر ملف `FASHION_API_POSTMAN_COLLECTION.json`
4. اضغط `Import`

### 2. **إعداد Environment Variables:**

#### **Base URL:**
```
base_url: https://localhost:7001
```

#### **Tokens (سيتم تحديثها تلقائياً):**
```
manager_token: (سيتم تحديثه بعد Manager Login)
team_member_token: (سيتم تحديثه بعد Team Member Login)
user_token: (سيتم تحديثه بعد User Login)
token: (مشترك للجميع)
```

---

## 🔐 **Auth Endpoints Testing:**

### 1. **Manager Login:**
```bash
POST https://localhost:7001/api/auth/manager/login
Content-Type: application/json

{
    "phoneNumber": "0123456789"
}
```

**Expected Response:**
```json
{
    "success": true,
    "message": "تم تسجيل الدخول بنجاح",
    "data": {
        "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
        "manager": {
            "id": 1,
            "name": "Ahmed Manager",
            "phoneNumber": "0123456789",
            "role": "Manager"
        }
    }
}
```

**Copy the token and update `manager_token` variable**

### 2. **Team Member Login:**
```bash
POST https://localhost:7001/api/auth/team-member/login
Content-Type: application/json

{
    "name": "Ahmed Younis",
    "phoneNumber": "0123457789"
}
```

**Expected Response:**
```json
{
    "success": true,
    "message": "تم تسجيل الدخول بنجاح",
    "data": {
        "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
        "teamMember": {
            "id": 1,
            "firstName": "Ahmed",
            "lastName": "Younis",
            "fullName": "Ahmed Younis",
            "phoneNumber": "0123457789",
            "idNumber": "123456789",
            "department": "Sales",
            "role": "TeamMember",
            "isActive": true
        }
    }
}
```

**Copy the token and update `team_member_token` variable**

---

## 👥 **Team Member Management Testing:**

### 1. **Add Team Member (as Manager):**
```bash
POST https://localhost:7001/api/team-member/add
Authorization: Bearer {manager_token}
Content-Type: application/json

{
    "firstName": "Ahmed",
    "lastName": "Younis",
    "phoneNumber": "0123457789",
    "idNumber": "123456789",
    "department": "Sales"
}
```

**Expected Response:**
```json
{
    "success": true,
    "message": "تم إضافة عضو الفريق بنجاح",
    "data": {
        "id": 1,
        "firstName": "Ahmed",
        "lastName": "Younis",
        "fullName": "Ahmed Younis",
        "phoneNumber": "0123457789",
        "idNumber": "123456789",
        "department": "Sales",
        "isActive": true
    }
}
```

### 2. **Get Team Members (as Manager):**
```bash
GET https://localhost:7001/api/team-member/list
Authorization: Bearer {manager_token}
```

### 3. **Update Team Member (as Manager):**
```bash
PUT https://localhost:7001/api/team-member/update/1
Authorization: Bearer {manager_token}
Content-Type: application/json

{
    "firstName": "Ahmed",
    "lastName": "Younis",
    "phoneNumber": "0123457789",
    "idNumber": "123456789",
    "department": "Sales"
}
```

### 4. **Activate/Deactivate Team Member:**
```bash
PUT https://localhost:7001/api/team-member/activate/1
Authorization: Bearer {manager_token}

PUT https://localhost:7001/api/team-member/deactivate/1
Authorization: Bearer {manager_token}
```

### 5. **Delete Team Member:**
```bash
DELETE https://localhost:7001/api/team-member/delete/1
Authorization: Bearer {manager_token}
```

---

## 🏠 **Fitting Room Requests Testing:**

### 1. **Get Fitting Room Requests:**
```bash
GET https://localhost:7001/api/fitting-room-requests
Authorization: Bearer {token}
```

### 2. **Complete Request (Manager/Team Member):**
```bash
PUT https://localhost:7001/api/fitting-room-requests/1/complete
Authorization: Bearer {token}
```

### 3. **Cancel Request (User):**
```bash
PUT https://localhost:7001/api/fitting-room-requests/1/cancel
Authorization: Bearer {user_token}
```

### 4. **Delete Request (Manager/Team Member):**
```bash
DELETE https://localhost:7001/api/fitting-room-requests/1
Authorization: Bearer {token}
```

---

## 🎯 **Testing Sequence:**

### **Step 1: Manager Login**
1. Run "Manager Login" request
2. Copy token from response
3. Update `manager_token` variable

### **Step 2: Add Team Member**
1. Run "Add Team Member" request
2. Note the team member ID from response

### **Step 3: Team Member Login**
1. Run "Team Member Login" request
2. Copy token from response
3. Update `team_member_token` variable

### **Step 4: Test Team Member Management**
1. Run "Get Team Members"
2. Run "Update Team Member" (change ID in URL)
3. Run "Activate/Deactivate Team Member"
4. Run "Delete Team Member" (change ID in URL)

### **Step 5: Test Fitting Room Requests**
1. Run "Get Fitting Room Requests"
2. Run "Complete Fitting Room Request"
3. Run "Cancel Fitting Room Request"
4. Run "Delete Fitting Room Request"

---

## 🔧 **Environment Variables Setup:**

### **In Postman:**
1. Go to `Environments`
2. Create new environment: `Fashion API`
3. Add variables:
   - `base_url`: `https://localhost:7001`
   - `manager_token`: (empty initially)
   - `team_member_token`: (empty initially)
   - `user_token`: (empty initially)
   - `token`: (empty initially)

### **Token Management:**
- After each login, copy the token from response
- Update the corresponding variable
- Use `{{variable_name}}` in requests

---

## ⚠️ **Important Notes:**

### **1. Base URL:**
- تأكد من أن الـ API يعمل على `https://localhost:7001`
- إذا كان الـ port مختلف، قم بتحديث `base_url`

### **2. Database Data:**
- تأكد من وجود بيانات في الـ database
- إذا لم تكن هناك بيانات، قم بإنشاء Manager أولاً

### **3. Authorization:**
- جميع الـ requests تحتاج إلى `Authorization` header
- استخدم `Bearer {token}` format

### **4. Error Handling:**
- تحقق من الـ response status codes
- اقرأ رسائل الخطأ بعناية

---

## 🎉 **Expected Results:**

### ✅ **Success Responses:**
- Status: `200 OK`
- `success: true`
- `message`: رسالة نجاح بالعربية
- `data`: البيانات المطلوبة

### ❌ **Error Responses:**
- Status: `400 Bad Request` أو `401 Unauthorized`
- `success: false`
- `message`: رسالة خطأ بالعربية

**Happy Testing!** 🚀 
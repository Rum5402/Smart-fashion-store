# Deactivate Team Member Testing Guide 🔧

## 🔍 **المشكلة:**
عند إلغاء تفعيل عضو الفريق، يظل `isActive = true` في الـ response.

## 🚀 **خطوات الاختبار:**

### 1. **اختبار Deactivate:**
```bash
PUT https://localhost:7001/api/team-member/deactivate/2
Authorization: Bearer {manager_token}
```

**Expected Response:**
```json
{
    "success": true,
    "message": "تم إلغاء تفعيل عضو الفريق بنجاح",
    "data": true
}
```

### 2. **اختبار Get Team Members (للتحقق):**
```bash
GET https://localhost:7001/api/team-member/list
Authorization: Bearer {manager_token}
```

**Expected Response:**
```json
{
    "success": true,
    "message": "تم جلب أعضاء الفريق بنجاح",
    "data": [
        {
            "id": 2,
            "firstName": "Sara",
            "lastName": "Ahmed",
            "fullName": "Sara Ahmed",
            "phoneNumber": "0123457790",
            "idNumber": "123456790",
            "department": "Marketing",
            "isActive": false,  // يجب أن تكون false
            "lastLoginAt": null,
            "profileImageUrl": null,
            "createdAt": "2024-01-02T00:00:00Z"
        }
    ],
    "totalCount": 2,
    "activeCount": 1  // يجب أن ينقص واحد
}
```

### 3. **اختبار Activate (للتحقق من العكس):**
```bash
PUT https://localhost:7001/api/team-member/activate/2
Authorization: Bearer {manager_token}
```

**Expected Response:**
```json
{
    "success": true,
    "message": "تم تفعيل عضو الفريق بنجاح",
    "data": true
}
```

---

## 🔧 **Logging المضافة:**

### **Deactivate Logs:**
```
Team Member ID: 2, Current State: نشط
Team Member ID: 2, New State: غير نشط
```

### **Activate Logs:**
```
Team Member ID: 2, Current State: غير نشط
Team Member ID: 2, New State: نشط
```

---

## ⚠️ **نقاط التحقق:**

### 1. **Authorization:**
- ✅ تأكد من أن الـ token صحيح
- ✅ تأكد من أن المستخدم Manager

### 2. **Team Member ID:**
- ✅ تأكد من أن ID = 2 موجود
- ✅ تأكد من أن المدير يملك الصلاحية

### 3. **Database:**
- ✅ تأكد من أن الـ database متصل
- ✅ تأكد من أن الـ changes محفوظة

### 4. **Caching:**
- ✅ تأكد من عدم وجود caching
- ✅ جرب refresh للـ response

---

## 🎯 **خطوات Debugging:**

### **Step 1: Check Logs**
1. شغل الـ API
2. نفذ Deactivate request
3. تحقق من الـ console logs

### **Step 2: Check Database**
1. افتح الـ database
2. تحقق من جدول TeamMembers
3. تأكد من أن IsActive = false

### **Step 3: Check Response**
1. نفذ Get Team Members
2. تحقق من الـ response
3. تأكد من أن isActive = false

---

## 🔄 **Testing Sequence:**

### **1. Get Current State:**
```bash
GET /api/team-member/list
```

### **2. Deactivate Member:**
```bash
PUT /api/team-member/deactivate/2
```

### **3. Verify Deactivation:**
```bash
GET /api/team-member/list
```

### **4. Activate Member:**
```bash
PUT /api/team-member/activate/2
```

### **5. Verify Activation:**
```bash
GET /api/team-member/list
```

---

## 🎉 **Expected Results:**

### **After Deactivate:**
- ✅ `isActive: false` في الـ response
- ✅ `activeCount` ينقص واحد
- ✅ Logs تظهر التغيير

### **After Activate:**
- ✅ `isActive: true` في الـ response
- ✅ `activeCount` يزيد واحد
- ✅ Logs تظهر التغيير

**إذا لم تعمل، تحقق من الـ logs في الـ console!** 🔍 
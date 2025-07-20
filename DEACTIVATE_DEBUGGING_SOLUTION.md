# Deactivation Debugging Solution 🔧

## 🔍 **المشكلة:**
عند إلغاء تفعيل عضو الفريق، يظل `isActive = true` في الـ response.

## ✅ **الحلول المطبقة:**

### 1. **إضافة Logging شامل:**
```csharp
// في DeactivateTeamMemberAsync
Console.WriteLine($"Team Member ID: {teamMemberId}, Current State: {currentState}");
Console.WriteLine($"Team Member ID: {teamMemberId}, New State: غير نشط");

// في GetTeamMembersAsync
Console.WriteLine($"Manager ID: {managerId}, Total Team Members: {teamMembers.Count}");
foreach (var tm in teamMembers)
{
    var status = tm.IsActive ? "نشط" : "غير نشط";
    Console.WriteLine($"Team Member ID: {tm.Id}, Name: {tm.FullName}, Status: {status}");
}
```

### 2. **إضافة GetActiveTeamMembersAsync:**
```csharp
public async Task<ApiResponse<List<TeamMemberDto>>> GetActiveTeamMembersAsync(int managerId)
{
    var activeTeamMembers = allTeamMembers.Where(tm => 
        tm.ManagerId == managerId && tm.IsActive).ToList();
    
    // Returns only active team members
}
```

### 3. **إضافة Endpoint جديد:**
```csharp
[HttpGet("active")]
[AuthorizeRoles("Manager")]
public async Task<ActionResult<ApiResponse<List<TeamMemberDto>>>> GetActiveTeamMembers()
```

---

## 🚀 **خطوات الاختبار:**

### 1. **اختبار Deactivate مع Logging:**
```bash
PUT https://localhost:7001/api/team-member/deactivate/2
Authorization: Bearer {manager_token}
```

**تحقق من الـ Console Logs:**
```
Team Member ID: 2, Current State: نشط
Team Member ID: 2, New State: غير نشط
```

### 2. **اختبار Get All Team Members:**
```bash
GET https://localhost:7001/api/team-member/list
Authorization: Bearer {manager_token}
```

**تحقق من الـ Console Logs:**
```
Manager ID: 1, Total Team Members: 2
Team Member ID: 1, Name: Ahmed Younis, Status: نشط
Team Member ID: 2, Name: Sara Ahmed, Status: غير نشط
Total Count: 2, Active Count: 1
```

### 3. **اختبار Get Active Team Members فقط:**
```bash
GET https://localhost:7001/api/team-member/active
Authorization: Bearer {manager_token}
```

**Expected Response:**
```json
{
    "success": true,
    "message": "تم جلب الأعضاء النشطين بنجاح",
    "data": [
        {
            "id": 1,
            "firstName": "Ahmed",
            "lastName": "Younis",
            "fullName": "Ahmed Younis",
            "phoneNumber": "0123457789",
            "idNumber": "123456789",
            "department": "Sales",
            "isActive": true
        }
    ],
    "totalCount": 1,
    "activeCount": 1
}
```

---

## 🎯 **نقاط التحقق:**

### 1. **Database Changes:**
- ✅ تأكد من أن `IsActive = false` في الـ database
- ✅ تأكد من أن `UpdatedAt` تم تحديثه

### 2. **Console Logs:**
- ✅ تحقق من الـ logs في الـ console
- ✅ تأكد من أن الـ state تغير

### 3. **Response Validation:**
- ✅ تحقق من الـ response مباشرة
- ✅ تأكد من عدم وجود caching

---

## 🔄 **Testing Sequence:**

### **Step 1: Get Current State**
```bash
GET /api/team-member/list
```
تحقق من الـ logs في الـ console

### **Step 2: Deactivate Member**
```bash
PUT /api/team-member/deactivate/2
```
تحقق من الـ logs في الـ console

### **Step 3: Verify Deactivation**
```bash
GET /api/team-member/list
```
تحقق من الـ logs في الـ console

### **Step 4: Get Only Active Members**
```bash
GET /api/team-member/active
```
تحقق من أن العضو غير موجود في الـ response

---

## 🎉 **Expected Results:**

### **After Deactivate:**
- ✅ Console logs تظهر التغيير
- ✅ `isActive: false` في الـ response
- ✅ `activeCount` ينقص واحد
- ✅ العضو لا يظهر في `/active` endpoint

### **After Activate:**
- ✅ Console logs تظهر التغيير
- ✅ `isActive: true` في الـ response
- ✅ `activeCount` يزيد واحد
- ✅ العضو يظهر في `/active` endpoint

---

## ⚠️ **إذا لم تعمل:**

### 1. **تحقق من الـ Database:**
```sql
SELECT Id, FirstName, LastName, IsActive, UpdatedAt 
FROM TeamMembers 
WHERE ManagerId = 1;
```

### 2. **تحقق من الـ Console Logs:**
- تأكد من أن الـ API يعمل
- تحقق من الـ logs في الـ console

### 3. **تحقق من الـ Authorization:**
- تأكد من أن الـ token صحيح
- تأكد من أن المستخدم Manager

**الآن جرب الاختبار وتحقق من الـ console logs!** 🔍 
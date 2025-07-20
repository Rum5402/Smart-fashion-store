# Controllers Explanation - Fitting Room System

## Why Two Controllers?

### 1. `FittingRoomController` - إدارة غرف الملابس
**Route**: `/api/fitting-rooms`
**Purpose**: إدارة غرف الملابس نفسها (للعرض والإحصائيات فقط)

#### Endpoints:
- `GET /api/fitting-rooms` - عرض جميع غرف الملابس
- `GET /api/fitting-rooms/status` - إحصائيات غرف الملابس
- `GET /api/fitting-rooms/available` - عرض الغرف المتاحة

#### Users:
- Staff/Manager (للإدارة والمراقبة)

#### Function:
- **Display only** - للعرض والإحصائيات
- **No assignment** - لا يتم تعيين غرف محددة
- **Statistics** - إحصائيات استخدام الغرف

---

### 2. `FittingRoomRequestController` - إدارة طلبات غرف الملابس
**Route**: `/api/fitting-room-requests`
**Purpose**: إدارة طلبات غرف الملابس (الوظيفة الرئيسية)

#### Endpoints:
- `POST /api/fitting-room-requests` - إنشاء طلب جديد (Customer)
- `GET /api/fitting-room-requests` - عرض جميع الطلبات (Manager)
- `GET /api/fitting-room-requests/my-requests` - عرض طلباتي (Customer)
- `PUT /api/fitting-room-requests/{id}` - تحديث طلب (Manager)
- `GET /api/fitting-room-requests/status/{status}` - طلبات بحالة معينة

#### Users:
- **Customers**: إنشاء طلبات وعرض طلباتهم
- **Managers**: إدارة جميع الطلبات

#### Function:
- **Core functionality** - الوظيفة الرئيسية للنظام
- **Request management** - إدارة الطلبات
- **Automatic responses** - الردود التلقائية

---

## النظام الجديد (التلقائي)

### قبل التغيير:
```
Customer → Request → Manager → Assign Room → Customer
```

### بعد التغيير:
```
Customer → Request → System → Auto Response → Customer
```

## الفرق في الاستخدام

### `FittingRoomController` (للإدارة فقط):
```javascript
// للعرض والإحصائيات
GET /api/fitting-rooms/status
// Response: { totalRooms: 10, availableRooms: 8, occupiedRooms: 2 }

GET /api/fitting-rooms/available  
// Response: [{ id: 1, roomNumber: "A1", isAvailable: true }, ...]
```

### `FittingRoomRequestController` (الوظيفة الرئيسية):
```javascript
// إنشاء طلب
POST /api/fitting-room-requests
// Request: { itemId: 123 }
// Response: "The item will be ready in the fitting room within 2 minutes"

// عرض طلباتي
GET /api/fitting-room-requests/my-requests
// Response: [{ id: 1, itemName: "Blue Jacket", status: "Completed" }, ...]
```

## التوصية

### الحفاظ على كلا الـ Controllers لأن:

1. **`FittingRoomRequestController`**: الوظيفة الرئيسية للنظام
2. **`FittingRoomController`**: للإدارة والمراقبة والإحصائيات

### إذا أردت تبسيط أكثر:
يمكن حذف `FittingRoomController` والاعتماد فقط على `FittingRoomRequestController` لأن النظام أصبح تلقائي.

## الخلاصة

- **`FittingRoomRequestController`**: للنظام الرئيسي (إدارة الطلبات)
- **`FittingRoomController`**: للإدارة والمراقبة (اختياري) 
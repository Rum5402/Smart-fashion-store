# Store Control Center API Guide

## نظرة عامة

تم تحديث Store Controller ليتوافق مع واجهة المستخدم المطلوبة للوحة تحكم المتجر. يوفر الـ API إدارة شاملة للفئات، الفلاتر، البانرات، وإعدادات العلامة التجارية.

## التصنيفات (Categories)

### إدارة الفئات

#### 1. الحصول على جميع الفئات
```http
GET /api/store/categories
Authorization: Bearer {token}
```

**الاستجابة:**
```json
{
  "success": true,
  "message": "Categories retrieved successfully",
  "data": [
    {
      "id": 1,
      "name": "Women",
      "description": "Women's clothing category",
      "imageUrl": "/images/categories/women.jpg",
      "parentCategoryId": null,
      "parentCategoryName": null,
      "displayOrder": 1,
      "isActive": true,
      "createdAt": "2024-01-01T00:00:00Z",
      "updatedAt": null,
      "subCategories": [
        {
          "id": 2,
          "name": "Dresses",
          "description": "Women's dresses",
          "imageUrl": "/images/categories/dresses.jpg",
          "parentCategoryId": 1,
          "parentCategoryName": "Women",
          "displayOrder": 1,
          "isActive": true,
          "itemsCount": 20
        }
      ],
      "itemsCount": 50
    }
  ],
  "totalCount": 1
}
```

#### 2. إنشاء فئة جديدة
```http
POST /api/store/categories
Authorization: Bearer {token}
Content-Type: application/json

{
  "name": "Women",
  "description": "Women's clothing category",
  "imageUrl": "/images/categories/women.jpg",
  "parentCategoryId": null,
  "displayOrder": 1
}
```

#### 3. إنشاء فئة فرعية
```http
POST /api/store/categories
Authorization: Bearer {token}
Content-Type: application/json

{
  "name": "Dresses",
  "description": "Women's dresses",
  "imageUrl": "/images/categories/dresses.jpg",
  "parentCategoryId": 1,
  "displayOrder": 1
}
```

#### 4. تحديث فئة
```http
PUT /api/store/categories/{id}
Authorization: Bearer {token}
Content-Type: application/json

{
  "name": "Women's Fashion",
  "description": "Updated women's clothing category",
  "imageUrl": "/images/categories/women-updated.jpg",
  "parentCategoryId": null,
  "displayOrder": 1,
  "isActive": true
}
```

#### 5. تبديل حالة الفئة (نشط/غير نشط)
```http
PATCH /api/store/categories/{id}/toggle-status
Authorization: Bearer {token}
```

#### 6. حذف فئة
```http
DELETE /api/store/categories/{id}
Authorization: Bearer {token}
```

## الفلاتر (Filters)

### إدارة الفلاتر

#### 1. الحصول على جميع الفلاتر
```http
GET /api/store/filters
Authorization: Bearer {token}
```

**الاستجابة:**
```json
{
  "success": true,
  "message": "Filters retrieved successfully",
  "data": [
    {
      "id": 1,
      "name": "Size",
      "description": "Product size filter",
      "type": 0,
      "selectionType": 1,
      "options": ["XS", "S", "M", "L", "XL", "XXL"],
      "isActive": true,
      "displayOrder": 1,
      "createdAt": "2024-01-01T00:00:00Z",
      "updatedAt": null
    }
  ],
  "totalCount": 1
}
```

#### 2. إنشاء فلتر الحجم
```http
POST /api/store/filters
Authorization: Bearer {token}
Content-Type: application/json

{
  "name": "Size",
  "description": "Product size filter",
  "type": 0,
  "selectionType": 1,
  "options": ["XS", "S", "M", "L", "XL", "XXL"],
  "displayOrder": 1
}
```

#### 3. إنشاء فلتر اللون
```http
POST /api/store/filters
Authorization: Bearer {token}
Content-Type: application/json

{
  "name": "Color",
  "description": "Product color filter",
  "type": 2,
  "selectionType": 1,
  "options": ["Red", "White", "Black", "Blue", "Green"],
  "displayOrder": 2
}
```

#### 4. إنشاء فلتر النوع
```http
POST /api/store/filters
Authorization: Bearer {token}
Content-Type: application/json

{
  "name": "Type",
  "description": "Product type filter",
  "type": 1,
  "selectionType": 0,
  "options": ["Shirt", "Bottom", "Dress", "Skirt", "Jacket"],
  "displayOrder": 3
}
```

#### 5. إنشاء فلتر النمط
```http
POST /api/store/filters
Authorization: Bearer {token}
Content-Type: application/json

{
  "name": "Style",
  "description": "Product style filter",
  "type": 3,
  "selectionType": 1,
  "options": ["Sport", "Casual", "Formal", "Vintage"],
  "displayOrder": 4
}
```

#### 6. إنشاء فلتر السعر
```http
POST /api/store/filters
Authorization: Bearer {token}
Content-Type: application/json

{
  "name": "Price",
  "description": "Product price filter",
  "type": 4,
  "selectionType": 0,
  "options": ["100-200", "200-300", "300-500", "500+"],
  "displayOrder": 5
}
```

#### 7. إنشاء فلتر الترويج
```http
POST /api/store/filters
Authorization: Bearer {token}
Content-Type: application/json

{
  "name": "Promotion",
  "description": "Product promotion filter",
  "type": 5,
  "selectionType": 0,
  "options": ["New Arrival", "On Sale", "Clearance"],
  "displayOrder": 6
}
```

#### 8. تحديث فلتر
```http
PUT /api/store/filters/{id}
Authorization: Bearer {token}
Content-Type: application/json

{
  "name": "Size Filter",
  "description": "Updated product size filter",
  "type": 0,
  "selectionType": 1,
  "options": ["XS", "S", "M", "L", "XL", "XXL", "XXXL"],
  "displayOrder": 1,
  "isActive": true
}
```

#### 9. تبديل حالة الفلتر
```http
PATCH /api/store/filters/{id}/toggle-status
Authorization: Bearer {token}
```

#### 10. حذف فلتر
```http
DELETE /api/store/filters/{id}
Authorization: Bearer {token}
```

## البانرات (Banners)

### إدارة البانرات

#### 1. الحصول على جميع البانرات
```http
GET /api/store/banners
Authorization: Bearer {token}
```

**الاستجابة:**
```json
{
  "success": true,
  "message": "Banners retrieved successfully",
  "data": [
    {
      "id": 1,
      "name": "Summer Collection 2024",
      "imageUrl": "/images/banners/summer-collection.jpg",
      "linkUrl": "/collections/summer",
      "isActive": true,
      "startDate": null,
      "endDate": null,
      "displayOrder": 1,
      "createdAt": "2024-01-01T00:00:00Z",
      "updatedAt": null
    }
  ],
  "totalCount": 1
}
```

#### 2. إنشاء بانر جديد
```http
POST /api/store/banners
Authorization: Bearer {token}
Content-Type: application/json

{
  "name": "Summer Collection 2024",
  "imageUrl": "/images/banners/summer-collection.jpg",
  "linkUrl": "/collections/summer",
  "displayOrder": 1
}
```

#### 3. تحديث بانر
```http
PUT /api/store/banners/{id}
Authorization: Bearer {token}
Content-Type: application/json

{
  "name": "Summer Collection 2024 Updated",
  "imageUrl": "/images/banners/summer-collection-updated.jpg",
  "linkUrl": "/collections/summer-2024",
  "displayOrder": 1,
  "isActive": true
}
```

#### 4. تبديل حالة البانر
```http
PATCH /api/store/banners/{id}/toggle-status
Authorization: Bearer {token}
```

#### 5. حذف بانر
```http
DELETE /api/store/banners/{id}
Authorization: Bearer {token}
```

## إعدادات العلامة التجارية (Brand Settings)

### إدارة إعدادات العلامة التجارية

#### 1. الحصول على إعدادات العلامة التجارية
```http
GET /api/store/brand-settings
Authorization: Bearer {token}
```

**الاستجابة:**
```json
{
  "success": true,
  "message": "Brand settings retrieved successfully",
  "data": {
    "id": 1,
    "storeName": "ZARA Fashion Store",
    "tagline": "Fashion that speaks your style",
    "logoUrl": "/images/logo/zara-logo.png",
    "primaryColor": "#0000FF",
    "isActive": true,
    "createdAt": "2024-01-01T00:00:00Z",
    "updatedAt": null
  }
}
```

#### 2. تحديث إعدادات العلامة التجارية
```http
PUT /api/store/brand-settings
Authorization: Bearer {token}
Content-Type: application/json

{
  "storeName": "ZARA Fashion Store",
  "tagline": "Fashion that speaks your style",
  "logoUrl": "/images/logo/zara-logo.png",
  "primaryColor": "#0000FF"
}
```

## النقاط العامة (Public Endpoints)

### النقاط المتاحة للجميع

#### 1. معلومات المتجر
```http
GET /api/store/info
```

#### 2. موقع المتجر
```http
GET /api/store/location
```

#### 3. معلومات الاتصال
```http
GET /api/store/contact
```

#### 4. وصف المتجر
```http
GET /api/store/description
```

#### 5. بانرات المتجر (عامة)
```http
GET /api/store/banners/public
```

#### 6. فئات المتجر (عامة)
```http
GET /api/store/categories/public
```

#### 7. فلاتر المتجر (عامة)
```http
GET /api/store/filters/public
```

#### 8. مجموعات الفلاتر
```http
GET /api/store/filter-presets
```

## أنواع الفلاتر (Filter Types)

| القيمة | النوع | الوصف |
|--------|-------|-------|
| 0 | Size | حجم المنتج |
| 1 | Type | نوع المنتج |
| 2 | Color | لون المنتج |
| 3 | Style | نمط المنتج |
| 4 | Price | سعر المنتج |
| 5 | Promotion | ترويج المنتج |

## أنواع الاختيار (Selection Types)

| القيمة | النوع | الوصف |
|--------|-------|-------|
| 0 | Single | اختيار واحد |
| 1 | Multi | اختيار متعدد |

## رموز الحالة (HTTP Status Codes)

| الرمز | الوصف |
|-------|-------|
| 200 | نجح الطلب |
| 201 | تم إنشاء المورد بنجاح |
| 400 | بيانات الطلب غير صحيحة |
| 401 | غير مصرح (يحتاج مصادقة) |
| 403 | محظور (يحتاج صلاحيات) |
| 404 | المورد غير موجود |
| 500 | خطأ في الخادم |

## ملاحظات مهمة

1. **المصادقة**: جميع نقاط الإدارة تتطلب مصادقة JWT مع صلاحيات Admin أو Manager
2. **التحقق من البيانات**: يتم التحقق من صحة البيانات المرسلة
3. **التسلسل الهرمي**: تدعم الفئات التسلسل الهرمي (فئات رئيسية وفرعية)
4. **الحالة**: يمكن تبديل حالة أي عنصر (نشط/غير نشط)
5. **النقاط العامة**: متاحة للجميع ولا تحتاج مصادقة

## أمثلة الاستخدام

### إنشاء فئة مع فئات فرعية
```bash
# 1. إنشاء الفئة الرئيسية
curl -X POST "https://localhost:7001/api/store/categories" \
  -H "Authorization: Bearer YOUR_TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Women",
    "description": "Women's clothing",
    "displayOrder": 1
  }'

# 2. إنشاء فئة فرعية
curl -X POST "https://localhost:7001/api/store/categories" \
  -H "Authorization: Bearer YOUR_TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Dresses",
    "description": "Women's dresses",
    "parentCategoryId": 1,
    "displayOrder": 1
  }'
```

### إنشاء فلتر متعدد الاختيارات
```bash
curl -X POST "https://localhost:7001/api/store/filters" \
  -H "Authorization: Bearer YOUR_TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Size",
    "description": "Product size filter",
    "type": 0,
    "selectionType": 1,
    "options": ["XS", "S", "M", "L", "XL", "XXL"],
    "displayOrder": 1
  }'
```

### تحديث إعدادات العلامة التجارية
```bash
curl -X PUT "https://localhost:7001/api/store/brand-settings" \
  -H "Authorization: Bearer YOUR_TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "storeName": "ZARA Fashion Store",
    "tagline": "Fashion that speaks your style",
    "logoUrl": "/images/logo/zara-logo.png",
    "primaryColor": "#0000FF"
  }'
``` 
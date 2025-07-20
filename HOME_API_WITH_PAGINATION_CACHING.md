# Home API with Pagination & Caching - Fashion Store

## 🚀 الميزات الجديدة

### ✅ **Pagination (الصفحات)**
- دعم الصفحات للمنتجات الكثيرة
- إمكانية تحديد عدد العناصر في الصفحة
- ترتيب المنتجات حسب معايير مختلفة
- معلومات الصفحات (الصفحة الحالية، إجمالي الصفحات، إلخ)

### ✅ **Caching (التخزين المؤقت)**
- تخزين مؤقت للبيانات لتحسين الأداء
- إحصائيات الـ cache (hit rate, miss rate)
- إدارة الـ cache (حذف، مسح، إلخ)
- انتهاء صلاحية تلقائي للبيانات

## 📋 API Endpoints الجديدة

### 1. Pagination Endpoints

#### الحصول على منتجات مع pagination
```http
GET /api/home/shop/{category}?page=1&pageSize=10&sortBy=price&sortDirection=asc
```

**Parameters:**
- `page`: رقم الصفحة (افتراضي: 1)
- `pageSize`: عدد العناصر في الصفحة (افتراضي: 10)
- `sortBy`: حقل الترتيب (name, price, createdAt, category, style, productType)
- `sortDirection`: اتجاه الترتيب (asc, desc)

**Response:**
```json
{
  "success": true,
  "message": "Products for Woman category retrieved successfully",
  "data": {
    "category": "Woman",
    "products": [...],
    "totalCount": 25,
    "filterOptions": {...}
  }
}
```

#### الحصول على منتجات مع pagination متقدم
```http
GET /api/home/shop/{category}/paginated?page=1&pageSize=10&sortBy=price&sortDirection=desc
```

**Response:**
```json
{
  "success": true,
  "message": "Paginated products for Woman category retrieved successfully",
  "data": {
    "data": [...],
    "totalCount": 25,
    "totalPages": 3,
    "currentPage": 1,
    "pageSize": 10,
    "hasNextPage": true,
    "hasPreviousPage": false,
    "nextPage": 2,
    "previousPage": null,
    "firstPage": 1,
    "lastPage": 3
  }
}
```

### 2. Cache Management Endpoints

#### الحصول على إحصائيات الـ cache
```http
GET /api/cache/stats
```

**Response:**
```json
{
  "success": true,
  "message": "Cache statistics retrieved successfully",
  "data": {
    "totalEntries": 15,
    "memoryUsage": 1048576,
    "hitRate": 85.5,
    "missRate": 14.5
  }
}
```

#### مسح جميع الـ cache
```http
DELETE /api/cache/clear
```

#### حذف عنصر من الـ cache
```http
DELETE /api/cache/remove/{key}
```

#### حذف عناصر من الـ cache حسب النمط
```http
DELETE /api/cache/remove-pattern?pattern=home.*
```

#### التحقق من وجود عنصر في الـ cache
```http
GET /api/cache/exists/{key}
```

#### مسح cache الصفحة الرئيسية
```http
DELETE /api/cache/clear-home
```

#### مسح cache المتجر لفئة معينة
```http
DELETE /api/cache/clear-shop/{category}
```

## 🔧 Cache Configuration

### Cache Durations (مدة التخزين المؤقت)

| Data Type | Duration | Reason |
|-----------|----------|---------|
| Home Page Data | 15 minutes | بيانات متغيرة بشكل متوسط |
| Banners | 30 minutes | بيانات ثابتة نسبياً |
| New Collection | 20 minutes | بيانات متغيرة |
| Best Sellers | 25 minutes | بيانات متغيرة |
| On Sale | 15 minutes | بيانات متغيرة بسرعة |
| Categories | 60 minutes | بيانات ثابتة |
| Featured Products | 20 minutes | بيانات متغيرة |
| Filter Options | 30 minutes | بيانات ثابتة نسبياً |
| Shop Category | 10 minutes | بيانات متغيرة |
| Paginated Products | 10 minutes | بيانات متغيرة |

### Cache Keys (مفاتيح التخزين المؤقت)

```csharp
// Home page data
"home_page_data"

// Individual sections
"banners"
"new_collection"
"best_sellers"
"on_sale"
"featured_products"
"categories"

// Shop by category
"shop_category_{category}_{page}_{pageSize}"

// Paginated products
"paginated_products_{category}_{page}_{pageSize}_{sortBy}_{sortDirection}"

// Filter options
"filter_options_{category}"
```

## 📊 Cache Statistics

### Hit Rate (معدل الإصابات)
- **85%+**: أداء ممتاز
- **70-85%**: أداء جيد
- **<70%**: يحتاج تحسين

### Memory Usage (استخدام الذاكرة)
- يتم مراقبة استخدام الذاكرة
- تنظيف تلقائي عند الحاجة

## 🎯 أمثلة الاستخدام

### React Example
```javascript
// الحصول على منتجات مع pagination
const getPaginatedProducts = async (category, page = 1, pageSize = 10) => {
  try {
    const response = await fetch(
      `/api/home/shop/${category}/paginated?page=${page}&pageSize=${pageSize}&sortBy=price&sortDirection=desc`
    );
    const data = await response.json();
    
    if (data.success) {
      const { data: products, totalPages, currentPage, hasNextPage, hasPreviousPage } = data.data;
      
      setProducts(products);
      setPagination({
        currentPage,
        totalPages,
        hasNextPage,
        hasPreviousPage
      });
    }
  } catch (error) {
    console.error('Error fetching paginated products:', error);
  }
};

// إدارة الـ cache
const clearHomeCache = async () => {
  try {
    await fetch('/api/cache/clear-home', { method: 'DELETE' });
    console.log('Home cache cleared');
  } catch (error) {
    console.error('Error clearing cache:', error);
  }
};

const getCacheStats = async () => {
  try {
    const response = await fetch('/api/cache/stats');
    const data = await response.json();
    
    if (data.success) {
      console.log('Cache hit rate:', data.data.hitRate + '%');
      console.log('Cache miss rate:', data.data.missRate + '%');
    }
  } catch (error) {
    console.error('Error getting cache stats:', error);
  }
};
```

### Angular Example
```typescript
// Home Service
@Injectable({
  providedIn: 'root'
})
export class HomeService {
  constructor(private http: HttpClient) {}

  getPaginatedProducts(category: string, pagination: PaginationRequestDto): Observable<PaginationResponseDto<ItemDto>> {
    const params = new HttpParams()
      .set('page', pagination.page.toString())
      .set('pageSize', pagination.pageSize.toString())
      .set('sortBy', pagination.sortBy || 'name')
      .set('sortDirection', pagination.sortDirection || 'asc');

    return this.http.get<ApiResponse<PaginationResponseDto<ItemDto>>>(
      `/api/home/shop/${category}/paginated`, { params }
    ).pipe(map(response => response.data));
  }

  clearHomeCache(): Observable<any> {
    return this.http.delete('/api/cache/clear-home');
  }

  getCacheStats(): Observable<CacheStatsDto> {
    return this.http.get<ApiResponse<CacheStatsDto>>('/api/cache/stats')
      .pipe(map(response => response.data));
  }
}
```

## 🚀 الميزات المتقدمة

### 1. **Smart Caching**
- تخزين مؤقت ذكي حسب نوع البيانات
- انتهاء صلاحية مختلف لكل نوع
- تنظيف تلقائي للذاكرة

### 2. **Advanced Pagination**
- ترتيب متعدد الحقول
- تصفية مع pagination
- معلومات شاملة عن الصفحات

### 3. **Cache Management**
- إحصائيات مفصلة
- إدارة ذكية للـ cache
- أدوات مسح محددة

### 4. **Performance Optimization**
- تحسين سرعة الاستجابة
- تقليل الحمل على قاعدة البيانات
- تحسين تجربة المستخدم

## 📈 Monitoring & Analytics

### Cache Performance Metrics
- **Hit Rate**: نسبة البيانات المسترجعة من الـ cache
- **Miss Rate**: نسبة البيانات المطلوبة من قاعدة البيانات
- **Memory Usage**: استخدام الذاكرة
- **Total Entries**: عدد العناصر المخزنة

### Pagination Analytics
- **Page Views**: عدد مرات عرض الصفحات
- **Sort Patterns**: أنماط الترتيب الأكثر استخداماً
- **Page Size Preferences**: أحجام الصفحات المفضلة

## 🔧 Configuration

### Cache Settings
```json
{
  "CacheSettings": {
    "DefaultExpirationMinutes": 30,
    "MaxMemoryUsageMB": 100,
    "EnableCompression": true,
    "EnableStatistics": true
  }
}
```

### Pagination Settings
```json
{
  "PaginationSettings": {
    "DefaultPageSize": 10,
    "MaxPageSize": 100,
    "DefaultSortBy": "name",
    "DefaultSortDirection": "asc"
  }
}
```

---

**🎉 تم إضافة Pagination و Caching بنجاح للـ Home API!**

### ✅ **الميزات المكتملة:**
1. **Pagination متقدم** - دعم الصفحات مع الترتيب والتصفية
2. **Caching ذكي** - تخزين مؤقت محسن مع إحصائيات
3. **Cache Management** - أدوات إدارة شاملة للـ cache
4. **Performance Monitoring** - مراقبة الأداء والإحصائيات
5. **Flexible Configuration** - إعدادات مرنة وقابلة للتخصيص

### 🚀 **النتيجة النهائية:**
- **أداء محسن** - استجابة أسرع للمستخدمين
- **قابلية التوسع** - دعم المنتجات الكثيرة
- **إدارة ذكية** - تحكم كامل في الـ cache
- **تجربة مستخدم محسنة** - تصفح سلس وسريع 
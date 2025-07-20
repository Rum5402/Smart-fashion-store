# Fashion Store API - Complete Testing Guide

## Base URL
```
https://localhost:7001
```

## Authentication Endpoints

### 1. Explore Mode
**POST** `/api/auth/explore-mode`
```json
{
  "deviceId": "device_123456",
  "sessionId": "session_789"
}
```

### 2. Guest Login
**POST** `/api/auth/guest-login`
```json
{
  "deviceId": "device_123456",
  "sessionId": "session_789"
}
```

### 3. Save Profile
**POST** `/api/auth/save-profile`
**Headers:** `Authorization: Bearer {token}`
```json
{
  "name": "Ahmed Ali",
  "phoneNumber": "+201234567890",
  "email": "ahmed.ali@example.com",
  "preferences": {
    "favoriteCategories": ["Men", "Women"],
    "favoriteStyles": ["Casual", "Formal"],
    "sizePreference": "M"
  }
}
```

### 4. Team Member Login
**POST** `/api/auth/team-member/login`
```json
{
  "name": "Sara Ahmed",
  "phoneNumber": "+201112223334"
}
```

### 5. Manager Login
**POST** `/api/auth/manager/login`
```json
{
  "phoneNumber": "+201234567890"
}
```

### 6. Create Manager (Admin Only)
**POST** `/api/auth/manager/create`
**Headers:** `Authorization: Bearer {adminToken}`
```json
{
  "name": "Mohammed Hassan",
  "phoneNumber": "+201234567891",
  "email": "mohammed.hassan@example.com",
  "storeName": "Fashion Elite Store",
  "storeDescription": "Premium fashion store offering the latest trends",
  "storeAddress": "123 Fashion Street, Cairo, Egypt",
  "notes": "New manager for downtown branch"
}
```

## Products (Items) Endpoints

### 1. Get All Products
**GET** `/api/items`

### 2. Get Product by ID
**GET** `/api/items/{id}`

### 3. Get New Collection
**GET** `/api/items/new-collection`

### 4. Get Best Sellers
**GET** `/api/items/best-sellers`

### 5. Get Products on Sale
**GET** `/api/items/on-sale`

### 6. Get Products by Category
**GET** `/api/items/category/{category}`
- Examples: `Men`, `Women`, `Kids`

### 7. Get Products by Type
**GET** `/api/items/type/{productType}`
- Examples: `T-Shirt`, `Sweatpants`, `Pants`, `Shirt`, `Shoes`

### 8. Get Products by Style
**GET** `/api/items/style/{style}`
- Examples: `Casual`, `Formal`, `Sport`, `Outing`

### 9. Get Products by Color
**GET** `/api/items/color/{color}`
- Examples: `Red`, `Blue`, `Black`, `White`, `Green`

### 10. Get Products by Price Range
**GET** `/api/items/price-range?minPrice={min}&maxPrice={max}`

### 11. Get Product Counts by Category
**GET** `/api/items/counts/categories`

### 12. Get Product Counts by Type
**GET** `/api/items/counts/product-types`

### 13. Get Product Counts by Style
**GET** `/api/items/counts/styles`

### 14. Get Product Counts by Color
**GET** `/api/items/counts/colors`

### 15. Get Price Statistics
**GET** `/api/items/price-statistics`

### 16. Search Products
**GET** `/api/items/search?query={searchTerm}`

### 17. Get Featured Products
**GET** `/api/items/featured`

### 18. Create Product (Manager/Admin)
**POST** `/api/items`
**Headers:** `Authorization: Bearer {managerToken}`
```json
{
  "name": "Premium Cotton T-Shirt",
  "description": "High-quality cotton t-shirt with modern design",
  "price": 89.99,
  "productCode": "TSH-001",
  "originalPrice": 120.00,
  "category": 1,
  "style": 1,
  "productType": 1,
  "storeActivity": 1,
  "fabricType": "100% Cotton",
  "subCategory": "Basic Tees",
  "brandName": "Fashion Elite",
  "availableSizes": ["S", "M", "L", "XL"],
  "availableColors": ["White", "Black", "Blue"],
  "imageUrls": [
    "https://example.com/images/tshirt-white.jpg",
    "https://example.com/images/tshirt-black.jpg",
    "https://example.com/images/tshirt-blue.jpg"
  ],
  "tags": ["cotton", "basic", "casual", "comfortable"],
  "isNewCollection": true,
  "isBestSeller": false,
  "isOnSale": true,
  "storeCategoryId": 1
}
```

### 19. Get Form Options
**GET** `/api/items/form-options/{type}`
- `/categories`
- `/product-types`
- `/styles`
- `/sizes`
- `/colors`
- `/suggested-tags`
- `/` (all options)

## Store Management Endpoints

### 1. Get All Categories (Manager/Admin)
**GET** `/api/store/categories`
**Headers:** `Authorization: Bearer {managerToken}`

### 2. Get Category by ID
**GET** `/api/store/categories/{id}`

### 3. Create Category
**POST** `/api/store/categories`
```json
{
  "name": "Summer Collection",
  "description": "Light and comfortable summer wear",
  "parentCategoryId": null,
  "displayOrder": 1,
  "isActive": true
}
```

### 4. Update Category
**PUT** `/api/store/categories/{id}`
```json
{
  "name": "Summer Collection 2024",
  "description": "Updated summer collection description",
  "parentCategoryId": null,
  "displayOrder": 2,
  "isActive": true
}
```

### 5. Delete Category
**DELETE** `/api/store/categories/{id}`

### 6. Toggle Category Status
**PATCH** `/api/store/categories/{id}/toggle-status`

### 7. Get All Filters
**GET** `/api/store/filters`

### 8. Get Filter by ID
**GET** `/api/store/filters/{id}`

### 9. Create Filter
**POST** `/api/store/filters`
```json
{
  "name": "Price Range Filter",
  "type": 1,
  "options": [
    {"value": "0-50", "label": "Under $50"},
    {"value": "50-100", "label": "$50 - $100"},
    {"value": "100-200", "label": "$100 - $200"},
    {"value": "200+", "label": "Over $200"}
  ],
  "isActive": true,
  "displayOrder": 1
}
```

### 10. Update Filter
**PUT** `/api/store/filters/{id}`
```json
{
  "name": "Updated Price Range Filter",
  "type": 1,
  "options": [
    {"value": "0-25", "label": "Under $25"},
    {"value": "25-75", "label": "$25 - $75"},
    {"value": "75-150", "label": "$75 - $150"},
    {"value": "150+", "label": "Over $150"}
  ],
  "isActive": true,
  "displayOrder": 2
}
```

### 11. Delete Filter
**DELETE** `/api/store/filters/{id}`

### 12. Toggle Filter Status
**PATCH** `/api/store/filters/{id}/toggle-status`

### 13. Get All Banners
**GET** `/api/store/banners`

### 14. Get Banner by ID
**GET** `/api/store/banners/{id}`

### 15. Create Banner
**POST** `/api/store/banners`
```json
{
  "title": "Summer Sale",
  "subtitle": "Up to 50% off",
  "imageUrl": "https://example.com/banners/summer-sale.jpg",
  "linkUrl": "/sale",
  "displayOrder": 1,
  "isActive": true,
  "startDate": "2024-06-01T00:00:00Z",
  "endDate": "2024-08-31T23:59:59Z"
}
```

### 16. Update Banner
**PUT** `/api/store/banners/{id}`
```json
{
  "title": "Summer Sale 2024",
  "subtitle": "Up to 60% off",
  "imageUrl": "https://example.com/banners/summer-sale-2024.jpg",
  "linkUrl": "/sale-2024",
  "displayOrder": 1,
  "isActive": true,
  "startDate": "2024-06-01T00:00:00Z",
  "endDate": "2024-08-31T23:59:59Z"
}
```

### 17. Delete Banner
**DELETE** `/api/store/banners/{id}`

### 18. Toggle Banner Status
**PATCH** `/api/store/banners/{id}/toggle-status`

### 19. Get Brand Settings
**GET** `/api/store/brand-settings`

### 20. Update Brand Settings
**PUT** `/api/store/brand-settings`
```json
{
  "storeName": "Fashion Elite",
  "storeDescription": "Premium fashion store offering the latest trends",
  "storeLogo": "https://example.com/logo.png",
  "primaryColor": "#1a1a1a",
  "secondaryColor": "#f8f9fa",
  "contactEmail": "info@fashionelite.com",
  "contactPhone": "+201234567890",
  "storeAddress": "123 Fashion Street, Cairo, Egypt",
  "socialMedia": {
    "facebook": "https://facebook.com/fashionelite",
    "instagram": "https://instagram.com/fashionelite",
    "twitter": "https://twitter.com/fashionelite"
  }
}
```

### 21. Get Store Info (Public)
**GET** `/api/store/info`

### 22. Get Store Location (Public)
**GET** `/api/store/location`

### 23. Get Store Contact (Public)
**GET** `/api/store/contact`

### 24. Get Store Description (Public)
**GET** `/api/store/description`

### 25. Get Store Banners (Public)
**GET** `/api/store/banners/public`

### 26. Get Store Categories (Public)
**GET** `/api/store/categories/public`

### 27. Get Store Filters (Public)
**GET** `/api/store/filters/public`

### 28. Get Store Filter Presets (Public)
**GET** `/api/store/filter-presets`

## Wishlist Endpoints

### 1. Get User Wishlist
**GET** `/api/wishlist`
**Headers:** `Authorization: Bearer {token}`

### 2. Add to Wishlist
**POST** `/api/wishlist/add`
**Headers:** `Authorization: Bearer {token}`
```json
{
  "itemId": 1
}
```

### 3. Remove from Wishlist
**DELETE** `/api/wishlist/remove`
**Headers:** `Authorization: Bearer {token}`
```json
{
  "itemId": 1
}
```

### 4. Check if Item in Wishlist
**GET** `/api/wishlist/check/{itemId}`
**Headers:** `Authorization: Bearer {token}`

### 5. Request from Wishlist
**POST** `/api/wishlist/request`
**Headers:** `Authorization: Bearer {token}`
```json
{
  "itemIds": [1, 2, 3],
  "preferredSize": "M",
  "preferredColor": "Blue",
  "notes": "Please bring these items to fitting room"
}
```

## Fitting Room Request Endpoints

### 1. Create Fitting Room Request (Customer)
**POST** `/api/fitting-room-requests`
**Headers:** `Authorization: Bearer {token}`
```json
{
  "itemIds": [1, 2, 3],
  "preferredSize": "M",
  "preferredColor": "Blue",
  "notes": "Please prepare these items for fitting",
  "requestedDate": "2024-01-15T14:00:00Z"
}
```

### 2. Get My Requests (Customer)
**GET** `/api/fitting-room-requests/my-requests`
**Headers:** `Authorization: Bearer {token}`

### 3. Cancel My Request (Customer)
**PUT** `/api/fitting-room-requests/cancel/{requestId}`
**Headers:** `Authorization: Bearer {token}`

### 4. Get All Requests (Manager/Team Member)
**GET** `/api/fitting-room-requests`
**Headers:** `Authorization: Bearer {managerToken}`

### 5. Get Requests by Status
**GET** `/api/fitting-room-requests/status/{status}`
**Headers:** `Authorization: Bearer {managerToken}`
- Status values: 0 (NewRequest), 1 (InProgress), 2 (Completed), 3 (Cancelled)

### 6. Get New Requests
**GET** `/api/fitting-room-requests/new`
**Headers:** `Authorization: Bearer {managerToken}`

### 7. Get Completed Requests
**GET** `/api/fitting-room-requests/completed`
**Headers:** `Authorization: Bearer {managerToken}`

### 8. Get Cancelled Requests
**GET** `/api/fitting-room-requests/cancelled`
**Headers:** `Authorization: Bearer {managerToken}`

### 9. Get Request by ID
**GET** `/api/fitting-room-requests/{requestId}`
**Headers:** `Authorization: Bearer {managerToken}`

### 10. Complete Request
**PUT** `/api/fitting-room-requests/{requestId}/complete`
**Headers:** `Authorization: Bearer {managerToken}`

### 11. Cancel Request by Staff
**PUT** `/api/fitting-room-requests/{requestId}/cancel-by-staff`
**Headers:** `Authorization: Bearer {managerToken}`

### 12. Delete Request
**DELETE** `/api/fitting-room-requests/{requestId}`
**Headers:** `Authorization: Bearer {managerToken}`

## Notification Endpoints

### 1. Get User Notifications
**GET** `/api/notifications`
**Headers:** `Authorization: Bearer {token}`

### 2. Respond to Notification (Admin)
**POST** `/api/notifications/{id}/respond`
**Headers:** `Authorization: Bearer {adminToken}`
```json
{
  "response": "Your fitting room request has been approved. Items will be ready at 2:00 PM."
}
```

## Manager Dashboard Endpoints

### 1. Get Dashboard Overview
**GET** `/api/manager/dashboard/overview`
**Headers:** `Authorization: Bearer {managerToken}`

### 2. Get Inventory Analytics
**GET** `/api/manager/dashboard/analytics/inventory`
**Headers:** `Authorization: Bearer {managerToken}`

### 3. Create Product
**POST** `/api/manager/dashboard/products`
**Headers:** `Authorization: Bearer {managerToken}`
```json
{
  "name": "Premium Cotton T-Shirt",
  "description": "High-quality cotton t-shirt with modern design",
  "price": 89.99,
  "productCode": "TSH-001",
  "originalPrice": 120.00,
  "category": 1,
  "style": 1,
  "productType": 1,
  "storeActivity": 1,
  "fabricType": "100% Cotton",
  "subCategory": "Basic Tees",
  "brandName": "Fashion Elite",
  "availableSizes": ["S", "M", "L", "XL"],
  "availableColors": ["White", "Black", "Blue"],
  "imageUrls": [
    "https://example.com/images/tshirt-white.jpg",
    "https://example.com/images/tshirt-black.jpg",
    "https://example.com/images/tshirt-blue.jpg"
  ],
  "tags": ["cotton", "basic", "casual", "comfortable"],
  "isNewCollection": true,
  "isBestSeller": false,
  "isOnSale": true,
  "storeCategoryId": 1
}
```

### 4. Update Product
**PUT** `/api/manager/dashboard/products/{id}`
**Headers:** `Authorization: Bearer {managerToken}`
```json
{
  "name": "Updated Premium Cotton T-Shirt",
  "description": "Updated description",
  "price": 95.99,
  "productCode": "TSH-001-UPDATED",
  "originalPrice": 130.00,
  "category": 1,
  "style": 1,
  "productType": 1,
  "storeActivity": 1,
  "fabricType": "100% Premium Cotton",
  "subCategory": "Premium Tees",
  "brandName": "Fashion Elite Premium",
  "availableSizes": ["S", "M", "L", "XL", "XXL"],
  "availableColors": ["White", "Black", "Blue", "Red"],
  "imageUrls": [
    "https://example.com/images/tshirt-white-updated.jpg",
    "https://example.com/images/tshirt-black-updated.jpg",
    "https://example.com/images/tshirt-blue-updated.jpg"
  ],
  "tags": ["cotton", "basic", "casual", "comfortable", "premium"],
  "isNewCollection": true,
  "isBestSeller": true,
  "isOnSale": false,
  "storeCategoryId": 1
}
```

### 5. Delete Product
**DELETE** `/api/manager/dashboard/products/{id}`
**Headers:** `Authorization: Bearer {managerToken}`

### 6. Toggle Product Status
**PUT** `/api/manager/dashboard/products/{id}/toggle-status`
**Headers:** `Authorization: Bearer {managerToken}`

### 7. Get Store Settings
**GET** `/api/manager/dashboard/store/settings`
**Headers:** `Authorization: Bearer {managerToken}`

### 8. Update Store Settings
**PUT** `/api/manager/dashboard/store/settings`
**Headers:** `Authorization: Bearer {managerToken}`
```json
{
  "storeName": "Fashion Elite Store",
  "storeDescription": "Premium fashion store offering the latest trends",
  "storeLogo": "https://example.com/logo.png",
  "primaryColor": "#1a1a1a",
  "secondaryColor": "#f8f9fa",
  "contactEmail": "info@fashionelite.com",
  "contactPhone": "+201234567890",
  "storeAddress": "123 Fashion Street, Cairo, Egypt"
}
```

### 9. Get Available Product Types
**GET** `/api/manager/dashboard/store/product-types`
**Headers:** `Authorization: Bearer {managerToken}`

### 10. Get Available Product Styles
**GET** `/api/manager/dashboard/store/product-styles`
**Headers:** `Authorization: Bearer {managerToken}`

### 11. Create Category
**POST** `/api/manager/dashboard/store/categories`
**Headers:** `Authorization: Bearer {managerToken}`
```json
{
  "name": "Summer Collection",
  "description": "Light and comfortable summer wear",
  "parentCategoryId": null,
  "displayOrder": 1,
  "isActive": true
}
```

### 12. Update Category
**PUT** `/api/manager/dashboard/store/categories/{id}`
**Headers:** `Authorization: Bearer {managerToken}`
```json
{
  "name": "Summer Collection 2024",
  "description": "Updated summer collection description",
  "parentCategoryId": null,
  "displayOrder": 2,
  "isActive": true
}
```

### 13. Delete Category
**DELETE** `/api/manager/dashboard/store/categories/{id}`
**Headers:** `Authorization: Bearer {managerToken}`

### 14. Toggle Category Status
**PUT** `/api/manager/dashboard/store/categories/{id}/toggle-status`
**Headers:** `Authorization: Bearer {managerToken}`

### 15. Create Filter
**POST** `/api/manager/dashboard/store/filters`
**Headers:** `Authorization: Bearer {managerToken}`
```json
{
  "name": "Price Range Filter",
  "type": 1,
  "options": [
    {"value": "0-50", "label": "Under $50"},
    {"value": "50-100", "label": "$50 - $100"},
    {"value": "100-200", "label": "$100 - $200"},
    {"value": "200+", "label": "Over $200"}
  ],
  "isActive": true,
  "displayOrder": 1
}
```

### 16. Update Filter
**PUT** `/api/manager/dashboard/store/filters/{id}`
**Headers:** `Authorization: Bearer {managerToken}`
```json
{
  "name": "Updated Price Range Filter",
  "type": 1,
  "options": [
    {"value": "0-25", "label": "Under $25"},
    {"value": "25-75", "label": "$25 - $75"},
    {"value": "75-150", "label": "$75 - $150"},
    {"value": "150+", "label": "Over $150"}
  ],
  "isActive": true,
  "displayOrder": 2
}
```

### 17. Delete Filter
**DELETE** `/api/manager/dashboard/store/filters/{id}`
**Headers:** `Authorization: Bearer {managerToken}`

### 18. Toggle Filter Status
**PUT** `/api/manager/dashboard/store/filters/{id}/toggle-status`
**Headers:** `Authorization: Bearer {managerToken}`

### 19. Create Banner
**POST** `/api/manager/dashboard/store/banners`
**Headers:** `Authorization: Bearer {managerToken}`
```json
{
  "title": "Summer Sale",
  "subtitle": "Up to 50% off",
  "imageUrl": "https://example.com/banners/summer-sale.jpg",
  "linkUrl": "/sale",
  "displayOrder": 1,
  "isActive": true,
  "startDate": "2024-06-01T00:00:00Z",
  "endDate": "2024-08-31T23:59:59Z"
}
```

### 20. Update Banner
**PUT** `/api/manager/dashboard/store/banners/{id}`
**Headers:** `Authorization: Bearer {managerToken}`
```json
{
  "title": "Summer Sale 2024",
  "subtitle": "Up to 60% off",
  "imageUrl": "https://example.com/banners/summer-sale-2024.jpg",
  "linkUrl": "/sale-2024",
  "displayOrder": 1,
  "isActive": true,
  "startDate": "2024-06-01T00:00:00Z",
  "endDate": "2024-08-31T23:59:59Z"
}
```

### 21. Delete Banner
**DELETE** `/api/manager/dashboard/store/banners/{id}`
**Headers:** `Authorization: Bearer {managerToken}`

### 22. Toggle Banner Status
**PUT** `/api/manager/dashboard/store/banners/{id}/toggle-status`
**Headers:** `Authorization: Bearer {managerToken}`

### 23. Get Brand Settings
**GET** `/api/manager/dashboard/store/brand-settings`
**Headers:** `Authorization: Bearer {managerToken}`

### 24. Update Brand Settings
**PUT** `/api/manager/dashboard/store/brand-settings`
**Headers:** `Authorization: Bearer {managerToken}`
```json
{
  "storeName": "Fashion Elite",
  "storeDescription": "Premium fashion store offering the latest trends",
  "storeLogo": "https://example.com/logo.png",
  "primaryColor": "#1a1a1a",
  "secondaryColor": "#f8f9fa",
  "contactEmail": "info@fashionelite.com",
  "contactPhone": "+201234567890",
  "storeAddress": "123 Fashion Street, Cairo, Egypt",
  "socialMedia": {
    "facebook": "https://facebook.com/fashionelite",
    "instagram": "https://instagram.com/fashionelite",
    "twitter": "https://twitter.com/fashionelite"
  }
}
```

### 25. Get Fitting Room Requests
**GET** `/api/manager/dashboard/requests`
**Headers:** `Authorization: Bearer {managerToken}`

## Product Filter Endpoints

### 1. Filter Products
**POST** `/api/product-filter/filter`
```json
{
  "searchTerm": "cotton",
  "categories": ["Men", "Women"],
  "styles": ["Casual"],
  "productTypes": ["T-Shirt", "Shirt"],
  "sizes": ["M", "L"],
  "colors": ["Blue", "Black"],
  "brandNames": ["Fashion Elite"],
  "fabricTypes": ["100% Cotton"],
  "minPrice": 50.0,
  "maxPrice": 200.0,
  "isNewCollection": true,
  "isBestSeller": false,
  "isOnSale": true,
  "sortOrder": "PriceLowToHigh",
  "page": 1,
  "pageSize": 20
}
```

### 2. Get Filter Metadata
**POST** `/api/product-filter/metadata`
```json
{
  "searchTerm": "cotton",
  "categories": ["Men"],
  "styles": ["Casual"]
}
```

### 3. Get Filter Options
**POST** `/api/product-filter/options/{filterType}`
- filterType: `Size`, `Color`, `Style`, `Type`, `Promotion`
```json
{
  "searchTerm": "cotton",
  "categories": ["Men"]
}
```

### 4. Get Price Range
**POST** `/api/product-filter/price-range`
```json
{
  "searchTerm": "cotton",
  "categories": ["Men", "Women"]
}
```

### 5. Get Quick Filters
**GET** `/api/product-filter/quick-filters`

### 6. Get Advanced Filters
**POST** `/api/product-filter/advanced-filters`
```json
{
  "searchTerm": "cotton",
  "categories": ["Men"]
}
```

## Team Member Management Endpoints

### 1. Add Team Member
**POST** `/api/team-member/add`
**Headers:** `Authorization: Bearer {managerToken}`
```json
{
  "name": "Ahmed Hassan",
  "phoneNumber": "+201112223334",
  "email": "ahmed.hassan@example.com",
  "role": "Sales Associate",
  "isActive": true
}
```

### 2. Get Team Members
**GET** `/api/team-member/list`
**Headers:** `Authorization: Bearer {managerToken}`

### 3. Get Active Team Members
**GET** `/api/team-member/active`
**Headers:** `Authorization: Bearer {managerToken}`

### 4. Deactivate Team Member
**PUT** `/api/team-member/deactivate/{teamMemberId}`
**Headers:** `Authorization: Bearer {managerToken}`

### 5. Activate Team Member
**PUT** `/api/team-member/activate/{teamMemberId}`
**Headers:** `Authorization: Bearer {managerToken}`

### 6. Update Team Member
**PUT** `/api/team-member/update/{teamMemberId}`
**Headers:** `Authorization: Bearer {managerToken}`
```json
{
  "name": "Ahmed Hassan Updated",
  "phoneNumber": "+201112223335",
  "email": "ahmed.hassan.updated@example.com",
  "role": "Senior Sales Associate",
  "isActive": true
}
```

### 7. Delete Team Member
**DELETE** `/api/team-member/delete/{teamMemberId}`
**Headers:** `Authorization: Bearer {managerToken}`

## Manager Profile Endpoints

### 1. Get Manager Profile
**GET** `/api/manager/profile`
**Headers:** `Authorization: Bearer {managerToken}`

### 2. Update Manager Profile
**PUT** `/api/manager/profile`
**Headers:** `Authorization: Bearer {managerToken}`
```json
{
  "name": "Mohammed Hassan Updated",
  "phoneNumber": "+201234567892",
  "email": "mohammed.hassan.updated@example.com",
  "storeName": "Fashion Elite Store Updated",
  "storeDescription": "Updated premium fashion store description",
  "storeAddress": "456 Updated Fashion Street, Cairo, Egypt",
  "notes": "Updated manager notes"
}
```

## Mix & Match Endpoints

### 1. Get Mix & Match Suggestions
**GET** `/api/mix-match/suggestions/{itemId}`

### 2. Get Mix & Match Outfits by Category
**GET** `/api/mix-match/outfits/{category}`
- Examples: `Men`, `Women`, `Kids`

### 3. Get Mix & Match Outfits by Style
**GET** `/api/mix-match/outfits/style/{style}`
- Examples: `Casual`, `Formal`, `Sport`

### 4. Get Mix & Match Outfits by Occasion
**GET** `/api/mix-match/outfits/occasion/{occasion}`
- Examples: `Work`, `Party`, `Casual`, `Formal`

### 5. Get Trending Mix & Match
**GET** `/api/mix-match/trending`

### 6. Get Personalized Mix & Match Recommendations
**GET** `/api/mix-match/recommendations?userPreferences={preferences}`

### 7. Save Mix & Match Combination
**POST** `/api/mix-match/save-combination`
```json
{
  "name": "Casual Summer Outfit",
  "description": "Perfect for casual summer days",
  "items": [1, 2, 3],
  "category": "Men",
  "style": "Casual",
  "occasion": "Casual",
  "tags": ["summer", "casual", "comfortable"]
}
```

### 8. Get Saved Mix & Match Combinations
**GET** `/api/mix-match/saved-combinations`

## Testing Tips

### 1. Authentication Flow
1. Start with guest login or explore mode to get a basic token
2. Use manager login to get manager token for admin functions
3. Use team member login for team member functions

### 2. Token Management
- Store tokens in Postman environment variables
- Use different tokens for different user roles
- Refresh tokens when they expire

### 3. Data Dependencies
- Create products before testing wishlist functions
- Create categories before creating products
- Create fitting room requests before testing staff functions

### 4. Error Testing
- Test with invalid IDs
- Test with missing required fields
- Test with unauthorized access
- Test with invalid data types

### 5. Performance Testing
- Test with large datasets
- Test pagination with different page sizes
- Test filtering with complex criteria

### 6. Security Testing
- Test with expired tokens
- Test with invalid tokens
- Test role-based access control
- Test with missing authorization headers

## Environment Variables

Set up these variables in your Postman environment:

```
baseUrl: https://localhost:7001
authToken: (from login responses)
managerToken: (from manager login)
adminToken: (from admin login)
```

## Common HTTP Status Codes

- `200 OK`: Request successful
- `201 Created`: Resource created successfully
- `400 Bad Request`: Invalid request data
- `401 Unauthorized`: Authentication required
- `403 Forbidden`: Insufficient permissions
- `404 Not Found`: Resource not found
- `500 Internal Server Error`: Server error

## Testing Checklist

- [ ] Test all authentication endpoints
- [ ] Test all product endpoints (public and admin)
- [ ] Test all store management endpoints
- [ ] Test all wishlist endpoints
- [ ] Test all fitting room request endpoints
- [ ] Test all notification endpoints
- [ ] Test all manager dashboard endpoints
- [ ] Test all product filter endpoints
- [ ] Test all team member management endpoints
- [ ] Test all manager profile endpoints
- [ ] Test all mix & match endpoints
- [ ] Test error scenarios
- [ ] Test authorization scenarios
- [ ] Test pagination and filtering
- [ ] Test data validation 
# Add New Product API Guide

This guide covers all the API endpoints needed for the "Add New Product" functionality in the manager dashboard.

## Overview

The Add New Product form includes the following sections:
- Basic Information (Product Name, Description, Category, Product Type)
- Price & Code (Price, Product Code)
- Product Variants (Available Colors, Available Sizes)
- Product Images (Upload functionality)
- Filters & Tags (Promotions, Tags/Keywords)

## API Endpoints

### 1. Get Form Options (All in One Request)

**Endpoint:** `GET /api/items/form-options`

**Description:** Retrieves all form options in a single request for populating dropdowns and lists.

**Response:**
```json
{
  "success": true,
  "message": "Form options retrieved successfully",
  "data": {
    "categories": [
      { "id": 1, "name": "Men" },
      { "id": 2, "name": "Women" },
      { "id": 3, "name": "Kids" }
    ],
    "productTypes": [
      { "id": 1, "name": "TShirt" },
      { "id": 2, "name": "Sweatpants" },
      { "id": 3, "name": "Pants" },
      { "id": 4, "name": "Shirt" },
      { "id": 5, "name": "Shoes" },
      { "id": 6, "name": "Other" }
    ],
    "styles": [
      { "id": 1, "name": "Casual" },
      { "id": 2, "name": "Formal" },
      { "id": 3, "name": "Sports" },
      { "id": 4, "name": "Outing" },
      { "id": 5, "name": "Other" }
    ],
    "sizes": ["XS", "S", "M", "L", "XL", "XXL"],
    "colors": [
      "Red", "Orange", "Brown", "Purple", "Pink",
      "Light Blue", "Dark Blue", "Green", "Light Yellow",
      "White", "Black", "Gray", "Navy", "Beige", "Olive"
    ],
    "suggestedTags": [
      "Formal", "Casual", "Sport", "Outing", "Classic", "Sporty",
      "Smart Casual", "Chic", "Athleisure", "Street Style", "Minimalist",
      "Bohemian", "Edgy", "Vintage", "Modern", "Elegant", "Comfortable"
    ]
  }
}
```

### 2. Individual Form Options Endpoints

#### Get Categories
**Endpoint:** `GET /api/items/form-options/categories`

#### Get Product Types
**Endpoint:** `GET /api/items/form-options/product-types`

#### Get Styles
**Endpoint:** `GET /api/items/form-options/styles`

#### Get Available Sizes
**Endpoint:** `GET /api/items/form-options/sizes`

#### Get Available Colors
**Endpoint:** `GET /api/items/form-options/colors`

#### Get Suggested Tags
**Endpoint:** `GET /api/items/form-options/suggested-tags`

### 3. Create New Product

**Endpoint:** `POST /api/items`

**Authorization:** Requires Manager or Admin role

**Request Body:**
```json
{
  "name": "Classic White T-Shirt",
  "description": "Premium cotton t-shirt with comfortable fit",
  "price": 299.99,
  "productCode": "TSH001",
  "originalPrice": 399.99,
  "category": 1,
  "style": 1,
  "productType": 1,
  "storeActivity": 1,
  "fabricType": "Cotton",
  "subCategory": "Basic Tees",
  "brandName": "Fashion Brand",
  "availableSizes": ["S", "M", "L", "XL"],
  "availableColors": ["White", "Black", "Gray"],
  "imageUrls": [
    "https://example.com/images/tshirt-front.jpg",
    "https://example.com/images/tshirt-back.jpg"
  ],
  "tags": ["Casual", "Comfortable", "Basic"],
  "isNewCollection": false,
  "isBestSeller": false,
  "isOnSale": true,
  "storeCategoryId": 1
}
```

**Response (Success - 201):**
```json
{
  "success": true,
  "message": "Product created successfully",
  "data": {
    "id": 123,
    "name": "Classic White T-Shirt",
    "description": "Premium cotton t-shirt with comfortable fit",
    "price": 299.99,
    "originalPrice": 399.99,
    "category": "Men",
    "style": "Casual",
    "productType": "TShirt",
    "primaryColor": "#FFFFFF",
    "availableSizes": ["S", "M", "L", "XL"],
    "availableColors": ["White", "Black", "Gray"],
    "imageUrls": [
      "https://example.com/images/tshirt-front.jpg",
      "https://example.com/images/tshirt-back.jpg"
    ],
    "tags": ["Casual", "Comfortable", "Basic"],
    "isNewCollection": false,
    "isBestSeller": false,
    "isOnSale": true,
    "isActive": true,
    "createdAt": "2024-01-15T10:30:00Z",
    "updatedAt": "2024-01-15T10:30:00Z"
  }
}
```

**Response (Error - 400):**
```json
{
  "success": false,
  "message": "Invalid request data",
  "errors": {
    "name": ["The Name field is required."],
    "price": ["The Price field must be greater than 0."]
  }
}
```

## Form Field Mapping

### Basic Information Section
- **Product Name** → `name` (required, max 200 characters)
- **Description** → `description` (optional, max 500 characters)
- **Category** → `category` (required, enum: Men=1, Women=2, Kids=3)
- **Product Type** → `productType` (required, enum: TShirt=1, Sweatpants=2, Pants=3, Shirt=4, Shoes=5, Other=6)

### Price & Code Section
- **Price** → `price` (required, must be > 0)
- **Product Code** → `productCode` (optional, for SKU/barcode)

### Product Variants Section
- **Available Colors** → `availableColors` (required, array of color names)
- **Available Sizes** → `availableSizes` (required, array of size codes)

### Product Images Section
- **Image URLs** → `imageUrls` (required, array of image URLs)
- **Image Guidelines:**
  - Use high-quality images with good lighting
  - Include front view, back view, and detail shots
  - First image becomes the main product thumbnail
  - Recommended size: 1000x1000 pixels or larger
  - Supported formats: JPG, PNG, WebP (Max 5MB each)

### Filters & Tags Section
- **Add Promotion** → `storeActivity` (enum for promotion type)
- **Product Tags/Keywords** → `tags` (array of tag strings)
- **Suggested Tags** → Available via `/api/items/form-options/suggested-tags`

## Additional Features

### Auto Color Detection
The system automatically detects the primary color from the first image URL provided in `imageUrls`.

### Validation Rules
- **Name:** Required, max 200 characters
- **Description:** Optional, max 500 characters
- **Price:** Required, must be greater than 0
- **Category:** Required, must be valid enum value
- **Style:** Required, must be valid enum value
- **Product Type:** Required, must be valid enum value
- **Available Sizes:** Required, must contain at least one size
- **Available Colors:** Required, must contain at least one color
- **Image URLs:** Required, must contain at least one image URL

### Authorization
- **Create Product:** Requires Manager or Admin role
- **Get Form Options:** Public access (no authentication required)

## Usage Examples

### Frontend Integration

1. **Load Form Options:**
```javascript
const response = await fetch('/api/items/form-options');
const formOptions = await response.json();
// Populate dropdowns with formOptions.data
```

2. **Create Product:**
```javascript
const productData = {
  name: "Product Name",
  description: "Product description",
  price: 299.99,
  category: 1, // Men
  style: 1, // Casual
  productType: 1, // TShirt
  availableSizes: ["S", "M", "L"],
  availableColors: ["Red", "Blue"],
  imageUrls: ["https://example.com/image1.jpg"],
  tags: ["Casual", "Comfortable"]
};

const response = await fetch('/api/items', {
  method: 'POST',
  headers: {
    'Content-Type': 'application/json',
    'Authorization': 'Bearer ' + token
  },
  body: JSON.stringify(productData)
});
```

## Error Handling

The API returns appropriate HTTP status codes:
- **200:** Success (for GET requests)
- **201:** Created (for successful product creation)
- **400:** Bad Request (validation errors)
- **401:** Unauthorized (missing or invalid token)
- **403:** Forbidden (insufficient permissions)
- **500:** Internal Server Error

All error responses include a consistent format with `success: false` and error details. 
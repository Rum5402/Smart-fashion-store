# Fashion Store API - Enhanced Backend

A comprehensive .NET Core Web API for a fashion e-commerce application with advanced filtering, mix & match functionality, and store management features.

## 🏪 Store Information

### ZARA - Cairo Festival City Mall
- **Location**: Cairo Festival City Mall (New Cairo)
- **Address**: Ring Road, Cairo Festival City, 2nd Floor, New Cairo, Cairo
- **Phone**: +2 01007972537
- **Description**: Step into a world of fashion that's always ahead. From timeless everyday wear to bold statement pieces, ZARA brings you the latest trends with a touch of effortless style.

## 🚀 Features

### 📱 Enhanced Product Filtering
- **Category Filtering**: Men, Women, Kids
- **Product Type Filtering**: T-Shirt, Sweatpants, Pants, Shirt, Shoes
- **Style Filtering**: Casual, Formal, Sport, Outing
- **Color Filtering**: Dynamic color detection and filtering
- **Price Range Filtering**: 100EGP - 10000EGP with real-time statistics
- **Promotion Filtering**: New Collection, Best Seller, On Sale
- **Advanced Search**: Text search across product names, descriptions, and tags

### 🎨 Mix & Match Functionality
- **Outfit Suggestions**: Smart outfit combinations
- **Category-based Outfits**: Complete looks by category
- **Style-based Outfits**: Combinations by style preference
- **Occasion-based Outfits**: Perfect looks for specific occasions
- **Trending Combinations**: Popular and trending outfit suggestions
- **Personalized Recommendations**: User preference-based suggestions
- **Save Combinations**: Save favorite outfit combinations

### 🏪 Store Management
- **Store Information**: Complete store details and branding
- **Location Services**: Store location with coordinates
- **Contact Information**: Multiple contact methods
- **Store Description**: About us, mission, vision, and values
- **Banner Management**: Promotional content and banners
- **Category Management**: Dynamic category system
- **Filter Management**: Advanced filtering system

### 📊 Enhanced Analytics
- **Product Counts**: Real-time counts by category, type, style, color
- **Price Statistics**: Min, max, and average price calculations
- **Metadata Generation**: Dynamic filter options with counts
- **Pagination Support**: Configurable page sizes and navigation

## 🛠️ API Endpoints

### Product Management
```
GET    /api/items                    # Get all products
GET    /api/items/{id}              # Get product by ID
GET    /api/items/new-collection    # Get new collection items
GET    /api/items/best-sellers      # Get best seller items
GET    /api/items/on-sale          # Get on sale items
GET    /api/items/featured         # Get featured products
```

### Enhanced Filtering
```
GET    /api/items/category/{category}           # Filter by category
GET    /api/items/type/{productType}           # Filter by product type
GET    /api/items/style/{style}                # Filter by style
GET    /api/items/color/{color}                # Filter by color
GET    /api/items/price-range                 # Filter by price range
GET    /api/items/search                      # Search products
```

### Product Analytics
```
GET    /api/items/counts/categories           # Product counts by category
GET    /api/items/counts/product-types        # Product counts by type
GET    /api/items/counts/styles               # Product counts by style
GET    /api/items/counts/colors               # Product counts by color
GET    /api/items/price-statistics            # Price range statistics
```

### Mix & Match
```
GET    /api/mixmatch/suggestions/{itemId}     # Get mix & match suggestions
GET    /api/mixmatch/outfits/{category}       # Get outfits by category
GET    /api/mixmatch/outfits/style/{style}    # Get outfits by style
GET    /api/mixmatch/outfits/occasion/{occasion} # Get outfits by occasion
GET    /api/mixmatch/trending                 # Get trending combinations
GET    /api/mixmatch/recommendations          # Get personalized recommendations
POST   /api/mixmatch/save-combination         # Save outfit combination
GET    /api/mixmatch/saved-combinations       # Get saved combinations
```

### Store Information
```
GET    /api/store/info                        # Get store information
GET    /api/store/brand-settings              # Get brand settings
GET    /api/store/location                    # Get store location
GET    /api/store/contact                     # Get contact information
GET    /api/store/description                 # Get store description
GET    /api/store/banners                     # Get store banners
GET    /api/store/categories                  # Get store categories
GET    /api/store/filters                     # Get store filters
GET    /api/store/filter-presets              # Get filter presets
```

### Advanced Product Filtering
```
POST   /api/productfilter/filter              # Advanced product filtering
POST   /api/productfilter/metadata            # Get filter metadata
POST   /api/productfilter/options/{filterType} # Get filter options
POST   /api/productfilter/price-range         # Get price range
GET    /api/productfilter/quick-filters       # Get quick filters
POST   /api/productfilter/advanced-filters    # Get advanced filters
```

## 🏗️ Architecture

### Project Structure
```
Fashion/
├── Fashion.Api/                 # Main API project
│   ├── Controllers/            # API controllers
│   ├── Middlewares/           # Custom middlewares
│   ├── Filters/               # Authorization filters
│   ├── Hubs/                  # SignalR hubs
│   └── Services/              # API services
├── Fashion.Contract/           # DTOs and interfaces
│   ├── DTOs/                  # Data transfer objects
│   └── Interface/             # Service interfaces
├── Fashion.Core/              # Domain entities
│   ├── Entities/              # Domain models
│   ├── Enums/                 # Enumerations
│   └── Interface/             # Repository interfaces
├── Fashion.Infrastructure/    # Data access layer
│   ├── Data/                  # DbContext
│   ├── Repositories/          # Repository implementations
│   └── Migrations/            # Entity Framework migrations
└── Fashion.Service/           # Business logic
    ├── Items/                 # Product services
    ├── Store/                 # Store management
    ├── Authentications/       # Authentication services
    └── Notifications/         # Notification services
```

### Key Technologies
- **.NET 8**: Latest .NET framework
- **ASP.NET Core**: Web API framework
- **Entity Framework Core**: ORM for data access
- **SignalR**: Real-time communication
- **JWT Authentication**: Secure API authentication
- **Swagger/OpenAPI**: API documentation
- **CORS**: Cross-origin resource sharing

## 🚀 Getting Started

### Prerequisites
- .NET 8 SDK
- SQL Server or SQL Server Express
- Visual Studio 2022 or VS Code

### Installation
1. Clone the repository
2. Update connection string in `appsettings.json`
3. Run Entity Framework migrations
4. Build and run the application

### Database Setup
```bash
# Run migrations
dotnet ef database update --project Fashion.Infrastructure --startup-project Fashion.Api
```

### API Documentation
Access Swagger documentation at: `https://localhost:7001/swagger`

## 📊 Sample API Responses

### Store Information
```json
{
  "success": true,
  "message": "Store information retrieved successfully",
  "data": {
    "brandName": "ZARA",
    "storeName": "ZARA - Cairo Festival City",
    "locationName": "Cairo Festival City Mall (New Cairo)",
    "address": "Ring Road, Cairo Festival City, 2nd Floor, New Cairo, Cairo",
    "phoneNumber": "+2 01007972537",
    "description": "Step into a world of fashion that's always ahead..."
  }
}
```

### Product Filtering
```json
{
  "success": true,
  "message": "Products filtered successfully",
  "data": {
    "products": [...],
    "totalCount": 7354,
    "currentPage": 1,
    "pageSize": 20,
    "totalPages": 368,
    "hasNextPage": true,
    "hasPreviousPage": false,
    "metadata": {
      "categories": [...],
      "styles": [...],
      "priceRange": {
        "minPrice": 100.0,
        "maxPrice": 10000.0,
        "averagePrice": 2500.0
      }
    }
  }
}
```

## 🔧 Configuration

### App Settings
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=...;Database=FashionDb;..."
  },
  "JwtSettings": {
    "SecretKey": "your-secret-key",
    "Issuer": "FashionApi",
    "Audience": "FashionClient"
  }
}
```

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Add tests if applicable
5. Submit a pull request

## 📝 License

This project is licensed under the MIT License.

## 📞 Support

For support and questions, please contact:
- **Phone**: +2 01007972537
- **Email**: cairo.festival@zara.com
- **Website**: https://www.zara.com

#!/bin/bash

echo "🚀 Starting Fashion API deployment preparation..."

# Check if Docker is installed
if ! command -v docker &> /dev/null; then
    echo "❌ Docker is not installed. Please install Docker first."
    exit 1
fi

# Build the Docker image locally to test
echo "🔨 Building Docker image..."
docker build -t fashion-api .

if [ $? -eq 0 ]; then
    echo "✅ Docker build successful!"
    echo "🎉 Your application is ready for Railway deployment!"
    echo ""
    echo "📋 Next steps:"
    echo "1. Push your code to GitHub"
    echo "2. Connect your repository to Railway"
    echo "3. Railway will automatically detect the Dockerfile and build your app"
    echo "4. Set up your environment variables in Railway dashboard"
    echo ""
    echo "🔧 Required environment variables:"
    echo "- DefaultConnection (Database connection string)"
    echo "- JwtSettings:SecretKey"
    echo "- JwtSettings:Issuer"
    echo "- JwtSettings:Audience"
else
    echo "❌ Docker build failed. Please check the error messages above."
    exit 1
fi 
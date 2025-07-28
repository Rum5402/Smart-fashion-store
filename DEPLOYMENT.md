# Railway Deployment Guide

## Quick Fix for Your Error

The error you encountered is because Railway was trying to run `dotnet restore` from the wrong directory. I've created the necessary files to fix this:

### Files Created:
- ✅ `Dockerfile` - Multi-stage Docker build
- ✅ `.dockerignore` - Optimizes build context
- ✅ `railway.toml` - Railway configuration
- ✅ `deploy.sh` - Local testing script
- ✅ Health check endpoint added to Program.cs

## Deployment Steps:

1. **Push your code to GitHub** (including the new files)

2. **Connect to Railway:**
   - Go to Railway dashboard
   - Create new project
   - Deploy from GitHub repo
   - Railway will auto-detect the Dockerfile

3. **Set Environment Variables:**
   ```
   DefaultConnection=your_database_connection_string
   JwtSettings__SecretKey=your_secret_key
   JwtSettings__Issuer=your_issuer
   JwtSettings__Audience=your_audience
   ASPNETCORE_ENVIRONMENT=Production
   ASPNETCORE_URLS=http://+:8080
   ```

4. **Deploy** - Railway will build and deploy automatically

## Test Locally First:
```bash
chmod +x deploy.sh
./deploy.sh
```

The Dockerfile now properly handles the multi-project structure and should resolve your `dotnet restore` error. 
namespace Fashion.Contract.DTOs.Common
{
    /// <summary>
    /// Unified API response model for consistent response format across all endpoints
    /// </summary>
    /// <typeparam name="T">Type of the data being returned</typeparam>
    public class ApiResponse<T>
    {
        /// <summary>
        /// Indicates whether the operation was successful
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Human-readable message describing the result
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// The actual data being returned
        /// </summary>
        public T? Data { get; set; }

        /// <summary>
        /// Total count for paginated results
        /// </summary>
        public int? TotalCount { get; set; }

        /// <summary>
        /// Active count for team members or other active entities
        /// </summary>
        public int? ActiveCount { get; set; }

        /// <summary>
        /// Error message if the operation failed
        /// </summary>
        public string? Error { get; set; }

        /// <summary>
        /// Additional error details for debugging
        /// </summary>
        public string? Details { get; set; }

        /// <summary>
        /// Creates a successful response
        /// </summary>
        public static ApiResponse<T> SuccessResponse(T data, string message = "Operation completed successfully", int? totalCount = null, int? activeCount = null)
        {
            return new ApiResponse<T>
            {
                Success = true,
                Message = message,
                Data = data,
                TotalCount = totalCount,
                ActiveCount = activeCount
            };
        }

        /// <summary>
        /// Creates an error response
        /// </summary>
        public static ApiResponse<T> ErrorResponse(string error, string? details = null)
        {
            return new ApiResponse<T>
            {
                Success = false,
                Error = error,
                Details = details
            };
        }
    }

    /// <summary>
    /// Non-generic API response for operations that don't return data
    /// </summary>
    public class ApiResponse
    {
        /// <summary>
        /// Indicates whether the operation was successful
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Human-readable message describing the result
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// Error message if the operation failed
        /// </summary>
        public string? Error { get; set; }

        /// <summary>
        /// Additional error details for debugging
        /// </summary>
        public string? Details { get; set; }

        /// <summary>
        /// Creates a successful response
        /// </summary>
        public static ApiResponse SuccessResponse(string message = "Operation completed successfully")
        {
            return new ApiResponse
            {
                Success = true,
                Message = message
            };
        }

        /// <summary>
        /// Creates an error response
        /// </summary>
        public static ApiResponse ErrorResponse(string error, string? details = null)
        {
            return new ApiResponse
            {
                Success = false,
                Error = error,
                Details = details
            };
        }
    }
} 
using Microsoft.Extensions.Configuration;

namespace Fashion.Service.Items
{
    public interface IColorDetectionService
    {
        Task<string> DetectPrimaryColorAsync(string imageUrl);
        Task<string> DetectPrimaryColorFromBytesAsync(byte[] imageBytes);
    }

    public class ColorDetectionService : IColorDetectionService
    {
        private readonly HttpClient _httpClient;
        private readonly string _aiModelEndpoint; // Endpoint for your AI model

        public ColorDetectionService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _aiModelEndpoint = configuration["AIModel:ColorDetectionEndpoint"] ?? "http://localhost:5000/api/color-detection";
        }

        public async Task<string> DetectPrimaryColorAsync(string imageUrl)
        {
            try
            {
                // Download the image
                var imageBytes = await _httpClient.GetByteArrayAsync(imageUrl);
                return await DetectPrimaryColorFromBytesAsync(imageBytes);
            }
            catch (Exception ex)
            {
                // Log the error and return default
                Console.WriteLine($"Error detecting color from URL {imageUrl}: {ex.Message}");
                return "Unknown";
            }
        }

        public async Task<string> DetectPrimaryColorFromBytesAsync(byte[] imageBytes)
        {
            try
            {
                // Prepare the request to your AI model
                using var content = new MultipartFormDataContent();
                using var imageStream = new MemoryStream(imageBytes);
                content.Add(new StreamContent(imageStream), "image", "product.jpg");

                // Send request to your AI model
                var response = await _httpClient.PostAsync(_aiModelEndpoint, content);
                
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    // Parse the AI model response
                    return ParseAIModelResponse(result);
                }
                else
                {
                    Console.WriteLine($"AI Model returned error: {response.StatusCode}");
                    return "Unknown";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error calling AI model: {ex.Message}");
                return "Unknown";
            }
        }

        private string ParseAIModelResponse(string aiResponse)
        {
            try
            {
                // This is a placeholder - you need to adjust based on your AI model's response format
                // Example response formats:
                
                // JSON format: {"primaryColor": "Red", "confidence": 0.95}
                if (aiResponse.Contains("\"primaryColor\""))
                {
                    var match = System.Text.RegularExpressions.Regex.Match(aiResponse, "\"primaryColor\":\\s*\"([^\"]+)\"");
                    if (match.Success)
                        return match.Groups[1].Value;
                }
                
                // Simple text format: "Red"
                if (!string.IsNullOrWhiteSpace(aiResponse))
                {
                    return aiResponse.Trim().Replace("\"", "");
                }
                
                // For now, return the raw response - you should implement proper parsing
                return aiResponse.Trim().Replace("\"", "");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error parsing AI response: {ex.Message}");
                return "Unknown";
            }
        }

        // Simple fallback method without System.Drawing dependency
        private string DetectColorSimple(byte[] imageBytes)
        {
            try
            {
                // This is a simplified version that doesn't require System.Drawing
                // In a real implementation, you would use a different image processing library
                // or rely entirely on your AI model
                
                // For now, return a default color
                return "Unknown";
            }
            catch (Exception)
            {
                return "Unknown";
            }
        }
    }
} 
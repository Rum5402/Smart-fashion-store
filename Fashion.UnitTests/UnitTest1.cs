using Fashion.Service.Items;
using Fashion.Core.Entities;
using Moq;
using Xunit;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fashion.Core.Interface;
using System.Linq;

namespace Fashion.UnitTests
{
    public class ItemServiceTests
    {
        [Fact]
        public async Task GetAllItemsAsync_ReturnsOnlyItemsForGivenStoreId()
        {
            // Arrange
            var storeId = 1;
            var items = new List<Item>
            {
                new Item { Id = 1, Name = "Item1", StoreId = 1, IsActive = true },
                new Item { Id = 2, Name = "Item2", StoreId = 2, IsActive = true },
                new Item { Id = 3, Name = "Item3", StoreId = 1, IsActive = true }
            };
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockRepo = new Mock<IGenericRepository<Item>>();
            mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(items);
            mockUnitOfWork.Setup(u => u.Repository<Item>()).Returns(mockRepo.Object);
            var mockCategoryRepo = new Mock<IGenericRepository<StoreCategory>>();
            mockCategoryRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<StoreCategory>());
            mockUnitOfWork.Setup(u => u.Repository<StoreCategory>()).Returns(mockCategoryRepo.Object);
            var service = new ItemService(mockUnitOfWork.Object, null!);

            // Act
            var result = await service.GetAllItemsAsync(storeId);

            // Assert
            Assert.All(result, i => Assert.Equal(storeId, i.StoreId));
            Assert.Equal(2, result.Count);
        }
    }
}

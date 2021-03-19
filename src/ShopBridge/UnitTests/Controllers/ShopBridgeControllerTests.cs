using FluentAssertions;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ShopBridge.Api;
using ShopBridge.Api.Controllers;
using ShopBridge.Api.Interfaces;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace UnitTests.Controllers
{
    public class ShopBridgeControllerTests
    {
        [Fact]
        public async Task ShouldGetAllTheProducts() 
        {
            var mockRepository = new Mock<ISqlRepository>();
            mockRepository.Setup(_ => _.GetAll(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Product>
                {
                    new Product
                    {
                        Id = "1",
                        Description = "Product description",
                        Name = "Name",
                        Price = 12.23M
                    }
                });

            var controller = new ShopBridgeController(mockRepository.Object, Mock.Of<IValidator<Product>>());

            var products = await controller.GetProducts();

            products.Should()
                .NotBeNull()
                .And.BeOfType<OkObjectResult>()
                .Which
                .StatusCode
                .Should()
                .Be(200);
        }

        [Fact]
        public async Task ShouldCreateTheProductSuccesfully() 
        {
            var mockRepository = new Mock<ISqlRepository>();
            mockRepository.Setup(_ => _.Get(It.IsAny<string>(),It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Product
                {
                    Id = "1",
                    Description = "Product description",
                    Name = "Name",
                    Price = 12.23M
                });

            mockRepository.Setup(_ => _.Create(It.IsAny<Product>(), It.IsAny<CancellationToken>()));

            var mockValidator = new Mock<IValidator<Product>>();
            mockValidator.Setup(_ => _.Validate(It.IsAny<Product>()))
                .Returns(new FluentValidation.Results.ValidationResult());

            var controller = new ShopBridgeController(mockRepository.Object, mockValidator.Object);

            var products = await controller.Create(new Product
            {
                Id = "1",
                Description = "Product description",
                Name = "Name",
                Price = 12.23M
            });

            products.Should()
                .NotBeNull()
                .And.BeOfType<OkObjectResult>()
                .Which
                .StatusCode
                .Should()
                .Be(200);
        }

        [Fact]
        public async Task ShouldUpdateTheProductSuccesfully()
        {
            var mockRepository = new Mock<ISqlRepository>();
            mockRepository.Setup(_ => _.Get(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Product
                {
                    Id = "1",
                    Description = "Product description",
                    Name = "Name",
                    Price = 12.23M
                });

            mockRepository.Setup(_ => _.Update(It.IsAny<string>(), It.IsAny<Product>(), It.IsAny<CancellationToken>()));

            var mockValidator = new Mock<IValidator<Product>>();
            mockValidator.Setup(_ => _.Validate(It.IsAny<Product>()))
                .Returns(new FluentValidation.Results.ValidationResult());

            var controller = new ShopBridgeController(mockRepository.Object, mockValidator.Object);

            var products = await controller.Update("1", new Product
            {
                Id = "1",
                Description = "Product description",
                Name = "Name",
                Price = 12.23M
            });

            products.Should()
                .NotBeNull()
                .And.BeOfType<OkObjectResult>()
                .Which
                .StatusCode
                .Should()
                .Be(200);
        }

        [Fact]
        public async Task ShouldDeleteTheProductSuccesfully()
        {
            var mockRepository = new Mock<ISqlRepository>();
            mockRepository.Setup(_ => _.Get(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Product
                {
                    Id = "1",
                    Description = "Product description",
                    Name = "Name",
                    Price = 12.23M
                });

            mockRepository.Setup(_ => _.Delete(It.IsAny<string>(), It.IsAny<CancellationToken>()));

            var controller = new ShopBridgeController(mockRepository.Object, Mock.Of<IValidator<Product>>());

            var products = await controller.Delete("1");

            products.Should()
                .NotBeNull()
                .And.BeOfType<OkObjectResult>()
                .Which
                .StatusCode
                .Should()
                .Be(200);
        }
    }
}

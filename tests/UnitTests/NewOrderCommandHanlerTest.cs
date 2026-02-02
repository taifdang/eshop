using Application.Abstractions;
using Application.Basket.Dtos;
using Application.Basket.Queries.GetCartList;
using Application.Catalog.Products.Queries.GetVariantById;
using Application.Common.Dtos;
using Application.Order.Commands.CreateOrder;
using Domain.Entities;
using Domain.Repositories;
using MediatR;
using Moq;

namespace UnitTests;

public class NewOrderCommandHanlerTest
{
    private readonly Mock<IOrderRepository> _orderRepositoryMock;
    private readonly Mock<IMediator> _mediator;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IDateTimeProvider> _currentTime;

    public NewOrderCommandHanlerTest()
    {
        _orderRepositoryMock = new Mock<IOrderRepository>();
        _mediator = new Mock<IMediator>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _currentTime = new Mock<IDateTimeProvider>();
    }

    [Fact]
    public async Task Handle_CreateOrder_Success()
    {
        // Arrange
        var customerId = Guid.NewGuid();
        var command = new CreateOrderCommand(customerId, Domain.Enums.PaymentMethod.COD, Domain.Enums.PaymentProvider.Unknown, "Street567", "City123", "12345");

        var variantId = Guid.NewGuid();
        var basket = new BasketDto(
             Guid.NewGuid(),
             customerId,
             new List<BasketItemDto>
             {
                new BasketItemDto(
                    variantId,
                    "T-Shirt Coolman",
                    "Shirt",
                    90000m,
                    "default.jpg",
                    2
                )
             },
             DateTime.UtcNow,
             null
         );

        _mediator
            .Setup(m => m.Send(It.IsAny<GetBasketQueryByCustomer>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(basket);

        var variant = new VariantDto
        {
            Id = variantId,
            ProductName = "T-Shirt Coolman",
            Title = "T-Shirt for man",
            Price = 90000m,
            Quantity = 10,
            Image = new ImageLookupDto { Url = "default.jpg" }
        };

        _mediator
           .Setup(m => m.Send(It.IsAny<GetVariantByIdQuery>(), It.IsAny<CancellationToken>()))
           .ReturnsAsync(variant);

        _orderRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<Order>()))
            .Returns(Task.CompletedTask);

        _unitOfWorkMock.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);
        _orderRepositoryMock.Setup(r => r.UnitOfWork).Returns(_unitOfWorkMock.Object);

        // Act
        var handler = new CreateOrderCommandHandler(_orderRepositoryMock.Object, _mediator.Object, _currentTime.Object);
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert   
        _orderRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Order>()), Times.Once);
        _orderRepositoryMock.Verify(u => u.UnitOfWork.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
         
        //Assert.NotEqual(Guid.Empty, result);
        Assert.NotNull(result);
    }
}

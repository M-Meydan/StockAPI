using AutoMapper;
using FluentValidation;
using MediatR;
using Moq;
using NUnit.Framework;
using System.Threading;
using System.Threading.Tasks;
using WebAPI.App.Trades.Commands.CreateTrade;
using WebAPI.App.Trades.Notifications;
using WebAPI.Domain.Entities;
using WebAPI.Infrastructure.Repositories;

namespace WebAPI.Tests.UnitTests.Trades.CreateTrade
{
    [TestFixture]
    public class CreateTradeCommandHandlerTests
    {
        private Mock<ITradeRepository> _transactionRepositoryMock;
        private Mock<IStockRepository> _stockRepositoryMock;
        private Mock<IValidator<CreateTradeCommand>> _validatorMock;
        private Mock<IMapper> _mapperMock;
        private Mock<IMediator> _mediatorMock;
        private CreateTradeCommandHandler _handler;

        [SetUp]
        public void Setup()
        {
            _transactionRepositoryMock = new Mock<ITradeRepository>();
            _stockRepositoryMock = new Mock<IStockRepository>();
            _validatorMock = new Mock<IValidator<CreateTradeCommand>>();
            _mapperMock = new Mock<IMapper>();
            _mediatorMock = new Mock<IMediator>();

            _handler = new CreateTradeCommandHandler(
                _transactionRepositoryMock.Object,
                _stockRepositoryMock.Object,
                _validatorMock.Object,
                _mapperMock.Object,
                _mediatorMock.Object);
        }

        [Test]
        public async Task Handle_ValidCommand_ReturnsTradeId()
        {
            // Arrange
            var tradeId = 1;
            var command = new CreateTradeCommand
            {
                Symbol = "AAPL",
                PriceInPound = 100,
                NumberOfShares = 10,
                BrokerId = 1
            };
            var entity = new Trade { Symbol = command.Symbol };

            _transactionRepositoryMock.Setup(x => x.CreateAsync(It.IsAny<Trade>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(tradeId);

            _mediatorMock.Setup(x => x.Publish(It.IsAny<TradeCreatedNotification>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            _mapperMock.Setup(x => x.Map<Trade>(command)).Returns(entity);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.AreEqual(tradeId, result);
        }


        [Test]
        public async Task Handle_WithValidCommand_ShouldSendTradeCreatedNotification()
        {
            // Arrange
            var command = new CreateTradeCommand
            {
                Symbol = "AAPL",
                PriceInPound = 100,
                NumberOfShares = 10,
                BrokerId = 1
            };
            var entity = new Trade { Symbol = command.Symbol };

            _transactionRepositoryMock.Setup(x => x.CreateAsync(It.IsAny<Trade>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            _mediatorMock.Setup(x => x.Publish(It.IsAny<TradeCreatedNotification>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            _mapperMock.Setup(x => x.Map<Trade>(command)).Returns(entity);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            _mediatorMock.Verify(x => x.Publish(It.IsAny<TradeCreatedNotification>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}

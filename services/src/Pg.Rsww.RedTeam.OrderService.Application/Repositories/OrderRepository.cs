using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using Pg.Rsww.RedTeam.Common.Models;
using Pg.Rsww.RedTeam.DataStorage;
using Pg.Rsww.RedTeam.DataStorage.Repositories;
using Pg.Rsww.RedTeam.OrderService.Application.Models;
using Pg.Rsww.RedTeam.OrderService.Application.Models.Entities;

namespace Pg.Rsww.RedTeam.OrderService.Application.Repositories;

public class OrderRepository : MongoChangeLoggingRepository<OrderEntity>
{
	private readonly ILogger<OrderRepository> _logger;

	public OrderRepository(IOptions<MongoSettings> mongoDBSettings,ILogger<OrderRepository> logger) : base(mongoDBSettings, "Orders")
	{
		_logger = logger;
	}

	public async Task<List<OrderEntity>> GetAsync(string customer)
	{
		if (string.IsNullOrWhiteSpace(customer))
		{
			return new List<OrderEntity>();
		}

		var builder = Builders<OrderEntity>.Filter;
		var customerIdFilter = builder.Eq(x => x.CustomerId, customer);

		return await _collection.Find(customerIdFilter).ToListAsync();
	}

	public async Task<string> CreateAsync(string offerId, string customerId)
	{
		var id = ObjectId.GenerateNewId().ToString();
		var order = new OrderEntity
		{
			Id = id,
			CustomerId = customerId,
			OfferId = offerId,
			CreatedAt = DateTime.UtcNow,
			Status = ReservationStatus.Created
		};


		await _collection.InsertOneAsync(order);
		return id;
	}

	public async Task<bool> AddPaymentToOrder(string orderId, string paymentId)
	{
		var builder = Builders<OrderEntity>.Filter;
		var orderIdFilter = builder.Eq(x => x.Id, orderId);

		var update = Builders<OrderEntity>
			.Update
			.Set(x => x.PaymentId, paymentId);

		var result = await _collection.UpdateOneAsync(orderIdFilter, update);
		return result.IsModifiedCountAvailable && result.ModifiedCount == 1;
	}

	public async Task<List<OrderEntity>> CancelOutdatedReservations(DateTime maxDateTime)
	{
		var builder = Builders<OrderEntity>.Filter;
		var filter = builder.Empty;

		var dateFilter = builder.Lte(x => x.CreatedAt, maxDateTime);
		filter &= dateFilter;


		var statusFilter = builder.Eq(x => x.Status, ReservationStatus.Created);
		filter &= statusFilter;

		var update = Builders<OrderEntity>
			.Update
			.Set(x => x.Status, ReservationStatus.Cancelled);

		var resultCursor = await _collection.FindAsync(filter);
		var reservations = resultCursor.ToList();


		var ids = reservations.Select(x => x.Id);
		var updateFilter = builder.In(x => x.Id, ids);

		var result = await _collection.UpdateManyAsync(updateFilter, update);
		var updatedCount = result.IsModifiedCountAvailable ? result.ModifiedCount : 0;
		if (updatedCount != reservations.Count)
		{
			_logger.Log(LogLevel.Error,"Could not cancel all outdated orders");
		}

		return reservations;
	}

	public async Task<long> UpdateStatus(string orderId, ReservationStatus done)
	{
		var builder = Builders<OrderEntity>.Filter;
		var orderIdFilter = builder.Eq(x => x.Id, orderId);

		var update = Builders<OrderEntity>
			.Update
			.Set(x => x.Status, ReservationStatus.Completed);

		var result = await _collection.UpdateOneAsync(orderIdFilter, update);
		return result.IsModifiedCountAvailable ? result.ModifiedCount : 0;
	}

	public async Task<OrderEntity> GetById(string orderId)
	{
		var builder = Builders<OrderEntity>.Filter;
		var orderIdFilter = builder.Eq(x => x.Id, orderId);

		var result = await _collection.Find(orderIdFilter).ToListAsync();
		return result.FirstOrDefault();
	}
}
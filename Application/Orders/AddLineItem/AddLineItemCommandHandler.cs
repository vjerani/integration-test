﻿using Application.Data;
using Domain.Orders;
using Domain.Products;
using MediatR;

namespace Application.Orders.AddLineItem;

internal sealed class AddLineItemCommandHandler : IRequestHandler<AddLineItemCommand>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AddLineItemCommandHandler(
        IOrderRepository orderRepository,
        IProductRepository productRepository,
        IUnitOfWork unitOfWork)
    {
        _orderRepository = orderRepository;
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(AddLineItemCommand request, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetByIdAsync(request.OrderId);

        if (order is null)
        {
            throw new OrderNotFoundException(request.OrderId);
        }

        var product = await _productRepository.GetByIdAsync(request.ProductId);

        if (product is null)
        {
            throw new ProductNotFoundException(request.ProductId);
        }

        order.AddLineItem(product.Id, new Money(request.Currency, request.Amount));

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}

﻿using Application.Abstractions.Links;
using Domain.Products;
using MediatR;

namespace Application.Products.GetById;

public record GetProductQuery(ProductId ProductId) : IRequest<ProductResponse>;

public record ProductResponse(
    Guid Id,
    string Name,
    string Sku,
    string Currency,
    decimal Amount)
{
    public List<Link> Links { get; set; } = new();
}
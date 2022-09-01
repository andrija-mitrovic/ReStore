﻿using Application.Common.DTOs;
using MediatR;

namespace Application.Products.Queries.GetProductById
{
    public record GetProductByIdQuery(int Id) : IRequest<ProductDto>
    {
    }
}

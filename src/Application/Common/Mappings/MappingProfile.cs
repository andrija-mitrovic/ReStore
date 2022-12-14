using Application.Common.DTOs;
using Application.Products.Commands.CreateProduct;
using Application.Products.Commands.UpdateProduct;
using AutoMapper;
using Domain.Entities;

namespace Application.Common.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Product, CreateProductCommand>().ReverseMap();
            CreateMap<Product, UpdateProductCommand>().ReverseMap();
            CreateMap<Product, ProductDto>().ReverseMap();

            CreateMap<Basket, BasketDto>().ReverseMap();

            CreateMap<BasketItem, BasketItemDto>()
                .ForMember(x => x.Name, y => { y.MapFrom(z => z.Product!.Name); })
                .ForMember(x => x.Price, y => { y.MapFrom(z => z.Product!.Price); })
                .ForMember(x => x.PictureUrl, y => { y.MapFrom(z => z.Product!.PictureUrl); })
                .ForMember(x => x.Brand, y => { y.MapFrom(z => z.Product!.Brand); })
                .ForMember(x => x.Type, y => { y.MapFrom(z => z.Product!.Type); });
        }
    }
}

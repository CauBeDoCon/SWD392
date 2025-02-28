using AutoMapper;
using SWD392.DB;
using SWD392.DTOs;
using SWD392.Models;

namespace SkincarePharmacyNetCore8.Helpers
{
    public class ApplicationMapper : Profile
    {
        public ApplicationMapper()
        {
        //      CreateMap<Product, ProductModel>()
        // .ForMember(dest => dest.Brand, opt => opt.MapFrom(src => src.Brand))
        // .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category))
        // .ForMember(dest => dest.Manufacturer, opt => opt.MapFrom(src => src.Manufacturer))
        // .ForMember(dest => dest.Unit, opt => opt.MapFrom(src => src.Unit))
        // .ForMember(dest => dest.ProductDetail, opt => opt.MapFrom(src => src.ProductDetail))
        // .ForMember(dest => dest.Packaging, opt => opt.MapFrom(src => src.Packaging))
        // .ForMember(dest => dest.BrandOrigin, opt => opt.MapFrom(src => src.BrandOrigin))
        // .ForMember(dest => dest.ManufacturedCountry, opt => opt.MapFrom(src => src.ManufacturedCountry));
        
        //     CreateMap<Product, ProductModel>().ReverseMap(); // Để lấy dữ liệu từ DB về
            CreateMap<ProductModel, Product>()
            .ForMember(dest => dest.Unit, opt => opt.Ignore()) // Nếu cần, có thể cấu hình thêm
            .ForMember(dest => dest.Brand, opt => opt.Ignore())
            .ForMember(dest => dest.Packaging, opt => opt.Ignore())
            .ForMember(dest => dest.Category, opt => opt.Ignore())
            .ForMember(dest => dest.BrandOrigin, opt => opt.Ignore())
            .ForMember(dest => dest.Manufacturer, opt => opt.Ignore())
            .ForMember(dest => dest.ManufacturedCountry, opt => opt.Ignore())
            .ForMember(dest => dest.ProductDetail, opt => opt.Ignore());

            CreateMap<Product, ProductModel>(); // Để lấy dữ liệu từ DB về

            CreateMap<Order , OrderResponse>().ReverseMap();
            
            CreateMap<Image, ImageModel>().ReverseMap();

            CreateMap<Unit, UnitModel>().ReverseMap();

            CreateMap<Brand, BrandModel>().ReverseMap();

            CreateMap<Packaging, PackagingModel>().ReverseMap();

            CreateMap<Category, CategoryModel>().ReverseMap();

            CreateMap<Solution, SolutionModel>().ReverseMap();

            CreateMap<BrandOrigin, BrandOriginModel>().ReverseMap();

            CreateMap<Manufacturer, ManufacturerModel>().ReverseMap();

            CreateMap<ManufacturedCountry, ManufacturedCountryModel>().ReverseMap();

            CreateMap<ProductDetail, ProductDetailModel>().ReverseMap();

            CreateMap<CartProduct, CartProductModel>().ReverseMap();

            CreateMap<DiscountCategory, DiscountCategoryDto>().ReverseMap();

            CreateMap<Discount, DiscountDto>().ReverseMap();
            CreateMap<Wallet, WalletDTO>().ReverseMap();

            CreateMap<Blog, BlogModel>().ReverseMap();
        }
    }
}

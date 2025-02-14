using AutoMapper;
using SWD392.DB;
using SWD392.Models;

namespace SkincarePharmacyNetCore8.Helpers
{
    public class ApplicationMapper : Profile
    {
        public ApplicationMapper()
        {
            CreateMap<Product, ProductModel>().ReverseMap();

            CreateMap<Image, ImageModel>().ReverseMap();

            CreateMap<UnitProduct, UnitProductModel>().ReverseMap();

            CreateMap<Unit, UnitModel>().ReverseMap();

            CreateMap<Brand, BrandModel>().ReverseMap();

            CreateMap<Packaging, PackagingModel>().ReverseMap();

            CreateMap<Category, CategoryModel>().ReverseMap();

            CreateMap<Solution, SolutionModel>().ReverseMap();

            CreateMap<BrandOrigin, BrandOriginModel>().ReverseMap();

            CreateMap<Manufacturer, ManufacturerModel>().ReverseMap();

            CreateMap<ManufacturedCountry, ManufacturedCountryModel>().ReverseMap();

            CreateMap<ProductDetail, ProductDetailModel>().ReverseMap();
        }
    }
}

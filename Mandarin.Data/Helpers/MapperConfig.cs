using AutoMapper;
using Mandarin.Data.Models;
using Mandarin.Data.ViewModels;

namespace Mandarin.Data.Helpers
{
    public class MapperConfig
    {
        private static readonly MapperConfiguration config;

        static MapperConfig()
        {
            config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ProductVM, Product>();
            });
        }

        public static IMapper GetMapper()
        {
            return config.CreateMapper();
        }
    }
}

using AutoMapper;
using Mandarin.Web.Models;
using Mandarin.Web.ViewModels;

namespace Mandarin.Web.Helpers
{
    public class MapperConfig
    {
        private static MapperConfiguration config;

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

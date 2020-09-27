using System;
using AutoMapper;
using Crawl.WebAPI.Common.Contract.Image;
using Crawl.WebAPI.Common.Domain.Entities;

namespace Crawl.WebAPI.Common.Automapper
{
	public class ImageAutomapperProfile : Profile
	{
		public ImageAutomapperProfile()
		{
			CreateMap<ImageEntity, ImageResponseData>()
				.ForMember(dest => dest.Version, opt => opt.MapFrom(src => src.Version))
				.ForMember(dest => dest.ImageAppKey, opt => opt.MapFrom(src => src.AppKey))
				.ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.ImageUrl))
				.ForMember(dest => dest.ImageWebSrc, opt => opt.MapFrom(src => MapImageWebSrc(src)));
		}

		private string MapImageWebSrc(ImageEntity src)
		{
			return src.Image != null && src.Image.Length > 0
				? "data:image/jpeg;base64," + Convert.ToBase64String(src.Image)
				: null;
		}
	}
}
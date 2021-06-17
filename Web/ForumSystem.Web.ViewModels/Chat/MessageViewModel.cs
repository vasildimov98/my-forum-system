namespace ForumSystem.Web.ViewModels.Chat
{
    using System;

    using AutoMapper;
    using ForumSystem.Data.Models;
    using ForumSystem.Services.Mapping;

    public class MessageViewModel : IMapFrom<Message>, IHaveCustomMappings
    {
        public string Content { get; set; }

        public DateTime CreatedOn { get; set; }

        public string User { get; set; }

        public string ImageSrc { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration
                .CreateMap<Message, MessageViewModel>()
                .ForMember(x => x.User, opt =>
                    opt.MapFrom(y => y.User.UserName));

            configuration
                .CreateMap<Message, MessageViewModel>()
                .ForMember(x => x.ImageSrc, opt =>
                    opt.MapFrom(y => y.User.HasImage ?
                        "/profileImages/" + y.User.ProfileImage.Id + y.User.ProfileImage.Extention :
                        "https://cdn.pixabay.com/photo/2015/10/05/22/37/blank-profile-picture-973460_1280.png"));
        }
    }
}

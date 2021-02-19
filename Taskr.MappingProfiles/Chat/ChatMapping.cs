using Taskr.Domain;
using Taskr.Dtos.Chat;

namespace Taskr.MappingProfiles.Chat
{
    public class ChatMapping : AutoMapper.Profile
    {
        public ChatMapping()
        {
            CreateMap<ApplicationUser, MessageSenderDto>();

            CreateMap<Message, MessageDto>()
                .ForMember(x => x.Sender, opt => opt.MapFrom(x => x.Sender));

            CreateMap<Domain.Chat, ChatDto>()
                .ForMember(x => x.NewestMessage, opt => opt.MapFrom<LastMessageValueResolver>())
                .ForMember(x => x.JobBudget, opt => opt.MapFrom(x => x.Job.InitialPrice))
                .ForMember(x => x.JobPhotos, opt => opt.MapFrom(x => x.Job.Photos))
                .ForMember(x => x.JobTitle, opt => opt.MapFrom(x => x.Job.Title))
                .ForMember(x => x.JobId, opt => opt.MapFrom(x => x.Job.Id));

            CreateMap<Domain.Chat, SingleChatDto>()
                .ForMember(x => x.JobBudget, opt => opt.MapFrom(x => x.Job.InitialPrice))
                .ForMember(x => x.JobPhotos, opt => opt.MapFrom(x => x.Job.Photos))
                .ForMember(x => x.JobTitle, opt => opt.MapFrom(x => x.Job.Title))
                .ForMember(x => x.JobId, opt => opt.MapFrom(x => x.Job.Id))
                .ForMember(x => x.RunnerId, opt => opt.MapFrom(x => x.Runner.Id))
                .ForMember(x => x.TaskrId, opt => opt.MapFrom(x => x.Taskr.Id));

        }
    }
}
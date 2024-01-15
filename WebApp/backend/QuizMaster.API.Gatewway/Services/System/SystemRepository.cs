using AutoMapper;
using Microsoft.EntityFrameworkCore;
using QuizMaster.API.Gateway.Models.System;
using QuizMaster.API.Gateway.SystemData.Contexts;

namespace QuizMaster.API.Gateway.Services.System
{
    public class SystemRepository
    {
        private readonly SystemDbContext dbContext;
        private readonly IMapper _mapper;
        public SystemRepository(SystemDbContext dbContext, IMapper _mapper)
        {
            this.dbContext = dbContext;
            this._mapper = _mapper;
        }

        public async Task EditSystemAboutAsync(AboutModel model)
        {
            SystemAbout? systemAbout = await dbContext.SystemAboutData.FindAsync(new SystemAbout { Id = 1 });
            systemAbout ??= new SystemAbout { Id = 1 };

            _mapper.Map(model, systemAbout);

            await dbContext.SaveChangesAsync();
        }

        public async Task EditSystemContactInformationAsync(ContactModel model)
        {
            SystemContact? systemContact = await dbContext.SystemContactData.FindAsync(new SystemContact { Id = 1 });
            systemContact ??= new SystemContact {  Id = 1 };

            _mapper.Map(model, systemContact);

            await dbContext.SaveChangesAsync();
        }

        public async Task<AboutModel> GetSystemAboutAsync()
        {
            return _mapper.Map<AboutModel>(await dbContext.SystemAboutData.FindAsync(new SystemAbout { Id = 1 }));
        }

        public async Task<ContactModel> GetContactInformationAsync()
        {
            return _mapper.Map<ContactModel>(await dbContext.SystemContactData.FindAsync(new SystemContact { Id = 1 }));
        }

        public async Task SaveReachingContactAsync(SubmitContactModel model)
        {
            await dbContext.SystemReachingContacts.AddAsync(_mapper.Map<ContactReaching>(model));
            await dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<ContactReaching>> GetContactReachingsAsync()
        {
            return await dbContext.SystemReachingContacts.ToListAsync();
        }

        public async Task SaveReviewsAsync(ReviewModel model)
        {
            await dbContext.SystemReviews.AddAsync(_mapper.Map<Reviews>(model));
            await dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<ReviewModel>> GetReviewsAsync()
        {
            return await dbContext.SystemReviews.ToListAsync();
        }
    }
}

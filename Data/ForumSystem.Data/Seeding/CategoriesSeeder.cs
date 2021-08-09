namespace ForumSystem.Data.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using ForumSystem.Data.Models;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.DependencyInjection;

    using static ForumSystem.Common.GlobalConstants;

    public class CategoriesSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.Categories.Any())
            {
                return;
            }

            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            var administrator = await userManager
                .FindByNameAsync("VasilDimov98");

            var categories = new List<Category>
            {
                new Category
                {
                    Name = "Animals",
                    Description = "<p>An animal (plural: animals) refers to&nbsp;<strong>any of the eukaryotic multicellular organisms of the biological kingdom Animalia</strong> is generally characterized to be heterotrophic, motile, having specialized sensory organs, lacking a cell wall, and growing from a blastula during embryonic development.</p>",
                    ImageUrl = "https://a-z-animals.com/media/2021/01/mammals-400x300.jpg",
                },
                new Category
                {
                    Name = "Money",
                    Description = "<p>Money is any item or verifiable record that is generally accepted as payment for goods and services and repayment of debts, such as taxes, in a particular country or socio-economic context.</p>",
                    ImageUrl = "http://wearethenationnews.com/wp-content/uploads/2021/05/Befriend-your-money-and-reap-the-benefits.jpeg",
                },
                new Category
                {
                    Name = "Question and answer",
                    Description = "<p>This is the place to ask and answer thought-provoking questions.</p>",
                    ImageUrl = "https://endivesoftware.com/blog/wp-content/uploads/2019/08/How-to-Develop-a-Question-Answer-Mobile-app-like-Quora.jpg",
                },
                new Category
                {
                    Name = "Sports",
                    Description = "<p>Sports News and Highlights from around the world.</p>",
                    ImageUrl = "https://buckssport.org/wp-content/uploads/2021/01/Return_of_Sports.jpg",
                },
                new Category
                {
                    Name = "World News",
                    Description = "<p>A place for major news from around every day.</p>",
                    ImageUrl = "https://www.bag.admin.ch/bag/en/home/das-bag/aktuell/news/news-02-08-2021/_jcr_content/image.imagespooler.png/1627648965663/588.1000/Icons-18.png",
                },
                new Category
                {
                    Name = "Books",
                    Description = "<p>It is our intent and purpose to foster and encourage in-depth discussion about all things related to books, authors, genres, or publishing in a safe, supportive environment.</p>",
                    ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/b/b6/Gutenberg_Bible%2C_Lenox_Copy%2C_New_York_Public_Library%2C_2009._Pic_01.jpg",
                },
                new Category
                {
                    Name = "Technology",
                    Description = "<p>The goal of this community is to provide a space dedicated to the intelligent discussion of innovations and changes to technology in our ever-changing world. We focus on high-quality news articles about technology and informative and thought-provoking self-posts.</p>",
                    ImageUrl = "https://www.hydrocarbons-technology.com/wp-content/uploads/sites/9/2020/09/shutterstock_1152185600-1440x1008-1-857x600.jpg",
                },
                new Category
                {
                    Name = "Movies",
                    Description = "<p>Movie News and Discussion</p>",
                    ImageUrl = "https://s3-us-west-2.amazonaws.com/flx-editorial-wordpress/wp-content/uploads/2018/03/13153742/RT_300EssentialMovies_700X250.jpg",
                },
                new Category
                {
                    Name = "Games",
                    Description = "<p>A game is a structured form of play, usually undertaken for entertainment or fun, and sometimes used as an educational tool. Games are distinct from work, which is usually carried out for remuneration, and from art, which is more often an expression of aesthetic or ideological elements.</p>",
                    ImageUrl = "https://cdn.vox-cdn.com/thumbor/dprVEYcYRnVxyAUJMi8e2uroflY=/0x0:1020x680/1200x800/filters:focal(429x259:591x421)/cdn.vox-cdn.com/uploads/chorus_image/image/64915557/2013-11-22_13-13-07.0.jpg",
                },
                new Category
                {
                    Name = "TV Series",
                    Description = "<p>A Community For The High Quality Discussion of Television.</p>",
                    ImageUrl = "https://phantom-marca.unidadeditorial.es/7703d5206d58c4e2696f4a93f0060605/resize/1320/f/jpg/assets/multimedia/imagenes/2020/12/23/16087305805597.jpg",
                },
            };

            foreach (var category in categories)
            {
                category.OwnerId = administrator.Id;
            }

            await dbContext.Categories.AddRangeAsync(categories);
            await dbContext.SaveChangesAsync();
        }
    }
}

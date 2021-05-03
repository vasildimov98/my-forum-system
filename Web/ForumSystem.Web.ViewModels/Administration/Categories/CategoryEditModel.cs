﻿namespace ForumSystem.Web.ViewModels.Administration.Categories
{
    using ForumSystem.Data.Models;
    using ForumSystem.Services.Mapping;

    public class CategoryEditModel : IMapFrom<Category>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Image { get; set; }
    }
}

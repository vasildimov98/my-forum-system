﻿namespace ForumSystem.Web.ViewModels.PartialViews
{
    public class PaginationViewModel
    {
        public int CurrentPage { get; set; }

        public int PreviousPage => this.CurrentPage - 1;

        public int NextPage => this.CurrentPage + 1;

        public int PagesCount { get; set; }

        public int TotalPosts { get; set; }

        public string RouteName { get; set; }
    }
}
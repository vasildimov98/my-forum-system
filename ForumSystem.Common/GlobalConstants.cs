namespace ForumSystem.Common
{
    public static class GlobalConstants
    {
        public const string SystemName = "Tellit";

        public const string AdministratorRoleName = "Administrator";
        public const string AdministratorAreaName = "Administration";

        public const string InvalidMessageKey = "InvalidMessage";
        public const string ErrorTitleKey = "ErrorTitleKey";
        public const string ErrorMessageKey = "ErrorMessageKey";
        public const string SuccessMessageKey = "SuccessMessage";

        public const string FamousCategoriesKey = "famousCategories";

        public const string InvalidFileImageLengthMessage = "File is missing or its too big! Allowed length is 10MB";
        public const string InvalidFileImageExtentionMessage = "Invalid file extention. Only png, jpeg, jpg are allowed";

        public const string InvalidApprovalMessage = "Something went wrong with the approval!";
        public const string InvalidNameMessage = "Category name is taken, please try another one.";
        public const string SuccessCategoryCreate = "Category is created and now is waiting for admin approval!";

        public const string InvalidPageRequest = "Invalid page number.";

        public const string ErrorNotFoundTitle = "404 Current Page Not Found";
        public const string ErrorNotFoundMessage = @"<p>Sorry, we're unable to bring you the page you're looking for. Please try:</p>
                                            <ul>
                                                  <li>Double checking the url</li>      
                                                  <li>Hitting the refresh button in your browser</li>      
                                            </ul>
                                             <p>Alternatively, please visit the <a href=""/"" class=""text-secondary"">Tellit forum system homepage.</a></p>";
    }
}

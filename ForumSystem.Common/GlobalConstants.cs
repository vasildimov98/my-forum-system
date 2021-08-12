namespace ForumSystem.Common
{
    public static class GlobalConstants
    {
        public const string SystemName = "Tellit";

        public const string AdministratorRoleName = "Administrator";

        public const string InvalidMessageKey = "InvalidMessage";
        public const string SuccessMessageKey = "SuccessMessage";

        public const string FamousCategoriesKey = "famousCategories";

        public const string InvalidFileImageLengthMessage = "File is missing or its too big! Allowed length is 10MB";
        public const string InvalidFileImageExtentionMessage = "Invalid file extention. Only png, jpeg, jpg are allowed";

        public const string InvalidApprovalMessage = "Something went wrong with the approval!";
        public const string InvalidNameMessage = "Category name is taken, please try another one.";
        public const string SuccessCategoryCreate = "Category is created and now is waiting for admin approval!";
    }
}

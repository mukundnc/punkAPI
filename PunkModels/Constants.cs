namespace PunkModels
{
    public static class Constants
    {
        public const string NameError = "Search string cannot to empty";
        public const string InvalidIdError = "Invalid BeerId";
        public const string RrError = "Rating is required";
        public const string UserError = "Username should be in email format";
        public const string RatingError = "Rating should be between 1 to 5";
        public const string AddFailedError = "Failed to add user rating";

        public const string ActionAttributeName = "name";
        public const string ActionAttributeBeerId = "beerId";
        public const string ActionAttributeUserRating = "rating";
        public const string EmailRegex = @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$";
    }
}

namespace ForumSystem.Data.Models
{
    using System;

    using ForumSystem.Data.Common.Models;

    public class ProfileImage : BaseModel<string>
    {
        public ProfileImage()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        public string Extention { get; set; }

        public string UserId { get; set; }

        public ApplicationUser User { get; set; }
    }
}

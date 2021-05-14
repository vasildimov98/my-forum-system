namespace ForumSystem.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;

    using ForumSystem.Data.Common.Models;

    public class ProfileImage : BaseModel<string>
    {
        public ProfileImage()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        public string Extention { get; set; }

        [ForeignKey(nameof(User))]
        public string UserId { get; set; }

        public ApplicationUser User { get; set; }
    }
}

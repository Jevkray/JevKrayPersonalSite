using System.ComponentModel.DataAnnotations;

namespace JevKrayPersonalSite.Models
{
    public class ProjectPictureModel
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public int Number { get; set; }

        [Url]
        public string ImageLink { get; set; }
    }
}

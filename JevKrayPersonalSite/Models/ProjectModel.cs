namespace JevKrayPersonalSite.Models
{
    public class ProjectModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string AuthorName { get; set; }
        public DateTimeOffset AdditionDate { get; set; }
        public string Description { get; set; }

        public bool HasPreview { get; set; }

        public List<ProjectPictureModel> ProjectPictures { get; set; }
    }
}

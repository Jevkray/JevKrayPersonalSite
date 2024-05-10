using JevKrayPersonalSite.Models;

namespace JevKrayPersonalSite.ViewModels
{
    public class UpdatesViewModel
    {
        public List<CommitModel> Commits { get; set; }
        public int TotalCount { get; set; }
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
    }
}

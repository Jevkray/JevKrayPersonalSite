using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JevKrayPersonalSite.Models
{
    public class CapchaSessionModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string SessionId { get; set; }
        public string CapchaCache { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}

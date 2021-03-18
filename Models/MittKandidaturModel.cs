using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Valgapplikasjon.Models
{
    [Index(nameof(BrukerId), IsUnique = true)]
    public class MittKandidaturModel
    {
        [Key]
        public int KandidatId { get; set; }

        [Column(TypeName="nvarchar(100)")]
        public string BrukerId { get; set; }
        public bool Sjekkboks { get; set; }

    }
}

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Valgapplikasjon.Areas.Identity
{
    // Add profile data for application users by adding properties to the ValgapplikasjonUser class
    public class ValgapplikasjonUser : IdentityUser
    {
        [PersonalData]
        [Column(TypeName = "nvarchar(100)")]
        public string Fornavn { get; set; }

        [PersonalData]
        [Column(TypeName = "nvarchar(100)")]
        public string Etternavn { get; set; }
        public byte[] Profilbilde { get; set; }
        public string BioTittel { get; set; }
        public string Bio { get; set; }
    }
}

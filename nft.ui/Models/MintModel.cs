using System.ComponentModel.DataAnnotations;

namespace nft.ui.Models
{
    public class MintModel
    {
        [Required]
        public int MintCount { get; set; } = 1;
    }
}

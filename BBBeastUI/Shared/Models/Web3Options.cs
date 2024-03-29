﻿namespace BBBeast.UI.Shared.Models
{
    public class Web3Options
    {
        public string Network { get; set; }
        public long ChainId { get; set; }
        public string ContractAddress { get; set; }
        public int MaxMintCount { get; set; }
        public string PublicMintCost { get; set; }
        public string PrivateMintCost { get; set; }
    }
}

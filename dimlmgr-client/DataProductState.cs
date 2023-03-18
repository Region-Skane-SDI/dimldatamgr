namespace RS.SDI.DimlMgr.Client
{
    /// <summary>
    /// Contains the operational metadata for a data product
    /// </summary>
    public class DataProductState
    {
        public Dimlid Dimlid { get; set; }
        public int ViewCount { get; set; }
        public int LikeCount { get; set; }
        public string InformationOwner { get; set; }
        public string ProductOwner { get; set; }
        public bool IsAvailable { get; set; }
        public DateTime LastUpdate { get; set; }
        public string SqlConnectionString { get; set; }
    }
}
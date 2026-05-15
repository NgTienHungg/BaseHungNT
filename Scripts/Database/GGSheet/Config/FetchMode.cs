namespace HungNT.Database
{
    public enum FetchMode
    {
        /// <summary>Public CSV export URL (sheet must be readable by link / public).</summary>
        PublicCsv,

        /// <summary>Google Sheets API v4 with API key (enable Sheets API, restrict key if needed).</summary>
        ApiKey
    }
}
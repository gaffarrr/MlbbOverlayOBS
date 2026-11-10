namespace MlbbOverlay.Backend
{
    public class MlStats
    {
        public string CurrentRank { get; set; } = "MYTHIC";
        public string TargetRank { get; set; } = "GLORY";

        // Properti baru pengganti teks label, nyimpen pilihan Dropdown
        public string TargetRankLogo { get; set; } = "glory";

        public int CurrentStars { get; set; } = 27;
        public int TargetStars { get; set; } = 50;
        public int Wins { get; set; } = 11;
        public int Losses { get; set; } = 2;
        public int WinStreak { get; set; } = 0;

        public int TotalMatches => Wins + Losses;
        public int WinRate => TotalMatches == 0 ? 0 : (int)Math.Round((double)Wins / TotalMatches * 100);
        public int StarsNeeded => TargetStars - CurrentStars;
    }

    public class MlStatsState
    {
        public MlStats Stats { get; set; } = new MlStats();
    }
}

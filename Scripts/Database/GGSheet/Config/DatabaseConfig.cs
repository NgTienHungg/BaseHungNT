using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace HungNT.Database
{
    [CreateAssetMenu(menuName = "HungNT/Database/Database Config (GGSheet)", fileName = "DatabaseConfig")]
    public class DatabaseConfig : ScriptableObject
    {
        [Title("Spreadsheet")]
        [InfoBox("Public CSV: sheet must be shared (Anyone with the link can view). API key: enable Sheets API in Google Cloud.")]
        [InlineButton(nameof(OpenSpreadsheetInBrowser), "Open")]
        [SerializeField]
        private string _sheetId;

        [SerializeField]
        private string _profileName = "Game DB";

        [SerializeField]
        private FetchMode _fetchMode = FetchMode.PublicCsv;

        [ShowIf(nameof(IsApiKeyMode))]
        [SerializeField]
        private string _apiKey;

        [Title("Tables")]
        [SerializeField]
        [ListDrawerSettings(ListElementLabelName = "TableName")]
        private List<SheetProfile> _profiles = new List<SheetProfile>();

        [SerializeField, HideInInspector]
        private int _nextProfileIndex;

        public string SheetId
        {
            get => _sheetId;
            set => _sheetId = value;
        }

        public string ProfileName
        {
            get => _profileName;
            set => _profileName = value;
        }

        public FetchMode FetchMode
        {
            get => _fetchMode;
            set => _fetchMode = value;
        }

        public string ApiKey
        {
            get => _apiKey;
            set => _apiKey = value;
        }

        public List<SheetProfile> Profiles => _profiles;

        private bool IsApiKeyMode() => _fetchMode == FetchMode.ApiKey;

        private void OpenSpreadsheetInBrowser()
        {
            if (string.IsNullOrWhiteSpace(_sheetId))
                return;

            Application.OpenURL("https://docs.google.com/spreadsheets/d/" + _sheetId.Trim());
        }
    }
}

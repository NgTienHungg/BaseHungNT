using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Networking;
using Uri = System.Uri;

namespace HungNT.Database.Editor
{
    public static class GGSheetFetcher
    {
        public static List<string[]> FetchWorksheet(DatabaseConfig config, WorksheetInfo worksheet)
        {
            if (config == null || string.IsNullOrWhiteSpace(config.SheetId))
                throw new ArgumentException("DatabaseConfig.SheetId is empty.");

            if (worksheet == null || string.IsNullOrWhiteSpace(worksheet.SheetName))
                throw new ArgumentException("Worksheet name is empty.");

            string tabRaw = worksheet.SheetName.Trim();
            string sheetId = config.SheetId.Trim();

            switch (config.FetchMode)
            {
                case FetchMode.PublicCsv:
                    if (!string.IsNullOrWhiteSpace(config.ApiKey))
                        EnsureWorksheetTabExistsOnSpreadsheet(sheetId, config.ApiKey.Trim(), tabRaw);
                    return FetchCsv(sheetId, tabRaw);
                case FetchMode.ApiKey:
                    if (string.IsNullOrWhiteSpace(config.ApiKey))
                        throw new ArgumentException("FetchMode.ApiKey requires Api Key on DatabaseConfig.");
                    EnsureWorksheetTabExistsOnSpreadsheet(sheetId, config.ApiKey.Trim(), tabRaw);
                    return FetchViaApi(sheetId, tabRaw, config.ApiKey.Trim());
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>GET spreadsheet metadata — báo lỗi rõ nếu không có tab trùng <paramref name="expectedTabTitle"/>.</summary>
        private static void EnsureWorksheetTabExistsOnSpreadsheet(string sheetId, string apiKey, string expectedTabTitle)
        {
            string url =
                "https://sheets.googleapis.com/v4/spreadsheets/" +
                Uri.EscapeDataString(sheetId) +
                "?fields=sheets.properties.title&key=" +
                Uri.EscapeDataString(apiKey);

            string json;
            try
            {
                json = HttpGetText(url);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(
                    $"Không kiểm tra được danh sách tab trên spreadsheet (sheet id: {sheetId}). {ex.Message}");
            }

            if (!string.IsNullOrEmpty(json) && json.TrimStart().StartsWith("{") && json.Contains("\"error\""))
            {
                int len = Mathf.Min(json.Length, 400);
                string snip = len < json.Length ? json.Substring(0, len) + "…" : json;
                throw new InvalidOperationException(
                    $"Sheets API không đọc được metadata (kiểm tra API key & quyền). Phản hồi: {snip}");
            }

            List<string> titles = ExtractSheetTitlesFromSpreadsheetJson(json);
            if (titles.Count == 0)
            {
                throw new InvalidOperationException(
                    $"Không đọc được danh sách tab từ spreadsheet (sheet id: {sheetId}).");
            }

            for (int i = 0; i < titles.Count; i++)
            {
                if (string.Equals(titles[i], expectedTabTitle, StringComparison.Ordinal))
                    return;
            }

            throw new InvalidOperationException(FormatTabNotFoundMessage(expectedTabTitle, titles));
        }

        private static string FormatTabNotFoundMessage(string expectedTabTitle, List<string> availableTitles)
        {
            string list = string.Join("\", \"", availableTitles);
            return
                $"[GGSheet] Không tìm thấy tab có SheetName = \"{expectedTabTitle}\". " +
                $"Trên Google Sheets, tên tab thanh dưới cùng phải khớp chính xác (kể cả hoa/thường). " +
                $"Các tab hiện có: \"{list}\".";
        }

        private static List<string> ExtractSheetTitlesFromSpreadsheetJson(string json)
        {
            var list = new List<string>();
            if (string.IsNullOrEmpty(json))
                return list;

            foreach (Match m in Regex.Matches(json, "\"title\"\\s*:\\s*\"((?:\\\\.|[^\\\\\"])*)\""))
            {
                if (!m.Success)
                    continue;
                string raw = m.Groups[1].Value;
                list.Add(raw.Replace("\\\"", "\"").Replace("\\\\", "\\"));
            }

            return list;
        }

        private static string HttpGetText(string url)
        {
            using var req = UnityWebRequest.Get(url);
            req.timeout = 60;
            var op = req.SendWebRequest();
            while (!op.isDone)
            {
            }

#if UNITY_2020_1_OR_NEWER
            if (req.result != UnityWebRequest.Result.Success)
#else
            if (req.isNetworkError || req.isHttpError)
#endif
            {
                throw new InvalidOperationException($"{req.error}\n{req.downloadHandler?.text}");
            }

            return req.downloadHandler?.text ?? "";
        }

        private static List<string[]> FetchCsv(string sheetId, string tabName)
        {
            string url =
                "https://docs.google.com/spreadsheets/d/" +
                Uri.EscapeDataString(sheetId) +
                "/gviz/tq?tqx=out:csv&sheet=" +
                Uri.EscapeDataString(tabName);

            return RequestText(url, body =>
            {
                ValidateGvizCsvBody(body, tabName, sheetId);
                var rows = CsvParser.Parse(body);
                ValidateCsvHasRowsForTab(rows, tabName, sheetId);
                return rows;
            });
        }

        private static List<string[]> FetchViaApi(string sheetId, string sheetTitle, string apiKey)
        {
            string range = EncodeSheetRangeForApi(sheetTitle);
            string url =
                "https://sheets.googleapis.com/v4/spreadsheets/" +
                Uri.EscapeDataString(sheetId) +
                PathSegmentValues +
                range +
                "?key=" +
                Uri.EscapeDataString(apiKey);

            return RequestText(url, body =>
            {
                if (!string.IsNullOrEmpty(body))
                {
                    var trimmed = body.TrimStart();
                    if (trimmed.StartsWith("{") && trimmed.Contains("\"error\""))
                    {
                        int len = Mathf.Min(body.Length, 480);
                        string snip = len < body.Length ? body.Substring(0, len) + "…" : body;
                        if (snip.IndexOf("Unable to parse range", StringComparison.OrdinalIgnoreCase) >= 0 ||
                            snip.IndexOf("not found", StringComparison.OrdinalIgnoreCase) >= 0 ||
                            snip.IndexOf("INVALID_ARGUMENT", StringComparison.OrdinalIgnoreCase) >= 0)
                        {
                            throw new InvalidOperationException(
                                $"[GGSheet] Không tìm thấy tab hoặc range hợp lệ cho SheetName = \"{sheetTitle}\". " +
                                $"Kiểm tra tên tab trên Google Sheets (khớp chính xác). Chi tiết: {snip}");
                        }

                        throw new InvalidOperationException(
                            $"Sheets API lỗi cho tab '{sheetTitle}'. Phản hồi: {snip}");
                    }
                }

                List<string[]> parsed = SheetsApiValuesParser.Parse(body);
                ValidateCsvHasRowsForTab(parsed, sheetTitle, sheetId);
                return parsed;
            });
        }

        private const string PathSegmentValues = "/values/";

        private static void ValidateCsvHasRowsForTab(List<string[]> rows, string tabName, string sheetId)
        {
            if (rows == null || rows.Count == 0)
            {
                throw new InvalidOperationException(
                    $"[GGSheet] Không có dữ liệu cho tab \"{tabName}\" (0 hàng sau khi đọc). " +
                    "Thường do **WorksheetInfo.SheetName** không trùng tên tab trên Google Sheets " +
                    $"(ví dụ tab thật là \"ItemTable\" nhưng để \"Item\"), hoặc tab hoàn toàn trống. Sheet id: {sheetId}");
            }
        }

        /// <summary>Detect HTML / empty / Google error pages from public CSV export (wrong tab name, private doc, etc.).</summary>
        private static void ValidateGvizCsvBody(string body, string tabName, string sheetId)
        {
            if (string.IsNullOrWhiteSpace(body))
                throw new InvalidOperationException(
                    $"[GGSheet] CSV rỗng cho tab \"{tabName}\". " +
                    "Không tìm thấy worksheet đó hoặc sheet chưa public — kiểm tra **SheetName** trùng tên tab trên thanh dưới Google Sheets " +
                    $"(trùng chữ, hoa/thường) và share (Anyone with the link can view). Sheet id: {sheetId}");

            string t = body.TrimStart();
            if (t.StartsWith("<!DOCTYPE", StringComparison.OrdinalIgnoreCase) ||
                t.StartsWith("<html", StringComparison.OrdinalIgnoreCase))
                throw new InvalidOperationException(
                    $"[GGSheet] Google trả HTML thay vì CSV cho tab \"{tabName}\" — thường do **tên tab sai**, file không public, hoặc cần đăng nhập. Sheet id: {sheetId}");

            if (body.IndexOf("Bad Request", StringComparison.OrdinalIgnoreCase) >= 0 ||
                body.IndexOf("invalid query", StringComparison.OrdinalIgnoreCase) >= 0 ||
                body.IndexOf("specifies an invalid", StringComparison.OrdinalIgnoreCase) >= 0)
                throw new InvalidOperationException(
                    $"[GGSheet] Yêu cầu CSV không hợp lệ cho tab \"{tabName}\". " +
                    $"Chắc chắn **WorksheetInfo.SheetName** trùng tên tab thật trên file. Sheet id: {sheetId}");

            if (body.IndexOf("We're sorry", StringComparison.OrdinalIgnoreCase) >= 0 ||
                body.IndexOf("cannot be displayed", StringComparison.OrdinalIgnoreCase) >= 0)
                throw new InvalidOperationException(
                    $"[GGSheet] Google không trả CSV cho tab \"{tabName}\" (trang lỗi). Kiểm tra tên tab và quyền truy cập. Sheet id: {sheetId}");
        }

        /// <summary>Path segment for /values/{range}: quote sheet title if needed.</summary>
        private static string EncodeSheetRangeForApi(string sheetTitle)
        {
            if (string.IsNullOrEmpty(sheetTitle))
                return Uri.EscapeDataString("A1");

            bool quote = sheetTitle.Contains(" ") || sheetTitle.Contains("'") || sheetTitle.Contains("!");
            string range = quote
                ? "'" + sheetTitle.Replace("'", "''") + "'"
                : sheetTitle;

            return Uri.EscapeDataString(range);
        }

        private static List<string[]> RequestText(string url, Func<string, List<string[]>> parse)
        {
            using var req = UnityWebRequest.Get(url);
            req.timeout = 120;
            var op = req.SendWebRequest();
            while (!op.isDone)
            {
            }

#if UNITY_2020_1_OR_NEWER
            if (req.result != UnityWebRequest.Result.Success)
#else
            if (req.isNetworkError || req.isHttpError)
#endif
            {
                string snippet = req.downloadHandler?.text ?? "";
                const int max = 600;
                if (snippet.Length > max)
                    snippet = snippet.Substring(0, max) + "…";
                DebugEx.LogEditorError(
                    $"{("[GGSheet]").Bold()} {"HTTP / network error".Color("red")}: " +
                    $"{(req.error ?? "").Color("orange")}\n{snippet}");
                throw new InvalidOperationException($"GGSheet fetch failed ({url}): {req.error}\n{req.downloadHandler?.text}");
            }

            return parse(req.downloadHandler?.text ?? "");
        }
    }
}

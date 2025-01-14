﻿using System.Collections.Generic;
using System.Linq;
using commonItems;

namespace ImperatorToCK3.CK3.Titles {
	// This is a recursive class that scrapes 00_landed_titles.txt (and related files) looking for title colors, landlessness,
	// and most importantly relation between baronies and barony provinces so we can link titles to actual clay.
	// Since titles are nested according to hierarchy we do this recursively.
	public class LandedTitles : Parser {
		public void LoadTitles(string fileName) {
			RegisterKeys();
			ParseFile(fileName);
			ClearRegisteredRules();

			LinkCapitals();
		}
		public void LoadTitles(BufferedReader reader) {
			RegisterKeys();
			ParseStream(reader);
			ClearRegisteredRules();

			LinkCapitals();
		}
		public void InsertTitle(Title? title) {
			if (title is null) {
				Logger.Warn("Cannot insert null Title to LandedTitles!");
				return;
			}
			if (!string.IsNullOrEmpty(title.Name)) {
				StoredTitles[title.Name] = title;
			} else {
				Logger.Warn("Not inserting a Title with empty name!");
			}
		}
		public void EraseTitle(string name) {
			if (StoredTitles.TryGetValue(name, out var titleToErase)) {
				var deJureLiege = titleToErase.DeJureLiege;
				if (deJureLiege is not null) {
					deJureLiege.DeJureVassals.Remove(name);
				}

				var deFactoLiege = titleToErase.DeFactoLiege;
				if (deFactoLiege is not null) {
					deFactoLiege.DeFactoVassals.Remove(name);
				}

				foreach (var vassal in titleToErase.DeJureVassals.Values) {
					if (vassal is not null) {
						vassal.DeJureLiege = null;
					}
				}
				foreach (var vassal in titleToErase.DeFactoVassals.Values) {
					if (vassal is not null) {
						vassal.DeFactoLiege = null;
					}
				}

				if (titleToErase.ImperatorCountry is not null) {
					titleToErase.ImperatorCountry.CK3Title = null;
				}
			}
			StoredTitles.Remove(name);
		}
		public Title? GetCountyForProvince(ulong provinceID) {
			var counties = StoredTitles.Values.Where(title => title.Rank == TitleRank.county);
			foreach (var county in counties) {
				if (county?.CountyProvinces.Contains(provinceID) == true) {
					return county;
				}
			}
			return null;
		}

		public Dictionary<string, Title> StoredTitles { get; } = new(); // title name, title

		private void RegisterKeys() {
			RegisterRegex(@"(e|k|d|c|b)_[A-Za-z0-9_\-\']+", (reader, titleNameStr) => {
				// Pull the titles beneath this one and add them to the lot, overwriting existing ones.
				var newTitle = new Title(titleNameStr);
				newTitle.LoadTitles(reader);

				Title.AddFoundTitle(newTitle, StoredTitles);
			});
			RegisterRegex(CommonRegexes.Catchall, ParserHelpers.IgnoreAndLogItem);
		}
		
		private void LinkCapitals() {
			foreach (var title in StoredTitles.Values) {
				if (title.CapitalCounty is not null && title.CapitalCounty.Value.Value is null) {
					var countyName = title.CapitalCounty.Value.Key;
					title.CapitalCounty = new(countyName, StoredTitles[countyName]);
				}
			}
		}
	}
}

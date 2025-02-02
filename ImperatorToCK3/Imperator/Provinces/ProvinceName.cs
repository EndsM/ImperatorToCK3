﻿using commonItems;

namespace ImperatorToCK3.Imperator.Provinces {
	public class ProvinceName : Parser {
		public string Name { get; private set; } = "";

		public ProvinceName(BufferedReader reader) {
			RegisterKeys();
			ParseStream(reader);
			ClearRegisteredRules();
		}
		private void RegisterKeys() {
			RegisterKeyword("name", reader => {
				Name = new SingleString(reader).String;
			});
			RegisterRegex(CommonRegexes.Catchall, ParserHelpers.IgnoreAndLogItem);
		}
	}
}

﻿using System;
using System.Collections.Generic;
using commonItems;

namespace ImperatorToCK3.Imperator.Pops {
    class PopsBloc : Parser {
        public Pops PopsFromBloc { get; private set; } = new();
        public PopsBloc(BufferedReader reader) {
            RegisterKeys();
            ParseStream(reader);
            ClearRegisteredRules();
        }
        private void RegisterKeys() {
            RegisterKeyword("population", (reader) => {
                PopsFromBloc.LoadPops(reader);
            });
            RegisterRegex(CommonRegexes.Catchall, ParserHelpers.IgnoreAndLogItem);
        }
    }
}

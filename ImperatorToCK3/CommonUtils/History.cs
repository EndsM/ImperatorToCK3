﻿using System.Collections.Generic;
using commonItems;

namespace ImperatorToCK3.CommonUtils {
	public class History {
		public Dictionary<string, SimpleField> SimpleFields { get; } = new(); // fieldName, field
		public Dictionary<string, ContainerField> ContainerFields { get; } = new(); // fieldName, field

		public History() { }
		public History(Dictionary<string, SimpleField> simpleFields, Dictionary<string, ContainerField> containerFields) {
			this.SimpleFields = simpleFields;
			this.ContainerFields = containerFields;
		}

		public string? GetSimpleFieldValue(string fieldName, Date date) {
			if (SimpleFields.TryGetValue(fieldName, out var value)) {
				return value.GetValue(date);
			}
			return null;
		}
		public List<string>? GetContainerFieldValue(string fieldName, Date date) {
			if (ContainerFields.TryGetValue(fieldName, out var value)) {
				return value.GetValue(date);
			}
			return null;
		}

		public void AddSimpleFieldValue(string fieldName, string value, Date date) {
			if (SimpleFields.ContainsKey(fieldName)) {
				SimpleFields[fieldName].AddValueToHistory(value, date);
			} else {
				var field = new SimpleField(null);
				field.AddValueToHistory(value, date);
				SimpleFields.Add(fieldName, field);
			}
		}
		public void AddContainerFieldValue(string fieldName, List<string> value, Date date) {
			if (ContainerFields.ContainsKey(fieldName)) {
				ContainerFields[fieldName].AddValueToHistory(value, date);
			} else {
				var field = new ContainerField(new());
				field.AddValueToHistory(value, date);
				ContainerFields.Add(fieldName, field);
			}
		}
	}
}

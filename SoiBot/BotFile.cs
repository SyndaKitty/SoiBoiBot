using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SoiBot {
    public class BotFile {
        string filePath;

        public BotFile(string filePath) {
            this.filePath = filePath;
        }
        
        public void RecordVariable(string name, int value) {
            RecordVariable(name, value.ToString());
        }

        public void RecordVariable(string name, float value) {
            RecordVariable(name, value.ToString());
        }

        public void RecordVariable(string name, string value) {
            var values = ReadFile();
            if (!values.ContainsKey(name)) {
                values.Add(name, value);
            }
            else values[name] = value;
            WriteFile(values);
        }

        public string GetVariable(string name) {
            var values = ReadFile();
            if (values.ContainsKey(name)) return values[name];
            return null;
        }

        Dictionary<string, string> ReadFile() {
            Dictionary<string, string> values = new Dictionary<string, string>();
            if (!File.Exists(filePath)) {
                return values;
            }

            var lines = File.ReadAllLines(filePath);
            foreach (var line in lines) {
                var parts = line.Split(new[] { '=' }, StringSplitOptions.RemoveEmptyEntries);
                string key = parts[0];
                string value = parts[1];
            }
            return values;
        }

        void WriteFile(Dictionary<string, string> values) {
            StringBuilder buffer = new StringBuilder();
            foreach (var kvp in values) {
                string key = kvp.Key;
                string value = kvp.Value;
                buffer.AppendLine($"{key}={value}");
            }

            File.WriteAllText(filePath, buffer.ToString());
        }
    }
}

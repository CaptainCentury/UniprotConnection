using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UniprotService;
using Xunit;

namespace UniprotServiceTests
{
    public class UniprotTests
    {
        [Fact]
        public void JsonToEntryTest()
        {
            var jsonString = "{accession: \"P123\", comments: [{\"type\":\"FUNCTION\",\"text\":[{\"value\":\"dehydrogenase\"}]}]}";
            UniprotEntry entry = JsonConvert.DeserializeObject<UniprotEntry>(jsonString);

            Assert.Equal("P123", entry.UniprotId);
            Assert.Equal("dehydrogenase", entry.Function);
        }

        [Fact]
        public void JsonSequenceTest()
        {
            var entry = new UniprotEntry("P123", "function...", "ABC...");

            Assert.Equal("{\n  \"sequence\": \"ABC...\"\n}", entry.JsonSequence.ToString());
        }

        [Fact]
        public void JsonFunctionTest()
        {
            var jsonString1 = "{\"type\":\"FUNCTION\",\"text\":[{\"value\":\"PART 1\"}, {\"value\":\"PART 2\"}]}";
            var jsonObject1 = JObject.Parse(jsonString1);

            var jsonString2 = "{\"type\":\"FUNCTION\",\"text\":[{\"value\":\"PART 3\"}, {\"value\":\"PART 4\"}]}";
            var jsonObject2 = JObject.Parse(jsonString2);

            var jsonArray = new JArray();
            jsonArray.Add(jsonObject1);
            jsonArray.Add(jsonObject2);

            var entry = new UniprotEntry();
            entry.JsonComments = jsonArray;

            Assert.Equal("PART 1, PART 2, PART 3, PART 4", entry.Function);
        }

        [Fact]
        public void ExampleP21802()
        {
            string jsonString = System.IO.File.ReadAllText(@"examples/example_P21802.txt");
            UniprotEntry entry = JsonConvert.DeserializeObject<UniprotEntry>(jsonString);

            Assert.Equal("P21802", entry.UniprotId);
            Assert.Equal("Tyrosine-protein kinase that acts as cell-surface receptor for fibroblast growth factors", 
                         entry.Function.Substring(0, Math.Min(entry.Function.Length, 88)));
            
            Assert.Equal("MVSWGRFICLVVVTMATLSL", entry.Sequence.Substring(0, 20));
        }
    }
}

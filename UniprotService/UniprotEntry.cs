using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;

namespace UniprotService
{
    public class UniprotEntry
    {
        public UniprotEntry()
        {
            UniprotId = "UNIPROT ID NOT FOUND";
            Function = "FUNCTION NOT FOUND";
            Sequence = "SEQUENCE NOT FOUND";
        }
        public UniprotEntry(string id, string function, string sequence)
        {
            UniprotId = id;
            Function = function;
            Sequence = sequence;
        }

        [JsonProperty("accession")]
        public string UniprotId { get; set; }
        
        [JsonProperty("comments")]
        public JArray JsonComments 
        {
            get 
            {
                var array = new JArray();
                var jsonFunction = JObject.Parse($"{{ \"type\":\"FUNCTION\", \"text\":[ {{ \"value\":\"{Function}\" }} ] }}"); 
                array.Add(jsonFunction);
                return array;
            }
            set
            {
                var functionTexts = value
                    .Where(commentElement => (string)commentElement.SelectToken("type") == "FUNCTION")
                    .Select(functionElement => (JArray)functionElement.SelectToken("text"))
                    .SelectMany(textArrayElement => textArrayElement.Select(textElement => textElement.SelectToken("value")))
                    .Select(valueElement => valueElement.Value<string>()).ToArray();
                
                Function = string.Join(", ", functionTexts); 
            }
        }

        public string Function { get; set; }
        
        [JsonProperty("sequence")]
        public JObject JsonSequence {
            get 
            { 
                string jsonString = $"{{sequence: \"{Sequence}\"}}";
                return JObject.Parse(jsonString);
            }
            set 
            {
                var jObject = value;
                Sequence = (string)jObject.SelectToken("sequence");
            }
        }

        public string Sequence { get; set; }
    }
}
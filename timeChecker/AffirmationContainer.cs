using System.Text.Json;
using System.Text.Json.Serialization;

namespace timeChecker
{
    public partial class AffirmationContainer
    {
        [JsonPropertyName("affirmations")]
        public Affirmation[] Affirmations { get; set; }
        [JsonIgnore]
        private int _affirmationNumber;
        [JsonIgnore]
        public int currentAffirmation
        {
            get { return _affirmationNumber; }
            set
            {
                if (value >= Affirmations?.Length) _affirmationNumber = 0;
                else _affirmationNumber = value;
            }
        }
        public AffirmationContainer()
        {
            Affirmations = [];
            currentAffirmation = 0;
        }
    }

    public partial class Affirmation
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("text")]
        public string Text { get; set; }
    }

    public partial class AffirmationContainer
    {
        public static AffirmationContainer FromJson(string json) => JsonSerializer.Deserialize<AffirmationContainer>(json);
    }
}

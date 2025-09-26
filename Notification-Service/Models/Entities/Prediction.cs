namespace Notification_Service.Models.Entities
{
    public class Prediction
    {
        public string? Description { get; set; }
        public string? Id { get; set; }
        public string? Place_Id { get; set; }
        public string? Reference { get; set; }
        public List<MatchedSubstrings>? matched_substrings { get; set; } //No respetamos la nomenclatura, debido a que esta clase es de google, por ende debemos seguir su nomenclatura
        public StructuredFormatting? structured_formatting { get; set; }
        public List<Terms>? Terms { get; set; }
        public List<string>? Types { get; set; }
        public string? Lat { get; set; }
        public string? Lng { get; set; }
    }
    public class MatchedSubstrings
    {
        public int? Length { get; set; }
        public int? Offset { get; set; }
    }

    public class StructuredFormatting
    {
        public string? main_text { get; set; }
        public string? secondary_text { get; set; }
    }

    public class Terms
    {
        public int? Offset { get; set; }
        public string? Value { get; set; }
    }
}

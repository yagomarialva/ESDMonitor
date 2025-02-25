namespace BiometricFaceApi.Models
{
    public class SqlStructModel
    {
        public string? TableName { get; set; }
        public string? CommandToCreateTable { get; set; }
        public string? CommandToSequence { get; set; }
        public string? CommandToTrigger { get; set; }
        public string? CommandToPopulete { get; set; }
        public string? CommandToTrigger1 { get; set; }
    }
}

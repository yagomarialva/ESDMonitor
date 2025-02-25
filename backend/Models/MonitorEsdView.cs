using System.Text.Json.Serialization;

namespace BiometricFaceApi.Models
{
    public class MonitorEsdView
    {
        
        public MonitorEsdModel MonitorsEsd { get; set; }
        public int PositionSequence { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public LastLogMonitorEsdModel LogOperator { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public LastLogMonitorEsdModel LogJig { get; set; }
        public MonitorEsdView()
        {
            LogOperator = new LastLogMonitorEsdModel();
            LogJig = new LastLogMonitorEsdModel();

        }
    }
}

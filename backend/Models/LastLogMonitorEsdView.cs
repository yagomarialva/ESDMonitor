namespace BiometricFaceApi.Models
{
    public class LastLogMonitorEsdView
    {
        public LastLogMonitorEsdModel LastLogMonitorEsdModel { get; set; }
        public LastLogMonitorEsdModel LogOperator { get; set; }
        public LastLogMonitorEsdModel LogJig { get; set; }
        public LastLogMonitorEsdView()
        {
            LogOperator = new LastLogMonitorEsdModel();
            LogJig = new LastLogMonitorEsdModel();
        }
    }
}

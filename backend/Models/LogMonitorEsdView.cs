namespace BiometricFaceApi.Models
{
    public class LogMonitorEsdViewModel
    {
        public int LogMonitorEsdID { get; set; }
        public string LogOperator { get; set; }
        public string LogJig { get; set; }
        public MonitorEsdModel MonitorsEsd { get; set; }
        public LogMonitorEsdModel LogMonitorEsd { get; set; }
        public LogMonitorEsdViewModel() 
        {
            MonitorsEsd = new MonitorEsdModel();
            LogMonitorEsd = new LogMonitorEsdModel();
        }
    }
}

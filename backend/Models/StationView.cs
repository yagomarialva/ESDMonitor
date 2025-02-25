namespace BiometricFaceApi.Models
{
    public class StationView
    {
        public int LinkStationAndLineID { get; set; }
        public int StationViewID { get; set; }
        public StationModel Station { get; set; }
        public List<MonitorEsdView> MonitorsEsd { get; set; }
        public StationView()
        {
            MonitorsEsd = new List<MonitorEsdView>();
            Station = new StationModel();
        }
    }
}

namespace BiometricFaceApi.Models
{
    public class LineView
    {
        public LineModel Line { get; set; }
        public List<StationView> Stations { get; set; }
        public LineView()
        {

            Line = new LineModel();
            Stations = new List<StationView>();
        }
    }
}

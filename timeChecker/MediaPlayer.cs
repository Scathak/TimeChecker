using System.Windows.Media;
using System.IO;

namespace timeChecker
{
    public class AlarmPlayer(string fileName)
    {
        private readonly string _fileName = fileName;

        public void StartAlarm()
        {
            var theread1 = new Thread(new ThreadStart(NewAlarm));
            theread1.Start();
        }
        private void NewAlarm()
        {
            var uri = new Uri(Path.Combine(AppDomain.CurrentDomain.BaseDirectory
                , "wav"
                , _fileName));
            var player = new MediaPlayer();
            player.Open(uri);
            player.Play();
        }
    }
}

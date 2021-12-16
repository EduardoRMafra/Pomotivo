using System.Media;

namespace Pomotivo.Entities
{
    public class Time
    {
        public static int HH; 
        public static int MM;
        public static int SS;
        static SoundPlayer alarm = new SoundPlayer();

        public Time(int hh, int mm,int ss)
        {
            HH = hh;
            MM = mm;
            SS = ss;
            alarm.Stream = Properties.Resources.alarmClock;
        }
        public static void StartTimer() //diminui 1 segundo e converte as horas em minutos e os minutos em segundos
        {
            SS--;

            if (HH <= 0 && MM <= 0 && SS <= 0)
            {
                HH = 0;
                MM = 0;
                SS = 0;

                alarm.Play();
            }
            else if (SS <= 0 && MM > 0)
            {
                MM--;
                SS = 59;
            }
            else if (SS <= 0 && MM <= 0 && HH > 0)
            {
                HH--;
                MM = 59;
                SS = 59;
            }
        }
        public override string ToString()
        {
            return HH + ":" + MM + ":" + SS;
        }
    }
}

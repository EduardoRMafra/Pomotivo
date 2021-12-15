using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pomotivo.Entities
{
    public class Time
    {
        public static int HH; 
        public static int MM;
        public static int SS;

        public Time(int hh, int mm,int ss)
        {
            HH = hh;
            MM = mm;
            SS = ss;
        }
        public static void StartTimer() //diminui 1 segundo e converte as horas em minutos e os minutos em segundos
        {
            if (HH <= 0 && MM <= 0 && SS <= 0)
            {
                HH = 0;
                MM = 0;
                SS = 0;

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
            
            SS--;
        }
        public override string ToString()
        {
            return HH + ":" + MM + ":" + SS;
        }
    }
}

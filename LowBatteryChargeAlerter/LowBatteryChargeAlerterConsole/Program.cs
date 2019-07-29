using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
using LowBatteryChargeAlerter;
using Microsoft.Win32;

namespace LowBatteryChargeAlerterConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            
            Console.WriteLine("Ingrese email que va a recibir las alertas del estado de bateria");
            string receiver = Console.ReadLine();
            System.Timers.Timer aTimer = new System.Timers.Timer();
            aTimer.Elapsed += delegate { OnTimedEvent(receiver); };
            aTimer.Interval = 300000; //Intervalo de 5 minutos
            aTimer.Enabled = true;
            Console.WriteLine("Ejecución iniciada, cada {0} minutos recibirá una alerta si la batería del equipo " +
                "se encuentra bajo 20% y desconectado de la corriente de energia", aTimer.Interval / 60000);
            Console.WriteLine("Presione \'q\' y posteriormente Enter en cualquier momento para salir.");
            while (Console.Read() != 'q') ;
            aTimer.Dispose();
        }

        // Specify what you want to happen when the Elapsed event is raised.
        private static void OnTimedEvent(string receiver)
        {
            System.Windows.Forms.PowerStatus powerStatus = SystemInformation.PowerStatus;
            //string message = 
            //    ("Estado actual de carga de batería: "+ powerStatus.BatteryChargeStatus + ", <br />" +
            //    "Duracion de la carga de batería en minutos: "+ powerStatus.BatteryFullLifetime / 60 + ", <br />" +
            //    "El porcentaje actual de la batería es: " + powerStatus.BatteryLifePercent * 100+ "%,  <br />" +
            //    "Cantidad de minutos restantes en uso de batería: "+ powerStatus.BatteryLifeRemaining / 60 + ", <br />" +
            //    "El cable de corriente se encuentra: "+ powerStatus.PowerLineStatus);
            if (powerStatus.PowerLineStatus.ToString().Equals("Offline") && powerStatus.BatteryLifePercent < 0.20)
            {
                string message = string.Format("Conecte la linea de corriente, el equipo se encuentra desconectado y le queda {0}% de bateria", (int)powerStatus.BatteryLifePercent * 100);
                MailSender sender = new MailSender("orellanasmtp@gmail.com", receiver, message, "Conecte el cargador", null);

                if (sender.enviaMail())
                    Console.WriteLine("Se ha enviado 1 email a "+ receiver);
                else
                    Console.WriteLine("Error al enviar email");
            }
        }
    }
}

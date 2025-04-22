using System;
using System.Threading;

namespace TimeAlarmConsoleApp
{
    // 1. Define a delegate that matches the signature of the event handlers
    public delegate void AlarmEventHandler(object sender, EventArgs args);

    // 2. Publisher class that raises the alarm event
    class AlarmClock
    {
        // The event to which subscribers can attach
        public event AlarmEventHandler RaiseAlarm;

        private readonly TimeSpan targetTime;

        public AlarmClock(TimeSpan targetTime)
        {
            this.targetTime = targetTime;
        }

        // Method to start monitoring the time
        public void Start()
        {
            Console.WriteLine($"Alarm set for {targetTime:hh\\:mm\\:ss}. Monitoring system time...");
            while (true)
            {
                Console.WriteLine("Tick...");
                TimeSpan now = DateTime.Now.TimeOfDay;
                if (now.Hours == targetTime.Hours &&
                    now.Minutes == targetTime.Minutes &&
                    now.Seconds == targetTime.Seconds)
                {
                    OnRaiseAlarm();
                    break;
                }
                Thread.Sleep(1000);
            }
        }

        // Raises the RaiseAlarm event
        protected virtual void OnRaiseAlarm()
        {
            RaiseAlarm?.Invoke(this, EventArgs.Empty);
        }
    }

    class Program
    {
        // 3. Subscriber method that handles the alarm
        static void RingAlarm(object sender, EventArgs args)
        {
            Console.WriteLine("*****  ALARM!  *****");
            Console.WriteLine("The target time has been reached.");
        }

        static void Main(string[] args)
        {
            Console.Write("Enter target time (HH:MM:SS): ");
            string input = Console.ReadLine();

            if (!TimeSpan.TryParseExact(input, "c", null, out TimeSpan target))
            {
                Console.WriteLine("Invalid format. Please use HH:MM:SS.");
                return;
            }

            // 4. Create publisher and subscribe the RingAlarm handler
            AlarmClock clock = new AlarmClock(target);
            clock.RaiseAlarm += RingAlarm;

            // 5. Start monitoring
            clock.Start();

            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }
    }
}

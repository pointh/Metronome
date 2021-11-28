using System;
using System.Threading;

namespace Metronome {
    public class Tick : EventArgs {
        public DateTime d { get; set; }
    }

    public delegate void TickHandler(Tick t);
        
    class Metronome {
        public event TickHandler TickEvent;
        public void Start()
        {
            for (; ; )
            {
                Thread.Sleep(1000);
                if (TickEvent != null)
                {
                    Tick t = new Tick() { d = DateTime.Now };
                    TickEvent(t);
                }
            }
        }
    }

    class Listener {
        private ConsoleColor color;
        public Listener(ConsoleColor c) => color = c;
        public void Subscribe(Metronome m) {
            Console.ForegroundColor = color;
            m.TickEvent += WriteTime;
        }
        private void WriteTime(Tick t) {
            Console.ForegroundColor = color;
            Console.WriteLine(t.d);
        }
    }

    class Program {
        static void Main(string[] args) {
            Metronome m = new Metronome();

            Listener l = new Listener(ConsoleColor.Green);
            Listener p = new Listener(ConsoleColor.Red);
            l.Subscribe(m);
            p.Subscribe(m);

            m.Start();
        }
    }
}

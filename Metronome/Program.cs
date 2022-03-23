using System;
using System.Threading;

namespace Metronome {
    // obsah události = aktuální čas
    // ale může to být jakákoliv informace o tom, co se na objektu stalo
    public class Tick : EventArgs {
        public DateTime d { get; set; }
    }

    // typ metod, které se mohou připojit k události z ostatních objektů
    public delegate void TickHandler(Tick t);
        
    class Metronome {
        public event TickHandler TickEvent; // hlídá se, jestli obsahuje typově správné metody
        public void Start()
        {
            for (; ; )
            {
                Thread.Sleep(1000);
                if (TickEvent != null)
                {
                    Tick t = new Tick() { d = DateTime.Now }; // vytvoří objekt s informací o stavu
                    TickEvent?.Invoke(t); // ... a spustí se všechny připojené metody s argumentem t
                }
            }
        }
    }

    class Listener {
        private ConsoleColor color;
        public Listener(ConsoleColor c) => color = c;
        public void Subscribe(Metronome m) { 
            Console.ForegroundColor = color;
            m.TickEvent += WriteTime; // k události metronomu připojí VLASTNÍ metodu
        }
        private void WriteTime(Tick t) { // vlastní metoda zpracování události
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

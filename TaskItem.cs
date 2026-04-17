using System;
using System.Drawing;
using System.Threading;

public class TaskItem
{
    public Point Location { get; private set; }
    public int Number { get; private set; }
    public Brush Brush { get; private set; }

    private Thread thread;
    private volatile bool isRunning = true;
    private readonly object lockObj = new object();

    public TaskItem(Point location)
    {
        Location = location;
        Random rnd = new Random(Guid.NewGuid().GetHashCode());
        Brush = new SolidBrush(Color.FromArgb(
            rnd.Next(50, 256),
            rnd.Next(50, 256),
            rnd.Next(50, 256)
        ));
    }

    public void Start(Action onUpdate)
    {
        thread = new Thread(() =>
        {
            int i = 1;

            while (isRunning)
            {
                lock (lockObj)
                {
                    Number = i++;
                }

                onUpdate?.Invoke();

                Thread.Sleep(300); 
            }
        });

        thread.IsBackground = true; 
        thread.Start();
    }

    public void Stop()
    {
        isRunning = false; 
    }

    public int GetNumber()
    {
        lock (lockObj)
        {
            return Number;
        }
    }
}
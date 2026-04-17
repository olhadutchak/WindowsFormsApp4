using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

public class TaskManager
{
    private List<TaskItem> tasks = new List<TaskItem>();
    private readonly object lockObj = new object();

    public void AddTask(Point location, Action onUpdate)
    {
        var task = new TaskItem(location);

        lock (lockObj)
        {
            tasks.Add(task);
        }

        task.Start(onUpdate);
    }

    public void RemoveNearest(Point clickPoint)
    {
        TaskItem nearest = null;

        lock (lockObj)
        {
            if (tasks.Count == 0) return;

            nearest = tasks
                .OrderBy(t => Distance(t.Location, clickPoint))
                .First();

            tasks.Remove(nearest);
        }

        nearest.Stop();
    }

    public void StopAll()
    {
        List<TaskItem> copy;

        lock (lockObj)
        {
            copy = new List<TaskItem>(tasks);
            tasks.Clear();
        }

        foreach (var t in copy)
        {
            t.Stop();
        }
    }

    public List<TaskItem> GetTasks()
    {
        lock (lockObj)
        {
            return new List<TaskItem>(tasks);
        }
    }

    private double Distance(Point p1, Point p2)
    {
        return Math.Sqrt(Math.Pow(p1.X - p2.X, 2) +
                         Math.Pow(p1.Y - p2.Y, 2));
    }
}
using DomainLayer.Objects;
using System;
using System.Timers;

public class Session {
    public Guid Id { get; set; }
    public int WebshopId { get; set; }
    private static System.Timers.Timer _timer;

    public delegate void SessionExpiredEventHandler(object sender, EventArgs e);
    public event SessionExpiredEventHandler SessionExpired;

    public Session() {
        Id = Guid.NewGuid();
        _timer = new System.Timers.Timer(300000);
        _timer.Elapsed += OnTimerElapsed;
        _timer.AutoReset = false;
        _timer.Start();
    }

    private void OnTimerElapsed(object sender, ElapsedEventArgs e) {
        SessionExpired?.Invoke(this, EventArgs.Empty);

        _timer.Stop();
        _timer.Dispose();
    }
}

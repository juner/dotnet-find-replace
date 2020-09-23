using System;

namespace FindReplaceUtility
{
    partial class Program
    {
        internal class Disposable : IDisposable
        {
            readonly Action Action;
            private Disposable(Action Action) => this.Action = Action;
            public static IDisposable Create(Action Action) => new Disposable(Action);
            private bool disposedValue;

            protected virtual void Dispose(bool disposing)
            {
                if (!disposedValue)
                {
                    try
                    {
                        Action.Invoke();
                    }
                    catch { }
                    disposedValue = true;
                }
            }
            ~Disposable()
            {

                Dispose(disposing: false);
            }

            void IDisposable.Dispose()
            {
                Dispose(disposing: true);
                GC.SuppressFinalize(this);
            }
        }
    }
}

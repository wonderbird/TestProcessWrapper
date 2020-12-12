using System;
using System.Diagnostics;
using System.Text;
using System.Threading;

namespace katarabbitmq.bdd.tests.Steps
{
    // Regarding reading the StandardOutput, see https://stackoverflow.com/questions/7160187/standardoutput-readtoend-hangs
    public sealed class ProcessStreamBuffer : IDisposable
    {
        private readonly object _outputLock = new();
        private StringBuilder _output;
        private AutoResetEvent _outputWaitHandle;
        private bool _isDisposed;
        private Action<DataReceivedEventHandler> _unsubscribeFromEventAction;

        public string StreamContent
        {
            get
            {
                lock (_outputLock)
                {
                    return _output.ToString();
                }
            }
        }

        ~ProcessStreamBuffer() => Dispose(false);

        public void BeginCapturing(Action beginReadLine, Action<DataReceivedEventHandler> subscribeToEventAction, Action<DataReceivedEventHandler> unsubscribeFromEventAction)
        {
            _unsubscribeFromEventAction = unsubscribeFromEventAction;
            _outputWaitHandle = new AutoResetEvent(false);

            lock (_outputLock)
            {
                _output = new StringBuilder();
            }

            subscribeToEventAction(appendEventDataToOutputBuffer);
            beginReadLine();
        }

        private void appendEventDataToOutputBuffer(object sender, DataReceivedEventArgs eventArg)
        {
            if (eventArg.Data == null)
            {
                _outputWaitHandle.Set();
            }
            else
            {
                lock (_outputLock)
                {
                    _output.AppendLine(eventArg.Data);
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (_isDisposed)
            {
                return;
            }

            if (disposing)
            {
                _unsubscribeFromEventAction(appendEventDataToOutputBuffer);
                _outputWaitHandle?.Dispose();
            }

            _isDisposed = true;
        }
    }
}

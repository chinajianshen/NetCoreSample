using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace WebApp
{
    public class FilePrivider
    {
    }

    public interface IMessageWriter : IDisposable
    {
        Task WriteMessageAsync(string message, CancellationToken cancellationToken = default(CancellationToken));
    }

    public class FileWriter : IMessageWriter, IDisposable
    {
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public Task WriteMessageAsync(string message, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }
    }
}

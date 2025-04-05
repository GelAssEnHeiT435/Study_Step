using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Study_Step.Services
{
    public class ProgressableStreamContent : HttpContent
    {
        private readonly Stream _stream;
        private readonly int _bufferSize;
        private readonly CancellationToken _cancellationToken;

        public Action<long, long> Progress { get; set; }

        public ProgressableStreamContent(Stream stream, int bufferSize, CancellationToken token)
        {
            _stream = stream;
            _bufferSize = bufferSize;
            _cancellationToken = token;
        }

        protected override async Task SerializeToStreamAsync(Stream stream, TransportContext context)
        {
            var buffer = new byte[_bufferSize];
            long totalSent = 0;
            var totalLength = _stream.Length;

            while (true)
            {
                _cancellationToken.ThrowIfCancellationRequested();

                var bytesRead = await _stream.ReadAsync(buffer, 0, buffer.Length, _cancellationToken);
                if (bytesRead <= 0) break;

                await stream.WriteAsync(buffer, 0, bytesRead, _cancellationToken);
                totalSent += bytesRead;

                Progress?.Invoke(totalSent, totalLength);
            }
        }

        protected override bool TryComputeLength(out long length)
        {
            length = _stream.Length;
            return true;
        }

        protected override void Dispose(bool disposing)
        {
            _stream.Dispose();
            base.Dispose(disposing);
        }
    }
}

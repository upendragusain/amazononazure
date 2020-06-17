using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BooksScheduler
{
    public interface ICrawler<T>
    {
        Task<IEnumerable<T>> ProcessAsync(string uri);
    }
}

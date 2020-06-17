using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BooksScheduler
{
    public class TaskScheduler
    {
        private readonly BookContext _context;
        private readonly ILogger _logger;

        public TaskScheduler(
            BookContext context,
            ILogger log)
        {
            _context = context;
            _logger = log;
        }

        public async Task<int> ScheduleWithSemaphore(List<string> urls, int initialCount = 3, int maxCount = 3)
        {
            int counter = 0;

            initialCount = initialCount < 1 ? 3 : initialCount > 3 ? 3 : initialCount;
            maxCount = maxCount < 1 ? 3 : maxCount > 3 ? 3 : maxCount;

            using (var semaphore = new SemaphoreSlim(initialCount, maxCount))
            {
                List<Task> trackedTasks = new List<Task>();
                foreach (var url in urls)
                {
                    try
                    {
                        await semaphore.WaitAsync().ConfigureAwait(false);

                        trackedTasks.Add(Task.Run(async () =>
                        {
                            //get the page books
                            Crawler crawler = new Crawler();

                            _logger.LogInformation("Processing page {0}", url);
                            var pageBooks = await crawler.ProcessAsync(url);

                            if (pageBooks != null && pageBooks.Any())
                            {
                                await _context.InsertManyAsync(pageBooks);
                                _logger.LogInformation("Saved books to db");
                                counter++;
                            }

                            semaphore.Release();
                        }));
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError("{0}", ex);
                        semaphore.Release();
                        return counter;// stop on first error
                    }
                    //finally
                    //{
                    //    if(semaphore != null)
                    //        semaphore.Release();
                    //}
                }

                await Task.WhenAll(trackedTasks);
                return counter;
            }
        }

        public async Task<int> ScheduleSingleThread(List<string> urls)
        {
            int counter = 0;
            foreach (var url in urls)
            {
                try
                {
                    //get the page books
                    Crawler crawler = new Crawler();

                    //Console.WriteLine($"Processing {url}");
                    _logger.LogInformation("Processing page {0}", url);

                    var pageBooks = await crawler.ProcessAsync(url);

                    if (pageBooks != null && pageBooks.Any())
                    {
                        await _context.InsertManyAsync(pageBooks);
                        _logger.LogInformation("Saved books to db");
                        counter++;
                    }
                }
                catch (System.Exception ex)
                {
                    _logger.LogError("{0}", ex);
                    return counter;// stop on first error
                }
            }

            return counter;
        }
    }
}

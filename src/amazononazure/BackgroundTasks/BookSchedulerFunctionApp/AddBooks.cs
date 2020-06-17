using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using BooksScheduler;
using BooksScheduler.Model;
using System.Collections.Generic;
using System.Linq;

namespace BookSchedulerFunctionApp
{
    public static class AddBooks
    {
        [FunctionName("addbooks")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "addbooks/{mode:int}/{skip:int}/{take:int}")] HttpRequest req,
            int mode,
            int skip, 
            int take,
            ILogger log)
        {
            log.LogInformation($"C# HTTP trigger function processed a request with mode: {mode}, skip: {skip} and take: {take}");

            mode = mode < 0 ? 0 : mode > 1 ? 1 : mode;
            int urlsProcessed = 0;

            try
            {
                string database = Environment.GetEnvironmentVariable("database");
                string collection = Environment.GetEnvironmentVariable("collection");
                var connectionString = Environment.GetEnvironmentVariable("booksConnectionString");

                var _context = new BookContext(connectionString, database, collection);

                var scheduler = new BooksScheduler.TaskScheduler(_context, log);

                var urls = SetUpURLs();
                var urls_part = urls.Skip(skip).Take(take).ToList();
                
                switch (mode)
                {
                    case 0:
                        urlsProcessed = await scheduler.ScheduleSingleThread(urls_part);
                        break;

                    case 1:
                        urlsProcessed = await scheduler.ScheduleWithSemaphore(urls_part);
                        break;

                    default:
                        urlsProcessed = await scheduler.ScheduleSingleThread(urls_part);
                        break;
                }

                Console.WriteLine($"Processed {urlsProcessed}/{urls_part.Count} urls.");

            }
            catch (Exception exception)
            {
                log.LogError(exception.Message, exception);
                return new OkObjectResult($"An error occurred, {exception.Message}");
            }

            return new OkObjectResult($"Processed { urlsProcessed } / { take } urls.");
        }

        private static List<string> SetUpURLs(
            int searchPageTotalCount = 75)
        {
            var alphabet = Enumerable.Range('a', 26).ToList();
            var allUrls = new List<string>();
            for (int i = 0; i < 26; i++)
            {
                for (int j = 1; j <= searchPageTotalCount; j++)
                {
                    var link = $"https://www.amazon.co.uk/s?k="
                        + Convert.ToChar(alphabet[i]).ToString()
                        + "&i=stripbooks&page="
                        + j
                        + "&qid=1590338955&ref=sr_pg_"
                        + j;
                    allUrls.Add(link);
                }
            }

            return allUrls;
        }
    }
}

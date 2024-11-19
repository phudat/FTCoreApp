var totalRecordsToSync = listOrderReturnWillBeSync.Count;
  
var tasks = new List<Task>();
var semaphore = new SemaphoreSlim(maxParallelTasks); 

for (var i = 0; i < totalRecordsToSync; i += batchSize)
{
    var batchOrderReturnCodes = listOrderReturnWillBeSync.Skip(i).Take(batchSize).ToList();
    await semaphore.WaitAsync();
    tasks.Add(Task.Run(async () =>
    {
        try
        {
            await ProcessBatch(batchOrderReturnCodes);
        }
        finally
        {
            semaphore.Release();
        }
    }));
}
await Task.WhenAll(tasks);
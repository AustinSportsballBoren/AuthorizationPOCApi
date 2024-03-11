using System.Collections.Concurrent;
using OpenFGA;

public interface IOpenFGAService
{
    void WriteTest(WriteTestRequest openFGAWriteTestRequest);
}

public class OpenFGAService : IOpenFGAService
{

    IOpenFGA _openFga;
    ILogger<OpenFGAService> _logger;

    Random random = new();

    //The maximum number of relationships you can include per request for openfga  
    const int DB_WRITES_PER_REQUEST = 100;

    public OpenFGAService(IOpenFGA openFga, ILogger<OpenFGAService> logger)
    {
        _openFga = openFga;
        _logger = logger;
    }

    public void WriteTest(WriteTestRequest openFGAWriteTestRequest)
    {
        PopulateDBMultiThreadedOptimized(openFGAWriteTestRequest);
    }

    private void PopulateDBMultiThreadedOptimized(WriteTestRequest writeTestRequest, CabinetRelation cabRelation = CabinetRelation.Member)
    {
        for (int i = 0; i < writeTestRequest.NumberOfCabinets; i++)
        {
            string cabinet = GenerateCabinetId(i + 1);

            var envelopeBatches = GenerateBatches((int)writeTestRequest.EnvelopesPerCabinet!, DB_WRITES_PER_REQUEST);
            ConcurrentBag<string> envelopeIds = new ConcurrentBag<string>();

            Parallel.ForEach(Enumerable.Range(Math.Min(1, envelopeBatches.Count * DB_WRITES_PER_REQUEST), (int)writeTestRequest.EnvelopesPerCabinet!), idx =>
            {
                var envelopeId = $"envelope:envelope{idx}in{cabinet.Replace(":", string.Empty)}";
                envelopeIds.Add(envelopeId);
            });

            ConcurrentBag<string> userIds = new ConcurrentBag<string>(Enumerable.Range(1, (int)writeTestRequest.UsersPerCabinet!)
                .Select(userId => $"user:user{userId}in{cabinet.Replace(":", string.Empty)}"));

            AddRelationshipsToUsersInCabinets(cabinet, userIds, cabRelation);

            AddRelationshipsToCabinetsInEnvelopes(cabinet, envelopeIds);

            AddRelationshipsToUsersInEnvelopes(cabinet, userIds, envelopeIds, (int)writeTestRequest.RelationsPerEnvelope!);
        }
    }

    private void AddRelationshipsToUsersInCabinets(string cabinetName, ConcurrentBag<string> userIds, CabinetRelation cabinetPermission)
    {
        var userBatchTasks = GenerateBatches(userIds.Count, DB_WRITES_PER_REQUEST)
            .Select(async (userBatch, batchIndex) =>
            {
                var usersToBatchAdd = userBatch.Select((_, idx) =>
                {
                    var userId = (batchIndex * DB_WRITES_PER_REQUEST) + idx + 1;
                    return GenerateUserId(userId, cabinetName);
                }).ToList();

                List<string> cabinets = new List<string> { cabinetName };
                List<string> relations = new List<string> { cabinetPermission.ToString().ToLowerInvariant() };

                await _openFga.AddRelations(usersToBatchAdd, relations, cabinets);
            }).ToList();

        Task.WhenAll(userBatchTasks).Wait();
        Console.WriteLine($"{userBatchTasks.Count()} user batch tasks completed.");
    }

    private void AddRelationshipsToCabinetsInEnvelopes(string cabinetName, ConcurrentBag<string> envelopeIds)
    {
        var envelopeBatchTasks = GenerateBatches(envelopeIds.Count, DB_WRITES_PER_REQUEST)
            .Select(async (envelopeBatch, batchIndex) =>
            {
                var envelopesToBatchAdd = envelopeBatch.Select((_, idx) =>
                {
                    var envelopeId = (batchIndex * DB_WRITES_PER_REQUEST) + idx + 1;
                    return GenerateEnvelopeId(envelopeId, cabinetName);
                }).ToList();

                List<string> cabinets = new List<string> { cabinetName };
                List<string> relations = new List<string> { "cabinet" };

                await _openFga.AddRelations(cabinets, relations, envelopesToBatchAdd);
            }).ToList();

        Task.WhenAll(envelopeBatchTasks).Wait();
        Console.WriteLine($"{envelopeBatchTasks.Count()} envelope batch tasks completed.");
    }

    private void AddRelationshipsToUsersInEnvelopes(string cabinetName, ConcurrentBag<string> userIds, ConcurrentBag<string> envelopeIds, int relationsPerEnvelope, UserRelation userRelation = UserRelation.Viewer)
    {
        var userEnvelopeTasks = new List<Task>();
        var envelopeIdsToTake = DB_WRITES_PER_REQUEST / relationsPerEnvelope;

        while (true)
        {
            var firstFew = new List<string>();
            for (int m = 0; m < envelopeIdsToTake; m++)
            {
                if (envelopeIds.TryTake(out var envelopeId))
                {
                    firstFew.Add(envelopeId);
                }
                else
                {
                    break; // If there are fewer than 20 items left in the bag, break the loop
                }
            }

            if (firstFew.Count == 0)
            {
                break; // If there are no more items left in the bag, exit the loop
            }

            var users = Enumerable.Range(0, relationsPerEnvelope)
                .Select(_ => userIds.ElementAt(random.Next(userIds.Count)))
                .Distinct()
                .ToList();

            userEnvelopeTasks.Add(_openFga.AddRelations(users, new List<string> { userRelation.ToString().ToLowerInvariant() }, firstFew.Distinct().ToList()));
        }

        Console.WriteLine($"{userEnvelopeTasks.Count} user envelope tasks completed.");
    }

    private string GenerateCabinetId(int id) => $"cabinet:{id}";
    private string GenerateUserId(int id, string cabinet) => $"user:user{id}in{cabinet.Replace(":", string.Empty)}";
    private string GenerateEnvelopeId(int id, string cabinet) => $"envelope:envelope{id}in{cabinet.Replace(":", string.Empty)}";

    private List<List<int>> GenerateBatches(int totalItems, int batchSize)
    {
        var batches = new List<List<int>>();
        int remainingItems = totalItems;
        while (remainingItems > 0)
        {
            var batch = new List<int>();
            for (int i = 0; i < Math.Min(batchSize, remainingItems); i++)
            {
                batch.Add(remainingItems / batchSize);
            }
            batches.Add(batch);
            remainingItems -= batchSize;
        }
        return batches;
    }
}
using Microsoft.AspNetCore.Mvc;

public class SpiceDBWriteTestRequest
{
    public string? Test { get; set; }
}

public class SpiceDBEndpoints {

    public void WriteTest([FromBody] SpiceDBWriteTestRequest openFGAWriteTestRequest)
    {
        Console.WriteLine(openFGAWriteTestRequest.Test);
    }
}
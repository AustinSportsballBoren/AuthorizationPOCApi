using Microsoft.AspNetCore.Mvc;

public class OpenFGAWriteTestRequest
{
    public string? Test { get; set; }
}

public class OpenFGAEndpoints {

    public IResult WriteTest([FromBody] OpenFGAWriteTestRequest openFGAWriteTestRequest)
    {
        return Results.Ok(openFGAWriteTestRequest.Test);
    }
}
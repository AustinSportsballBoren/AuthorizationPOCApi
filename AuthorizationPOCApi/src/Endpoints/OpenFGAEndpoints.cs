using Microsoft.AspNetCore.Mvc;
using OpenFGA;

public class OpenFGAEndpoints {

    public IResult WriteTest([FromServices] IOpenFGAService openFGAService, [FromBody] WriteTestRequest openFGAWriteTestRequest)
    {
        var ids = openFGAService.WriteTest(openFGAWriteTestRequest);
        return Results.Ok(ids);
    }
}
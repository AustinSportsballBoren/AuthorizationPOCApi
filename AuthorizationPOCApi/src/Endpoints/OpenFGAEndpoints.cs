using Microsoft.AspNetCore.Mvc;

public class OpenFGAEndpoints {

    public IResult WriteTest([FromServices] IOpenFGAService openFGAService, [FromBody] WriteTestRequest openFGAWriteTestRequest)
    {
        openFGAService.WriteTest(openFGAWriteTestRequest);
        return Results.Ok();
    }
}
using Microsoft.AspNetCore.Mvc;

public class OpenFGAEndpoints {

    public IResult WriteTest([FromBody] WriteTestRequest openFGAWriteTestRequest)
    {
        return Results.Ok(openFGAWriteTestRequest.NumberOfCabinets);
    }
}
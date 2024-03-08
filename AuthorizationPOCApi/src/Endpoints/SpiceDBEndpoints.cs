using Microsoft.AspNetCore.Mvc;

public class SpiceDBEndpoints {

    public IResult WriteTest([FromBody] WriteTestRequest openFGAWriteTestRequest)
    {
        return Results.Ok(openFGAWriteTestRequest.NumberOfCabinets);
    }
}
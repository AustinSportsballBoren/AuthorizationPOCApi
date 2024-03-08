using OpenFGA;

public interface IOpenFGAService {
    IResult WriteTest(WriteTestRequest openFGAWriteTestRequest);
}

public class OpenFGAService : IOpenFGAService {

    IOpenFGA _openFga;
    
    public OpenFGAService(IOpenFGA openFga)
    {
        _openFga = openFga;
    }

    public IResult WriteTest(WriteTestRequest openFGAWriteTestRequest)
    {
        return Results.Ok(openFGAWriteTestRequest.NumberOfCabinets);
    }
}
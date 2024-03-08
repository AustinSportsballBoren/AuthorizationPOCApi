public static class OpenFGARouteGroups
{
    public static RouteGroupBuilder MapOpenFGAEndpoints(this RouteGroupBuilder group)
    {
        var openFGAEndpoints = new OpenFGAEndpoints();

        group.MapPost("test", openFGAEndpoints.WriteTest);

        return group;
    }

}
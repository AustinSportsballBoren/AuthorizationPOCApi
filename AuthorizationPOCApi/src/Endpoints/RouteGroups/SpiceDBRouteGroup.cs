public static class SpiceDBRouteGroups
{
    public static RouteGroupBuilder MapSpiceDBEndpoints(this RouteGroupBuilder group)
    {
        var spiceDBEndpoints = new SpiceDBEndpoints();

        group.MapPost("write-test", spiceDBEndpoints.WriteTest);

        return group;
    }

}
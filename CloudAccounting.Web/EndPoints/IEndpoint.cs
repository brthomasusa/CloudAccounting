namespace CloudAccounting.Web.EndPoints;

    public interface IEndpoint
    {
        void MapEndpoint(IEndpointRouteBuilder app);
    }

namespace LiteApi.Contracts
{
    public interface IControllerBuilder
    {
        object Build(ControllerContext controllerCtx);
    }
}

namespace LiteApi.Contracts
{
    public interface IControllerBuilder
    {
        LiteController Build(ControllerContext controllerCtx);
    }
}

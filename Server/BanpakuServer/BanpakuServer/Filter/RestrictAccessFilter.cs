using Grpc.Core;
using MagicOnion.Server;
using MagicOnion.Server.Filters;

public class RestrictAccessFilter : MagicOnionFilterAttribute
{
    private static readonly string[] AllowedIps = { "192.168.1.27", "192.168.1.24", "192.168.1.26","192.168.1.31" };

    public override async ValueTask Invoke(ServiceContext context, Func<ServiceContext, ValueTask> next)
    {
        var remoteIp = context.CallContext.Peer;

        // IPアドレスの許可リストに存在しない場合、アクセスを拒否
        if (!AllowedIps.Any(ip => remoteIp.Contains(ip)))
        {
            // RpcException をスローしてアクセス拒否を通知
            throw new RpcException(new Status(StatusCode.PermissionDenied, "Access denied"));
        }

        await next(context);
    }
}
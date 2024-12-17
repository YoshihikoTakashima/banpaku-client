using MagicOnion;
using MagicOnion.Server;
using MagicOnion.Server.Hubs;

public class ChatHub : StreamingHubBase<IChatHub, IChatHubReceiver>, IChatHub


{
    private IGroup _group; // グループの管理
    private string _userName;

    public async Task JoinAsync(string userName)
    {
        Console.WriteLine("JoinAsync");
        _userName = userName;
        _group = await Group.AddAsync("DefaultGroup"); // ユーザーをグループに追加
        Console.WriteLine($"Current group name: {_group.GroupName}");
        // グループのメンバー数をログに出力
        var memberIds = await _group.GetMemberCountAsync();
        Console.WriteLine($"Current group members: {memberIds}");

        Broadcast(_group).OnMessageReceived("System", $"{_userName} joined the chat.");
    }

    public async Task LeaveAsync()
    {
        Broadcast(_group).OnMessageReceived("System", $"{_userName} left the chat.");
        await _group.RemoveAsync(Context); // ユーザーをグループから削除
    }

    public async Task SendMessageAsync(string message)
    {
//        await Task.Delay(200);
        if(!message.Equals("free") && !message.Equals("busy") )
            Console.WriteLine($"Current group message: {message}");
        Broadcast(_group).OnMessageReceived(_userName, message); // グループ全体にメッセージをブロードキャスト
    }

    protected override ValueTask OnDisconnected()
    {
        if (_group != null)
        {
            // AsTask() を呼び出して同期的に待機
            _group.RemoveAsync(Context).AsTask().Wait();

            Broadcast(_group).OnMessageReceived("System", $"{_userName} disconnected.");
        }
        return base.OnDisconnected();
    }
}
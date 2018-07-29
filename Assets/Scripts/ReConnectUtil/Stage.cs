public enum Stage
{
    StartStage,  //开始界面，点击开始游戏进入等待连接界面
    Prepare, // 开始游戏后的等待连接界面
    ChoosingRoleStage,
    GammingStage,
    OfflineStage,    //客户端掉线界面
    GameOverStage,
    ChangeNameStage
}
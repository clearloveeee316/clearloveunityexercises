MyHandler handler = OnMonsterDead;   
handler("哥布林");

handler += OnQuestUpdate;   
handler("哥布林"); 

handler -= OnMonsterDead;
handler("哥布林");
void OnMonsterDead(string name)      
{
    Console.WriteLine("怪物 " + name + " 死了，掉落金币");
}
void OnQuestUpdate(string msg)
{
    Console.WriteLine("任务进度更新：" + msg);
}
delegate void MyHandler(string msg);

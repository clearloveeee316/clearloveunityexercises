List<string> monsters = new List<string>();
monsters.Add("特朗德尔");
monsters.Add("灾厄");
monsters.Add("大黄");
monsters.Add("江狗");
monsters.Add("董梁");
foreach (string m in monsters) { Console.WriteLine(m); }
Dictionary<string, int> hpMap = new Dictionary<string, int>();
hpMap["特朗德尔"] = 1000;
hpMap["灾厄"] = 1500;
hpMap["大黄"] = 2000;
hpMap["江狗"] = 2500;
hpMap["董梁"] = 300000;
int Hp = hpMap["董梁"];
Console.WriteLine("董梁的血量是：" + Hp);
int hp;
if (hpMap.TryGetValue("孙悟空", out hp))
{
    Console.WriteLine("找到了：" + hp);
}
else
{
    Console.WriteLine("查无此人，按 0 处理");
}
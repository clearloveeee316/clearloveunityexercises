Player warrior = new Player();
warrior.Name = "战士";
warrior.HP = 100;
Player wizard = new Player();
wizard.Name = "法师";
wizard.HP = 70;
warrior.TakeDamage(20);
wizard.TakeDamage(25);
Console.WriteLine($"{warrior.Name}的血量为{warrior.HP}");
Console.WriteLine($"{wizard.Name}的血量为{wizard.HP}");
Player clone = warrior;     // 注意：没有 new！
clone.HP = 1;
Console.WriteLine($"{warrior.Name}的血量为{warrior.HP}");
class Player
{
    public string Name { get; set; }   
    public int HP { get; set; }

    public void TakeDamage(int dmg)
    {
        HP -= dmg;
        if (HP < 0)
            HP = 0;
    }
}
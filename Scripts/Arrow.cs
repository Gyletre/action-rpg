using Godot;
using System;

public partial class Arrow : Area2D
{
	public Vector2 direction = Vector2.Zero;
	public int damage = 0;
	Node2D owner;
	int speed = 100;
	
    public override void _Ready()
    {
        GD.Print("hello");
		owner = GetParent<Node2D>();
		GetTree().CreateTimer(5).Timeout += () => {
			//Delete
			GD.Print("Arrow Timed out");
		};
		
    }
	public void SetDamage(int value){
		damage = value;
	}
    public override void _PhysicsProcess(double delta)
    {
		Rotation = direction.Angle();
        Position += direction*speed*(float)delta;
    }
	private void OnBodyEntered(Node2D body){
		if(!owner.IsInGroup("player") && body is Player player){
			player.takeDamage(damage);
		}
		else if(owner.IsInGroup("player") && body is IEnemy enemy){
			enemy.Hit(damage);
		}
	}
}

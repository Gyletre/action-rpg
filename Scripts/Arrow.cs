using Godot;
using System;

public partial class Arrow : Area2D
{
	public Vector2 direction = Vector2.Zero;
	int damage = 0;
	Node2D owner;
	int speed = 100;
	
    public override void _Ready()
    {
		GetTree().CreateTimer(5).Timeout += () => {
			QueueFree();
		};
		
    }
	public void Setup(int value, Node2D ownr){
		damage = value;
		owner = ownr;
	}
    public override void _PhysicsProcess(double delta)
    {
		Rotation = direction.Angle();
        Position += direction*speed*(float)delta;
    }
	private void OnBodyEntered(Node2D body){
		if(!owner.IsInGroup("player") && body is Player player){
			player.takeDamage(damage);
			QueueFree();
		}
		else if(owner.IsInGroup("player") && body is IEnemy enemy){
			enemy.Hit(damage);
			QueueFree();
		}
	}
}

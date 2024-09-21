using Godot;
using System;

public partial class Cyclops : CharacterBody2D, IEnemy
{
	static CharacterBody2D player;
	int speed = 800;

	[Export]
	private int health = 5;
	[Export]
	private int damage = 1;
    public override void _EnterTree()
    {
		if (player == null){
			player = GetNode<CharacterBody2D>("../Player");
		}
    }
    public void Hit(int damageTaken)
	{
		GD.Print(damageTaken);
		health -= damageTaken;
		if(health <= 0){
			GetNode<AnimationPlayer>("AnimationPlayer").Play("death");
			GetTree().CreateTimer(0.5).Timeout += () =>{
				if(GetParent() is Spawner spawner){
					spawner.hasEnemy = false;
				}
				QueueFree();
			};
		}
	}
	public override void _PhysicsProcess(double delta)
	{
		if(IsInstanceValid(player)){
			Vector2 direction = (player.GlobalPosition-GlobalPosition).Normalized();
			if(Position.DistanceTo(player.Position) > 10 || Position.DistanceTo(player.Position)< 500)
			{
				Velocity = speed*(float)delta*direction;
			}
			else
			{
				Velocity = Vector2.Zero;
			}
			GetNode<Sprite2D>("Sprite").FlipH = direction.X < 0;
		}
		MoveAndSlide();
	}
}

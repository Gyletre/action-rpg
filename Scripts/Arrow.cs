using Godot;
using System;

public partial class Arrow : Area2D
{
	public Vector2 direction = Vector2.Zero;
	int speed = 100;
	string ownerName = "";
    public override void _Ready()
    {
        GD.Print("hello");
    }
    public override void _PhysicsProcess(double delta)
    {
        Position += direction*speed*(float)delta;
    }
	private void OnBodyEntered(Node2D body){
		if(body.IsInGroup("player")){

		}
		else if(ownerName == "Player" && body.IsInGroup("monster")){
			
		}
	}
}

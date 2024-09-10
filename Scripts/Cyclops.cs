using Godot;
using System;

public partial class Cyclops : CharacterBody2D, IEnemy
{
	[Export]
	private int health = 5;
	[Export]
	private int damage = 1;
	public void Hit(int damageTaken)
	{
		GD.Print(damageTaken);
		health -= damageTaken;
	}
	public override void _PhysicsProcess(double delta)
	{
		MoveAndSlide();
	}
}

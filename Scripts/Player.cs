using System;
using Godot;
using System.Collections;
using System.Collections.Generic;

public partial class Player : CharacterBody2D
{
	[Export]
	public int speed = 80;
	PackedScene arrowScene;
	bool dying = false;
	bool attackingMelee = false;
	bool attackingRanged = false;
	int health = 5;
	int damage = 2;
	Node2D weaponMarker;
	public override void _Ready()
	{
		arrowScene = (PackedScene)ResourceLoader.Load("res://Scenes/Projectiles/arrow.tscn");
		weaponMarker = GetNode<Node2D>("WeaponHolder");
	}
	public override void _PhysicsProcess(double delta)
	{
		if(dying) return;
		RotateWeapon();
		AttackCheck(delta);
		Vector2 inputVector = Vector2.Zero;
		inputVector.X = Input.GetActionStrength("right") - Input.GetActionStrength("left");
		inputVector.Y = Input.GetActionStrength("down") - Input.GetActionStrength("up");
		Velocity = inputVector * speed;
		MoveAndSlide();
		
	}
	public void OnMeleeBodyEntered(Node2D body)
	{
		if(attackingMelee && body.IsInGroup("monster"))
		{
			if(body is IEnemy enemy){
				enemy.Hit(damage);
			}
		}
	}
	public void takeDamage(int amount){
		health -= amount;
	}
	private void RotateWeapon()
	{
		Vector2 cursorPos = GetLocalMousePosition();
		weaponMarker.Rotation = cursorPos.Angle();
	}
	private void AttackCheck(double delta)
	{
		if(Input.IsActionJustPressed("attack") && !attackingMelee){
			Vector2 targetPosition = (GetGlobalMousePosition()-weaponMarker.GlobalPosition).Normalized();
			MeleeAttack(targetPosition);
			Arrow arrowTemp = arrowScene.Instantiate<Arrow>();
			arrowTemp.direction = targetPosition;
			arrowTemp.SetDamage(damage);
			AddChild(arrowTemp);
		}
	}
	private void MeleeAttack(Vector2 targetPos)
	{
		attackingMelee = true;
		Tween tween = CreateTween();
		tween.TweenProperty(weaponMarker, "position", targetPos*10, .3);
		GetTree().CreateTimer(0.3).Timeout += () => {
			ReturnDefault();
		};
		
	}
	private void ReturnDefault()
	{
		Tween tween = CreateTween();
		tween.TweenProperty(weaponMarker, "position", Vector2.Zero, .3);
		GetTree().CreateTimer(0.5).Timeout += () => {
			attackingMelee = false;
		};
	}
}

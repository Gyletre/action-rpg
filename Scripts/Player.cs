using Godot;

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
	Node2D meleeWeapon;
	Node2D rangedWeapon;
	CurAtk currentlyAttacking = CurAtk.Melee;
	enum CurAtk{
		Melee,
		Ranged,
		None
	}
	
	public override void _Ready()
	{
		arrowScene = (PackedScene)ResourceLoader.Load("res://Scenes/Projectiles/arrow.tscn");
		weaponMarker = GetNode<Node2D>("WeaponHolder");
		meleeWeapon = GetNode<Node2D>("WeaponHolder/Melee");
		rangedWeapon = GetNode<Node2D>("WeaponHolder/Ranged");
		rangedWeapon.Hide();
		

	}
	public override void _PhysicsProcess(double delta)
	{
		if(dying) return;
		RotateWeapon();
		CycleWeapon();
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
		GetNode<Sprite2D>("Sprite").FlipH = cursorPos.X < 0;
		weaponMarker.Rotation = cursorPos.Angle();
	}
	private void CycleWeapon(){
		if(Input.IsActionJustPressed("cycleWeapon")){
			if(currentlyAttacking == CurAtk.Melee){
				rangedWeapon.Show();
				currentlyAttacking = CurAtk.Ranged;
				meleeWeapon.Hide();
			}
			else if(currentlyAttacking == CurAtk.Ranged){
				meleeWeapon.Show();
				currentlyAttacking = CurAtk.Melee;
				rangedWeapon.Hide();
			}
		}
	}
	private void AttackCheck(double delta)
	{
		if(Input.IsActionJustPressed("attack"))
        {

            Vector2 targetPosition = (GetGlobalMousePosition() - weaponMarker.GlobalPosition).Normalized();
			if(currentlyAttacking == CurAtk.Melee && !attackingMelee){
				MeleeAttack(targetPosition);
			}
			else if (currentlyAttacking == CurAtk.Ranged &&!attackingRanged){
				RangedAttack(targetPosition);
			}
            
        }
    }
    private void MeleeAttack(Vector2 targetPos) //the process of 
	{
		attackingMelee = true;
		Tween tween = CreateTween();
		tween.TweenProperty(weaponMarker, "position", targetPos*30, .3);
		GetTree().CreateTimer(.3).Timeout += () => {
			Tween tween = CreateTween();
			tween.TweenProperty(weaponMarker, "position", Vector2.Zero, .3);
			GetTree().CreateTimer(0.5).Timeout += () => {
				attackingMelee = false;
			};
		};
		
	}
	private void RangedAttack(Vector2 targetPos)
    {
		attackingRanged = true;
        Arrow arrowTemp = arrowScene.Instantiate<Arrow>();
        arrowTemp.direction = targetPos;
        arrowTemp.Setup(damage,GetParent().GetNode<Node2D>("Player"));
		arrowTemp.GlobalPosition = GlobalPosition;
		GetParent().AddChild(arrowTemp);
		GetTree().CreateTimer(0.4).Timeout += () => {
			attackingRanged = false;
		};
        
    }
}

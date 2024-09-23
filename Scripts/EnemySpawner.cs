using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class EnemySpawner : Node2D
{
	[Export]
	public int enemyLimit = 3;
	int currentEnemies;
	[Export]
	Node2D[] spawners;
	PackedScene enemyScene;
	RandomNumberGenerator random = new RandomNumberGenerator();
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		enemyScene = (PackedScene)ResourceLoader.Load("res://Scenes/Monsters/cyclops.tscn");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (currentEnemies < enemyLimit)
		{
			int i = random.RandiRange(0, spawners.Length - 1);
			if (spawners[i] is Spawner spawner && !spawner.hasEnemy)
			{

				Node2D enemy = enemyScene.Instantiate<Node2D>();
				spawners[i].AddChild(enemy);
				spawner.hasEnemy = true;

				GD.Print(currentEnemies);

				/*Arrow arrowTemp = arrowScene.Instantiate<Arrow>();
				GetParent().AddChild(arrowTemp);*/
			}
		}
		currentEnemies = 0;
		foreach (Node2D node in spawners)
		{
			if (node is Spawner spawner)
			{
				if (spawner.hasEnemy)
				{
					currentEnemies++;
				}
			}
		}

	}
}

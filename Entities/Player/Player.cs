using Godot;
using System;

public class Player : KinematicBody2D
{
	// ground movment
	[Export] float GroundAcceleration = 2000f;
	[Export] float MaxWalkSpeed = 1500.0f;
	[Export] float Friction = 0.1f;

	// air movment
	[Export] float Gravity = 2000.0f;
	[Export] float JumpForce = 900.0f;
	[Export] float DecreasedGravity = 0.8f;
	[Export] float AirSpeed = 0.3f;

	[Export] float JumpBtnTime = 0.16f;
	float JumpBtnTimer = 2f;

	[Export] float GroundedTime = 0.2f;
	float GroundedTimer = 2f;

	bool IsJumping = false;

	// Stats
	int CokeScore = 0;
	[Export] float WeightImpact = 1f; // how much your weight should impact your movment
	[Export] float DrinkgingImpact = 1f; 
	float DrinkingSpeed = 1f;
	float Weight = 0f;

	// Drinking variables
	bool DrinkingCoke = false;
	float DrinkingTimer = 0;
	Node2D CokeNode;

	// children
	[Export] NodePath AnimationSpritePath;
	AnimatedSprite AnimationPlayer;

	Vector2 Velocity;

	public override void _Ready()
	{
		AnimationPlayer = GetNode<AnimatedSprite>(AnimationSpritePath);
		AnimationPlayer.Play("Idle");
	}

	public override void _Process(float delta)
	{
		InputAndControlls(delta);
		Drinking(delta);
	}

	public override void _PhysicsProcess(float delta)
	{

		if (IsJumping)
		{
			Velocity.y += Gravity * DecreasedGravity * delta;
		}
		else
		{
			Velocity.y += Gravity * delta;
		}

		Velocity = MoveAndSlide(Velocity, Vector2.Up);
		
	}

	void InputAndControlls(float delta)
	{
		JumpBtnTimer += delta;
		GroundedTimer += delta;

		// inputs
		if (Input.IsActionPressed("LEFT"))
		{
			if (Velocity.x > 0)
			{
				Velocity.x = Mathf.Lerp(Velocity.x, 0f, Friction);
			}
			AnimationPlayer.FlipH = false;
			AnimationPlayer.Play("Run");
			if (IsOnFloor())
			{
				Velocity.x -= (GroundAcceleration - Weight) * delta;
			}
			else
			{
				Velocity.x -= (GroundAcceleration * AirSpeed - Weight) * delta;
			}
			Velocity.x = Mathf.Max(Velocity.x, -MaxWalkSpeed);
		}
		else if (Input.IsActionPressed("RIGHT"))
		{
			if (Velocity.x < 0)
			{
				Velocity.x = Mathf.Lerp(Velocity.x, 0f, Friction);
			}
			AnimationPlayer.FlipH = true;
			AnimationPlayer.Play("Run");
			if (IsOnFloor())
			{
				Velocity.x += (GroundAcceleration - Weight) * delta;
			}
			else
			{
				Velocity.x += (GroundAcceleration * AirSpeed - Weight) * delta;
			}
			Velocity.x = Mathf.Min(Velocity.x, MaxWalkSpeed);

		}
		else
		{
			AnimationPlayer.Play("Idle");
			if (IsOnFloor())
			{
				Velocity.x = Mathf.Lerp(Velocity.x, 0f, Friction);
			}
			else
			{
				Velocity.x = Mathf.Lerp(Velocity.x, 0f, Friction * AirSpeed);
			}
		}

		if (IsOnFloor())
		{
			Velocity.y = 0.1f;
			GroundedTimer = 0f;
			IsJumping = false;

		}
		else
		{	
			AnimationPlayer.Play("Fall");
		}
		
		if (Input.IsActionPressed("JUMP"))
		{
			IsJumping = true;
			JumpBtnTimer = 0f;
		}
		else 
		{
			IsJumping = false;
		}

		if (JumpBtnTimer < JumpBtnTime && GroundedTimer < GroundedTime)
		{
			JumpBtnTimer = JumpBtnTime;
			GroundedTimer = GroundedTime;
			Velocity.y = -JumpForce;
		}
	}

	void Drinking(float delta)
	{
		if (!DrinkingCoke)
		{
			return;
		}

		DrinkingTimer += DrinkingSpeed * delta;
		if (DrinkingTimer >= (float)CokeNode.Get("CokeAmount"))
		{
			if ((bool)CokeNode.Get("DietCoke"))
			{
				Weight -= (float)CokeNode.Get("CokeAmount") * WeightImpact * 2;
			}
			else
			{
				Weight += (float)CokeNode.Get("CokeAmount") * WeightImpact;
			}
			DrinkingSpeed += (float)CokeNode.Get("CokeAmount") * DrinkgingImpact;
			DrinkingCoke = false;
			CokeScore += (int)CokeNode.Get("CokePoints");
			CokeNode.QueueFree();
		}
	}

	void AreaEntered(Area2D Area)
	{
		if (!DrinkingCoke && Area.GetParent<Node2D>().IsInGroup("Coke"))
		{
			DrinkingCoke = true;
			DrinkingTimer = 0f;
			CokeNode = Area.GetParent<Node2D>();;
		}
	}

	void AreaExited(Area2D Area)
	{
		try
		{
			if (Area.GetParent<Node2D>().GetInstanceId() == CokeNode.GetInstanceId() && Area.GetParent<Node2D>().IsInGroup("Coke"))
			{
				DrinkingCoke = false;
			}
		}
		catch
		{
			return;
		}
	}
}
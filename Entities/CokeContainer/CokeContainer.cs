using Godot;
using System;

public class CokeContainer : Node2D
{
    [Export] int CokePoints = 1;
    [Export] float CokeAmount = 1f; // Time to drink coke
    [Export] bool DietCoke = false;
}
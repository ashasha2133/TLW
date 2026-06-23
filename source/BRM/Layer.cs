using UnityEngine;

namespace BRM;

public static class Layer
{
	public const int DefaultN = 0;

	public const int TransparentFXN = 1;

	public const int Ignore_RaycastN = 2;

	public const int WaterN = 4;

	public const int UIN = 5;

	public const int PlayersN = 8;

	public const int GroundN = 9;

	public const int EnemyN = 10;

	public const int AABBN = 11;

	public const int FXN = 12;

	public const int NetworkObjectN = 13;

	public const int InnerContactN = 14;

	public const int noPhysicsN = 15;

	public const int AttackN = 16;

	public const int PlayerHitBoxN = 17;

	public const int EnemyAttackBoxN = 18;

	public const int EnemyHitBoxN = 19;

	public static readonly LayerMask Default;

	public static readonly LayerMask TransparentFX;

	public static readonly LayerMask Ignore_Raycast;

	public static readonly LayerMask Water;

	public static readonly LayerMask UI;

	public static readonly LayerMask FX;

	public static readonly LayerMask InnerContact;

	public static readonly LayerMask noPhysics;

	public static readonly LayerMask PlayerHitBox;

	public static readonly LayerMask EnemyAttackBox;

	public static readonly LayerMask Ground;

	public static readonly LayerMask GroundHit;

	public static readonly LayerMask GroundAABB;

	public static readonly LayerMask GroundEnemy;

	public static readonly LayerMask GroundPlayers;

	public static readonly LayerMask GroundNetworkObject;

	public static readonly LayerMask GroundAttack;

	public static readonly LayerMask GroundEnemyHit;

	public static readonly LayerMask GroundAABBHit;

	public static readonly LayerMask GroundPlayersHit;

	public static readonly LayerMask GroundEnemyAABB;

	public static readonly LayerMask GroundEnemyPlayers;

	public static readonly LayerMask GroundNetworkObjectHit;

	public static readonly LayerMask GroundEnemyAttack;

	public static readonly LayerMask GroundEnemyAABBHit;

	public static readonly LayerMask GroundAttackHit;

	public static readonly LayerMask GroundEnemyPlayersHit;

	public static readonly LayerMask GroundEnemyAttackHit;

	public static readonly LayerMask GroundEnemyNetworkObject;

	public static readonly LayerMask GroundEnemyNetworkObjectHit;

	public static readonly LayerMask GroundEnemyPlayersNetworkObject;

	public static readonly LayerMask GroundPlayersAttack;

	public static readonly LayerMask GroundPlayersAABB;

	public static readonly LayerMask GroundPlayersNetworkObject;

	public static readonly LayerMask GroundPlayersNetworkObjectHit;

	public static readonly LayerMask GroundPlayersAttackHit;

	public static readonly LayerMask GroundEnemyPlayersNetworkObjectHit;

	public static readonly LayerMask GroundPlayersAABBHit;

	public static readonly LayerMask GroundEnemyPlayersAttackHit;

	public static readonly LayerMask GroundEnemyPlayersAABBHit;

	public static readonly LayerMask GroundPlayersNetworkObjectAttack;

	public static readonly LayerMask GroundPlayersNetworkObjectAABB;

	public static readonly LayerMask GroundPlayersAttackAABB;

	public static readonly LayerMask GroundPlayersNetworkObjectAttackHit;

	public static readonly LayerMask GroundPlayersNetworkObjectAABBHit;

	public static readonly LayerMask GroundPlayersAttackAABBHit;

	public static readonly LayerMask GroundEnemyPlayersNetworkObjectAttack;

	public static readonly LayerMask GroundEnemyPlayersNetworkObjectAABB;

	public static readonly LayerMask GroundEnemyPlayersNetworkObjectAttackHit;

	public static readonly LayerMask GroundEnemyPlayersNetworkObjectAABBHit;

	public static readonly LayerMask GroundEnemyPlayersAttackAABB;

	public static readonly LayerMask GroundEnemyPlayersAttackAABBHit;

	public static readonly LayerMask GroundEnemyPlayersNetworkObjectAttackAABB;

	public static readonly LayerMask GroundEnemyPlayersNetworkObjectAttackAABBHit;

	public static readonly LayerMask Enemy;

	public static readonly LayerMask EnemyHit;

	public static readonly LayerMask EnemyAABB;

	public static readonly LayerMask EnemyPlayers;

	public static readonly LayerMask EnemyNetworkObject;

	public static readonly LayerMask EnemyAttack;

	public static readonly LayerMask EnemyAttackHit;

	public static readonly LayerMask EnemyAttackAABB;

	public static readonly LayerMask EnemyPlayersAABB;

	public static readonly LayerMask EnemyPlayersHit;

	public static readonly LayerMask EnemyPlayersNetworkObject;

	public static readonly LayerMask EnemyAABBHit;

	public static readonly LayerMask EnemyPlayersAttack;

	public static readonly LayerMask EnemyNetworkObjectAttack;

	public static readonly LayerMask EnemyPlayersNetworkObjectAttack;

	public static readonly LayerMask EnemyPlayersAABBHit;

	public static readonly LayerMask EnemyNetworkObjectHit;

	public static readonly LayerMask EnemyPlayersAttackHit;

	public static readonly LayerMask EnemyPlayersNetworkObjectAABB;

	public static readonly LayerMask EnemyPlayersNetworkObjectAttackHit;

	public static readonly LayerMask EnemyPlayersNetworkObjectAABBHit;

	public static readonly LayerMask EnemyPlayersNetworkObjectAttackAABB;

	public static readonly LayerMask EnemyPlayersNetworkObjectAttackAABBHit;

	public static readonly LayerMask Players;

	public static readonly LayerMask PlayersHit;

	public static readonly LayerMask PlayersAABB;

	public static readonly LayerMask PlayersNetworkObject;

	public static readonly LayerMask PlayersAttack;

	public static readonly LayerMask PlayersAttackHit;

	public static readonly LayerMask PlayersNetworkObjectAttack;

	public static readonly LayerMask PlayersAttackAABB;

	public static readonly LayerMask PlayersAttackAABBHit;

	public static readonly LayerMask PlayersAABBHit;

	public static readonly LayerMask PlayersNetworkObjectHit;

	public static readonly LayerMask PlayersNetworkObjectAABB;

	public static readonly LayerMask PlayersNetworkObjectAttackAABB;

	public static readonly LayerMask PlayersNetworkObjectAABBHit;

	public static readonly LayerMask PlayersNetworkObjectAttackHit;

	public static readonly LayerMask PlayersNetworkObjectAttackAABBHit;

	public static readonly LayerMask NetworkObject;

	public static readonly LayerMask NetworkObjectHit;

	public static readonly LayerMask NetworkObjectAABB;

	public static readonly LayerMask NetworkObjectAttack;

	public static readonly LayerMask NetworkObjectAttackAABB;

	public static readonly LayerMask NetworkObjectAABBHit;

	public static readonly LayerMask NetworkObjectAttackHit;

	public static readonly LayerMask NetworkObjectAttackAABBHit;

	public static readonly LayerMask Attack;

	public static readonly LayerMask AttackAABB;

	public static readonly LayerMask AttackHit;

	public static readonly LayerMask AttackAABBHit;

	public static readonly LayerMask AABB;

	public static readonly LayerMask AABBHit;

	public static readonly LayerMask EnemyHitBox;

	static Layer()
	{
		Default = 1;
		TransparentFX = 2;
		Ignore_Raycast = 4;
		Water = 16;
		UI = 32;
		FX = 4096;
		InnerContact = 16384;
		noPhysics = 32768;
		PlayerHitBox = 131072;
		EnemyAttackBox = 262144;
		Ground = 512;
		Enemy = 1024;
		Players = 256;
		NetworkObject = 8192;
		Attack = 65536;
		AABB = 2048;
		EnemyHitBox = 524288;
		GroundHit = (int)Ground | (int)EnemyHitBox;
		GroundAABBHit = (int)Ground | (int)AABB | (int)EnemyHitBox;
		GroundEnemyHit = (int)Ground | (int)Enemy | (int)EnemyHitBox;
		GroundPlayersHit = (int)Ground | (int)Players | (int)EnemyHitBox;
		GroundNetworkObjectHit = (int)Ground | (int)NetworkObject | (int)EnemyHitBox;
		GroundAttackHit = (int)Ground | (int)Attack | (int)EnemyHitBox;
		GroundEnemyPlayersHit = (int)Ground | (int)Enemy | (int)Players | (int)EnemyHitBox;
		GroundEnemyAABBHit = (int)Ground | (int)Enemy | (int)AABB | (int)EnemyHitBox;
		GroundEnemyAttackHit = (int)Ground | (int)Enemy | (int)Attack | (int)EnemyHitBox;
		GroundEnemyNetworkObjectHit = (int)Ground | (int)Enemy | (int)NetworkObject | (int)EnemyHitBox;
		GroundPlayersNetworkObject = (int)Ground | (int)Players | (int)NetworkObject;
		GroundPlayersNetworkObjectHit = (int)Ground | (int)Players | (int)NetworkObject | (int)EnemyHitBox;
		GroundEnemyPlayersNetworkObjectHit = (int)Ground | (int)Enemy | (int)Players | (int)NetworkObject | (int)EnemyHitBox;
		GroundEnemyPlayersAttackHit = (int)Ground | (int)Enemy | (int)Players | (int)Attack | (int)EnemyHitBox;
		GroundEnemyPlayersAABBHit = (int)Ground | (int)Enemy | (int)Players | (int)AABB | (int)EnemyHitBox;
		GroundPlayersAttackHit = (int)Ground | (int)Players | (int)Attack | (int)EnemyHitBox;
		GroundPlayersAABBHit = (int)Ground | (int)Players | (int)AABB | (int)EnemyHitBox;
		GroundPlayersNetworkObjectAttackHit = (int)Ground | (int)NetworkObject | (int)Players | (int)Attack | (int)EnemyHitBox;
		GroundPlayersNetworkObjectAABBHit = (int)Ground | (int)Players | (int)Attack | (int)AABB | (int)EnemyHitBox;
		GroundEnemyPlayersAttackAABB = (int)Ground | (int)Enemy | (int)Players | (int)Attack | (int)AABB;
		GroundPlayersAttackAABBHit = (int)Ground | (int)Players | (int)Attack | (int)AABB | (int)EnemyHitBox;
		GroundEnemyPlayersAttackAABBHit = (int)Ground | (int)Enemy | (int)Players | (int)Attack | (int)AABB | (int)EnemyHitBox;
		GroundEnemyPlayersNetworkObjectAttackHit = (int)Ground | (int)Enemy | (int)Players | (int)NetworkObject | (int)Attack | (int)EnemyHitBox;
		GroundEnemyPlayersNetworkObjectAABBHit = (int)Ground | (int)Enemy | (int)Players | (int)NetworkObject | (int)AABB | (int)EnemyHitBox;
		GroundEnemyPlayersNetworkObjectAttackAABBHit = (int)Ground | (int)Enemy | (int)Players | (int)NetworkObject | (int)Attack | (int)AABB | (int)EnemyHitBox;
		EnemyHit = (int)Enemy | (int)EnemyHitBox;
		EnemyAttackHit = (int)Enemy | (int)Attack | (int)EnemyHitBox;
		EnemyPlayersHit = (int)Enemy | (int)Players | (int)EnemyHitBox;
		EnemyAABBHit = (int)Enemy | (int)AABB | (int)EnemyHitBox;
		EnemyNetworkObjectHit = (int)Enemy | (int)NetworkObject | (int)EnemyHitBox;
		EnemyPlayersAABBHit = (int)Enemy | (int)Players | (int)AABB | (int)EnemyHitBox;
		EnemyPlayersAttackHit = (int)Enemy | (int)Players | (int)Attack | (int)EnemyHitBox;
		EnemyPlayersNetworkObjectAttackHit = (int)Enemy | (int)Players | (int)NetworkObject | (int)Attack | (int)EnemyHitBox;
		EnemyPlayersNetworkObjectAABBHit = (int)Enemy | (int)Players | (int)NetworkObject | (int)AABB | (int)EnemyHitBox;
		EnemyPlayersNetworkObjectAttackAABBHit = (int)Enemy | (int)Players | (int)NetworkObject | (int)Attack | (int)AABB | (int)EnemyHitBox;
		PlayersHit = (int)Players | (int)EnemyHitBox;
		PlayersAttackHit = (int)Players | (int)Attack | (int)EnemyHitBox;
		PlayersAABBHit = (int)Players | (int)AABB | (int)EnemyHitBox;
		PlayersAttackAABBHit = (int)Players | (int)Attack | (int)AABB | (int)EnemyHitBox;
		PlayersNetworkObjectHit = (int)Players | (int)NetworkObject | (int)EnemyHitBox;
		PlayersNetworkObjectAABBHit = (int)Players | (int)NetworkObject | (int)AABB | (int)EnemyHitBox;
		PlayersNetworkObjectAttackHit = (int)Players | (int)NetworkObject | (int)Attack | (int)EnemyHitBox;
		PlayersNetworkObjectAttackAABBHit = (int)Players | (int)NetworkObject | (int)Attack | (int)AABB | (int)EnemyHitBox;
		NetworkObjectHit = (int)NetworkObject | (int)EnemyHitBox;
		NetworkObjectAABBHit = (int)NetworkObject | (int)AABB | (int)EnemyHitBox;
		NetworkObjectAttackHit = (int)NetworkObject | (int)Attack | (int)EnemyHitBox;
		NetworkObjectAttackAABBHit = (int)NetworkObject | (int)Attack | (int)AABB | (int)EnemyHitBox;
		AttackHit = (int)Attack | (int)EnemyHitBox;
		AABBHit = (int)AABB | (int)EnemyHitBox;
		AttackAABBHit = (int)Attack | (int)AABB | (int)EnemyHitBox;
		GroundEnemy = (int)Ground | (int)Enemy;
		GroundPlayers = (int)Ground | (int)Players;
		GroundAttack = (int)Ground | (int)Attack;
		GroundAABB = (int)Ground | (int)AABB;
		GroundPlayersNetworkObjectAABB = (int)Ground | (int)Players | (int)NetworkObject | (int)AABB;
		GroundEnemyPlayersNetworkObjectAABB = (int)Ground | (int)Enemy | (int)Players | (int)NetworkObject | (int)AABB;
		GroundPlayersAttackAABB = (int)Ground | (int)Players | (int)Attack | (int)AABB;
		GroundPlayersAABB = (int)Ground | (int)Players | (int)AABB;
		GroundEnemyAABB = (int)Ground | (int)Enemy | (int)AABB;
		GroundPlayersAttack = (int)Ground | (int)Players | (int)Attack;
		GroundEnemyAttack = (int)Ground | (int)Enemy | (int)Attack;
		GroundNetworkObject = (int)Ground | (int)NetworkObject;
		GroundPlayersNetworkObjectAttack = (int)Ground | (int)Players | (int)NetworkObject;
		GroundEnemyPlayersNetworkObjectAttackAABB = (int)Ground | (int)Enemy | (int)Players | (int)NetworkObject | (int)Attack | (int)AABB;
		GroundEnemyNetworkObject = (int)Ground | (int)Enemy | (int)NetworkObject;
		GroundEnemyPlayers = (int)Ground | (int)Enemy | (int)Players;
		GroundEnemyPlayersNetworkObject = (int)Ground | (int)Enemy | (int)Players | (int)NetworkObject;
		GroundEnemyPlayersNetworkObjectAttack = (int)Ground | (int)Enemy | (int)Players | (int)NetworkObject | (int)Attack;
		EnemyNetworkObject = (int)Enemy | (int)NetworkObject;
		EnemyPlayers = (int)Enemy | (int)Players;
		EnemyAttack = (int)Enemy | (int)Attack;
		EnemyAABB = (int)Enemy | (int)AABB;
		EnemyAttackAABB = (int)Enemy | (int)Attack | (int)AABB;
		EnemyPlayersAttack = (int)Enemy | (int)Players | (int)Attack;
		EnemyPlayersNetworkObject = (int)Enemy | (int)Players | (int)NetworkObject;
		EnemyPlayersAABB = (int)Enemy | (int)Players | (int)AABB;
		EnemyNetworkObjectAttack = (int)Enemy | (int)NetworkObject | (int)Attack;
		EnemyPlayersNetworkObjectAttack = (int)Enemy | (int)Players | (int)NetworkObject | (int)Attack;
		EnemyPlayersNetworkObjectAABB = (int)Enemy | (int)Players | (int)NetworkObject | (int)AABB;
		EnemyPlayersNetworkObjectAttackAABB = (int)Enemy | (int)Players | (int)NetworkObject | (int)Attack | (int)AABB;
		PlayersAttack = (int)Players | (int)Attack;
		PlayersAABB = (int)Players | (int)AABB;
		PlayersAttackAABB = (int)Players | (int)Attack | (int)AABB;
		PlayersNetworkObject = (int)Players | (int)NetworkObject;
		PlayersNetworkObjectAttack = (int)Players | (int)NetworkObject | (int)Attack;
		PlayersNetworkObjectAABB = (int)Players | (int)NetworkObject | (int)AABB;
		PlayersNetworkObjectAttackAABB = (int)Players | (int)NetworkObject | (int)Attack | (int)AABB;
		NetworkObjectAABB = (int)NetworkObject | (int)AABB;
		NetworkObjectAttack = (int)NetworkObject | (int)Attack;
		NetworkObjectAttackAABB = (int)NetworkObject | (int)Attack | (int)AABB;
		AttackAABB = (int)Attack | (int)AABB;
	}
}

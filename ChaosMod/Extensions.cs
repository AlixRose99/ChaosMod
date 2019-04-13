﻿using GTA;
using GTA.Native;

namespace ChaosMod
{
	public enum ParachuteState
	{
		None = -1,
		FreeFalling,
		Deploying,
		Gliding,
		LandingOrFallingToDoom,
	}

	public static class WorldExtension
	{
		/// <summary>
		/// Test if the given animation set is loaded for the character.
		/// </summary>
		public static bool HasAnimationSetLoaded(string set)
		{
			return Function.Call<bool>(Hash.HAS_ANIM_SET_LOADED, set);
		}

		/// <summary>
		/// Request the given animation set to be loaded.
		/// </summary>
		public static void RequestAnimationSet(string set)
		{
			Function.Call(Hash.REQUEST_ANIM_SET, set);
		}

		/// <summary>
		/// Start a script fire.
		/// </summary>
		public static int StartScriptFire(GTA.Math.Vector3 pos, int maxChildren, bool isGasFire)
		{
			return Function.Call<int>(Hash.START_SCRIPT_FIRE, pos.X, pos.Y, pos.Z, maxChildren, isGasFire);
		}

		/// <summary>
		/// Remove the scripted fire.
		/// </summary>
		public static void RemoveScriptFire(int handle)
		{
			Function.Call(Hash.REMOVE_SCRIPT_FIRE, handle);
		}
	}

	public static class PedExtension
	{
		/// <summary>
		/// Get the parachute state of a ped.
		/// </summary>
		public static ParachuteState GetParachuteState(this Ped ped)
		{
			var value = Function.Call<int>(Hash.GET_PED_PARACHUTE_STATE, ped.Handle);
			return (ParachuteState)value;
		}

		/// <summary>
		/// Set the parachute state of a ped.
		/// </summary>
		public static void SetParachuteState(this Ped ped, ParachuteState state)
		{
			Function.Call<int>(Hash.SET_PARACHUTE_TASK_TARGET, ped.Handle, (int)state);
		}

		/// <summary>
		/// Make the pedestrian drunk.
		/// </summary>
		public static void SetPedIsDrunk(this Ped ped, bool drunk)
		{
			Function.Call<int>(Hash.SET_PED_IS_DRUNK, ped.Handle, drunk);
		}

		/// <summary>
		/// Set which clipset to use when moving.
		/// </summary>
		public static void SetPedMovementClipset(this Ped ped, string set)
		{
			Function.Call(Hash.SET_PED_MOVEMENT_CLIPSET, ped.Handle, set, 1f);
		}

		/// <summary>
		/// Clear the current clipset.
		/// </summary>
		public static void ResetPedMovementClipset(this Ped ped)
		{
			Function.Call(Hash.RESET_PED_MOVEMENT_CLIPSET, ped.Handle, 1f);
		}

		/// <summary>
		/// Set the given ped on fire.
		/// 
		/// NOTE: This doesn't work very well.
		/// It seems like the game has a fixed limit on how many fires it can run at any given time, and there's no consistent way to clean it up.
		/// Even StopEntityFire doesn't help.
		/// </summary>
		public static Ped StartEntityFire(this Ped ped)
		{
			var handle = Function.Call<int>(Hash.START_ENTITY_FIRE, ped.Handle);
			return new Ped(handle);
		}

		/// <summary>
		/// Put out the fire on the current player.
		/// </summary>
		public static void StopEntityFire(this Ped ped)
		{
			Function.Call(Hash.STOP_ENTITY_FIRE, ped.Handle);
		}

		/// <summary>
		/// Make the ped stumble.
		/// </summary>
		public static void Stumble(this Ped ped, int durationMs)
		{
			ped.Euphoria.ArmsWindmillAdaptive.ResetArguments();

			ped.Euphoria.ArmsWindmillAdaptive.AngSpeed = 6f;
			ped.Euphoria.ArmsWindmillAdaptive.Amplitude = 1.5f;
			ped.Euphoria.ArmsWindmillAdaptive.LeftElbowAngle = 0.1f;
			ped.Euphoria.ArmsWindmillAdaptive.RightElbowAngle = 0.1f;
			ped.Euphoria.ArmsWindmillAdaptive.DisableOnImpact = true;
			ped.Euphoria.ArmsWindmillAdaptive.BendLeftElbow = true;
			ped.Euphoria.ArmsWindmillAdaptive.BendRightElbow = true;

			ped.Euphoria.ArmsWindmillAdaptive.Start(durationMs);

			ped.Euphoria.ApplyImpulse.ResetArguments();
			ped.Euphoria.ApplyImpulse.Impulse = ped.ForwardVector * -100;
			ped.Euphoria.ApplyImpulse.Start(durationMs);

			ped.Euphoria.BodyBalance.Start();
		}
	}
}

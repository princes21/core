﻿using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using System.Collections.Generic;
using System.Numerics;

namespace LeagueSandbox.GameServer.GameObjects.SpellNS
{
    public class CastInfo
    {
        public uint SpellHash { get; set; }
        public uint SpellNetID { get; set; }
        public byte SpellLevel { get; set; }
        public float AttackSpeedModifier { get; set; } = 1.0f;
        public ObjAIBase Owner { get; set; }
        public uint SpellChainOwnerNetID { get; set; } // TODO: Figure out what this is used for
        public uint PackageHash { get; set; }
        public uint MissileNetID { get; set; }
        public Vector3 TargetPosition { get; set; }
        public Vector3 TargetPositionEnd { get; set; }

        public List<CastTarget> Targets { get; set; }

        public float DesignerCastTime { get; set; }
        public float ExtraCastTime { get; set; }
        public float DesignerTotalTime { get; set; }
        public float Cooldown { get; set; }
        public float StartCastTime { get; set; }

        public bool IsAutoAttack { get; set; } = false;
        public bool UseAttackCastTime { get; set; } = false;
        public bool UseAttackCastDelay { get; set; } = false;
        public bool IsSecondAutoAttack { get; set; } = false;
        public bool IsForceCastingOrChannel { get; set; } = false;
        public bool IsOverrideCastPosition { get; set; } = false;
        public bool IsClickCasted { get; set; } = false;

        public byte SpellSlot { get; set; }
        /// <summary>
        /// Hack for Leblanc's Mimic (R)
        /// </summary>
        public byte OgSpellSlot { get; set; } = 255;
        public float ManaCost { get; set; }
        public Vector3 SpellCastLaunchPosition { get; set; }
        public int AmmoUsed { get; set; }
        public float AmmoRechargeTime { get; set; }

        /// <summary>
        /// Adds the specified unit to the list of CastTargets.
        /// </summary>
        /// <param name="target">Unit to add.</param>
        public void AddTarget(AttackableUnit target)
        {
            Targets.Add(new CastTarget(target, CastTarget.GetHitResult(target, IsAutoAttack, Owner.IsNextAutoCrit)));
        }

        /// <summary>
        /// Removes the specified unit from the list of targets for this spell.
        /// </summary>
        /// <param name="target">Unit to remove.</param>
        public bool RemoveTarget(AttackableUnit target)
        {
            if (!Targets.Exists(t => t.Unit == target))
            {
                return false;
            }

            Targets.RemoveAt(Targets.FindIndex(t => t.Unit == target));

            return true;
        }

        /// <summary>
        /// Sets the CastTarget of the given slot to the given unit.
        /// An index outside the bounds of the list will be appended.
        /// </summary>
        /// <param name="target">Unit to input.</param>
        /// <param name="index">Index to set.</param>
        public void SetTarget(AttackableUnit target, int index)
        {
            if (Targets.Count - 1 < index)
            {
                AddTarget(target);
                return;
            }

            Targets[index] = new CastTarget(target, CastTarget.GetHitResult(target, IsAutoAttack, Owner.IsNextAutoCrit));
        }
    }
}

public enum UnitType //�Ⱦ�����
{
    Player,
    Minion,
    Enemy,
    EnemyBase,
    Boss,
}


public enum SkillName
{
    None = 0,
    SelfHealing = 10001,
    ShieldDeployment = 10002,
    WingFlap = 20011,
    LifeDrain = 20012,
    HealingAura = 30011,
    SpeedAura = 30012,
    HammerAttack = 10011,
    LegendHammerAttack = 10012,
    AttackSpeedUP = 10021,
    HeroArrow  =10022,
}

public enum SkillAct
{
    P ,
    A

}
public enum TargetSearchType
{
    Target,
    Range,
    Ora,

}

public enum BaseStat
{
    None,
    Atk,
    HP
}



public enum SkillEffectType
{
    DeBuff,
    Buff,
}

public enum BuffName
{
    ATKBoost = 0,
    ATKSpeedBoost,
    Heal,
    FullImmune,
    Poison,
    Stun,
    KnockBack,
    MoveSpeed,
}
public enum Chr_job
{
    magic,
    melee,
    projectile,
}

public enum MinionUnitIndex
{
    Player = 10000,
    Warrior = 11110,
    Knight_Elite = 11210,
    Hammer_Elite = 11220,

    Knight_Hero = 11310,
    Hammer_Hero = 11320,

    Archer_Elite = 12210,
    Archer_Hero = 12310,

}


public enum UnitGrade
{
    Player,
    Normal,
    Elite,
    Hero,
    Boss,
}
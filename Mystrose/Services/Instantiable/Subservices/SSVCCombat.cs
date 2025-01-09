namespace Mystrose.Services.Instantiable.Subservices;

public class SSVCCombat(ISVCFlashAPI service) : Subservice<ISVCFlashAPI>(service)
{

    #region Methods: Service
    public void Rest()
    {
        Execute(() =>
        {
            Service.CallGameFunction("world.rest");
        });
    }

    public void Attack(Monster monster)
    {
        Execute(() =>
        {
            Target(monster);
            Service.CallGameFunction("world.approachTarget");
        });
    }
    
    public void Attack(Avatar avatar)
    {
        Execute(() =>
        {
            Target(avatar);
            Service.CallGameFunction("world.approachTarget");
        });
    }

    public void Skill(ActiveSkill skill)
    {
        Execute(() =>
        {
            if (skill.ActionType is ActionType.AutoAttack)
            {
                Service.CallGameFunction("world.approachTarget");
                return;
            }

            string actionString = JSONParser.Serialize(skill.ActionType);
            JsonObject skillObject = Service.CallGameFunction<JsonObject>("world.getActionByRef", actionString);
            string skillString = JSONParser.Serialize(skillObject);
            
            Service.CallGameFunction("world.testAction", skillString);
        });
    }
    
    public void Target(Monster monster)
    {
        Execute(() =>
        {
            JsonObject monsterObject = Service.CallGameFunction<JsonObject>("world.getMonster", monster.MonMapID);
            string monsterString = JSONParser.Serialize(monsterObject);
            
            Service.CallGameFunction("world.setTarget", monsterString);
        });
    }
    
    public void Target(Avatar avatar)
    {
        Execute(() =>
        {
            JsonObject avatarObject = Service.CallGameFunction<JsonObject>("world.getAvatarByUserName", avatar.Name);
            string avatarString = JSONParser.Serialize(avatarObject);
            
            Service.CallGameFunction("world.setTarget", avatarString);
        });
    }
    
    public void CancelAttack()
    {
        Execute(() =>
        {
            Service.CallGameFunction("world.cancelAutoAttack");
        });
    }

    public void CancelTarget()
    {
        Execute(() =>
        {
            Service.CallGameFunction("world.cancelTarget");
        });
    }
    
    public void CancelSelfTarget()
    {
        Execute(() =>
        {
            JsonObject? targetObject = Service.GetGameObject<JsonObject?>("world.myAvatar.target", null);
            
            if (targetObject is null || !targetObject["isMyAvatar"]!.GetValue<bool>())
            {
                return;
            }
            
            CancelTarget();
        });
    }
    #endregion

    #region Methods: Overrides
    protected override void Log(string message)
    {
        HSVCLogger.Instance.LogOnConsole(message, Service.Identifier.Codename, "SSVCCombat");
    }
    #endregion

}

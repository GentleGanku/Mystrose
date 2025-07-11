namespace Mystrose.Services.Instantiable.Subservices;

public class SSVCQuest(ISVCFlashAPI service) : Subservice<ISVCFlashAPI>(service)
{

    #region Methods: Service
    public void LoadQuests(params int[] questIds)
    {
        Execute(() =>
        {
            Service.CallGameFunction("world.getQuests", questIds);
        });
    }

    public void ShowQuests(params int[] questIds)
    {
        Execute(() =>
        {
            Service.CallGameFunction("world.showQuests", string.Join(',', questIds), "q");
        });
    }
    
    public void AcceptQuest(Quest quest)
    {
        Execute(() =>
        {
            Service.CallGameFunction("world.acceptQuest", quest.ID);
        });
    }
    
    public void AcceptQuest(int questId)
    {
        Execute(() =>
        {
            Service.CallGameFunction("world.acceptQuest", questId);
        });
    }
    
    public void AbandonQuest(Quest quest)
    {
        Execute(() =>
        {
            Service.CallGameFunction("world.abandonQuest", quest.ID);
        });
    }
    
    public void AbandonQuest(int questId)
    {
        Execute(() =>
        {
            Service.CallGameFunction("world.abandonQuest", questId);
        });
    }

    public bool CompleteQuest(Quest quest, int itemId = -1, int turninsToComplete = 1)
    {
        return Execute(() =>
        {
            bool canTurnInQuest = Service.CallGameFunction<bool>("world.canTurnInQuest", quest.ID);
            
            if (!canTurnInQuest)
            {
                return false;
            }
            
            Service.CallGameFunction("world.tryQuestComplete", quest.ID, itemId, false, turninsToComplete);
            return true;
        });
    }
    
    public bool CompleteQuest(int questId, int itemId = -1, int turninsToComplete = 1)
    {
        return Execute(() =>
        {
            bool canTurnInQuest = Service.CallGameFunction<bool>("world.canTurnInQuest", questId);
            
            if (!canTurnInQuest)
            {
                return false;
            }
            
            Service.CallGameFunction("world.tryQuestComplete", questId, itemId, false, turninsToComplete);
            return true;
        });
    }
    
    public bool CompleteQuestAtMax(Quest quest, int itemId = -1)
    {
        return Execute(() =>
        {
            bool canTurnInQuest = Service.CallGameFunction<bool>("world.canTurnInQuest", quest.ID);
            
            if (!canTurnInQuest)
            {
                return false;
            }
            
            int turninsToComplete = Service.CallGameFunction<int>("world.maximumQuestTurnIns", quest.ID);
            Service.CallGameFunction("world.tryQuestComplete", quest.ID, itemId, false, turninsToComplete);
            return true;
        });
    }
    
    public bool CompleteQuestAtMax(int questId, int itemId = -1)
    {
        return Execute(() =>
        {
            bool canTurnInQuest = Service.CallGameFunction<bool>("world.canTurnInQuest", questId);
            
            if (!canTurnInQuest)
            {
                return false;
            }
            
            int turninsToComplete = Service.CallGameFunction<int>("world.maximumQuestTurnIns", questId);
            Service.CallGameFunction("world.tryQuestComplete", questId, itemId, false, turninsToComplete);
            return true;
        });
    }

    public void UpdateQuest(Quest quest)
    {
        Execute(() =>
        {
            Service.CallGameFunction("world.setQuestValue", quest.ChainIndex, quest.ChainSlot);
        });
    }
    
    public void UpdateQuest(int chainIndex, int chainSlot)
    {
        Execute(() =>
        {
            Service.CallGameFunction("world.setQuestValue", chainIndex, chainSlot);
        });
    }
    #endregion

    #region Methods: Overrides
    protected override void Log(string message)
    {
        HSVCLogger.Instance.LogOnConsole(message, Service.Identifier.Codename, "SSVCQuest");
    }
    #endregion

}

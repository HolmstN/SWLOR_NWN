UPDATE Player
SET PrimaryResidencePCBaseID = NULL,
	PrimaryResidencePCBaseStructureID = NULL;

TRUNCATE TABLE chatlog;

DELETE FROM PCBaseStructureItem;

DELETE FROM PCBaseStructurePermission;

DELETE FROM PCBasePermission;

UPDATE PCBaseStructure SET ParentPCBaseStructureID = null;
DELETE FROM PCBaseStructure;

DELETE FROM PCBase;

DELETE FROM Area;

DELETE FROM PCCraftedBlueprint;

DELETE FROM PCCooldown;

DELETE FROM PCCustomEffect;

DELETE FROM PCKeyItem;

DELETE FROM PCPerk;

DELETE FROM PCSkill;

DELETE FROM PCObjectVisibility;

DELETE FROM PCOutfit;

DELETE FROM PCOverflowItem;

DELETE FROM PCSearchSiteItem;

DELETE FROM PCSearchSite;

DELETE FROM PCRegionalFame;

DELETE FROM PCQuestItemProgress;

DELETE FROM PCQuestKillTargetProgress;

DELETE FROM PCQuestStatus;

DELETE FROM BugReport;

DELETE FROM PCMapPin;

TRUNCATE TABLE PCImpoundedItem;

DELETE FROM PCPerkRefund;

DELETE FROM BankItem;

DELETE FROM Bank;

DELETE FROM PCMarketListing;

DELETE FROM PCMapProgression;

DELETE FROM PCSkillPool;

DELETE FROM Message;

TRUNCATE TABLE ModuleEvent;

DELETE FROM PCGuildPoint;

DELETE FROM Player;

DELETE FROM GrowingPlant;

TRUNCATE TABLE Error;

DELETE FROM DMAction;

